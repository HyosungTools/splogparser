using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static SipSession.MediaStream;

namespace SipSession
{

   public class MediaTransferItem
   {
      public DateTime timestamp;
      public long bytesTransferred;

      public MediaTransferItem(DateTime timestamp, long bytesTransferred)
      {
         this.timestamp = timestamp;
         this.bytesTransferred = bytesTransferred;
      }
   }

   public class MediaTransferErrorItem
   {
      public DateTime timestamp;
      public long goodPackets;
      public long lostPackets;
      public long jitterMsec;

      public MediaTransferErrorItem(DateTime timestamp, long lostPackets, long goodPackets, long jitterMsec)
      {
         this.timestamp = timestamp;
         this.lostPackets = lostPackets;
         this.goodPackets = goodPackets;
         this.jitterMsec = jitterMsec;
      }
   }

   public class MediaStreamData
   {
      private DateTime firstTimestamp = DateTime.MinValue;
      private DateTime lastTimestamp = DateTime.MinValue;

      private long totalBytesTransferred = 0;
      private double averageTransferRateBps = 0;

      private DateTime firstErrorTimestamp = DateTime.MinValue;
      private DateTime lastErrorTimestamp = DateTime.MinValue;

      private long totalGoodPackets = 0;
      private long totalLostPackets = 0;
      private long maximumJitterMsec = 0;
      private double averagePacketLoss = 0;

      private Queue<MediaTransferItem> transferItems = new Queue<MediaTransferItem>();
      private Queue<MediaTransferErrorItem> transferErrorItems = new Queue<MediaTransferErrorItem>();

      private TimeSpan movingAverageInterval = TimeSpan.FromSeconds(1);

      private double movingAverageBytesTransferredRunningTotal = 0;
      private double movingAverageTransferRateBps = 0;

      private long movingAverageGoodPacketsRunningTotal = 0;
      private long movingAverageLostPacketsRunningTotal = 0;
      private double packetLossMovingAveragePercent = 0;
      private double packetLossMovingAveragePeakPercent = 0;

      /// <summary>
      /// Initializes a new instance of the <see cref="MediaStreamData"/> class.
      /// </summary>
      public MediaStreamData()
      {
      }

      /// <summary>
      /// Adds to the accumulated errors.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <param name="lostPackets">the number of packets lost</param>
      /// <param name="goodPackets">the number of packets received correctly</param>
      /// <param name="jitterMsec">jitter in Msec</param>
      public void AccumulateTransferErrors(DateTime timestamp, long lostPackets, long goodPackets, int jitterMsec)
      {
         // calculate moving average to identify excessive losses

         if (lostPackets < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(lostPackets));
         }

         if (goodPackets < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(goodPackets));
         }

         if (timestamp < this.lastErrorTimestamp)
         {
            throw new ArgumentOutOfRangeException(nameof(timestamp));
         }

         if (this.firstErrorTimestamp == DateTime.MinValue)
         {
            this.firstErrorTimestamp = timestamp;
         }

         this.lastErrorTimestamp = timestamp;

         if (jitterMsec > this.maximumJitterMsec)
         {
            this.maximumJitterMsec = jitterMsec;
         }

         // accumulate grand totals

         this.totalGoodPackets += goodPackets;
         this.totalLostPackets += lostPackets;

         if (this.totalGoodPackets > 0)
         {
            this.averagePacketLoss = (double)this.totalLostPackets / this.totalGoodPackets * 100;
         }
         
         // update moving average

         TimeSpan interval = this.lastErrorTimestamp - this.firstErrorTimestamp;

         // drop items older than 1 second from the queue and remove their value from the running total
         while (transferErrorItems.Count > 0 && timestamp.Subtract(transferErrorItems.Peek().timestamp) > this.movingAverageInterval)
         {
            MediaTransferErrorItem removedItem = transferErrorItems.Dequeue();

            this.movingAverageGoodPacketsRunningTotal -= removedItem.goodPackets;
            this.movingAverageLostPacketsRunningTotal -= removedItem.lostPackets;
         }

         // add the new report and add values to the running total

         transferErrorItems.Enqueue(new MediaTransferErrorItem(timestamp, lostPackets, goodPackets, jitterMsec));
         this.movingAverageGoodPacketsRunningTotal += goodPackets;
         this.movingAverageLostPacketsRunningTotal += lostPackets;

         // now calculate the running average

         if (this.movingAverageGoodPacketsRunningTotal > 0)
         {
            this.packetLossMovingAveragePercent = Math.Round((double)this.movingAverageLostPacketsRunningTotal / (this.movingAverageGoodPacketsRunningTotal + this.movingAverageLostPacketsRunningTotal) * 100, 2);

            if (this.packetLossMovingAveragePercent > this.packetLossMovingAveragePeakPercent)
            {
               this.packetLossMovingAveragePeakPercent = this.packetLossMovingAveragePercent;
            }
         }
         else
         {
            this.packetLossMovingAveragePercent = 0;
         }
      }

      /// <summary>
      /// Adds to the accumulated bytes-transferred.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <param name="transferCount">0 or more</param>
      /// <exception cref="ArgumentOutOfRangeException">thrown if the transfer count is negative, or the timestamp is earlier than the previous timestamp</exception>
      public void AccumulateBytesTransferred(DateTime timestamp, long transferCount)
      {
         if (transferCount < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(transferCount));
         }

         if (timestamp < this.lastTimestamp)
         {
            throw new ArgumentOutOfRangeException(nameof(timestamp));
         }

         if (this.firstTimestamp == DateTime.MinValue)
         {
            this.firstTimestamp = timestamp;
         }

         this.lastTimestamp = timestamp;
         this.totalBytesTransferred += transferCount;

         // update average transfer rate

         TimeSpan interval = this.lastTimestamp - this.firstTimestamp;

         if (interval.TotalSeconds == 0)
         {
            this.averageTransferRateBps = this.totalBytesTransferred;
         }
         else
         {
            this.averageTransferRateBps = this.totalBytesTransferred / interval.TotalSeconds;
         }

         // update moving average transfer rate

         // drop items older than 1 second from the queue
         while( transferItems.Count > 0 && timestamp.Subtract(transferItems.Peek().timestamp) > this.movingAverageInterval )
         {
            MediaTransferItem removedItem = transferItems.Dequeue();

            this.movingAverageBytesTransferredRunningTotal -= removedItem.bytesTransferred;
         }

         transferItems.Enqueue(new MediaTransferItem(timestamp, transferCount));
         this.movingAverageBytesTransferredRunningTotal += transferCount;

         if (transferItems.Count == 1)
         {
            this.movingAverageTransferRateBps = this.movingAverageBytesTransferredRunningTotal;
         }
         else
         {
            double totalSeconds = timestamp.Subtract(transferItems.Peek().timestamp).TotalSeconds;

            if (totalSeconds != 0)
            {
               this.movingAverageTransferRateBps = this.movingAverageBytesTransferredRunningTotal / totalSeconds;
            }
         }

         // TODO - consider adding a transfer event depending on the value of movingAverageTransferRate
      }

      /// <summary>
      /// Gets a value representing the transfer rate in bytes/second,
      /// </summary>
      public double AverageTransferRateBps {  get { return this.averageTransferRateBps; } }

      /// <summary>
      /// Gets a value representing the moving average of the transfer rate in bytes/second.
      /// </summary>
      public double MovingAverageTransferRateBps { get { return this.movingAverageTransferRateBps; } }

      /// <summary>
      /// Gets a vale representing the number of good packets received.
      /// </summary>
      public long TotalGoodPackets { get { return this.totalGoodPackets; } }

      /// <summary>
      /// Gets a value representing the number of packets lost.
      /// </summary>
      public long TotalLostPackets { get { return this.totalLostPackets; } }

      /// <summary>
      /// Gets a value representing the total packet loss.
      /// </summary>
      public double TotalPacketLossPercentage
      {
         get
         {
            if (totalGoodPackets == 0)
            {
               return 0;
            }

            return (double)TotalLostPackets / totalGoodPackets * 100;
         }
      }

      /// <summary>
      /// Gets the maximum jitter reported, in msec.
      /// </summary>
      public long MaximumJitterMsec { get { return this.maximumJitterMsec; } }

      /// <summary>
      /// Gets a value representing the moving average of the percentage packet loss.
      /// </summary>
      public double MovingAveragePacketLossPercentage { get { return this.packetLossMovingAveragePercent; } }

      /// <summary>
      /// Gets a value representing the peak moving average of the percentage packet loss.
      /// </summary>
      public double MovingAveragePeakPacketLossPercentage { get { return this.packetLossMovingAveragePeakPercent; } }

      /// <summary>
      /// Gets a value which is the total number of media bytes transferred.
      /// </summary>
      public double TotalBytesTransferred { get { return this.totalBytesTransferred; } }

      /// <summary>
      /// Gets a value which is the time of the first transfer.
      /// </summary>
      public DateTime Timestamp_FirstTransfer {  get { return this.firstTimestamp; }}

      /// <summary>
      /// Gets a value which is the time of the last transfer.
      /// </summary>
      public DateTime Timestamp_LastTransfer {  get { return this.lastTimestamp; }}


   }

   public class MediaStreamEvent
   {
      private DateTime timestamp;
      private string tag;
      private string description;
      private long count;

      /// <summary>
      /// Initializes a new instance of the <see cref="MediaStreamEvent"/> class.
      /// </summary>
      /// <param name="timestamp">Time the event was reported.</param>
      /// <param name="tag">A short tag.</param>
      /// <param name="description">A description.</param>
      /// <param name="count">The number of items affected - the type of the item will vary according to the description.</param>
      public MediaStreamEvent(DateTime timestamp, string tag, string description, long count)
      {
         this.timestamp = timestamp;
         this.tag = tag;
         this.description = description;
         this.count = count;
      }

      public DateTime Timestamp { get { return this.timestamp; } }

      public string Tag { get { return this.tag; } }

      public string Description { get { return this.description; } }

      public long Count { get { return this.count; } }
   }

   /// <summary>
   /// A class that accumulates event totals into fixed-duration time buckets.
   /// </summary>
   public class MediaTimeSeries
   {
      public const int TimeBucketSeconds = 5;
      public const int TimeBucketDuration = 600;
      public readonly DateTime BucketZeroTimestamp = DateTime.MinValue;

      // time series table is an alternative to discrete totals such as movingAverage
      private DataTable timeSeriesTable = null;

      private bool isTableFinalized = false;

      /// <summary>
      /// Constructor
      /// </summary>
      public MediaTimeSeries(DateTime zeroTimestamp)
      {
         this.timeSeriesTable = this.InitializeTimeBuckets(zeroTimestamp, MediaTimeSeries.TimeBucketDuration, MediaTimeSeries.TimeBucketSeconds);
         this.BucketZeroTimestamp = zeroTimestamp;
      }


      /// <summary>
      /// Gets a reference to the time series table, making sure it has been finalized first.
      /// </summary>
      /// <returns></returns>
      public DataTable GetFinalizedTimeSeriesTable()
      {
         FinalizeTimeBuckets();
         return this.timeSeriesTable;
      }


      /// <summary>
      /// Adds the information in the media event to a bucket in the time series table.
      /// </summary>
      /// <param name="mediaEvent"></param>
      /// <exception cref="Exception">An exception thrown if the update could not be made due to a coding bug.</exception>
      public void UpdateTimeBucket(MediaStreamEvent mediaEvent)
      {
         if (isTableFinalized)
         {
            throw new Exception("The table is already finalized, no more events can be added.");
         }

         Regex regex;
         Match m;
         bool success;
         string payLoad = mediaEvent.Description;

         string delim = ";";
         string[] subs;

         try
         {
            DataRow row = GetTimeBucketDataRow(mediaEvent.Timestamp);

            if (row == null)
            {
               // the session hasn't started yet
               return;
            }

            switch (mediaEvent.Tag)
            {
               /* Device/Transmitter/Receiver States */

               case "microphone-open":
               case "speaker-open":
               case "camera-open":
                  // mediaEvent.Description = $"{m.Groups["device"].Value} ({m.Groups["num"].Value}- {m.Groups["name"].Value})":
                  regex = new Regex("(?<device>.*) \\((?<num>[0-9]*)- (?<name>.*)\\)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     switch (mediaEvent.Tag)
                     {
                        case "microphone-open":
                           row["microphonestate"] = $"{row.Field<string>("microphonestate")}open {m.Groups["name"]}{delim}";
                           break;

                        case "speaker-open":
                           row["speakerstate"] = $"{row.Field<string>("speakerstate")}open {m.Groups["name"]}{delim}";
                           break;

                        case "camera-open":
                           row["camerastate"] = $"{row.Field<string>("camerastate")}open {m.Groups["name"]}{delim}";
                           break;

                        default:
                           break;
                     }
                  }
                  break;


               case "microphone-on":
               case "speaker-on":
               case "camera-on":
                  // mediaEvent.Description = $"set ({m.Groups["index"].Value}- {m.Groups["device"].Value}) {m.Groups["handle"].Value}":
                  regex = new Regex("set \\((?<index>[0-9]*)- (?<device>.*)\\) (?<handle>[0-9A-Fa-f]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     switch (mediaEvent.Tag)
                     {
                        case "microphone-on":
                           row["microphonestate"] = $"{row.Field<string>("microphonestate")} on {m.Groups["device"]}{delim}";
                           break;

                        case "speaker-on":
                           row["speakerstate"] = $"{row.Field<string>("speakerstate")} on {m.Groups["device"]}{delim}";
                           break;

                        case "camera-on":
                           row["camerastate"] = $"{row.Field<string>("camerastate")} on {m.Groups["device"]}{delim}";
                           break;

                        default:
                           break;
                     }
                  }
                  break;


               case "microphone-closed":
               case "speaker-closed":
               case "camera-closed":
                  // mediaEvent.Description = $"({m.Groups["device"].Value}) {m.Groups["handle"].Value}":
                  regex = new Regex("\\((?<device>.*)\\) (?<handle>[0-9A-Fa-f]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     switch (mediaEvent.Tag)
                     {
                        case "microphone-closed":
                           row["microphonestate"] = $"{row.Field<string>("microphonestate")}closed {m.Groups["device"]}{delim}";
                           break;

                        case "speaker-closed":
                           row["speakerstate"] = $"{row.Field<string>("speakerstate")}closed {m.Groups["device"]}{delim}";
                           break;

                        case "camera-closed":
                           row["camerastate"] = $"{row.Field<string>("camerastate")}closed {m.Groups["device"]}{delim}";
                           break;

                        default:
                           break;
                     }
                  }
                  break;


               case "video-transmitter-start":
               case "audio-receiver-start":
               case "video-receiver-start":

               case "video-transmitter-pause":
               case "audio-receiver-pause":
               case "video-receiver-pause":

               case "audio-receiver-startprocessing":

               case "video-transmitter-stop":
               case "audio-receiver-stop":
               case "video-receiver-stop":

               case "video-receiver-initialize":

                  subs = mediaEvent.Tag.Split('-');

                  // field names:
                  //  audiotransmitterstate
                  //  audioreceiverstate
                  //  videotransmitterstate
                  //  videoreceiverstate
                  string name = $"{subs[0]}{subs[1]}state";
                  row[$"{name}"] = $"{row.Field<string>($"{name}")}{subs[2]}{delim}";
                  break;

               case "audio-receiver-local-controlport":
               case "audio-receiver-local-mediaport":
               case "audio-transmitter-remote-controlport":
               case "audio-transmitter-remote-mediaport":
                  break;

               /* Camera */

               case "camera-set-resolution":
                  // mediaEvent.Description = $"{width}x{height} pixels":
                  regex = new Regex("(?<width>[0-9]*)x(?<height>[0-9]*) pixels");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["cameraresolution"] = $"{m.Groups["width"].Value}x{m.Groups["width"].Value}";
                  }
                  break;

               case "camera-set-capture-resolution":
                  // mediaEvent.Description = $"{width}x{height} pixels, framerate {frameRate} fps":
                  regex = new Regex("(?<width>[0-9]*)x(?<height>[0-9]*) pixels, framerate (?<framerate>[0-9]*) fps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["cameracaptureresolution"] = $"{m.Groups["width"].Value}x{m.Groups["width"].Value}";
                     row["cameraframerate"] = int.Parse(m.Groups["framerate"].Value);
                  }
                  break;


               /* Microphone, Speaker */

               case "audio-volume-low":
               case "audio-volume-normal":
               case "audio-volume-high":
                  // mediaEvent.Description = $"{Math.Round(speakerVolumePercent)}% of max":
                  regex = new Regex("(?<volume>[0-9]*)% of max");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     int vol = int.Parse(m.Groups["volume"].Value);

                     if (vol < row.Field<long>("lowestvolume"))
                     {
                        row["lowestvolume"] = vol;
                     }

                     if (vol > row.Field<long>("highestvolume"))
                     {
                        row["highestvolume"] = vol;
                     }

                     subs = mediaEvent.Tag.Split('-');
                     row["volumestate"] = subs[2];
                  }
                  break;

               /* Audio Config */

               case "audio-max-packetsize":
                  // mediaEvent.Description = $"{maxPacketSize} bytes":
                  regex = new Regex("(?<size>[0-9]*)% bytes");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["audioreceivepacketsize"] = long.Parse(m.Groups["size"].Value);
                  }
                  break;

               case "audio-transmitter-set-codec-bitrate":
                  // mediaEvent.Description = $"{bitRate} bps":
                  regex = new Regex("(?<bitrate>[0-9]*) bps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["audiotransmitterstreambitrate"] = long.Parse(m.Groups["bitrate"].Value);
                  }
                  break;

               case "audio-receiver-codec":
                  // mediaEvent.Description = $"{codecName}, bitrate {bitRate} bps, framesize {decodedFrameSize} bytes":
                  regex = new Regex("(?<codec>.*), bitrate (?<bitrate>[0-9]*) bps, framesize (?<framesize>[0-9]*) bytes");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["audioreceivercodec"] = m.Groups["codec"].Value;
                     row["audioreceiverstreambitrate"] = long.Parse(m.Groups["bitrate"].Value);
                     row["audioreceiverframesize"] = long.Parse(m.Groups["framesize"].Value);
                  }
                  break;

               case "audio-get-codec-parameters":
                  // mediaEvent.Description = $"frame {decodedFrameSize} bytes, duration {frameDuration} msec, samplerate {sampleRate} bps":
                  regex = new Regex("frame (?<framesize>[0-9]*) bytes, duration (?<duration>[0-9]*) msec, samplerate (?<samplerate>[0-9]*) bps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["audiotransmittersamplerate"] = long.Parse(m.Groups["samplerate"].Value);
                     row["audiotransmitterframeduration"] = long.Parse(m.Groups["duration"].Value);
                     row["audiotransmitterframesize"] = long.Parse(m.Groups["framesize"].Value);
                  }
                  break;


               /* Audio Stats & Errors */

               case "audio-receiver-quality":
                  // mediaEvent.Description = $"Audio QOS - lost {lostPackets}, LastSeq# {lastSeqNumber}":
                  regex = new Regex("Audio QOS - lost (?<lost>[0-9]*), LastSeq# (?<lastseq>[0-9]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     // be careful not to double-count - see audio-quality
                     long totallost = row.Field<long>("audioframeslost") + long.Parse(m.Groups["lost"].Value);
                     row["audioframeslost"] = $"{totallost}";

                     // TODO: calculate good from lastseq?
                     // see audio-quality
                  }
                  break;

               case "audio-codec-frame":
                  // mediaEvent.Description = $"size {decodedFrameSize} bytes, {frameDuration} msec, samplerate {sampleRate} bps":
                  regex = new Regex("size (?<framesize>[0-9]*) bytes, (?<duration>[0-9]*) msec, samplerate (?<samplerate>[0-9]*) bps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["audioreceiversamplerate"] = long.Parse(m.Groups["samplerate"].Value);
                     row["audioreceiverframeduration"] = long.Parse(m.Groups["duration"].Value);
                     row["audioreceiverframesize"] = long.Parse(m.Groups["framesize"].Value);
                  }
                  break;

               case "audio-received-outoforder":
                  // mediaEvent.Description = $"received packet out of order? LastSeq# {m.Groups["lastseqnum"].Value}":
                  regex = new Regex("received packet out of order\\? LastSeq# (?<seqnum>[0-9]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     long totaloutoforder = row.Field<long>("audioreceivedoutoforder") + 1;
                     row["audioreceivedoutoforder"] = $"{totaloutoforder}";

                     // TODO: calculate good from lastseq?
                     // see audio-quality
                  }
                  break;

               case "audio-quality":
                  // mediaEvent.Description = $"Audio QOS - lost {lostPackets} out of {countPackets} ({percentLost}%), LastSeq# {m.Groups["lastseqnum"].Value}, Good {goodAudioSeqReceived}, jitter {m.Groups["fillMS"].Value} msec.":
                  regex = new Regex("Audio QOS - lost (?<lost>[0-9]*) out of (?<count>[0-9]*) \\((?<percentlost>[0-9\\.]*)%\\), LastSeq# (?<seqnum>[0-9]*), Good (?<goodrecv>[0-9]*), jitter (?<jitter>[0-9]*) msec.");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     // be careful not to double-count - see audio-receiver-quality
                     long totallost = row.Field<long>("audioframeslost") + long.Parse(m.Groups["lost"].Value);
                     row["audioframeslost"] = $"{totallost}";

                     long percentlost = (long)Math.Round(double.Parse(m.Groups["percentlost"].Value), 0, MidpointRounding.ToEven);
                     if (percentlost > row.Field<long>("audioframeslostpercent"))
                     {
                        row["audioframeslostpercent"] = $"{percentlost}";
                     }

                     // be careful not to double-count - see audio-receiver-quality
                     long totalgood = row.Field<long>("audioframesgood") + long.Parse(m.Groups["goodrecv"].Value);
                     row["audioframesgood"] = $"{totalgood}";

                     long jitter = long.Parse(m.Groups["jitter"].Value);
                     if (jitter > row.Field<long>("audiojitter"))
                     {
                        row["audiojitter"] = $"{jitter}";
                     }
                  }
                  break;

               case "audio-receiver-ok":
                  // ok - it means there is no loss, or the period of losses is at an end?
                  break;


               /* Direct3D Display errors */

               case "display-update-error":
                  // mediaEvent.Description = "Direct3D screen update error";
                  long totalerrors = row.Field<long>("direct3derrors") + 1;
                  row["direct3derrors"] = totalerrors;
                  break;


               /* Video Config */

               case "video-set-resolution":
                  // mediaEvent.Description = $"framerate {frameRate} fps, bitrate {useBitRate} bps, {width}x{height} pixels":
                  regex = new Regex("framerate (?<framerate>[0-9]*) fps, bitrate (?<bitrate>[0-9]*) bps, (?<width>[0-9]*)x(?<height>[0-9]*) pixels");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videotransmitterstreamresolution"] = $"{m.Groups["width"].Value}x{m.Groups["width"].Value}";
                     row["videotransmitterstreamframerate"] = int.Parse(m.Groups["framerate"].Value);
                     row["videotransmitterstreambitrate"] = int.Parse(m.Groups["bitrate"].Value);
                  }
                  break;

               case "video-set-parameters":
                  // mediaEvent.Description = $"framerate {frameRate} fps, maxbitrate {maxBitRate} bps, {width}x{height} pixels":
                  regex = new Regex("framerate (?<framerate>[0-9]*) fps, bitrate (?<bitrate>[0-9]*) bps, (?<width>[0-9]*)x(?<height>[0-9]*) pixels");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videotransmitterstreamresolution"] = $"{m.Groups["width"].Value}x{m.Groups["width"].Value}";
                     row["videotransmitterstreamframerate"] = int.Parse(m.Groups["framerate"].Value);
                     row["videotransmitterstreambitrate"] = int.Parse(m.Groups["bitrate"].Value);
                  }
                  break;

               case "video-resolution-changed":
                  // mediaEvent.Description = $"{width}x{height} pixels":
                  regex = new Regex("(?<width>[0-9]*)x(?<height>[0-9]*) pixels");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videotransmitterstreamresolution"] = $"{m.Groups["width"].Value}x{m.Groups["width"].Value}";
                  }
                  break;

               case "video-set-MTU":
                  // mediaEvent.Description = $"{mtuSize} bytes":
                  regex = new Regex("(?<size>[0-9]*) bytes");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videotransmitterMTU"] = long.Parse(m.Groups["size"].Value);
                  }
                  break;

               case "video-set-packetsize":
                  // mediaEvent.Description = $"{packetSize} bytes":
                  regex = new Regex("(?<size>[0-9]*) bytes");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videostreampacketsize"] = long.Parse(m.Groups["size"].Value);
                  }
                  break;

               case "video-set-bitrate":
                  // mediaEvent.Description = $"{newBitRate} bps":
                  regex = new Regex("(?<bitrate>[0-9]*) bps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videotransmitterstreambitrate"] = long.Parse(m.Groups["bitrate"].Value);
                  }
                  break;

               case "video-modify-bandwidth":
                  // mediaEvent.Description = $"{bandwidth} bps":
                  regex = new Regex("(?<bandwidth>[0-9]*) bps");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["videostreambandwidth"] = long.Parse(m.Groups["bandwidth"].Value);
                  }
                  break;

               case "video-set-mode":
                  // mediaEvent.Description = $"{modeName}":
                  row["videomodes"] = $"{row.Field<string>("videomodes")}{mediaEvent.Description}{delim}";
                  break;

               case "video-get-H264-parameters":
                  // mediaEvent.Description = $"bitrate {bitrate} bps, maxMacroblocks/sec {macroBlocksPerSec}, macroBlocks/frame {fs}, maxframes/sec {fr}":
                  regex = new Regex("(?<bitrate>[0-9]*) bps, maxMacroblocks/sec (?<mbps>[0-9]*), macroBlocks/frame (?<mbpf>[0-9]*), maxframes/sec (?<framerate>[0-9]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     row["h264bitrate"] = int.Parse(m.Groups["bitrate"].Value);
                     row["h264framerate"] = int.Parse(m.Groups["framerate"].Value);
                  }
                  break;

               case "video-set-codec":
                  // mediaEvent.Description = $"{codecName}":
                  row["videocodec"] = mediaEvent.Description;
                  break;


               /* Video Stats & Errors */

               case "video-frame-cropped":
                  // mediaEvent.Description = $"size {frameSize}, max {maxExpected} bytes":
                  regex = new Regex("size (?<framesize>[0-9]*), max (?<expected>[0-9]*) bytes");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     long totalcropped = row.Field<long>("videocroppedframes") + 1;
                     row["videocroppedframes"] = $"{totalcropped}";
                  }
                  break;

               case "video-quality":
                  // mediaEvent.Description = $"Video QOS - lost {videoLost} packets, LastSeq# {int.Parse(m.Groups["hinum"].Value)}":
                  regex = new Regex("Video QOS - lost (?<lost>[0-9]*) packets, LastSeq# (?<seq>[0-9]*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     long totallost = row.Field<long>("videopacketslost") + long.Parse(m.Groups["lost"].Value);
                     row["videopacketslost"] = $"{totallost}";
                  }
                  break;

               case "video-quality-ok":
                  // ok - it means there is no loss, or the period of losses is at an end?
                  break;

               case "video-receive-frame-sync-warning":
                  // mediaEvent.Description = "Video QOS - frame lost":
                  long totalsync = row.Field<long>("videoframesyncwarnings") + 1;
                  row["videoframesyncwarnings"] = $"{totalsync}";
                  break;

               case "video-receive-frame-process":
                  break;

               case "video-receiver-quality":
                  // mediaEvent.Description = "Video QOS - internal frame lost":
                  // mediaEvent.Description = "Video QOS - previous frame lost":
                  if (mediaEvent.Description.Contains("internal") || mediaEvent.Description.Contains("previous"))
                  {
                     long totallost = row.Field<long>("videoframeslost") + 1;
                     row["videoframeslost"] = $"{totallost}";
                  }
                  break;

               case "video-qos-activate-PLI":
               case "video-qos-PLI-IFrame-requested":
               case "video-qos-PLI-IFrame-request-acknowledged":
               case "video-qos-PLI-activation-requested":
               case "video-Fb_PLI-reported":
                  // video engine requests I-Frame to be retransmitted ... after a loss
                  break;


               /* RTP Audio-Video Send/Receive Counts (DEBUG required) */

               case "audio-sent":
               case "video-sent":
                  // $"Sent {m.Groups["buffLen"].Value} bytes to {m.Groups["addr"].Value}:{m.Groups["port"].Value} ({entity})";
                  regex = new Regex("Sent (?<sent>[0-9]*) bytes to (?<addr>.*):(?<port>[0-9]*) (?<entity>.*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     if (mediaEvent.Tag.StartsWith("audio"))
                     {
                        long totalsent = row.Field<long>("audiobytessent") + long.Parse(m.Groups["sent"].Value);
                        row["audiobytessent"] = $"{totalsent}";
                     }
                     else
                     {
                        long totalsent = row.Field<long>("videobytessent") + long.Parse(m.Groups["sent"].Value);
                        row["videobytessent"] = $"{totalsent}";
                     }

                     row["remoteentity"] = m.Groups["entity"].Value;
                     row["remoteaddress"] = $"{m.Groups["addr"].Value}";
                  }
                  break;

               case "audio-received":
               case "video-received":
                  // $"Received {m.Groups["received"].Value} bytes from {m.Groups["addr"].Value}:{m.Groups["port"].Value} ({entity})";
                  regex = new Regex("Received (?<received>[0-9]*) bytes from (?<addr>.*):(?<port>[0-9]*) (?<entity>.*)");
                  m = regex.Match(payLoad);
                  success = m.Success;
                  if (success)
                  {
                     if (mediaEvent.Tag.StartsWith("audio"))
                     {
                        long totalreceived = row.Field<long>("audiobytesreceived") + long.Parse(m.Groups["received"].Value);
                        row["audiobytesreceived"] = $"{totalreceived}";
                     }
                     else
                     {
                        long totalreceived = row.Field<long>("videobytesreceived") + long.Parse(m.Groups["received"].Value);
                        row["videobytesreceived"] = $"{totalreceived}";
                     }

                     row["remoteentity"] = $"{m.Groups["entity"].Value}";
                     row["remoteaddress"] = $"{m.Groups["addr"].Value}";
                  }
                  break;

               default:
                  break;
            }

            CommitRowChanges();
         }

         catch
         {
            RejectRowChanges();
            throw;
         }
      }


      /// <summary>
      /// Gets the time series table in CSV format
      /// </summary>
      /// <returns>A list of strings.</returns>
      public List<string> TimeBucketsToCsv()
      {
         List<string> timeSeriesCsv = new List<string>();

         if (timeSeriesTable == null)
         {
            return timeSeriesCsv;
         }

         this.FinalizeTimeBuckets();

         // build formatted lines from the table data, in CSV format for import by Excel.
         // do not use comma's in the row data fields - it is the CSV field delimiter

         StringBuilder allColumnsText = new StringBuilder();

         // first row output contains the headers
         foreach (DataColumn seriesCol in timeSeriesTable.Columns)
         {
            allColumnsText.Append($"{seriesCol.ColumnName},");
         }
         allColumnsText.AppendLine();

         foreach (DataRow seriesRow in timeSeriesTable.Rows)
         {
            // 17:18:04.845 csv

            string formatTimeMsec = "HH:mm:ss.fff";

            string timeText = seriesRow.Field<string>("bucketoffset");
            allColumnsText.Append($"{timeText}");

            timeText = seriesRow.Field<DateTime>("time").ToString(formatTimeMsec);
            allColumnsText.Append($",{seriesRow.Field<DateTime>("time").ToString(formatTimeMsec)}");

            /*
            timeText = seriesRow.Field<DateTime>("adjustedtime").ToString(formatTimeMsec);
            allColumnsText.Append($",{seriesRow.Field<DateTime>("adjustedtime").ToString(formatTimeMsec)}");
            */

            foreach (DataColumn seriesCol in timeSeriesTable.Columns)
            {
               if (seriesCol.DataType == typeof(string))
               {
                  allColumnsText.Append($",{seriesRow.Field<string>(seriesCol.ColumnName)}");
               }

               else if (seriesCol.DataType == typeof(long))
               {
                  allColumnsText.Append($",{seriesRow.Field<long>(seriesCol.ColumnName)}");
               }

               else if (seriesCol.DataType == typeof(DateTime))
               {
                  // time field already added above
                  // allColumnsText.Append($"{comma}{seriesRow.Field<DateTime>("time").ToString(formatTime)}");
               }

               else if (seriesCol.ColumnName == "adjustedtime")
               {
                  // adjustedtime field already added above
                  // allColumnsText.Append($"{comma}{seriesRow.Field<DateTime>("adjustedtime").ToString(formatTime)}");
               }

               else
               {
                  throw new Exception($"TimeSeriesEventCsvBuckets unexpected field type {seriesCol.DataType.ToString()}");
               }
            }

            allColumnsText.AppendLine();
         }

         timeSeriesCsv.Add(allColumnsText.ToString());

         return timeSeriesCsv;
      }


      /// <summary>
      /// Initializes a time buckets data table whose name (Session_MMDD_HHMM) is based on the value of zeroTimestamp.
      /// </summary>
      /// <param name="zeroTimestamp">The starting time value for the table.</param>
      /// <param name="totalSeconds">The total seconds covered by all the time buckets in the table.</param>
      /// <param name="bucketSeconds">The size in seconds of each time bucket.</param>
      /// <returns>An initialized time buckets table.</returns>
      /// <exception cref="ArgumentOutOfRangeException">Thrown if the identified parameter is out of range.</exception>
      private DataTable InitializeTimeBuckets(DateTime zeroTimestamp, int totalSeconds, int bucketSeconds)
      {
         // expect to use 600 second (10 minutes) as a standard.  To avoid supporting hours, only up to 59 minutes is allowed.
         if (totalSeconds < 1 || totalSeconds > 59 * 60)
         {
            throw new ArgumentOutOfRangeException(nameof(totalSeconds), "out of range 1-(59 * 60)");
         }

         // expect to use 10 seconds as a standard
         if (bucketSeconds < 1 || bucketSeconds > 10)
         {
            throw new ArgumentOutOfRangeException(nameof(bucketSeconds), "out of range 1-10");
         }

         // there must be minimum 2 buckets, so that other methods can determine the bucket length by comparing the first
         // two rows.
         if (totalSeconds / bucketSeconds < 2)
         {
            throw new ArgumentOutOfRangeException(nameof(totalSeconds), "minimum number of time buckets is two");
         }

         DataTable timeSeriesTable = new DataTable();

         // base the table name on the session start time
         timeSeriesTable = new DataTable($"Session_{zeroTimestamp.ToString("MMdd_HHmmss")}");

         timeSeriesTable.Columns.Add(new DataColumn("summary", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("rownumber", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("bucketoffset", typeof(string)));   // mm:ss
         timeSeriesTable.Columns.Add(new DataColumn("time", typeof(DateTime)));
         timeSeriesTable.Columns.Add(new DataColumn("adjustedtime", typeof(string)));   // a formula will go here ("=rowcol+TIMEADJUSTMENT")

         // for convenience of plotting line charts in Excel:
         // - put the state and text fields first and the numeric fields last
         // - of the numeric fields, put the sent/received/error statistics first

         timeSeriesTable.Columns.Add(new DataColumn("microphonestate", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("camerastate", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("speakerstate", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("localentity", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("localaddress", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("remoteentity", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("remoteaddress", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("audioreceiverstate", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceivercodec", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("audiotransmitterstate", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("volumestate", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("cameracaptureresolution", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("cameraresolution", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("videotransmitterstate", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("videotransmitterstreamresolution", typeof(string)));

         timeSeriesTable.Columns.Add(new DataColumn("videoreceiverstate", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("videocodec", typeof(string)));
         timeSeriesTable.Columns.Add(new DataColumn("videomodes", typeof(string)));

         // network statistics

         timeSeriesTable.Columns.Add(new DataColumn("audiobytessent", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audiobytesreceived", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceivedoutoforder", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioframesgood", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioframeslost", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioframeslostpercent", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audiojitter", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("videobytessent", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videobytesreceived", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videocroppedframes", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videoframesyncwarnings", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videoframesgood", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videoframeslost", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videoframeslostpercent", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videopacketslost", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videojitter", typeof(long)));

         // dynamically-adjusted values

         timeSeriesTable.Columns.Add(new DataColumn("audioreceivepacketsize", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceiverstreambitrate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceiverframesize", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceiversamplerate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audioreceiverframeduration", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("audiotransmitterstreambitrate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audiotransmittersamplerate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audiotransmitterframeduration", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("audiotransmitterframesize", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("lowestvolume", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("highestvolume", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("direct3derrors", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("cameraframerate", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("videostreampacketsize", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videostreambandwidth", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("videotransmitterMTU", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videotransmitterstreamframerate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("videotransmitterstreambitrate", typeof(long)));

         timeSeriesTable.Columns.Add(new DataColumn("h264framerate", typeof(long)));
         timeSeriesTable.Columns.Add(new DataColumn("h264bitrate", typeof(long)));

         // prepopulate the table with a bucket for every time slot

         DataRow row = null;
         int rowNumber = 1;   // first row is the header row

         for (int secondsOffset = 0; secondsOffset < totalSeconds; secondsOffset += bucketSeconds)
         {
            int bucketNumber = secondsOffset / bucketSeconds;

            // convert to bucket start time
            // hours is not supported - but if hours exceeds "1" an exception is thrown
            int bucketOffset = bucketNumber * bucketSeconds;

            int hours = bucketOffset / 3600;
            if (hours > 1)
            {
               throw new ArgumentException(nameof(totalSeconds), "The total number of seconds cannot be greater than one hour.");
            }

            int minutes = (bucketOffset - hours * 3600) / 60;
            int seconds = (bucketOffset - hours * 3600) - minutes * 60;
            DateTime bucketStart = DateTime.MinValue.AddMinutes(minutes).AddSeconds(seconds);

            // initialize the 'row'

            row = timeSeriesTable.NewRow();

            // placeholder for the narrative summary
            row["summary"] = string.Empty;

            row["rownumber"] = rowNumber++;
            row["bucketoffset"] = bucketStart.ToString("mm:ss");
            row["time"] = zeroTimestamp.AddSeconds(secondsOffset);
            row["adjustedtime"] = "=rowcol+TIMEADJUSTMENT";    // row to be substituted when loaded into excel

            row["microphonestate"] = string.Empty;
            row["camerastate"] = string.Empty;
            row["speakerstate"] = string.Empty;

            row["localentity"] = string.Empty;
            row["localaddress"] = string.Empty;

            row["remoteentity"] = string.Empty;
            row["remoteaddress"] = string.Empty;

            row["audioreceiverstate"] = string.Empty;
            row["audioreceivepacketsize"] = 0;
            row["audioreceivercodec"] = string.Empty;
            row["audioreceiverstreambitrate"] = 0;
            row["audioreceiverframesize"] = 0;
            row["audioreceiversamplerate"] = 0;
            row["audioreceiverframeduration"] = 0;

            row["audiotransmitterstate"] = string.Empty;
            row["audiotransmitterstreambitrate"] = 0;
            row["audiotransmittersamplerate"] = 0;
            row["audiotransmitterframeduration"] = 0;
            row["audiotransmitterframesize"] = 0;

            row["volumestate"] = string.Empty;
            row["lowestvolume"] = long.MaxValue;
            row["highestvolume"] = long.MinValue;

            row["cameracaptureresolution"] = string.Empty;
            row["cameraframerate"] = 0;
            row["cameraresolution"] = string.Empty;

            row["direct3derrors"] = 0;

            row["videoreceiverstate"] = string.Empty;
            row["videocodec"] = string.Empty;
            row["videostreampacketsize"] = 0;
            row["videostreambandwidth"] = 0;
            row["videomodes"] = string.Empty;

            row["videotransmitterstate"] = string.Empty;
            row["videotransmitterMTU"] = 0;
            row["videotransmitterstreamresolution"] = string.Empty;
            row["videotransmitterstreamframerate"] = 0;
            row["videotransmitterstreambitrate"] = 0;

            row["h264framerate"] = 0;
            row["h264bitrate"] = 0;

            row["audiobytessent"] = 0;
            row["audiobytesreceived"] = 0;
            row["audioreceivedoutoforder"] = 0;
            row["audioframesgood"] = 0;
            row["audioframeslost"] = 0;
            row["audioframeslostpercent"] = 0;
            row["audiojitter"] = 0;

            row["videocroppedframes"] = 0;
            row["videoframesyncwarnings"] = 0;
            row["videobytessent"] = 0;
            row["videobytesreceived"] = 0;
            row["videoframesgood"] = 0;
            row["videoframeslost"] = 0;
            row["videoframeslostpercent"] = 0;
            row["videopacketslost"] = 0;
            row["videojitter"] = 0;

            timeSeriesTable.Rows.Add(row);
         }

         timeSeriesTable.AcceptChanges();

         return timeSeriesTable;
      }


      /// <summary>
      /// Finalize certain fields in the time buckets before converting to CSV.
      /// </summary>
      public void FinalizeTimeBuckets()
      {
         // timeSeriesTable.DefaultView.Sort = "bucketoffset ASC";

         // post-process certain fields
         foreach (DataRow seriesRow in this.timeSeriesTable.Rows)
         {
            foreach (DataColumn seriesCol in this.timeSeriesTable.Columns)
            {
               string columnName = seriesCol.ColumnName;

               switch (columnName)
               {
                  case "lowestvolume":
                  case "highestvolume":

                     long volume = seriesRow.Field<long>(columnName);
                     if (volume == long.MinValue || volume == long.MaxValue)
                     {
                        // 9223372036854775807,-9223372036854775808
                        seriesRow[columnName] = 0;
                     }
                     break;
               }
            }

            seriesRow.AcceptChanges();
         }

         this.timeSeriesTable.AcceptChanges();

         this.isTableFinalized = true;
      }


      /// <summary>
      /// Gets a reference to a row in the time series table, corresponding to a timestamp.
      /// </summary>
      /// <param name="timestamp">Timestamp to compare.</param>
      /// <returns>A data row, or null if a valid row cannot be found.</returns>
      /// <exception cref="Exception">An exception resulting from a coding error.</exception>
      private DataRow GetTimeBucketDataRow(DateTime timestamp)
      {
         if (timeSeriesTable == null)
         {
            // throw new Exception("The time series table is empty.");
            return null;
         }

         TimeSpan offsetTime = timestamp - this.BucketZeroTimestamp;

         int secondsOffset = (int)Math.Truncate(offsetTime.TotalSeconds);

         if (secondsOffset < 0)
         {
            // corresponding to the first row
            secondsOffset = 0;
         }

         if (secondsOffset > MediaTimeSeries.TimeBucketDuration)
         {
            // throw new Exception($"The timestamp is beyond the last row of the series table.");
            return null;
         }

         int bucketNumber = secondsOffset / MediaTimeSeries.TimeBucketSeconds;

         if (bucketNumber < 0 || bucketNumber >= timeSeriesTable.Rows.Count)
         {
            // coding bug
            throw new Exception($"The time series table does not contain bucket# {bucketNumber}.");
         }

         DataRow row = timeSeriesTable.Rows[bucketNumber];
         DateTime rowStartTimestamp = row.Field<DateTime>("time");
         DateTime rowEndTimestamp = rowStartTimestamp.AddSeconds(MediaTimeSeries.TimeBucketSeconds) - TimeSpan.FromMilliseconds(1);

         if (timestamp < rowStartTimestamp || timestamp > rowEndTimestamp)
         {
            // string formatDateTime = "yyMMdd HH:mm:ss.fff";
            // throw new Exception($"The time series table row is not correct - the timesstamp is {rowStartTimestamp.ToString(formatDateTime)}.");

            return null;
         }

         return row;
      }


      /// <summary>
      /// Commits changes to table rows, which have been made since the last Commit or Reject.
      /// </summary>
      private void CommitRowChanges()
      {
         timeSeriesTable.AcceptChanges();
      }


      /// <summary>
      /// Rejects changes to table rows, which have been made since the last Commit or Reject.
      /// </summary>
      private void RejectRowChanges()
      {
         timeSeriesTable.RejectChanges();
      }
   }

   public class MediaStream: IDisposable
   {
      private string streamName = string.Empty;
      private MediaType mediaType;
      private StreamDirection streamDirection;

      private MediaStreamData mediaStreamData;

      private List<MediaStreamEvent> mediaStreamEvents = new List<MediaStreamEvent>();

      public enum StreamDirection { Incoming, Outgoing };
      public enum MediaType { Audio, Video };

      private double packetLossReportingThreshold = 10;    // 10 percent

      /// <summary>
      /// Initializes a new instance of the <see cref="MediaStream"/> class.
      /// </summary>
      /// <param name="type">The stream media type.</param>
      /// <param name="direction">The direction of the stream.</param>
      public MediaStream(MediaType type, StreamDirection direction)
      {
         this.mediaType = type;
         this.streamDirection = direction;
         this.streamName = $"{this.mediaType.ToString()}-{this.streamDirection.ToString()}";

         this.mediaStreamData = new MediaStreamData();
      }


      /// <summary>
      /// Destructor
      /// </summary>
      ~MediaStream()
      {
         // Console.WriteLine("~MediaStream");

         this.Dispose();
      }


      /// <summary>
      /// Dispose of memory assets
      /// </summary>
      public void Dispose()
      {
         if (this.mediaStreamData != null)
         {
            this.mediaStreamData = null;
         }

         if (this.mediaStreamEvents != null)
         {
            this.mediaStreamEvents.Clear();
            this.mediaStreamEvents = null;
         }
      }


      /// <summary>
      /// Log quantity of data bytes transferred.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <param name="count">number of bytes transferred</param>
      public void LogDataTransferred(DateTime timestamp, long count)
      {
         this.mediaStreamData.AccumulateBytesTransferred(timestamp, count);
      }


      /// <summary>
      /// Log quantity of packets lost and network jitter.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <param name="lostPackets">the number of packets lost</param>
      /// <param name="goodPackets">the number of packets received correctly</param>
      /// <param name="jitterMsec">jitter in Msec</param>
      public void LogDataTransferErrors(DateTime timestamp, long lostPackets, long goodPackets, int jitterMsec)
      {
         this.mediaStreamData.AccumulateTransferErrors(timestamp, lostPackets, goodPackets, jitterMsec);

         if (this.mediaStreamData.MovingAveragePacketLossPercentage > this.packetLossReportingThreshold)
         {
            this.LogStreamEvent(timestamp, "packet-loss-excessive", $"loss {this.mediaStreamData.MovingAveragePacketLossPercentage}% exceeds {this.packetLossReportingThreshold}%.  Peak loss {this.mediaStreamData.MovingAveragePeakPacketLossPercentage}%.", 1);
         }
      }


      /// <summary>
      /// Logs a stream event for later analysis.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <param name="tag"></param>
      /// <param name="description"></param>
      /// <param name="count"></param>
      /// <returns>A MediaStreamEvent.</returns>
      public MediaStreamEvent LogStreamEvent(DateTime timestamp, string tag, string description, long count)
      {
         MediaStreamEvent streamEvent = new MediaStreamEvent(timestamp, tag, description, count);

         // don't log high-volume events that are already captured in the SipSession TimeSeries buckets
         // but log everything else needed for the descriptive timeline output to the log file

         switch (tag)
         {
            case "audio-sent":
            case "video-sent":
            case "audio-received":
            case "video-received":
            case "video-frame-cropped":
               break;

            default:
               this.mediaStreamEvents.Add(streamEvent);
               break;
         }

         return streamEvent;
      }


      /// <summary>
      /// Gets the moving average transfer rate in bytes/second.
      /// </summary>
      public double MovingAverageBps { get { return this.mediaStreamData.MovingAverageTransferRateBps; } }


      /// <summary>
      /// Gets the printable name of the stream.
      /// </summary>
      public string StreamName { get { return this.streamName; } }

      /// <summary>
      /// Gets the stream type.
      /// </summary>
      public string StreamType { get { return this.mediaType.ToString(); } }

      /// <summary>
      /// Gets the stream direction.
      /// </summary>
      public string Direction { get { return this.streamDirection.ToString(); } }

      /// <summary>
      /// Gets or a sets the value of the endpoint address.
      /// </summary>
      public string EndpointAddress { get; set; }

      /// <summary>
      /// Gets or sets the value of the control port.
      /// </summary>
      public string ControlPort { get; set; }

      /// <summary>
      /// Gets or sets the value of the port.
      /// </summary>
      public string Port { get; set; }

      /// <summary>
      /// Gets a value of the number of incoming packets lost.
      /// </summary>
      public long PacketsLost { get { return this.mediaStreamData.TotalLostPackets; } }

      /// <summary>
      /// Gets a value of the number of incoming packets received correctly.
      /// </summary>
      public long PacketsReceived { get { return this.mediaStreamData.TotalGoodPackets; } }

      /// <summary>
      /// Gets the maximum jitter reported, in msec.
      /// </summary>
      public long MaximumJitterMsec { get { return this.mediaStreamData.MaximumJitterMsec; } }

      /// <summary>
      /// Gets the moving average packet loss percent, calculated as the quantity lost divided by the number received 
      /// over a time interval.
      /// </summary>
      /// <returns>0-100</returns>
      public double MovingAveragePacketLoss
      {
         get
         {
            return this.mediaStreamData.MovingAveragePacketLossPercentage;
         }
      }

      /// <summary>
      /// Gets the average packet loss percent, calculated as the quantity lost divided by the number received.
      /// </summary>
      /// <returns>0-100</returns>
      public double AveragePacketLoss
      {
         get
         {
            return this.mediaStreamData.TotalPacketLossPercentage;
         }
      }

      /// <summary>
      /// Gets a list of the raw error reports.
      /// </summary>
      public List<MediaStreamEvent> RawMediaStreamEvents { get { return this.mediaStreamEvents; } }


      /// <summary>
      /// Gets the media stream data object.
      /// </summary>
      public MediaStreamData MediaStreamData { get { return this.mediaStreamData; } }


      /// <summary>
      /// Gets a formatted list of media stream events and counts
      /// </summary>
      /// <param name="summarizeConsecutive">true if adjacent messages with identical tag values should be summarized in one line with a time range</param>
      public List<string> MediaStreamEventDescriptions(bool summarizeConsecutive)
      {
         List<string> eventSummaries = new List<string>();

         DataTable eventsTable = new DataTable("events");
         eventsTable.Columns.Add(new DataColumn("tag", typeof(string)));
         eventsTable.Columns.Add(new DataColumn("description", typeof(string)));
         eventsTable.Columns.Add(new DataColumn("count", typeof(long)));
         eventsTable.Columns.Add(new DataColumn("earliest", typeof(DateTime)));
         eventsTable.Columns.Add(new DataColumn("latest", typeof(DateTime)));


         bool summarizeCounts = false;   // group and count all of the errors by description
         bool isConsecutive = false;
         DataRow previousRow = null;

         foreach (MediaStreamEvent @event in this.RawMediaStreamEvents)
         {
            // returns 0 or more rows having the same tag
            DataRow[] result = eventsTable.Select($"tag = '{@event.Tag}'");

            if (previousRow != null && @event.Tag != previousRow.Field<string>("tag"))
            {
               previousRow = null;
               isConsecutive = false;
            }

            // if summarizing counts, there will only be one row in the result having that description
            if (result.Length == 1 && (summarizeCounts || (summarizeConsecutive && isConsecutive)))
            {
               // one output row per unique description - update the count and dates there

               DataRow row = result[0];
               row["count"] = row.Field<long>("count") + @event.Count;

               if (@event.Timestamp < row.Field<DateTime>("earliest"))
               {
                  row["earliest"] = @event.Timestamp;
               }

               if (@event.Timestamp > row.Field<DateTime>("latest"))
               {
                  row["latest"] = @event.Timestamp;
               }
            }

            // summarizing input rows having the same description, there will be 1 or more rows in the result
            // but we only want to update the one nearest the end of the list
            else if (result.Length > 1 && summarizeConsecutive && isConsecutive)
            {
               // choose the last one
               previousRow = result[result.Length - 1];

               previousRow["count"] = previousRow.Field<long>("count") + @event.Count;

               if (@event.Timestamp < previousRow.Field<DateTime>("earliest"))
               {
                  previousRow["earliest"] = @event.Timestamp;
               }

               if (@event.Timestamp > previousRow.Field<DateTime>("latest"))
               {
                  previousRow["latest"] = @event.Timestamp;
               }
            }

            else
            {
               // add a new description entry
               eventsTable.Rows.Add(@event.Tag, @event.Description, @event.Count, @event.Timestamp, @event.Timestamp);

               previousRow = eventsTable.Rows[eventsTable.Rows.Count - 1];
               isConsecutive = true;
            }
         }

         eventsTable.AcceptChanges();
         eventsTable.DefaultView.Sort = "earliest ASC, description ASC";

         string formatTime = "HH:mm:ss.fff";

         DateTime previousTimestamp = DateTime.MinValue;

         // scan the sorted rows

         foreach (DataRow row in eventsTable.Rows)
         {
            // 17:18:04.845 to 17:18:35.805 (count) tag                          description

            // pad the count to 5 digit positions
            string countText = row.Field<long>("count").ToString("####0");
            if (countText == "1")
            {
               countText = string.Empty;
            }

            if (countText.Length < 5)
            {
               countText = new string(' ', 5 - countText.Length) + countText;
            }

            // pad the tag to 30 chars
            string tagText = row.Field<string>("tag");
            if (tagText.Length < 30)
            {
               tagText += new string(' ', 30 - tagText.Length);
            }

            string formattedLine = $"{countText} {tagText} {row.Field<string>("description")}";

            if (row.Field<DateTime>("earliest") == row.Field<DateTime>("latest"))
            {
               if (row.Field<DateTime>("earliest") == previousTimestamp)
               {
                  eventSummaries.Add($"                             {formattedLine}");
               }
               else
               {
                  eventSummaries.Add($"{row.Field<DateTime>("earliest").ToString(formatTime)}                 {formattedLine}");
               }
            }
            else
            {
               eventSummaries.Add($"{row.Field<DateTime>("earliest").ToString(formatTime)} to {row.Field<DateTime>("latest").ToString(formatTime)} {formattedLine}");
            }

            previousTimestamp = row.Field<DateTime>("earliest");
         }
         
         eventsTable.Dispose();
         eventsTable = null;

         return eventSummaries;
      }
   }
}

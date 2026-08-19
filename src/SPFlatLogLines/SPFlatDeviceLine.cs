using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// A decoded flat record for a device that has its own per-device flat view (IPM/IDC/PIN).
   ///
   /// Rather than a per-message line subclass (as CDM/CIM use), a device line simply carries the
   /// generic SPFlatRecord; the device's table filters by Record.Device and routes on Record.Method /
   /// Record.Payload. Requires the device-attributing framing in SPFlatLogHandler (a line must carry
   /// its own 0003&lt;DEV&gt;0007ACTIVEX envelope for Record.Device to be reliable).
   /// </summary>
   public class SPFlatDeviceLine : SPFlatLine
   {
      public SPFlatRecord Record { get; private set; }

      public SPFlatDeviceLine(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.Device)
         : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         Record = SPFlatRecord.Decode(logLine);
      }

      public string Device { get { return Record != null ? Record.Device : "?"; } }
      public string DeviceMethod { get { return Record != null ? Record.Method : ""; } }
      public string DevicePayload { get { return Record != null ? Record.Payload : ""; } }
   }
}

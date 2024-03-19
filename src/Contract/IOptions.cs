using System;

namespace Contract
{
   public enum ParseType
   {
      AP, AT, AE, AW, AV, SP, RT, SS, BE, II
   }

   public interface IOptions
   {
      bool IsAP { get; }
      bool IsAT { get; }
      bool IsAE { get; }
      bool IsAW { get; }
      bool IsAV { get; }
      bool IsSP { get; }
      bool IsRT { get; }
      bool IsII { get; }
      bool IsSS { get; }
      bool IsBE { get; }

      bool RunView(ParseType parseType, string viewName);

      string APViews { get; set; }
      string ATViews { get; set; }
      string AEViews { get; set; }
      string AWViews { get; set; }
      string AVViews { get; set; }
      string SPViews { get; set; }
      string RTViews { get; set; }
      string IIViews { get; set; }
      string SSViews { get; set; }
      string BEViews { get; set; }
      bool RawLogLine { get; set; }

      string InputFile { get; set; }
      string Suffix();

      DateTime StartTime { get; set; }
      DateTime EndTime { get; set; }
   }
}

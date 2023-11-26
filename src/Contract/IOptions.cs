namespace Contract
{
   public enum ParseType
   {
      AP, AT, AW, SP, RT
   }

   public interface IOptions
   {
      bool IsAP { get; }
      bool IsAT { get; }
      bool IsAW { get; }
      bool IsSP { get; }
      bool IsRT { get; }

      bool RunView(ParseType parseType, string viewName);

      string APViews { get; set; }
      string ATViews { get; set; }
      string AWViews { get; set; }
      string SPViews { get; set; }
      string RTViews { get; set; }
      string InputFile { get; set; }

      string Suffix(); 
   }
}

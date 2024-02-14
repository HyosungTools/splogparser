namespace Contract
{
   public enum ParseType
   {
      AP, AT, AE, AW, SP, RT, SS
   }

   public interface IOptions
   {
      bool IsAP { get; }
      bool IsAT { get; }
      bool IsAE { get; }
      bool IsAW { get; }
      bool IsSP { get; }
      bool IsRT { get; }
      bool IsSS { get; }

      bool RunView(ParseType parseType, string viewName);

      string APViews { get; set; }
      string ATViews { get; set; }
      string AEViews { get; set; }
      string AWViews { get; set; }
      string SPViews { get; set; }
      string RTViews { get; set; }
      string SSViews { get; set; }
      string InputFile { get; set; }

      string Suffix(); 
   }
}

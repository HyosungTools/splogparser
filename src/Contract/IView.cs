namespace Contract
{
   public interface IView
   {

      /// Property to return the type of this View for display/logging purposes.
      ParseType parseType { get; }

      /// Property to return the name of this View for display/logging purposes.
      string Name { get; }

      /// <summary>
      /// Instruct the View to prepare for Time Series processing 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Initialize(IContext ctx);

      /// <summary>
      /// Instruct the view to process this line 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Process(ILogFileHandler logFileHandler);

      /// <summary>
      /// Instruct the View TimeSeries processing is over. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PostProcess(IContext ctx);

      /// <summary>
      /// Instruction to the view to write out its datatable to Excel. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void WriteExcel(IContext ctx);

      /// <summary>
      /// Instruction to the view to cleanup. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Cleanup(IContext ctx);
   }
}

using System.Data;

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
      /// Gets the collection of tables for the dataset 
      /// </summary>
      /// <returns></returns>
      DataSet GetDataSet();

      /// <summary>
      /// Instruct the view to process this line 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PreProcess(IContext ctx);

      /// <summary>
      /// Instruct the view to process this line 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Process(IContext ctx);

      /// <summary>
      /// Instruct the View TimeSeries processing is over. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PostProcess(IContext ctx);

      /// <summary>
      /// Preparation for analyze 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PreAnalyze(IContext ctx);

      /// <summary>
      /// Run the analyze Views 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Analyze(IContext ctx);

      /// <summary>
      /// Run post-analyze steps.  
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PostAnalyze(IContext ctx);

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

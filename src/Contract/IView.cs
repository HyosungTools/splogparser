namespace Contract
{ 
    public interface IView
    {

        /// Property to return the type of this View for display/logging purposes.
        string ViewType { get; }

        /// Property to return the name of this View for display/logging purposes.
        string Name { get; }

        /// <summary>
        /// Instruction to the view to process the merged log file. 
        /// </summary>
        /// <param name="ctx">Context for the command</param>
        void Process(IContext ctx);

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
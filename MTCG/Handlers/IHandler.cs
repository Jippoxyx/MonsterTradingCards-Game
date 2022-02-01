namespace MTCG.Handlers {

    // All paths need to implement the handler interface
    internal interface IHandler {

        /// <summary>
        /// processes task
        /// </summary>
        /// <returns>answer to handle task</returns>
        public string Handle();
    }
}
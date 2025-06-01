namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// Generic response wrapper class used for service methods.
    /// </summary>
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }

        public Response(string errorMessage, object returnValue)
        {
            ErrorMessage = errorMessage;
            ReturnValue = returnValue;
        }
    }
}
namespace Backend.ServiceLayer
{
    /// <summary>
    /// Generic response wrapper class used for service methods.
    /// </summary>
    /// <typeparam name="T">The return type of the service method.</typeparam>
    public class Response
    {
        public string ErrorMsg { get; set; }
        public object RetVal { get; set; }

        public Response(string errorMsg, object retVal) {
            ErrorMsg = errorMsg;
            RetVal = retVal;
        }
    }
}
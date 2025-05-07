namespace Backend.ServiceLayer
{
    /// <summary>
    /// Generic response wrapper class used for service methods.
    /// </summary>
    /// <typeparam name="T">The return type of the service method.</typeparam>
    public class Response
    {
        public string ErrorMsg { get; init; }
        public object RetVal { get; init; }

        public Response() { }
        public Response(string errorMsg, object retVal) {
            ErrorMsg = errorMsg;
            RetVal = retVal;
        }
        public Response(object retVal)
        {
            RetVal = retVal;
        }
        public Response(string errorMsg, bool flag=false)
        {
            if (!flag)
                ErrorMsg = errorMsg;
            else RetVal = errorMsg;
        }
    }
}
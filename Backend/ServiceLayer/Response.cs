namespace Backend.ServiceLayer
{
    /// <summary>
    /// Generic response wrapper class used for service methods.
    /// </summary>
    /// <typeparam name="T">The return type of the service method.</typeparam>
    public class Response<T>
    {
        public string? ErrorMsg { get; set; }
        public T? RetVal { get; set; }

        public Response(string? errorMsg, T? retVal) {
            ErrorMsg = errorMsg;
            RetVal = retVal;
        }
    }
}
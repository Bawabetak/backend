namespace bawabetak_backend.Exceptions
{
    public abstract class BaseException : Exception
    {
        public ResponseKeys ResponseKey { get; }
        public int StatusCode { get; }

        protected BaseException(ResponseKeys responseKey, int statusCode = StatusCodes.Status400BadRequest)
            : base(ResponseMesages.GetMessage(responseKey))
        {
            ResponseKey = responseKey;
            StatusCode = statusCode;
        }
    }
}

namespace bawabetak_backend.Helpers.Static
{
    public static class ResponseMesages
    {
        public static readonly Dictionary<ResponseKeys, string> Messages = new Dictionary<ResponseKeys, string>
        {
            [ResponseKeys.InternalServerError] = "Internal server error.",
            [ResponseKeys.TooManyRequests] = "Too many requests. Please try again later.",
            [ResponseKeys.success] = "Request processed successfully."



        };
        public static string GetMessage(ResponseKeys key)
        {
            return Messages.ContainsKey(key) ? Messages[key] : "Unknown response key.";
        }
    }
}

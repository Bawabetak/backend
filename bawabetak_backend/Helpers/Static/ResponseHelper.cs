
namespace bawabetak_backend.Helpers.Static
{
    public static class ResponseHelper
    {
        public static ApiResponse Success(ResponseKeys responseKey, dynamic data = default!)
        {
            return new ApiResponse(true,ResponseMesages.GetMessage(responseKey) , data);
        }
        public static ApiResponse Error(ResponseKeys responseKey, dynamic data = default!)
        {
            return new ApiResponse(false,ResponseMesages.GetMessage(responseKey) , data);
        }
    }
}

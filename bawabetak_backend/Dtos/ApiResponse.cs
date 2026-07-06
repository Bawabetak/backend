namespace bawabetak_backend.Dtos
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = default!;
        public dynamic Data { get; set; } = default!;

        public ApiResponse(bool Success, string Message = default!, dynamic Data = default!)
        {
            this.Success = Success;
            this.Message = Message;
            this.Data = Data;
        }
    }
}

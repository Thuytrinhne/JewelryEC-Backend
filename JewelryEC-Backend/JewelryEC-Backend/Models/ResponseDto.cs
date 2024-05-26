namespace JewelryEC_Backend.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public object? Result { get; set; }
        public ResponseDto()
        {
        }
        public ResponseDto(bool isSuccess, string  errorMessages)
        {
            IsSuccess = isSuccess;
            Message = errorMessages;
        }
        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}

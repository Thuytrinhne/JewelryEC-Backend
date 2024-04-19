namespace JewelryEC_Backend.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public string? Message { get; set; }
        public object? Result { get; set; }
        public ResponseDto()
        {
            ErrorMessages = new List<string>();
        }
        public ResponseDto(bool isSuccess, List<string> errorMessages)
        {
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
        }
        public ResponseDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}

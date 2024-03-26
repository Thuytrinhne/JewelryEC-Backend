namespace JewelryEC_Backend.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object? Result { get; set; }
        public ResponseDto()
        {
            ErrorMessages = new List<string>();
        }

    }
}

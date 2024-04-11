using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Core.Utilities.Results
{
    public class ErrorResult: ResponseDto
    {
        public ErrorResult(string message) : base(false)
        {
            base.ErrorMessages.Add(message);
        }

        public ErrorResult() : base(false)
        {

        }

    }
}

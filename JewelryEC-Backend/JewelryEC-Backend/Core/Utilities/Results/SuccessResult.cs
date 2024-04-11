using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Core.Utilities.Results
{
    public class SuccessResult: ResponseDto
    {
        public SuccessResult(string message) : base(true)
        {
            base.ErrorMessages.Add(message);
        }

        public SuccessResult() : base(true)
        {

        }

    }
}

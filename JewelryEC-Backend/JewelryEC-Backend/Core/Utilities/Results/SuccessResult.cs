using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Core.Utilities.Results
{
    public class SuccessResult: ResponseDto
    {
        public SuccessResult(string message) : base(true)
        {
            base.Message = message;
        }

        public SuccessResult(string message, object result) : base(true)
        {
            base.Result = result;
            base.Message = message;
        }
        public SuccessResult( object result) : base(true)
        {
            base.Message = "Request successfully";
            base.Result = result;
        }
        public SuccessResult() : base(true)
        {
            base.Result = "Request successfully";
        }
    }
}

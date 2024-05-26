using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models;

namespace JewelryEC_Backend.Core.Utilities.Results
{
    public class SuccessDataResult<T> : ResponseDto where T : class, new()
    {
        public SuccessDataResult(T data, string message) : base(true)
        {
            base.Message = message;
            base.Result = data;
        }

        public SuccessDataResult(T data) : base(true)
        {
            base.Result = data;
        }
    }
}


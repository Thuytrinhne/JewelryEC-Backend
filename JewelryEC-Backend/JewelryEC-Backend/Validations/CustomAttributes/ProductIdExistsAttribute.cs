using JewelryEC_Backend.UnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Validations.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ProductIdExistsAttribute : ValidationAttribute
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductIdExistsAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
      

            if (value != null)
            {
                var productFrmDb = _unitOfWork.Products.GetProduct((Product => Product.Id == (Guid.Parse(value.ToString()))));
                if(productFrmDb == null)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
          
            }
            return ValidationResult.Success;
        }
    }
}

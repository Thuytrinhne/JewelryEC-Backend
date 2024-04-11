using JewelryEC_Backend.Core.DataAccess.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace JewelryEC_Backend.DataAccess.Concrete
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category>, ICategoryDal
    {
        public EfCategoryDal(AppDbContext context) : base(context)
        {
        }
    }
    
}

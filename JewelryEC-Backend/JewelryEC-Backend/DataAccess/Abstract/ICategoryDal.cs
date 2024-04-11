using JewelryEC_Backend.Core.DataAccess;
using JewelryEC_Backend.Models.Categories;

namespace JewelryEC_Backend.DataAccess.Abstract
{
    public interface ICategoryDal: IEntityRepository<Category>
    {
    }
}

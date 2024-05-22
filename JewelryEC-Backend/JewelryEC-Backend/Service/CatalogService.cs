using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace JewelryEC_Backend.Service
{
    public class CatalogService : ICatalogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Guid CreateCatalog(Catalog catalogToCreate)
        {
            try
            {
                _unitOfWork.Catalogs.Add(catalogToCreate);
                _unitOfWork.Save();
                return catalogToCreate.Id;
            }catch (Exception ex)
            {
                throw new Exception($"Could not create catalog {catalogToCreate.Id}");
            }

        }

        public bool DeleteCatalog(Guid id)
        {
            try
            {
                Catalog obj = GetCatalogById(id);
                if (obj != null)
                {
                    _unitOfWork.Catalogs.Remove(obj);
                    _unitOfWork.Save();
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not delete catalog {id}");
            }
        }
        public IEnumerable<Catalog> FilterCatalogs(Guid? parentId = null, string name = null)
        {
            IEnumerable<Catalog> catalogs;
            if (parentId.HasValue && !string.IsNullOrEmpty(name))
            {
                catalogs = _unitOfWork.Catalogs.Find(c => c.ParentId == parentId && c.Name == name.Trim());
            }
            else if (parentId.HasValue)
            {
                catalogs = _unitOfWork.Catalogs.Find(c => c.ParentId == parentId);
            }
            else if (!string.IsNullOrEmpty(name))
            {

                catalogs = _unitOfWork.Catalogs.Find(c => c.Name == name.Trim());
            }
            else
                catalogs= ListCatalogs();
            return catalogs;

        }

        public Catalog GetCatalogById(Guid id)
        {

            try
            {
                return _unitOfWork.Catalogs.GetById(id);
              
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, ví dụ: ghi log hoặc trả về một giá trị mặc định
                throw new Exception($"Could not found catalog {id}");
            }
        }

        public async Task<PaginationResult<Catalog>> GetCatalogsByPage(PaginationRequest request)
        {
            return await _unitOfWork.Catalogs.GetCatalogsByPage(request);
        }

        public IEnumerable<Catalog> ListCatalogs()
        {
            return  _unitOfWork.Catalogs.GetAll();   
        }

        public bool UpdateCatalog(Catalog catalogToUpdate)
        {
            if (catalogToUpdate == null)
            {
                throw new ArgumentNullException("The catalog to update cannot be null");
            }

            try
            {
                var catalogFrmDb = GetCatalogById(catalogToUpdate.Id);
                if (catalogFrmDb != null)
                {
                    _unitOfWork.Catalogs.Update(catalogToUpdate);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update catalog {catalogToUpdate.Id}", ex);
            }
        }

        
    }
}

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

        public bool CreateCatalog(Catalog catalogToCreate)
        {
            try
            {
                _unitOfWork.Catalogs.Add(catalogToCreate);
                _unitOfWork.Save();
            }catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteCatalog(Guid id)
        {
            try
            {
                Catalog obj = GetCatalogById(id);
                _unitOfWork.Catalogs.Remove(obj);
                _unitOfWork.Save();
            }catch(Exception ex)
            {
                return false;
            }
            return true;
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
             return _unitOfWork.Catalogs.GetById(id);
        }

        public IEnumerable<Catalog> ListCatalogs()
        {
            return  _unitOfWork.Catalogs.GetAll();   
        }

        public bool UpdateCatalog(Catalog catalogToUpdate)
        {
            try
            {
               _unitOfWork.Catalogs.Update(catalogToUpdate);
               _unitOfWork.Save();
            }catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}

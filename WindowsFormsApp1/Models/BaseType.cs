using PMPK.DAL;
using System.Collections.Generic;
using System.Linq;

namespace PMPK.Models
{
    public abstract class BaseType
    {
        public abstract int Id { get; set; }

        //Получает список указанных связанных данных без обращения к БД
        public IEnumerable<TEntity> GetPropertiesOfEntity<TEntity>(GenericRepository<TEntity> entity, string property) where TEntity : class
        {
            return entity.Get(includeProperties: property).ToList();
        }
        //Получает первое существо по указанным связанным данным без обращения к БД
        public TEntity GetFirstPropertyOfEntity<TEntity>(GenericRepository<TEntity> entity, string property) where TEntity : BaseType
        {
            return GetPropertiesOfEntity(entity, property).Where(x => x.Id == Id).FirstOrDefault();
        }
    }
}

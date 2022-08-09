using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PMPK.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal PMPKContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(PMPKContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }
       
        //Получить данные из БД со связанными данными(перегрузка метода)
        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            IQueryable<TEntity> entities = DbSet;
            return entities.Select(selector).ToList();
        }

        //Получить данные из БД со связанными данными(перегрузка метода)
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""){
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);

            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
       
        //Получить сущность из БД по id(перегрузка метода)
        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }
       
        //Добавить сущность в БД
        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        //Получить количество сущностей в репозитории
        public virtual int Count()
        {
            return DbSet.CountAsync().Result;
        }
       
        public virtual TEntity FirstOrDefault()
        {
            return DbSet.FirstOrDefaultAsync().Result;
        }

        //Удалить сущность из БД по id(перегрузка метода)
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        //Удалить сущность из БД по id(перегрузка метода)
        public virtual void Delete(TEntity entityToDelete)
        {

            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }

        //Перезагрузить сущность из БД по id
        public virtual void Reload(TEntity entity)
        {
            Context.Entry(entity).Reload();
        }

        //Обновить сущность в БД по id
        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

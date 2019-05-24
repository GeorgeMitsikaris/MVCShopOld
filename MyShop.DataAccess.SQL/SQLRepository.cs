using MyShop.Core;
using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DataContext _context;
        private DbSet<T> dbSet;

        public SQLRepository(DataContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var t = Find(id);
            if (_context.Entry<T>(t).State == EntityState.Detached)
                dbSet.Attach(t);

            dbSet.Remove(t);
        }

        public T Find(string id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            _context.Entry<T>(t).State = EntityState.Modified;
        }
    }
}

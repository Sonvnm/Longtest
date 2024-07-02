using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reposiotry.Models;
namespace Reposiotry
{
    public class Repositorybase<T> where T : class
    {
        private readonly BankAccountTypeContext _context;
        private readonly DbSet<T> _dbSet;

        public Repositorybase()
        {
            _context = new BankAccountTypeContext();
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ECart.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private ECartEntities _context;

        public UnitOfWork(ECartEntities context)
        {
            _context = context;
        }

        public void Add<T>(T obj)
            where T : class
        {
            var set = _context.Set<T>();
            set.Add(obj);
        }
        //ExceptionChange
        public void Update<T>(T obj)
            where T : class
        {
            var set = _context.Set<T>();
            set.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        void IUnitOfWork.Remove<T>(T obj)
        {
            var set = _context.Set<T>();
            set.Remove(obj);
        }

        void IUnitOfWork.Remove<T>(List<T> obj)
        {
            var set = _context.Set<T>();
            set.RemoveRange(obj);
        }

        public IQueryable<T> Query<T>()
            where T : class
        {
            return _context.Set<T>();
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<Boolean> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Attach<T>(T newUser) where T : class
        {
            var set = _context.Set<T>();
            set.Attach(newUser);
        }
        public void Detach<T>(T newUser) where T : class
        {
            _context.Entry(newUser).State = EntityState.Detached;
        }
        public void Dispose()
        {
            _context = null;
        }

        public DbSet<T> GetEntities<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Rollback()
        {
            _context.Dispose();
        }
    }
}
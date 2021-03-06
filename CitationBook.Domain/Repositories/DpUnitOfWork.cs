using System;
using System.Data;
using System.Data.SqlClient;
using CitationBook.Domain.Entities;
using CitationBook.Domain.Interfaces;

namespace CitationBook.Domain.Repositories
{
    public class DpUnitOfWork : IUnitOfWork
    {
        private IDbConnection db;
        private CitationRepository citationRepository;
        private CategoryRepository categoryRepository;
        

        public DpUnitOfWork(string connectionString)
        {
            db = new SqlConnection(connectionString);
        }
        
        public IRepository<Citation> Citations
        {
            get
            {
                if (citationRepository == null)
                    citationRepository = new CitationRepository(db);
                return citationRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }

        #region Pattern Disposable
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
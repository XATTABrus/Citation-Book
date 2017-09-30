using System;
using CitationBook.Domain.Entities;

namespace CitationBook.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Citation> Citations { get; }
        IRepository<Category> Categories { get; }
        
    }
}
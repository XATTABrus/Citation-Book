using System;
using System.Collections.Generic;

namespace CitationBook.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllElements();
        T GetElementById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
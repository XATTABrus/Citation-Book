using System;
using System.Collections.Generic;
using CitationBook.Domain.Entities;
using CitationBook.Domain.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace CitationBook.Domain.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IDbConnection _dbConnection;
        public CategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Category> GetAllElements()
        {
            return _dbConnection.Query<Category>("SELECT * FROM Categories");
        }

        public Category GetElementById(int id)
        {
            return _dbConnection.Query<Category>("SELECT * FROM Categories WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Create(Category item)
        {
            var sqlQuery = "INSERT INTO Categories(Name) VALUES(@Name); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            int? itmeId = _dbConnection.Query<int>(sqlQuery, item).FirstOrDefault();
            item.Id = itmeId.Value;
        }

        public void Update(Category item)
        {
            var sqlQuery = "UPDATE Categories SET " +
                            "Name = @Name " +
                            "WHERE Id = @Id";

            _dbConnection.Execute(sqlQuery, item);
        }

        public void Delete(int id)
        {
            var sqlQuery = "DELETE FROM Categories WHERE Id = @id";
            _dbConnection.Execute(sqlQuery, new { id });
        }
    }
}
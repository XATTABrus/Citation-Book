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
    public class CitationRepository : IRepository<Citation>
    {
        private readonly IDbConnection _dbConnection;

        public CitationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Citation> GetAllElements()
        {
            return _dbConnection.Query<Citation>("SELECT * FROM Citations");
        }

        public Citation GetElementById(int id)
        {
            return _dbConnection.Query<Citation>("SELECT * FROM Citations WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Create(Citation item)
        {
            var sqlQuery = "INSERT INTO Citations " +
                            "(Text, Author, CreateDate, CategoryId) " +
                            "VALUES(@Text, @Author, @CreateDate, @CategoryId); " +
                            "SELECT CAST(SCOPE_IDENTITY() as int)";

            int? itmeId = _dbConnection.Query<int>(sqlQuery, item).FirstOrDefault();
            item.Id = itmeId.Value;
        }

        public void Delete(int id)
        {
            var sqlQuery = "DELETE FROM Citations WHERE Id = @id";
            _dbConnection.Execute(sqlQuery, new { id });
        }

        public void Update(Citation item)
        {
            var sqlQuery = "UPDATE Citations SET " +
                            "Text = @Text, Author = @Author, " +
                            "CategoryId = @CategoryId " +
                            "WHERE Id = @Id";

            _dbConnection.Execute(sqlQuery, item);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CitationBook.Domain.Repositories;
using CitationBook.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using CitationBook.Domain.Interfaces;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CitationBook.Tests.IntegrationTests
{
    [TestClass]
    public class CitationRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        public CitationRepositoryTests()
        {
            var connectionString = Settings.GetConnectionString();
            _unitOfWork = new DpUnitOfWork(connectionString);
        }

        [TestMethod]
        public void CreateCitationTest()
        {
            var category1 = new Category{Id = 0, Name = "Test1"};
            var category2 = new Category{Id = 0, Name = "Test2"};
            
            _unitOfWork.Categories.Create(category1);
            _unitOfWork.Categories.Create(category2);

            var citation1 = new Citation
            {
                Author = "Author1", 
                CreateDate = DateTime.Now,
                Text = "TestText1",
                CategoryId = category1.Id
            };

            var citation2 = new Citation
            {
                Author = "Author2", 
                CreateDate = DateTime.Now,
                Text = "TestText2",
                CategoryId = category2.Id
            };

            _unitOfWork.Citations.Create(citation1);
            _unitOfWork.Citations.Create(citation2);            
            
            Assert.AreNotEqual(0, citation1.Id);
            Assert.AreNotEqual(0, citation2.Id);
            Assert.AreNotEqual(citation1.Id, citation2.Id);
        }

        [TestMethod]
        public void GetAllCitation()
        {
            var countCitation = _unitOfWork.Citations.GetAllElements();

            Assert.AreEqual(2, countCitation.Count());
        }

        [TestMethod]
        public void GetCitationByIdTest()
        {
            var citations = _unitOfWork.Citations.GetAllElements();
            var citation = citations.FirstOrDefault();
            var citationDb = _unitOfWork.Citations.GetElementById(citation.Id);

            Assert.AreEqual(citation.Id, citationDb.Id);
        }

        [TestMethod]
        public void UpdateCitationTest()
        {
            var citations = _unitOfWork.Citations.GetAllElements();
            var citationOld = citations.FirstOrDefault();
            
            citationOld.Author = "Author bla bla bla";
            citationOld.Text = "Text bla bla bla";
            citationOld.CategoryId = _unitOfWork.Categories.GetAllElements().LastOrDefault().Id;
            citationOld.CreateDate = DateTime.Now;

            _unitOfWork.Citations.Update(citationOld);

            var citationNew = _unitOfWork.Citations.GetElementById(citationOld.Id);

            Assert.AreEqual(citationOld.Author, citationNew.Author);
            Assert.AreEqual(citationOld.Text, citationNew.Text);
            Assert.AreEqual(citationOld.CategoryId, citationNew.CategoryId); 
            Assert.AreNotEqual(citationOld.CreateDate, citationNew.CreateDate);                                                  
        }

        [TestMethod]
        public void ExceptionDeleteCategoryWithCitationTest()
        {
            var category = _unitOfWork.Categories.GetAllElements().LastOrDefault();
            
            Assert.ThrowsException<SqlException>(() => {_unitOfWork.Categories.Delete(category.Id);});
        }

        [TestMethod]
        public void DeleteCitationTest()
        {
            var citationsBeforeDelete = _unitOfWork.Citations.GetAllElements();
            foreach (var item in citationsBeforeDelete)
            {
                _unitOfWork.Citations.Delete(item.Id);
            }

            var categoriesBeforeDelete = _unitOfWork.Categories.GetAllElements();
            foreach (var item in categoriesBeforeDelete)
            {
                _unitOfWork.Categories.Delete(item.Id);
            }

            var citationsAfterDelete = _unitOfWork.Citations.GetAllElements();            
            var categoriesAfterDelete = _unitOfWork.Categories.GetAllElements();

            Assert.AreEqual(0, citationsAfterDelete.Count());
            Assert.AreEqual(0, categoriesAfterDelete.Count());            
        }
    }
}
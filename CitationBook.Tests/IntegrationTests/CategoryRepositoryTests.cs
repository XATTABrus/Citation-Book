using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CitationBook.Domain.Repositories;
using CitationBook.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using CitationBook.Domain.Interfaces;

namespace CitationBook.Tests.IntegrationTests
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryRepositoryTests()
        {
            var connectionString = Settings.GetConnectionString();
            _unitOfWork = new DpUnitOfWork(connectionString);
        }

        [TestMethod]
        public void CreateCategoryTest()
        {
            var category1 = new Category{Id = 0, Name = "Test1"};
            var category2 = new Category{Id = 0, Name = "Test2"};
            
            _unitOfWork.Categories.Create(category1);
            _unitOfWork.Categories.Create(category2);
            
            Assert.AreNotEqual(0, category1.Id);
            Assert.AreNotEqual(0, category2.Id);
            Assert.AreNotEqual(category1.Id, category2.Id);
        }

        [TestMethod]
        public void GetAllCategory()
        {
            var countCategory = _unitOfWork.Categories.GetAllElements();

            Assert.AreEqual(2, countCategory.Count());
        }

        [TestMethod]
        public void GetCategoryByIdTest()
        {
            var categories = _unitOfWork.Categories.GetAllElements();
            var category = categories.FirstOrDefault();
            
            var categoryDb = _unitOfWork.Categories.GetElementById(category.Id);

            Assert.AreEqual(category.Id, categoryDb.Id);
        }

        [TestMethod]
        public void UpdateCategoryTest()
        {
            var categories = _unitOfWork.Categories.GetAllElements();
            var categoryOld = categories.FirstOrDefault();
            
            categoryOld.Name = "Bla bla bla";
            _unitOfWork.Categories.Update(categoryOld);

            var categoryNew = _unitOfWork.Categories.GetElementById(categoryOld.Id);

            Assert.AreEqual(categoryOld.Name, categoryNew.Name);            
        }

        [TestMethod]
        public void DeleteCategoriesTest()
        {
            var categoriesBeforeDelete = _unitOfWork.Categories.GetAllElements();
            foreach (var item in categoriesBeforeDelete)
            {
                _unitOfWork.Categories.Delete(item.Id);
            }

            var categoriesAfterDelete = _unitOfWork.Categories.GetAllElements();

            Assert.AreEqual(0, categoriesAfterDelete.Count());
        }
    }
}
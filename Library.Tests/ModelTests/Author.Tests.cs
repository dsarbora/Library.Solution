using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestClass]
    public class AuthorTest : IDisposable
    {
        public AuthorTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        public void Dispose()
        {
            Author.ClearAll();
            //Author.ClearAll();
            //Patron.ClearAll();
        }

        [TestMethod]
        public void Save_SavesAuthorToDatabase_True()
        {
            Author newAuthor = new Author("Herman Melville");
            newAuthor.Save();
            Author testAuthor = Author.GetAll()[0];
            Assert.AreEqual(newAuthor, testAuthor);
        }

        [TestMethod]
        public void Find_FindsAuthorById_Author()
        {
            Author newAuthor = new Author("Herman Melville");
            newAuthor.Save();
            int id = newAuthor.GetId();
            Author foundAuthor = Author.Find(id);
            Assert.AreEqual(newAuthor, foundAuthor);
        }

        [TestMethod]
        public void GetAll_GetsAllAuthors_AuthorList()
        {
            Author newAuthor = new Author("Herman Melville");
            Author testAuthor = new Author("R.L. Stine");
            newAuthor.Save();
            testAuthor.Save();
            List<Author> allAuthors = Author.GetAll();
            List<Author> testList = new List<Author>{newAuthor, testAuthor};
            CollectionAssert.AreEqual(allAuthors, testList);
        }

        [TestMethod]
        public void Update_UpdatesAuthorInDatabase_Author()
        {
            Author newAuthor = new Author("Herman Melville");
            newAuthor.Save();
            newAuthor.Update("R.L. Stine");

            Author foundAuthor = Author.GetAll()[0];
            Assert.AreEqual(newAuthor, foundAuthor);
        }
    }
}
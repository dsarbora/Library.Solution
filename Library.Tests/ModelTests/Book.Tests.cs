using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestClass]
    public class BookTest : IDisposable
    {
        public BookTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
        }

        public void Dispose()
        {
            Book.ClearAll();
            Author.ClearAll();
            //Patron.ClearAll();
        }

        [TestMethod]
        public void Save_SavesBookToDatabase_True()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            Book testBook = Book.GetAll()[0];
            Assert.AreEqual(newBook, testBook);
        }

        [TestMethod]
        public void AddCopies_AddsCopyOfBook_True()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            newBook.SetCopies(3);
            newBook.AddCopies();
            int numberOfCopies = newBook.GetAllCopies().Count;
            Assert.AreEqual (3, numberOfCopies);
        }

        [TestMethod]
        public void Find_FindsBookById_Book()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            int id = newBook.GetId();
            Book foundBook = Book.Find(id);
            Assert.AreEqual(newBook, foundBook);
        }

        [TestMethod]
        public void GetAll_GetsAllBooks_BookList()
        {
            Book newBook = new Book("Moby Dick");
            Book testBook = new Book("Goosebumps");
            newBook.Save();
            testBook.Save();
            List<Book> allBooks = Book.GetAll();
            List<Book> testList = new List<Book>{newBook, testBook};
            CollectionAssert.AreEqual(allBooks, testList);
        }

        [TestMethod]
        public void Update_UpdatesBookInDatabase_Book()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            newBook.Update("Goosebumps");

            Book foundBook = Book.GetAll()[0];
            Assert.AreEqual(newBook, foundBook);
        }

        [TestMethod]
        public void DeleteCopy_DeletesCopy_True()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            newBook.SetCopies(5);
            newBook.AddCopies();
            List<int> allCopies = newBook.GetAllCopies();
            newBook.DeleteCopy(allCopies[0]);
            int result = newBook.GetAllCopies().Count;
            Assert.AreEqual(allCopies.Count-1, result);
        }

        [TestMethod]
        public void AddAuthor_AddsAuthorToBook_AuthorList()
        {
            Book newBook = new Book("Moby Dick");
            newBook.Save();
            Author newAuthor = new Author("Herman Melville");
            newAuthor.Save();
            newBook.AddAuthor(newAuthor.GetId());
            List<Author> authors = newBook.GetAuthors();
            List<Author> testList = new List<Author>{newAuthor};
            CollectionAssert.AreEqual(authors, testList);
        }
    }
}
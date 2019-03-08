using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {

    [HttpGet("/patrons")]
    public ActionResult Index()
    {
      return View(Patron.GetAll());
    }
    [HttpGet("/patrons/new")]
    public ActionResult New()
    {
        return View();
    }
    
    [HttpPost("/patrons/create")]
    public ActionResult Create(string name)
    {
        Patron newPatron = new Patron(name);
        newPatron.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/patrons/{id}/")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Patron patron = Patron.Find(id);
        model["patron"] = patron;

        List<Book> allBooks = patron.GetCheckedOutBooks();
        
        List<Dictionary<string, object>> allBooksAndAuthors = new List<Dictionary<string, object>>();
        foreach(Book book in allBooks)
        {
            Dictionary<string, object> bookModel = new Dictionary<string, object>();
            bookModel["book"] = book as Book;
            bookModel["authors"] = book.GetAuthors();
            allBooksAndAuthors.Add(bookModel);
        }
        model.Add("books",allBooksAndAuthors);
        return View(model);
    }

    [HttpGet("/patrons/{id}/checkout")]
    public ActionResult Checkout(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model["patron"] = Patron.Find(id);
        model["books"] = Book.GetAll();
        return View(model);
    }

    [HttpPost("/patrons/{id}/get_book")]
    public ActionResult GetBook(int id, int bookId)
    {
        Patron patron = Patron.Find(id);
        patron.AddBook(bookId, 0, true, DateTime.Now, DateTime.Now);
        return RedirectToAction("Show");
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class BooksController : Controller
  {

    [HttpGet("/books")]
    public ActionResult Index()
    {
      return View(Book.GetAll());
    }

    [HttpGet("/books/new")]
    public ActionResult New()
    {
        return View(Author.GetAll());
    }

    [HttpPost("/books/create")]
    public ActionResult Create(string title, int copies, int authorId)
    {
        Book newBook = new Book(title);
        newBook.Save();
        newBook.AddAuthor(authorId);
        if (copies != 0)
        {
            newBook.SetCopies(copies);
            newBook.AddCopies();
        }
        return RedirectToAction("Index");
    }

    [HttpGet("/books/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Book newBook = Book.Find(id);
        List<Author> bookAuthors = newBook.GetAuthors();
        List<int> copies = newBook.GetAllCopies();
        model.Add("book", newBook);
        model.Add("authors", bookAuthors);
        model.Add("copies", copies);
        return View(model);

    }

  }
}
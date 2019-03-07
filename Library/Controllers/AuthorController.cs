using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {

    [HttpGet("/authors")]
    public ActionResult Index()
    {
      return View(Author.GetAll());
    }

    [HttpGet("/authors/new")]
    public ActionResult New()
    {
        return View();
    }

    [HttpPost("/authors/create")]
    public ActionResult Create(string name)
    {
        Author newAuthor = new Author(name);
        newAuthor.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/authors/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author newAuthor = Author.Find(id);
        model["author"] = newAuthor;
        model["books"] = newAuthor.GetBooks();
        return View(model);
    }

    [HttpGet("authors/{id}/edit")]
    public ActionResult Edit(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author newAuthor = Author.Find(id);
      List<Book> authorBooks = newAuthor.GetBooks();
      List<Book> allBooks = Book.GetAll();
      model.Add("author", newAuthor);
      model.Add("authorBooks", authorBooks);
      model.Add("allBooks", allBooks);
      return View(model);
    }

    [HttpPost("/authors/{id}/update")]
    public ActionResult Update(int id, string name)
    {
      Author newAuthor = Author.Find(id);
      newAuthor.Update(name);
      return RedirectToAction("Show");

    }

    [HttpPost("/authors/{id}/deletebook/")]
    public ActionResult DeleteBook(int id, int bookId)
    {
      Author newAuthor = Author.Find(id);
      newAuthor.DeleteBook(bookId);
      Console.WriteLine("{0} {1}", bookId, id);
      return RedirectToAction("Show");
    }

  }
}
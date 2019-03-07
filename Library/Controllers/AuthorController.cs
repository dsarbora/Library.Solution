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

  }
}
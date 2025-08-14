using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
        return View("Index");
    }
    public IActionResult VerTareas(){
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
        @ViewBag.tareas = BD.VerTareas(usuario.ID);
        return View();
    }
    public IActionResult NuevaTarea(){
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
        @ViewBag.tareas = BD.VerTareas(usuario.ID);
        return View();
    }
}

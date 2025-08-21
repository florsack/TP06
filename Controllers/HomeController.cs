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
        @ViewBag.estaRegistrado = null;
        if (HttpContext.Session.GetString("usuario")!= null )
        {
                int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
                Usuario user = BD.GetUsuario(usuario);
                @ViewBag.Usuario = user;
                @ViewBag.estaRegistrado = user.ID;
                List<Tarea> tareas = BD.VerTareas(usuario);
                @ViewBag.tareas = tareas;
        }
        return View("Index");
    }
    public IActionResult FinalizarTarea(int Id, bool Finalizada)
    {
        if (Finalizada)
        {
            BD.FinalizarTarea(Id);
        }
        else
        {
            BD.DesfinalizarTarea(Id);
        }
        return RedirectToAction("Index");
    }
    public IActionResult NuevaTarea()
    {
        if (HttpContext.Session.GetString("usuario")!= null)
            {
                int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
                Usuario user = BD.GetUsuario(usuario);
                @ViewBag.Usuario = user;
                @ViewBag.estaRegistrado = user.ID;
            }
        return View();
    }
    public IActionResult GuardarTarea(string Titulo, string Descripcion, DateTime Fecha)
    {
        int IdUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
        BD.AnadirTarea(Titulo, Descripcion, Fecha, IdUsuario);
        return RedirectToAction("Index");
    }
    public IActionResult EliminarTarea(int Id)
    {
        BD.EliminarTarea(Id);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult ModificarTarea(int Id)
    {
        if (HttpContext.Session.GetString("usuario")!= null)
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
            Tarea tarea = BD.VerTarea(Id);
            @ViewBag.Tarea = tarea;
        }
        return View();
    }
    public IActionResult ModificarTareaGuardar(int id, string titulo, string descripcion, DateTime fecha)
    {
        BD.ModificarTarea(id, titulo, descripcion, fecha);
        return RedirectToAction("Index");
    }
}


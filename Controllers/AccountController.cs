using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;
using System.Web;
using System.IO;
namespace TP06.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index(){
        return View();
    }
    public IActionResult LogIn(){
        return View("LogIn");
    }
    private IActionResult LogInGuardar(string Usuario, string Password){
        int id = BD.Login(Usuario, Password);

        if(id == 0){
            ViewBag.segundoIntento = true;
            return View ("LogIn");
        }
        else{
            HttpContext.Session.SetString("usuario", Objetos.ObjectToString(BD.GetUsuario(id)));
            ViewBag.usuario = BD.GetUsuario(id);
            return View("Index");
        }
    }
    [HttpPost]
    public IActionResult SignIn(string usuario, string contrasena, IFormFile foto)
    {
        ViewBag.Usuario = usuario;
        if (foto != null && foto.Length > 0)
        {
            string rutaDestino = Server.MapPath("~/Fotos/" + foto.FileName);
            foto.OpenReadStream(rutaDestino);
            ViewBag.MensajeFoto = "Foto recibida: " + foto.FileName;
        }
        else
        {
            ViewBag.MensajeFoto = "No se recibió ninguna foto.";
        }
            return View("Bienvenido");
    }
}
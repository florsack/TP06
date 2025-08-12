using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TP06.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private IWebHostEnvironment  _env;

    public AccountController(IWebHostEnvironment  env)
    {
        _env = env;
    }
    public IActionResult Index(){
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
        return View();
    }
    public IActionResult LogIn(){
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
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
    public IActionResult SignIn(){
        Usuario usuario = Objetos.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        @ViewBag.estaRegistrado = usuario.ID;
        return View("Registro");
    }
    [HttpPost]
    public IActionResult GuardarSignIn(string usuario, string contrasena, IFormFile foto)
    {
        ViewBag.Usuario = usuario;
        if (foto != null && foto.Length > 0)
        {
            string rutaDestino = Path.Combine(_env.WebRootPath, "Fotos", foto.FileName);
            using (var stream = new FileStream(rutaDestino, FileMode.Create))
            {
                foto.CopyTo(stream);
            }
            ViewBag.MensajeFoto = "Foto recibida: " + foto.FileName;
        }
        else
        {
            ViewBag.MensajeFoto = "No se recibió ninguna foto.";
        }
            return View("Registro");
            HttpContext.Session.SetString("usuario", Objetos.ObjectToString(BD.GetUsuario(id)));
    }
    public IActionResult LogOut (){
        HttpContext.Session.Remove("Usuario");
        return View("Index");
    }
}
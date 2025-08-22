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
    
    public IActionResult LogIn(){
        if (HttpContext.Session.GetString("usuario")!= null)
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
        }
        return View("LogIn");
    }

    [HttpPost]
    public IActionResult LogInGuardar(string Usuario, string Password){
        HttpContext.Session.Remove("usuario");
        int id = BD.Login(Usuario, Password);
        if(id == 0){
            ViewBag.segundoIntento = true;
            if (HttpContext.Session.GetString("usuario")!= null)
            {
                int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
                Usuario user = BD.GetUsuario(usuario);
                @ViewBag.Usuario = user;
                @ViewBag.estaRegistrado = user.ID;
            }
            return View ("LogIn");
        }
        else{
            HttpContext.Session.SetString("usuario", id.ToString());
            ViewBag.usuario = BD.GetUsuario(id);
            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult SignIn(){
        if (HttpContext.Session.GetString("usuario")!= null)
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
        }
        return View("registro");
    }
    [HttpPost]
    public IActionResult SignInGuardar(string usuario, string contrasena, IFormFile foto, string nombre, string apellido)
    {
        ViewBag.Usuario = usuario;
        string rutaDestino = "";
        
        if (foto != null && foto.Length > 0)
        {
            string nombreArchivo = foto.FileName;

            rutaDestino = Path.Combine(_env.WebRootPath, "imagenes");
            if (!Directory.Exists(rutaDestino))
                Directory.CreateDirectory(rutaDestino);
            string rutaCompleta = Path.Combine(rutaDestino, nombreArchivo);
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                foto.CopyTo(stream);
            }
        }
        HttpContext.Session.Remove("usuario");
        BD.Registro(usuario, contrasena, apellido, rutaDestino, nombre);
        int id = BD.Login(usuario, contrasena);
        HttpContext.Session.SetString("usuario", id.ToString());
        return RedirectToAction("Index", "Home");
    }
    public IActionResult LogOut (){
        HttpContext.Session.Remove("usuario");
        return RedirectToAction("Index", "Home");
    }
     public IActionResult VerCuenta(){
        if (HttpContext.Session.GetString("usuario")!= null)
        {
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
        }
        return View();
     }
}
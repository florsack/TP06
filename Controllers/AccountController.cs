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
        int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
        Usuario user = BD.GetUsuario(usuario);
        @ViewBag.Usuario = user;
        @ViewBag.estaRegistrado = user.ID;
        return View("LogIn");
    }
    private IActionResult LogInGuardar(string Usuario, string Password){
        int id = BD.Login(Usuario, Password);
        if(id == 0){
            ViewBag.segundoIntento = true;
            int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
            return View ("LogIn");
        }
        else{
            HttpContext.Session.SetString("usuario", id.ToString());
            ViewBag.usuario = BD.GetUsuario(id);
            return RedirectToAction("Index");
        }
    }
    public IActionResult SignIn(){
        int usuario = int.Parse(HttpContext.Session.GetString("usuario"));
            Usuario user = BD.GetUsuario(usuario);
            @ViewBag.Usuario = user;
            @ViewBag.estaRegistrado = user.ID;
        return View("registro");
    }
    [HttpPost]
    public IActionResult GuardarSignIn(string usuario, string contrasena, IFormFile foto, string nombre, string apellido)
    {
        ViewBag.Usuario = usuario;
        string rutaDestino = "";
        if (foto != null && foto.Length > 0)
        {
            rutaDestino = Path.Combine(_env.WebRootPath, "Fotos", foto.FileName);
            using (var stream = new FileStream(rutaDestino, FileMode.Create))
            {
                foto.CopyTo(stream);
            }
        }
        BD.Registro(usuario, contrasena, apellido, rutaDestino, nombre);
        int id = BD.Login(usuario, contrasena);
        HttpContext.Session.SetString("usuario", id.ToString());
        return Redirect("Index");
    }
    public IActionResult LogOut (){
        HttpContext.Session.Remove("Usuario");
        return RedirectToAction("Index");
    }
}
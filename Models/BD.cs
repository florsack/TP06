
    /*
    Tiene que tener
        Login (usuario, contraseña) -> tipo usuario
        Registro (usuario) -> void
        Agregar tarea(tarea) -> void
        Modificar tarea (tarea) -> void
        Eliminar tarea (id de tarea) -> void
        Ver tarea(id de tarea) -> tarea
        Ver tareas(id de usuario) -> List<Tarea> -> devuelve la 
            lista de todas las tareas de un usuario
        Finalizar tarea (id de tarea) -> void -> cambia el bool 
            de finalizada a true
    */

namespace TP06.Models;
using Microsoft.Data.SqlClient;
using Dapper;

public static class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=TP06BD; Integrated Security=True; TrustServerCertificate=True;";

    public static Usuario Login(string Usuario, string Password){
        int Id = 0;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT Id FROM Usuario WHERE Usuario = @Usuario AND Password = @Password";
            Id = connection.QueryFirstOrDefault<int>(query, new { Usuario,  Password});
        }
        Usuario usuario = null;
        if(Id != 0){
            usuario = GetUsuario(Id);
            ActualizarFecha(Id);
        }
        return usuario;
    }
    private static Usuario GetUsuario(int id){
        Usuario usuario = null;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT * FROM Usuario WHERE ID = @Id";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new {id});
        }
        return usuario;
    }
    private static void ActualizarFecha(int ID){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE [Usuario] SET [FechaUL] = GETDATE() WHERE ID = @ID";
             connection.Execute(query, new {ID});
        }
    }
    


}
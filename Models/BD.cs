
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

    public static int Login(string Usuario, string Password){
        int Id = obtenerIdUsuario(Usuario, Password);
        if (Id !=0){
            ActualizarFecha(Id);
        }
        return Id;
    }
    private static int obtenerIdUsuario(string Usuario, string Password){
        int Id = 0;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT Id FROM Usuario WHERE Usuario = @Usuario AND Password = @Password";
            Id = connection.QueryFirstOrDefault<int>(query, new { Usuario,  Password});
        }
        return Id;
    }
    public static Usuario GetUsuario(int id){
        Usuario usuario = null;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT * FROM Usuario WHERE ID = @Id";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new {id});
        }
        return usuario;
            }
    private static void ActualizarFecha(int ID){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Usuario SET FechaUL = GETDATE() WHERE ID = @ID";
             connection.Execute(query, new {ID});
        }
    }
   public static bool Registro (string Usuario, string Password, string Apellido, string Foto, string Nombre){
        int idParaChequear = obtenerIdUsuario(Usuario, Password);
        bool sePudoRegistrar = true;
        if (idParaChequear == 0){
            using(SqlConnection connection = new SqlConnection(_connectionString)){
                string query = "INSERT INTO Usuario (Nombre, Apellido, User, Password, Foto) VALUES (@Nombre, @Apellido, @Usuario, @Password, @Foto)";
                connection.Execute(query, new {Usuario, Password, Apellido, Foto, Nombre});
            }
        } else{
            sePudoRegistrar = false;
        }
        return sePudoRegistrar;
   }
   public static void AnadirTarea (string Titulo, string Descripcion, DateTime Fecha, int IdUsuario){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "INSERT INTO Tareas (Titulo, Descripcion, Fecha, Finalizada, IdUsuario) VALUES (@Titulo, @Descripcion, @Fecha, 0, @IdUsuario)";
            connection.Execute(query, new {Titulo, Descripcion, Fecha, IdUsuario });
        }
    }
    public static void EliminarTarea (int Id){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "DELETE FROM Tareas WHERE Id = @Id";
            connection.Execute(query, new {Id});
        }
    }
    public static List<Tarea> VerTareas(int IdUsuario){
        List<Tarea> tareas = new List<Tarea> ();
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "Select * From Tareas Where IdUsuario = @IdUsuario";
            tareas = connection.Query<Tarea>(query, new {IdUsuario}).ToList();
        }
        return tareas;
    }
    public static void FinalizarTarea(int Id){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Tarea SET Finalizada = 1 WHERE ID = @Id";
             connection.Execute(query, new {Id});
        }
    }
    public static void DesfinalizarTarea(int Id){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Tarea SET Finalizada = 0 WHERE ID = @Id";
             connection.Execute(query, new {Id});
        }
    }
    public static Tarea VerTarea(int Id)
    {
        Tarea tarea = new Tarea();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Tarea WHERE Id = @Id";
            tarea = connection.QueryFirstOrDefault<Tarea>(query, new { Id });
        }
        return tarea;
    }
    public static void ModificarTarea(int Id, string Titulo, string Descripcion, DateTime Fecha ){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Tarea SET Titulo = @Titulo, Descripcion = @Descripcion, Fecha = @Fecha  WHERE ID = @Id";
            connection.Execute(query, new {Id});
        }
    }
}
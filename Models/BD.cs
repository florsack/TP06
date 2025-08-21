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
            string query = "SELECT Id FROM Usuarios WHERE NombreUsuario = @Usuario AND Password = @Password";
            Id = connection.QueryFirstOrDefault<int>(query, new { Usuario,  Password});
        }
        return Id;
    }
    public static Usuario GetUsuario(int id){
        Usuario usuario = null;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT * FROM Usuarios WHERE ID = @Id";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new {id});
        }
        return usuario;
            }
    private static void ActualizarFecha(int ID){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Usuarios SET FechaUL = GETDATE() WHERE ID = @ID";
             connection.Execute(query, new {ID});
        }
    }
   public static bool Registro (string Usuario, string Password, string Apellido, string Foto, string Nombre){
        bool sePudoRegistrar = ChequearUsuarioRegistro(Usuario);
        if (sePudoRegistrar){
            using(SqlConnection connection = new SqlConnection(_connectionString)){
                string query = "INSERT INTO Usuarios (Nombre, Apellido, NombreUsuario, Password, Foto, FechaUL) VALUES (@Nombre, @Apellido, @Usuario, @Password, @Foto, GETDATE())";
                connection.Execute(query, new {Usuario, Password, Apellido, Foto, Nombre});
            }
        }
        return sePudoRegistrar;
   }
   private static bool ChequearUsuarioRegistro(string Usuario){
        int Id = 0;
        bool existeUsuario = false;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT Id FROM Usuarios WHERE NombreUsuario = @Usuario";
            Id = connection.QueryFirstOrDefault<int>(query, new { Usuario});
        }
        if(Id == 0){
            existeUsuario = true;
        }
        return !existeUsuario;
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
            string query = "UPDATE Tareas SET Finalizada = 1 WHERE ID = @Id";
             connection.Execute(query, new {Id});
        }
    }
    public static void DesfinalizarTarea(int Id){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Tareas SET Finalizada = 0 WHERE ID = @Id";
             connection.Execute(query, new {Id});
        }
    }
    public static Tarea VerTarea(int Id)
    {
        Tarea tarea = new Tarea();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Tareas WHERE Id = @Id";
            tarea = connection.QueryFirstOrDefault<Tarea>(query, new { Id });
        }
        return tarea;
    }
    public static void ModificarTarea(int Id, string Titulo, string Descripcion, DateTime Fecha ){
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "UPDATE Tareas SET Titulo = @Titulo, Descripcion = @Descripcion, Fecha = @Fecha  WHERE ID = @Id";
            connection.Execute(query, new {Id, Titulo, Descripcion, Fecha});
        }
    }
}
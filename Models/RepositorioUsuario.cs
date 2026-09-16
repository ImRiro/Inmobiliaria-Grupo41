using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
{
    public RepositorioUsuario(IConfiguration configuration) : base(configuration)
    {

    }

    private const string SelectBase = @"
        SELECT Id, Email, Clave, Rol, Nombre, Apellido, Avatar_Url, Activo
        FROM Usuario";

    private static Usuario LeerUsuario(MySqlDataReader reader)
    {
        return new Usuario
        {
            IdUsuario = reader.GetInt32(reader.GetOrdinal("Id")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Clave = reader.GetString(reader.GetOrdinal("Clave")),
            Rol = reader.GetString(reader.GetOrdinal("Rol")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
            AvatarUrl = reader.IsDBNull(reader.GetOrdinal("Avatar_Url")) ? null : reader.GetString(reader.GetOrdinal("Avatar_Url")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
        };
    }

    public async Task<List<Usuario>> ObtenerTodosAsync()
    {
        var lista = new List<Usuario>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE Activo = 1 ORDER BY Apellido, Nombre";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(LeerUsuario(reader));
        }

        return lista;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return LeerUsuario(reader);
        }
        return null;
    }

    public async Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE Email = @Email AND Activo = 1";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return LeerUsuario(reader);
        }
        return null;
    }

    public async Task<int> CrearAsync(Usuario usuario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO Usuario (Email, Clave, Rol, Nombre, Apellido, Avatar_Url, Activo)
                      VALUES (@Email, @Clave, @Rol, @Nombre, @Apellido, @AvatarUrl, @Activo);
                      SELECT LAST_INSERT_ID();";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Clave", usuario.Clave);
        command.Parameters.AddWithValue("@Rol", usuario.Rol);
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@AvatarUrl", (object?)usuario.AvatarUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", usuario.Activo);

        var nuevoId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(nuevoId);
    }

    public async Task ActualizarAsync(Usuario usuario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Usuario SET Email = @Email, Rol = @Rol, Nombre = @Nombre,
                      Apellido = @Apellido, Avatar_Url = @AvatarUrl, Activo = @Activo
                      WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Rol", usuario.Rol);
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@AvatarUrl", (object?)usuario.AvatarUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", usuario.Activo);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarPerfilAsync(Usuario usuario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Usuario SET Nombre = @Nombre, Apellido = @Apellido, Avatar_Url = @AvatarUrl
                      WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@AvatarUrl", (object?)usuario.AvatarUrl ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarClaveAsync(int id, string claveHasheada)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "UPDATE Usuario SET Clave = @Clave WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Clave", claveHasheada);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "UPDATE Usuario SET Activo = 0 WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> ContarAsync()
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT COUNT(*) FROM Usuario";
        using var command = new MySqlCommand(query, connection);
        var resultado = await command.ExecuteScalarAsync();
        return Convert.ToInt32(resultado);
    }
}

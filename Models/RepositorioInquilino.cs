
using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
{
    public RepositorioInquilino(IConfiguration configuration) : base(configuration)
    {
        
    }
    
    public async Task<List<Inquilino>> ObtenerTodosAsync()
    {
        var lista = new List<Inquilino>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, DNI, Nombre, Apellido, Email FROM Inquilinos";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Inquilino
            {
                IdInquilino = reader.GetInt32(reader.GetOrdinal("Id")),
                DNI = reader.GetString(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            });
        }

        return lista;
    }

    public async Task<Inquilino?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, DNI, Nombre, Apellido, Email FROM Inquilinos WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Inquilino
            {
                IdInquilino = reader.GetInt32(reader.GetOrdinal("Id")),
                DNI = reader.GetString(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            };
        }
        return null;
    }

    public async Task CrearAsync(Inquilino Inquilino)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO Inquilinos (DNI, Nombre, Apellido, Email) 
                       VALUES (@DNI, @Nombre, @Apellido, @Email)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@DNI", Inquilino.DNI);
        command.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
        command.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
        command.Parameters.AddWithValue("@Email", Inquilino.Email);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Inquilino Inquilino)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Inquilinos 
                       SET DNI = @DNI, Nombre = @Nombre, Apellido = @Apellido, Email = @Email 
                       WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@DNI", Inquilino.DNI);
        command.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
        command.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
        command.Parameters.AddWithValue("@Email", Inquilino.Email);
        command.Parameters.AddWithValue("@Id", Inquilino.IdInquilino);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM Inquilinos WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}
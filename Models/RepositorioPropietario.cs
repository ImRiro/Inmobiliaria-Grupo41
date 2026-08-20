using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
{
    public RepositorioPropietario(IConfiguration configuration) : base(configuration)
    {
        
    }
    
    public async Task<List<Propietario>> ObtenerTodosAsync()
    {
        var lista = new List<Propietario>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, DNI, Nombre, Apellido, Email FROM Propietarios";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Propietario
            {
                IdPropietario = reader.GetInt32(reader.GetOrdinal("Id")),
                DNI = reader.GetString(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            });
        }

        return lista;
    }

    public async Task<Propietario?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, DNI, Nombre, Apellido, Email FROM Propietarios WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Propietario
            {
                IdPropietario = reader.GetInt32(reader.GetOrdinal("Id")),
                DNI = reader.GetString(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Email = reader.GetString(reader.GetOrdinal("Email"))
            };
        }
        return null;
    }

    public async Task CrearAsync(Propietario propietario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO Propietarios (DNI, Nombre, Apellido, Email) 
                       VALUES (@DNI, @Nombre, @Apellido, @Email)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@DNI", propietario.DNI);
        command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
        command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
        command.Parameters.AddWithValue("@Email", propietario.Email);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Propietario propietario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Propietarios 
                       SET DNI = @DNI, Nombre = @Nombre, Apellido = @Apellido, Email = @Email 
                       WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@DNI", propietario.DNI);
        command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
        command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
        command.Parameters.AddWithValue("@Email", propietario.Email);
        command.Parameters.AddWithValue("@Id", propietario.IdPropietario);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM Propietarios WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}
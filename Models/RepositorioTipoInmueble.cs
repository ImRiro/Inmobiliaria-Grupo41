using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioTipoInmueble : RepositorioBase, IRepositorioTipoInmueble
{
    public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration)
    {

    }

    public async Task<List<TipoInmueble>> ObtenerTodosAsync()
    {
        var lista = new List<TipoInmueble>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, Nombre FROM TipoInmueble";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new TipoInmueble
            {
                IdTipoInmueble = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
            });
        }

        return lista;
    }

    public async Task<TipoInmueble?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, Nombre FROM TipoInmueble WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new TipoInmueble
            {
                IdTipoInmueble = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
            };
        }
        return null;
    }

    public async Task CrearAsync(TipoInmueble tipoInmueble)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO TipoInmueble (Nombre) VALUES (@Nombre)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Nombre", tipoInmueble.Nombre);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(TipoInmueble tipoInmueble)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "UPDATE TipoInmueble SET Nombre = @Nombre WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Nombre", tipoInmueble.Nombre);
        command.Parameters.AddWithValue("@Id", tipoInmueble.IdTipoInmueble);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM TipoInmueble WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}

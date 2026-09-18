using MySqlConnector;
using Inmobiliaria_.Net_Core.Models;

public class RepositorioImagenInmueble : RepositorioBase, IRepositorioImagenInmueble
{
    public RepositorioImagenInmueble(IConfiguration configuration) : base(configuration)
    {
        
    }

    public async Task<List<ImagenInmueble>> ObtenerPorInmuebleAsync(int idInmueble)
    {
        var lista = new List<ImagenInmueble>();
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, id_inmueble, url FROM imageninmueble WHERE id_inmueble = @IdInmueble ORDER BY Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdInmueble", idInmueble);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new ImagenInmueble
            {
                Id = reader.GetInt32("Id"),
                IdInmueble = reader.GetInt32("id_inmueble"),
                Url = reader.GetString("url")
            });
        }
        return lista;
    }

    public async Task<ImagenInmueble?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, id_inmueble, url FROM imageninmueble WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new ImagenInmueble
            {
                Id = reader.GetInt32("Id"),
                IdInmueble = reader.GetInt32("id_inmueble"),
                Url = reader.GetString("url")
            };
        }
        return null;
    }

    public async Task<int> AltaAsync(ImagenInmueble imagen)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO imageninmueble (id_inmueble, url) VALUES (@IdInmueble, @Url); SELECT LAST_INSERT_ID();";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdInmueble", imagen.IdInmueble);
        command.Parameters.AddWithValue("@Url", imagen.Url);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<int> EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM imageninmueble WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync();
    }
}
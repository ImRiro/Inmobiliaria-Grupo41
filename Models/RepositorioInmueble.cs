using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
{
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {
        
    }
    
    private const string SelectBase = @"
        SELECT i.Id, i.IdPropietario, i.IdTipoInmueble, i.Direccion, i.Latitud, i.Longitud,
               i.Activo, i.Metros_Cuadrados, i.Habitaciones,
               CONCAT(p.Nombre, ' ', p.Apellido) AS NombrePropietario,
               t.Nombre AS NombreTipoInmueble
        FROM Inmueble i
        INNER JOIN Propietarios p ON i.IdPropietario = p.Id
        INNER JOIN TipoInmueble t ON i.IdTipoInmueble = t.Id";

    private static Inmueble LeerInmueble(MySqlDataReader reader)
    {
        return new Inmueble
        {
            IdInmueble = reader.GetInt32(reader.GetOrdinal("Id")),
            IdPropietario = reader.GetInt32(reader.GetOrdinal("IdPropietario")),
            IdTipoInmueble = reader.GetInt32(reader.GetOrdinal("IdTipoInmueble")),
            Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
            Latitud = reader.GetDecimal(reader.GetOrdinal("Latitud")),
            Longitud = reader.GetDecimal(reader.GetOrdinal("Longitud")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
            Metros_Cuadrados = reader.GetInt32(reader.GetOrdinal("Metros_Cuadrados")),
            Habitaciones = reader.GetInt32(reader.GetOrdinal("Habitaciones")),
            NombrePropietario = reader.GetString(reader.GetOrdinal("NombrePropietario")),
            NombreTipoInmueble = reader.GetString(reader.GetOrdinal("NombreTipoInmueble"))
        };
    }

    public async Task<List<Inmueble>> ObtenerTodosAsync()
    {
        var lista = new List<Inmueble>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " ORDER BY i.Id";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(LeerInmueble(reader));
        }

        return lista;
    }

    public async Task<Inmueble?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE i.Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return LeerInmueble(reader);
        }
        return null;
    }

    public async Task CrearAsync(Inmueble inmueble)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO Inmueble (IdPropietario, IdTipoInmueble, Direccion, Latitud, Longitud, Activo, Metros_Cuadrados, Habitaciones)
                      VALUES (@IdPropietario, @IdTipoInmueble, @Direccion, @Latitud, @Longitud, @Activo, @Metros_Cuadrados, @Habitaciones)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
        command.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
        command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
        command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
        command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
        command.Parameters.AddWithValue("@Activo", inmueble.Activo);
        command.Parameters.AddWithValue("@Metros_Cuadrados", inmueble.Metros_Cuadrados);
        command.Parameters.AddWithValue("@Habitaciones", inmueble.Habitaciones);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Inmueble inmueble)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Inmueble SET IdPropietario = @IdPropietario, IdTipoInmueble = @IdTipoInmueble, Direccion = @Direccion, 
                      Latitud = @Latitud, Longitud = @Longitud, Activo = @Activo, Metros_Cuadrados = @Metros_Cuadrados, 
                      Habitaciones = @Habitaciones WHERE Id = @IdInmueble";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
        command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
        command.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
        command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
        command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
        command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
        command.Parameters.AddWithValue("@Activo", inmueble.Activo);
        command.Parameters.AddWithValue("@Metros_Cuadrados", inmueble.Metros_Cuadrados);
        command.Parameters.AddWithValue("@Habitaciones", inmueble.Habitaciones);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM Inmueble WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}
using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
{
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {
        
    }
    
    private const string SelectBase = @"
        SELECT i.Id, i.IdPropietario, i.IdTipoInmueble, i.Direccion, i.Latitud, i.Longitud, i.Porcentaje_Sena,
               i.Disponible, i.Metros_Cuadrados, i.Habitaciones,
               CONCAT(p.Nombre, ' ', p.Apellido) AS NombrePropietario,
               t.Nombre AS NombreTipoInmueble
        FROM Inmueble i
        INNER JOIN Propietarios p ON i.IdPropietario = p.Id
        INNER JOIN TipoInmueble t ON i.IdTipoInmueble = t.Id
        WHERE i.Activo = 1";

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
            Porcentaje_Sena = reader.GetDecimal(reader.GetOrdinal("Porcentaje_Sena")),
            Disponible = reader.GetBoolean(reader.GetOrdinal("Disponible")),
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

        var query = SelectBase + " AND i.Id = @Id";
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

        var query = @"INSERT INTO Inmueble (IdPropietario, IdTipoInmueble, Direccion, Latitud, Longitud, Porcentaje_Sena, Disponible, Metros_Cuadrados, Habitaciones)
                      VALUES (@IdPropietario, @IdTipoInmueble, @Direccion, @Latitud, @Longitud, @Porcentaje_Sena, @Disponible, @Metros_Cuadrados, @Habitaciones)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
        command.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
        command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
        command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
        command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
        command.Parameters.AddWithValue("@Porcentaje_Sena", inmueble.Porcentaje_Sena);
        command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
        command.Parameters.AddWithValue("@Metros_Cuadrados", inmueble.Metros_Cuadrados);
        command.Parameters.AddWithValue("@Habitaciones", inmueble.Habitaciones);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Inmueble inmueble)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Inmueble SET IdPropietario = @IdPropietario, IdTipoInmueble = @IdTipoInmueble, Direccion = @Direccion, 
                      Latitud = @Latitud, Longitud = @Longitud, Porcentaje_Sena = @Porcentaje_Sena, Disponible = @Disponible, Metros_Cuadrados = @Metros_Cuadrados, 
                      Habitaciones = @Habitaciones WHERE Id = @IdInmueble";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
        command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
        command.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
        command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
        command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
        command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
        command.Parameters.AddWithValue("@Porcentaje_Sena", inmueble.Porcentaje_Sena);
        command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
        command.Parameters.AddWithValue("@Metros_Cuadrados", inmueble.Metros_Cuadrados);
        command.Parameters.AddWithValue("@Habitaciones", inmueble.Habitaciones);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "UPDATE Inmueble SET activo = 0 WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Inmueble>> ObtenerDisponiblesAsync(DateTime desde, DateTime hasta, int? idTipoInmueble = null)
{
    var lista = new List<Inmueble>();

    using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    var query = SelectBase + @"
        AND i.Disponible = 1
        AND i.Id NOT IN (
            SELECT r.IdInmueble FROM Reserva r
            WHERE r.Fecha_Cancelacion IS NULL
              AND r.Fecha_Desde < @Hasta
              AND r.Fecha_Hasta > @Desde
        )
        AND (@IdTipoInmueble IS NULL OR i.IdTipoInmueble = @IdTipoInmueble)
        ORDER BY i.Id";

    using var command = new MySqlCommand(query, connection);
    command.Parameters.AddWithValue("@Desde", desde.Date);
    command.Parameters.AddWithValue("@Hasta", hasta.Date);
    command.Parameters.AddWithValue("@IdTipoInmueble", (object?)idTipoInmueble ?? DBNull.Value);

    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
        lista.Add(LeerInmueble(reader));

    return lista;
}
}
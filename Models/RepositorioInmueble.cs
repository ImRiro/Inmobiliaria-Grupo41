using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
{
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {
        
    }
    
    public async Task<List<Inmueble>> ObtenerTodosAsync()
    {
        var lista = new List<Inmueble>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT IdInmueble, IdPropietario, IdTipoInmueble, Direccion, Latitud, Longitud, Activo, Metros_Cuadrados, Habitaciones FROM Inmueble";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Inmueble
            {
                IdInmueble = reader.GetInt32(reader.GetOrdinal("IdInmueble")),
                IdPropietario = reader.GetInt32(reader.GetOrdinal("IdPropietario")),
                IdTipoInmueble = reader.GetInt32(reader.GetOrdinal("IdTipoInmueble")),
                Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                Latitud = reader.GetDecimal(reader.GetOrdinal("Latitud")),
                Longitud = reader.GetDecimal(reader.GetOrdinal("Longitud")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                Metros_Cuadrados = reader.GetDecimal(reader.GetOrdinal("Metros_Cuadrados")),
                Habitaciones = reader.GetInt32(reader.GetOrdinal("Habitaciones"))
            });
        }

        return lista;
    }

    public async Task<Inmueble?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT IdInmueble, IdPropietario, IdTipoInmueble, Direccion, Latitud, Longitud, Activo, Metros_Cuadrados, Habitaciones FROM Inmueble WHERE IdInmueble = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Inmueble
            {
                IdInmueble = reader.GetInt32(reader.GetOrdinal("IdInmueble")),
                IdPropietario = reader.GetInt32(reader.GetOrdinal("IdPropietario")),
                IdTipoInmueble = reader.GetInt32(reader.GetOrdinal("IdTipoInmueble")),
                Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                Latitud = reader.GetDecimal(reader.GetOrdinal("Latitud")),
                Longitud = reader.GetDecimal(reader.GetOrdinal("Longitud")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                Metros_Cuadrados = reader.GetDecimal(reader.GetOrdinal("Metros_Cuadrados")),
                Habitaciones = reader.GetInt32(reader.GetOrdinal("Habitaciones"))
            };
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
                      Habitaciones = @Habitaciones WHERE IdInmueble = @IdInmueble";
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

        var query = "DELETE FROM Inmueble WHERE IdInmueble = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}
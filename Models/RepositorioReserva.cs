using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioReserva : RepositorioBase, IRepositorioReserva
{
    public RepositorioReserva(IConfiguration configuration) : base(configuration)
    {
        
    }

    public async Task<List<Reserva>> ObtenerTodosAsync()
    {
        var lista = new List<Reserva>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, IdInmueble, IdInquilino, Fecha_Desde, Fecha_Hasta, Fecha_Cancelacion, Monto_Diario, Costo_Total FROM Reserva";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Reserva
            {
                IdReserva = reader.GetInt32(reader.GetOrdinal("Id")),
                IdInmueble = reader.GetInt32(reader.GetOrdinal("IdInmueble")),
                IdInquilino = reader.GetInt32(reader.GetOrdinal("IdInquilino")),
                Fecha_Desde = reader.GetDateTime(reader.GetOrdinal("Fecha_Desde")),
                Fecha_Hasta = reader.GetDateTime(reader.GetOrdinal("Fecha_Hasta")),
                Fecha_Cancelacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Cancelacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Fecha_Cancelacion")),
                Monto_Diario = reader.GetDecimal(reader.GetOrdinal("Monto_Diario")),
                Costo_Total = reader.GetDecimal(reader.GetOrdinal("Costo_Total"))
            });
        }

        return lista;
    }

    public async Task<Reserva?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, IdInmueble, IdInquilino, Fecha_Desde, Fecha_Hasta, Fecha_Cancelacion, Monto_Diario, Costo_Total FROM Reserva WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Reserva
            {
                IdReserva = reader.GetInt32(reader.GetOrdinal("Id")),
                IdInmueble = reader.GetInt32(reader.GetOrdinal("IdInmueble")),
                IdInquilino = reader.GetInt32(reader.GetOrdinal("IdInquilino")),
                Fecha_Desde = reader.GetDateTime(reader.GetOrdinal("Fecha_Desde")),
                Fecha_Hasta = reader.GetDateTime(reader.GetOrdinal("Fecha_Hasta")),
                Fecha_Cancelacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Cancelacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Fecha_Cancelacion")),
                Monto_Diario = reader.GetDecimal(reader.GetOrdinal("Monto_Diario")),
                Costo_Total = reader.GetDecimal(reader.GetOrdinal("Costo_Total"))
            };
        }
        return null;
    }

    public async Task CrearAsync(Reserva reserva)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO Reserva (IdInmueble, IdInquilino, Fecha_Desde, Fecha_Hasta, Fecha_Cancelacion, Monto_Diario, Costo_Total)
                      VALUES (@IdInmueble, @IdInquilino, @Fecha_Desde, @Fecha_Hasta, @Fecha_Cancelacion, @Monto_Diario, @Costo_Total)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);
        command.Parameters.AddWithValue("@IdInquilino", reserva.IdInquilino);
        command.Parameters.AddWithValue("@Fecha_Desde", reserva.Fecha_Desde);
        command.Parameters.AddWithValue("@Fecha_Hasta", reserva.Fecha_Hasta);
        command.Parameters.AddWithValue("@Fecha_Cancelacion", reserva.Fecha_Cancelacion == DateTime.MinValue ? (object)DBNull.Value : reserva.Fecha_Cancelacion);
        command.Parameters.AddWithValue("@Monto_Diario", reserva.Monto_Diario);
        command.Parameters.AddWithValue("@Costo_Total", reserva.Costo_Total);

        await command.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(Reserva reserva)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE Reserva SET IdInmueble = @IdInmueble, IdInquilino = @IdInquilino, Fecha_Desde = @Fecha_Desde, 
                      Fecha_Hasta = @Fecha_Hasta, Fecha_Cancelacion = @Fecha_Cancelacion, Monto_Diario = @Monto_Diario, 
                      Costo_Total = @Costo_Total WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", reserva.IdReserva);
        command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);
        command.Parameters.AddWithValue("@IdInquilino", reserva.IdInquilino);
        command.Parameters.AddWithValue("@Fecha_Desde", reserva.Fecha_Desde);
        command.Parameters.AddWithValue("@Fecha_Hasta", reserva.Fecha_Hasta);
        command.Parameters.AddWithValue("@Fecha_Cancelacion", reserva.Fecha_Cancelacion == DateTime.MinValue ? (object)DBNull.Value : reserva.Fecha_Cancelacion);
        command.Parameters.AddWithValue("@Monto_Diario", reserva.Monto_Diario);
        command.Parameters.AddWithValue("@Costo_Total", reserva.Costo_Total);

        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM Reserva WHERE Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }
}
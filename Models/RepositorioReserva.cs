using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioReserva : RepositorioBase, IRepositorioReserva
{
    public RepositorioReserva(IConfiguration configuration) : base(configuration)
    {
        
    }

    private const string SelectBase = @"
        SELECT r.Id, r.IdInmueble, r.IdInquilino, r.Fecha_Desde, r.Fecha_Hasta, r.Fecha_Cancelacion,
                r.Monto_Diario, r.Costo_Total, r.IdUsuarioCreador, r.IdUsuarioFinalizador,
                inm.Direccion AS DireccionInmueble,
                CONCAT(q.Nombre, ' ', q.Apellido) AS NombreInquilino,
                cu.Nombre AS CreadorNombre, cu.Apellido AS CreadorApellido,
                fu.Nombre AS FinalizadorNombre, fu.Apellido AS FinalizadorApellido
        FROM Reserva r
        INNER JOIN Inmueble inm ON r.IdInmueble = inm.Id
        INNER JOIN Inquilinos q ON r.IdInquilino = q.Id
        LEFT JOIN Usuario cu ON r.IdUsuarioCreador = cu.Id
        LEFT JOIN Usuario fu ON r.IdUsuarioFinalizador = fu.Id";

    private static Reserva LeerReserva(MySqlDataReader reader)
    {
        return new Reserva
        {
            IdReserva = reader.GetInt32(reader.GetOrdinal("Id")),
            IdInmueble = reader.GetInt32(reader.GetOrdinal("IdInmueble")),
            IdInquilino = reader.GetInt32(reader.GetOrdinal("IdInquilino")),
            Fecha_Desde = reader.GetDateTime(reader.GetOrdinal("Fecha_Desde")),
            Fecha_Hasta = reader.GetDateTime(reader.GetOrdinal("Fecha_Hasta")),
            Fecha_Cancelacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Cancelacion")) ? null : reader.GetDateTime(reader.GetOrdinal("Fecha_Cancelacion")),
            Monto_Diario = reader.GetDecimal(reader.GetOrdinal("Monto_Diario")),
            Costo_Total = reader.GetDecimal(reader.GetOrdinal("Costo_Total")),
            DireccionInmueble = reader.GetString(reader.GetOrdinal("DireccionInmueble")),
            NombreInquilino = reader.GetString(reader.GetOrdinal("NombreInquilino")),
            IdUsuarioCreador = reader.IsDBNull(reader.GetOrdinal("IdUsuarioCreador")) ? null : reader.GetInt32(reader.GetOrdinal("IdUsuarioCreador")),
            IdUsuarioFinalizador = reader.IsDBNull(reader.GetOrdinal("IdUsuarioFinalizador")) ? null : reader.GetInt32(reader.GetOrdinal("IdUsuarioFinalizador")),
            UsuarioCreadorNombre = reader.IsDBNull(reader.GetOrdinal("CreadorNombre")) ? null : $"{reader.GetString(reader.GetOrdinal("CreadorNombre"))} {reader.GetString(reader.GetOrdinal("CreadorApellido"))}",
            UsuarioFinalizadorNombre = reader.IsDBNull(reader.GetOrdinal("FinalizadorNombre")) ? null : $"{reader.GetString(reader.GetOrdinal("FinalizadorNombre"))} {reader.GetString(reader.GetOrdinal("FinalizadorApellido"))}"
        };
    }

    public async Task<List<Reserva>> ObtenerTodosAsync()
    {
        var lista = new List<Reserva>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " ORDER BY r.Fecha_Desde DESC";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(LeerReserva(reader));
        }

        return lista;
    }

    public async Task<Reserva?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE r.Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return LeerReserva(reader);
        }
        return null;
    }

    public async Task CrearAsync(Reserva reserva, int? idUsuario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string queryPorcentajeSena = @"SELECT Porcentaje_Sena FROM Inmueble WHERE Id = @IdInmueble AND Activo = 1";

            decimal porcentajeSena;

            using (var command = new MySqlCommand(queryPorcentajeSena, connection, transaction))
            {
                command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);
                var result = await command.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("El inmueble no existe o no está activo.");
                }
                porcentajeSena = Convert.ToDecimal(result);
            }

            decimal montoSena = Math.Round(reserva.Costo_Total * porcentajeSena / 100m, 2);
            var reservaQuery = @"INSERT INTO Reserva (IdInmueble, IdInquilino, Fecha_Desde, Fecha_Hasta, Fecha_Cancelacion, Monto_Diario, Costo_Total, IdUsuarioCreador) VALUES (@IdInmueble, @IdInquilino, @Fecha_Desde, @Fecha_Hasta, @Fecha_Cancelacion, @Monto_Diario, @Costo_Total, @IdUsuarioCreador); SELECT LAST_INSERT_ID();";

            int idReserva;

            using (var command = new MySqlCommand(reservaQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);
                command.Parameters.AddWithValue("@IdInquilino", reserva.IdInquilino);
                command.Parameters.AddWithValue("@Fecha_Desde", reserva.Fecha_Desde);
                command.Parameters.AddWithValue("@Fecha_Hasta", reserva.Fecha_Hasta);
                command.Parameters.AddWithValue("@Fecha_Cancelacion", reserva.Fecha_Cancelacion == DateTime.MinValue ? (object)DBNull.Value : reserva.Fecha_Cancelacion);
                command.Parameters.AddWithValue("@Monto_Diario", reserva.Monto_Diario);
                command.Parameters.AddWithValue("@Costo_Total", reserva.Costo_Total);
                command.Parameters.AddWithValue("@IdUsuarioCreador", reserva.IdUsuarioCreador);

                idReserva = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            var queryPago = @"INSERT INTO pago (idreserva, concepto, monto, fecha, anulado, idusuariocreador)
                              VALUES (@IdReserva, @Concepto, @Monto, @Fecha, 0, @IdUsuarioCreador)";
            using (var cmdPago = new MySqlCommand(queryPago, connection, transaction))
            {
                cmdPago.Parameters.AddWithValue("@IdReserva", idReserva);
                cmdPago.Parameters.AddWithValue("@Concepto", "Seña de reserva");
                cmdPago.Parameters.AddWithValue("@Monto", montoSena);
                cmdPago.Parameters.AddWithValue("@Fecha", DateTime.Today);
                cmdPago.Parameters.AddWithValue("@IdUsuarioCreador", (object?)idUsuario ?? DBNull.Value);
                await cmdPago.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        // var query = @"INSERT INTO Reserva (IdInmueble, IdInquilino, Fecha_Desde, Fecha_Hasta, Fecha_Cancelacion, Monto_Diario, Costo_Total, IdUsuarioCreador)
        //               VALUES (@IdInmueble, @IdInquilino, @Fecha_Desde, @Fecha_Hasta, @Fecha_Cancelacion, @Monto_Diario, @Costo_Total, @IdUsuarioCreador)";
        // using var command = new MySqlCommand(query, connection);
        // command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);
        // command.Parameters.AddWithValue("@IdInquilino", reserva.IdInquilino);
        // command.Parameters.AddWithValue("@Fecha_Desde", reserva.Fecha_Desde);
        // command.Parameters.AddWithValue("@Fecha_Hasta", reserva.Fecha_Hasta);
        // command.Parameters.AddWithValue("@Fecha_Cancelacion", reserva.Fecha_Cancelacion == DateTime.MinValue ? (object)DBNull.Value : reserva.Fecha_Cancelacion);
        // command.Parameters.AddWithValue("@Monto_Diario", reserva.Monto_Diario);
        // command.Parameters.AddWithValue("@Costo_Total", reserva.Costo_Total);
        // command.Parameters.AddWithValue("@IdUsuarioCreador", reserva.IdUsuarioCreador);

        // await command.ExecuteNonQueryAsync();
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

    public async Task FinalizarConMultaAsync(int idReserva, DateTime fechaFinalizacion, decimal montoMulta, int? idUsuario)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var queryReserva = "UPDATE Reserva SET Fecha_Cancelacion = @Fecha AND IdUsuarioAnulador = @IdUsuario WHERE Id = @Id AND Fecha_Cancelacion IS NULL";
            using (var cmdReserva = new MySqlCommand(queryReserva, connection, transaction))
            {
                cmdReserva.Parameters.AddWithValue("@Id", idReserva);
                cmdReserva.Parameters.AddWithValue("@Fecha", fechaFinalizacion);
                cmdReserva.Parameters.AddWithValue("@IdUsuario", idUsuario);
                var filasAfectadas = await cmdReserva.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new InvalidOperationException("La reserva no existe o ya había sido finalizada anteriormente.");
                }
            }

            var queryPago = @"INSERT INTO pago (idreserva, concepto, monto, fecha, anulado, idusuariocreador)
                              VALUES (@IdReserva, @Concepto, @Monto, @Fecha, 0, @IdUsuarioCreador)";
            using (var cmdPago = new MySqlCommand(queryPago, connection, transaction))
            {
                cmdPago.Parameters.AddWithValue("@IdReserva", idReserva);
                cmdPago.Parameters.AddWithValue("@Concepto", "Multa por cancelación anticipada");
                cmdPago.Parameters.AddWithValue("@Monto", montoMulta);
                cmdPago.Parameters.AddWithValue("@Fecha", fechaFinalizacion);
                cmdPago.Parameters.AddWithValue("@IdUsuarioCreador", (object?)idUsuario ?? DBNull.Value);
                await cmdPago.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
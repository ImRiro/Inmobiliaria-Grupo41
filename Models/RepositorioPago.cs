using Inmobiliaria_.Net_Core.Models;
using MySqlConnector;

public class RepositorioPago : RepositorioBase, IRepositorioPago
{
    public RepositorioPago(IConfiguration configuration) : base(configuration)
    {

    }

    // LEFT JOIN a usuario (creador/anulador) porque pueden ser NULL (pago viejo sin auditoría, o nunca anulado)
    private const string SelectBase = @"
        SELECT p.Id, p.idreserva, p.concepto, p.monto, p.fecha, p.anulado, p.fecha_anulacion,
               p.idusuariocreador, p.idusuarioanulador,
               i.Nombre AS InqNombre, i.Apellido AS InqApellido,
               inm.direccion AS DireccionInmueble,
               cu.Nombre AS CreadorNombre, cu.Apellido AS CreadorApellido,
               au.Nombre AS AnuladorNombre, au.Apellido AS AnuladorApellido
        FROM pago p
        JOIN reserva r ON r.Id = p.idreserva
        JOIN inquilinos i ON i.Id = r.idinquilino
        JOIN inmueble inm ON inm.Id = r.idinmueble
        LEFT JOIN usuario cu ON cu.Id = p.idusuariocreador
        LEFT JOIN usuario au ON au.Id = p.idusuarioanulador";

    private static Pago LeerPago(MySqlDataReader reader)
    {
        return new Pago
        {
            IdPago = reader.GetInt32(reader.GetOrdinal("Id")),
            IdReserva = reader.GetInt32(reader.GetOrdinal("idreserva")),
            Concepto = reader.GetString(reader.GetOrdinal("concepto")),
            Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
            Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
            Anulado = reader.GetBoolean(reader.GetOrdinal("anulado")),
            FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("fecha_anulacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_anulacion")),
            IdUsuarioCreador = reader.IsDBNull(reader.GetOrdinal("idusuariocreador")) ? null : reader.GetInt32(reader.GetOrdinal("idusuariocreador")),
            IdUsuarioAnulador = reader.IsDBNull(reader.GetOrdinal("idusuarioanulador")) ? null : reader.GetInt32(reader.GetOrdinal("idusuarioanulador")),
            NombreInquilino = $"{reader.GetString(reader.GetOrdinal("InqNombre"))} {(reader.IsDBNull(reader.GetOrdinal("InqApellido")) ? "" : reader.GetString(reader.GetOrdinal("InqApellido")))}".Trim(),
            DireccionInmueble = reader.GetString(reader.GetOrdinal("DireccionInmueble")),
            UsuarioCreadorNombre = reader.IsDBNull(reader.GetOrdinal("CreadorNombre")) ? null : $"{reader.GetString(reader.GetOrdinal("CreadorNombre"))} {reader.GetString(reader.GetOrdinal("CreadorApellido"))}",
            UsuarioAnuladorNombre = reader.IsDBNull(reader.GetOrdinal("AnuladorNombre")) ? null : $"{reader.GetString(reader.GetOrdinal("AnuladorNombre"))} {reader.GetString(reader.GetOrdinal("AnuladorApellido"))}"
        };
    }

    public async Task<List<Pago>> ObtenerPorReservaAsync(int idReserva)
    {
        var lista = new List<Pago>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE p.idreserva = @IdReserva ORDER BY p.fecha DESC, p.Id DESC";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdReserva", idReserva);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(LeerPago(reader));
        }

        return lista;
    }

    public async Task<Pago?> ObtenerPorIdAsync(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = SelectBase + " WHERE p.Id = @Id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return LeerPago(reader);
        }
        return null;
    }

    public async Task<int> CrearAsync(Pago pago)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"INSERT INTO pago (idreserva, concepto, monto, fecha, anulado, idusuariocreador)
                      VALUES (@IdReserva, @Concepto, @Monto, @Fecha, 0, @IdUsuarioCreador);
                      SELECT LAST_INSERT_ID();";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdReserva", pago.IdReserva);
        command.Parameters.AddWithValue("@Concepto", pago.Concepto);
        command.Parameters.AddWithValue("@Monto", pago.Monto);
        command.Parameters.AddWithValue("@Fecha", pago.Fecha);
        command.Parameters.AddWithValue("@IdUsuarioCreador", (object?)pago.IdUsuarioCreador ?? DBNull.Value);

        var nuevoId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(nuevoId);
    }

    public async Task ActualizarConceptoAsync(int id, string concepto)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "UPDATE pago SET concepto = @Concepto WHERE Id = @Id AND anulado = 0";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Concepto", concepto);

        await command.ExecuteNonQueryAsync();
    }

    public async Task AnularAsync(int id, int? idUsuarioAnulador)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = @"UPDATE pago
                      SET anulado = 1, fecha_anulacion = @FechaAnulacion, idusuarioanulador = @IdUsuarioAnulador
                      WHERE Id = @Id AND anulado = 0";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@FechaAnulacion", DateTime.Today);
        command.Parameters.AddWithValue("@IdUsuarioAnulador", (object?)idUsuarioAnulador ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<decimal> ObtenerTotalPagadoAsync(int idReserva)
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var query = "SELECT COALESCE(SUM(monto), 0) FROM pago WHERE idreserva = @IdReserva AND anulado = 0";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdReserva", idReserva);

        var resultado = await command.ExecuteScalarAsync();
        return Convert.ToDecimal(resultado);
    }
}

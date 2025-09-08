using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supabase;
using taller_backend.ContextDB;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;


namespace taller_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionTestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Client _supabase;

        public ConnectionTestController(ApplicationDbContext context, Client supabase)
        {
            _context = context;
            _supabase = supabase;
        }

        /// <summary>
        /// Prueba la conexión a la base de datos mediante Entity Framework
        /// </summary>
        /// <returns>Resultado de la prueba de conexión</returns>
        [HttpGet("database")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();

                return Ok(new
                {
                    success = canConnect,
                    message = canConnect ? "✅ Conexión a Database exitosa" : "❌ No se pudo conectar a la Database",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "❌ Error de conexión a la Database",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

            [HttpGet("server-info")]
            public async Task<IActionResult> GetServerInfo()
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    await connection.OpenAsync();

                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                SELECT 
                    version() as version,
                    current_database() as database,
                    current_user as current_user,
                    inet_server_addr() as server_address,
                    inet_server_port() as server_port,
                    pg_postmaster_start_time() as start_time
            ";

                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        var result = new
                        {
                            Version = reader["version"].ToString(),
                            Database = reader["database"].ToString(),
                            CurrentUser = reader["current_user"].ToString(),
                            ServerAddress = reader["server_address"].ToString(),
                            ServerPort = reader["server_port"].ToString(),
                            StartTime = reader["start_time"].ToString()
                        };

                        await connection.CloseAsync();

                        return Ok(new
                        {
                            success = true,
                            data = result,
                            connectionType = "Session Pooler",
                            timestamp = DateTime.UtcNow
                        });
                    }

                    await connection.CloseAsync();
                    return NotFound();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        error = ex.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
            }

            [HttpGet("tables")]
            public async Task<IActionResult> GetTables()
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    await connection.OpenAsync();

                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                SELECT 
                    table_name,
                    table_schema
                FROM information_schema.tables 
                WHERE table_schema = 'public'
                ORDER BY table_name
            ";

                    var tables = new List<object>();
                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        tables.Add(new
                        {
                            Name = reader["table_name"].ToString(),
                            Schema = reader["table_schema"].ToString()
                        });
                    }

                    await connection.CloseAsync();

                    return Ok(new
                    {
                        success = true,
                        tableCount = tables.Count,
                        tables = tables,
                        timestamp = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        error = ex.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
            }

        [HttpGet("columns/{tableName}")]
        public async Task<IActionResult> GetTableColumns(string tableName, [FromQuery] string schema = "public")
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return BadRequest(new { success = false, message = "El nombre de la tabla es requerido" });
            }

            try
            {
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                        SELECT 
                            column_name,
                            data_type,
                            is_nullable,
                            character_maximum_length,
                            numeric_precision,
                            numeric_scale,
                            column_default,
                            ordinal_position
                        FROM information_schema.columns
                        WHERE table_schema = @schema AND table_name = @table
                        ORDER BY ordinal_position
                    ";

                var schemaParam = command.CreateParameter();
                schemaParam.ParameterName = "@schema";
                schemaParam.Value = schema;
                command.Parameters.Add(schemaParam);

                var tableParam = command.CreateParameter();
                tableParam.ParameterName = "@table";
                tableParam.Value = tableName;
                command.Parameters.Add(tableParam);

                var columns = new List<object>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    columns.Add(new
                    {
                        Name = reader["column_name"].ToString(),
                        DataType = reader["data_type"].ToString(),
                        IsNullable = reader["is_nullable"].ToString(),
                        MaxLength = reader["character_maximum_length"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["character_maximum_length"]),
                        NumericPrecision = reader["numeric_precision"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["numeric_precision"]),
                        NumericScale = reader["numeric_scale"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["numeric_scale"]),
                        Default = reader["column_default"].ToString(),
                        Position = reader["ordinal_position"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ordinal_position"])
                    });
                }

                await connection.CloseAsync();

                return Ok(new
                {
                    success = true,
                    table = tableName,
                    schema = schema,
                    columnCount = columns.Count,
                    columns = columns,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}

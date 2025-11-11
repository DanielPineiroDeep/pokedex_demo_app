using System.Data;
using MySql.Data.MySqlClient; // Es necesario instalar el paquete NUGET con las dependencias y librerías de MySQL
using dotenv.net;

namespace PruebaPokeApi.Services
{
    // Clase Singleton para la conexión y manejo de base de datos MySQL.
    public sealed class DBHandler
    {

        private static readonly Lazy<DBHandler> _instance =
            new Lazy<DBHandler>(() => new DBHandler());

        private readonly string _connectionString;
        private MySqlConnection _connection;

        // Instancia global de la clase DBHandler.
        public static DBHandler Instance => _instance.Value;

        // Constructor privado para impedir instanciación externa.
        private DBHandler()
        {
            try
            {
                // Cargar las variables de entorno
                DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { Path.Combine(AppContext.BaseDirectory, ".env") }));
                // Leer variables de entorno
                string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
                string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
                string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "pruebapokeapi_db";
                string user = Environment.GetEnvironmentVariable("DB_USER") ?? throw new InvalidOperationException("Usuario no definido.");
                string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("Contraseña no definida.");

                // Cadena de conexión obteniendo las variables desde el entorno.
                _connectionString = $"Server={host};Port={port};Database={database};User ID={user};Password={password};SslMode=Disabled;AllowPublicKeyRetrieval=True;";
                _connection = new MySqlConnection(_connectionString);
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Error en configuración de la base de datos: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Obtiene una conexión abierta a la base de datos.
        public MySqlConnection GetConnection()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
            {
                _connection.Open();
                // Console.ForegroundColor = ConsoleColor.Green;
                // Console.WriteLine("Conexión con la DB establecida");
                // Console.ResetColor();
            }
            return _connection;
        }

        // Ejecuta una consulta SELECT y devuelve los resultados en un DataTable.
        public DataTable ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(query, GetConnection());

            if (parameters != null)
            {
                foreach (var param in parameters) {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            using var adapter = new MySqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // Ejecuta una instrucción INSERT, UPDATE o DELETE.
        // Devuelve el número de filas afectadas.
        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(query, GetConnection());

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            return cmd.ExecuteNonQuery();
        }

        // Cierra la conexión actual, si está abierta.
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
                // Console.ForegroundColor = ConsoleColor.Green;
                // Console.WriteLine("Conexión con la DB finalizada");
                // Console.ResetColor();
            }
        }
    }
}

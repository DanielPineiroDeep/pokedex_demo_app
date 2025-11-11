using PruebaPokeApi.Model;
using PruebaPokeApi.Services;
using System.Data;
using System.Globalization;

namespace PruebaPokeApi.Controlador
{
    public class Controller
    {
        public Pokemon BuscarPokemonEnBD(string nameOrId)
        {
            if (DBHandler.Instance.GetConnection() != null)
            {
                // Buscar los datos del pokémon en la base de datos
                string query = @"SELECT * FROM Pokemon WHERE n_dex = @id OR nombre = @name;";
                // Para prevenir inyecciones SQL se pasan los datos para la consulta como parámetro
                var parameters = new Dictionary<string, object>();
                // Conversión a número por si es un id
                if (int.TryParse(nameOrId, out int dex))
                {
                    parameters["@id"] = dex;
                }
                else
                {
                    // Valor que no existe en la base de datos por si el parámetro no es un número entero
                    parameters["@id"] = -1;
                }
                parameters["@name"] = nameOrId;
                // Resultado de la búsqueda
                DataTable query_result = DBHandler.Instance.ExecuteQuery(query, parameters);
                if (query_result.Rows.Count > 0)
                {
                    int n_dex = int.Parse(query_result.Rows[0]["n_dex"].ToString());
                    string nombre = query_result.Rows[0]["nombre"].ToString();
                    string t_primario = query_result.Rows[0]["t_primario"].ToString();
                    string t_secundario = query_result.Rows[0]["t_secundario"].ToString();
                    string altura = query_result.Rows[0]["altura"].ToString();
                    string peso = query_result.Rows[0]["peso"].ToString();
                    return new Pokemon(n_dex, nombre, t_primario, t_secundario, altura, peso);
                } else
                {
                    return null;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error al inicializar la conexión a la base de datos");
                Console.ResetColor();
                return null;
            }
        }

        public async Task BuscarPokemonEnAPIAsync(string nameOrId)
        {
            // Instanciar el servicio de conexión a la API
            PokeApiClient client = new PokeApiClient();

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var response = await client.GetPokemonAsync(nameOrId);
            if (response is not null)
            {
                string st_n_dex = response.id;
                int n_dex = int.Parse(st_n_dex);
                string nombre = response.name;
                nombre = textInfo.ToTitleCase(nombre);
                string t_primario = "", t_secundario = "";
                if (response.types != null)
                {
                    bool primerTipo = true;
                    foreach (var tipo in response.types)
                    {
                        var nombreTipo = tipo.type?.name?.ToString();
                        if (!string.IsNullOrEmpty(nombreTipo))
                        {
                            switch (nombreTipo.ToLower())
                            {
                                case "fire":
                                    nombreTipo = "Fuego";
                                    break;
                                case "water":
                                    nombreTipo = "Agua";
                                    break;
                                case "grass":
                                    nombreTipo = "Planta";
                                    break;
                                case "electric":
                                    nombreTipo = "Eléctrico";
                                    break;
                                case "ice":
                                    nombreTipo = "Hielo";
                                    break;
                                case "fighting":
                                    nombreTipo = "Lucha";
                                    break;
                                case "poison":
                                    nombreTipo = "Veneno";
                                    break;
                                case "ground":
                                    nombreTipo = "Tierra";
                                    break;
                                case "flying":
                                    nombreTipo = "Volador";
                                    break;
                                case "psychic":
                                    nombreTipo = "Psíquico";
                                    break;
                                case "bug":
                                    nombreTipo = "Bicho";
                                    break;
                                case "rock":
                                    nombreTipo = "Roca";
                                    break;
                                case "ghost":
                                    nombreTipo = "Fantasma";
                                    break;
                                case "dragon":
                                    nombreTipo = "Dragón";
                                    break;
                                case "dark":
                                    nombreTipo = "Siniestro";
                                    break;
                                case "steel":
                                    nombreTipo = "Acero";
                                    break;
                                case "fairy":
                                    nombreTipo = "Hada";
                                    break;
                                default:
                                    nombreTipo = "Normal";
                                    break;
                            }
                            if (!primerTipo)
                            {
                                nombreTipo = textInfo.ToTitleCase(nombreTipo);
                                t_secundario = nombreTipo;
                            }
                            else
                            {
                                nombreTipo = textInfo.ToTitleCase(nombreTipo);
                                t_primario = nombreTipo;
                                primerTipo = false;
                            }
                        }
                    }
                }
                string altura = response.height;
                string peso = response.weight;

                // Guardar los datos del Pokémon en la base de datos
                Pokemon pokemon = new Pokemon(n_dex, nombre, t_primario, t_secundario, altura, peso);
                GuardarBusquedaEnBD(pokemon);

                // Cerrar la conexiñon con la API y liberar los recursos
                client.Dispose();
            }
        }

        public int GuardarBusquedaEnBD(Pokemon pokemon) {
            // Insertar los datos del Pokémon en la base de datos
            string query = @"INSERT INTO Pokemon VALUES (@n_dex, @nombre, @t_p, @t_s, @altura, @peso);";
            // Para prevenir inyecciones SQL se pasan los datos para la consulta como parámetro
            var parameters = new Dictionary<string, object>();
            parameters["@n_dex"] = pokemon.Id;
            parameters["@nombre"] = pokemon.Nombre;
            parameters["@t_p"] = pokemon.TipoPrincipal;
            parameters["@t_s"] = string.IsNullOrEmpty(pokemon.TipoSecundario) ? DBNull.Value : pokemon.TipoSecundario;
            parameters["@altura"] = pokemon.Altura;
            parameters["@peso"] = pokemon.Peso;
            return DBHandler.Instance.ExecuteNonQuery(query, parameters);
        }

        public void CerrarConexionConDB()
        {
            DBHandler.Instance.CloseConnection();
        }
    }
}

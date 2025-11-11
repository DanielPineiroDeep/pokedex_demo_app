using Newtonsoft.Json;

namespace PruebaPokeApi.Services
{
    public class PokeApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://pokeapi.co/api/v2/";

        public PokeApiClient(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // Obtiene un Pokémon por su nombre o ID.
        public async Task<dynamic?> GetPokemonAsync(string nameOrId)
        {
            return await GetAsync($"pokemon/{nameOrId}");
        }

        // Obtiene información sobre una habilidad.
        public async Task<dynamic?> GetAbilityAsync(string name)
        {
            return await GetAsync($"ability/{name}");
        }

        // Obtiene información sobre un tipo.
        public async Task<dynamic?> GetTypeAsync(string name)
        {
            return await GetAsync($"type/{name}");
        }

        // Método genérico para hacer solicitudes GET y deserializar JSON.
        private async Task<dynamic?> GetAsync(string endpoint)
        {
            try
            {
                // Llamada asíncrona a la API
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(json);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return null;
            }
        }

        // Método para cerrar la conexión y liberar los recursos
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}

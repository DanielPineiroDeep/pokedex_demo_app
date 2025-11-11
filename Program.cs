using PruebaPokeApi.Controlador;
using PruebaPokeApi.View;

namespace PruebaPokeApi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var view = new AppView(new Controller());
            await view.MostrarMenuAsync();
        }
    }
}

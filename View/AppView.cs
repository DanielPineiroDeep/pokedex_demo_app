using PruebaPokeApi.Controlador;
using PruebaPokeApi.Model;

namespace PruebaPokeApi.View
{
    public class AppView
    {
        public Controller Controller { get; set; }
        public AppView(Controller controller)
        {
            Controller = controller;
        }
        public async Task MostrarMenuAsync()
        {
            // Inicializar la variable que contiene la opción del menú con una opción que no esté en el rango para la ejecución controlada del bucle
            int opcion = -1;
            // Mostrar el menú
            do
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ POKÉDEX ===");
                Console.WriteLine(" 1. Buscar Pokémon por su número en la Pokédex o por su nombre \n\t(Recuerde que hay documentadas 1025 especies Pokémon diferentes)");
                Console.WriteLine(" 0. Salir de la aplicación");
                Console.Write("Seleccione una opción: ");

                // Validar que la entrada sea numérica
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = -1;
                }

                switch (opcion)
                {
                    case 1:
                        Console.Write("Introduzca el número o el nombre del Pokémon a buscar: ");
                        string entrada = Console.ReadLine()?.Trim() ?? "";

                        // Presentar la información del Pokémon
                        await MostrarDatosPokemonAsync(entrada);

                        Console.WriteLine("\nPresione una tecla para volver al menú...");
                        Console.ReadKey();
                        break;

                    case 0:
                        Console.WriteLine("Saliendo de la aplicación...");
                        Controller.CerrarConexionConDB();
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 0);
        }

        public async Task MostrarDatosPokemonAsync(string nameOrId)
        {
            if (Controller.BuscarPokemonEnBD(nameOrId) is null)
            {
                await Controller.BuscarPokemonEnAPIAsync(nameOrId);
            }

            Pokemon pokemon = Controller.BuscarPokemonEnBD(nameOrId);
            if (pokemon is not null)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"|\tNºPokédex: \t{pokemon.Id}");
                Console.WriteLine($"|\tNombre: \t{pokemon.Nombre}");
                Console.Write($"|\tTipo: \t");
                ColorearConsolaPorTipo(pokemon.TipoPrincipal);
                Console.Write($"{pokemon.TipoPrincipal}");
                Console.ResetColor();
                if (!string.IsNullOrEmpty(pokemon.TipoSecundario))
                {
                    Console.Write(" | ");
                    ColorearConsolaPorTipo(pokemon.TipoSecundario);
                    Console.Write($"{pokemon.TipoSecundario}");
                    Console.ResetColor();
                }
                Console.WriteLine();
                Console.WriteLine($"|\tAltura: \t{pokemon.Altura}");
                Console.WriteLine($"|\tPeso: \t{pokemon.Peso}");
                Console.WriteLine("-------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("\nNo se encontró un Pokémon con ese número o ese nombre.");
            }
        }
        public static void ColorearConsolaPorTipo(string nombreTipo)
        {
            Console.ForegroundColor = nombreTipo switch
            {
                "Fuego" => ConsoleColor.Red,
                "Agua" => ConsoleColor.Blue,
                "Planta" => ConsoleColor.Green,
                "Eléctrico" => ConsoleColor.Yellow,
                "Hielo" => ConsoleColor.Cyan,
                "Lucha" => ConsoleColor.DarkRed,
                "Veneno" => ConsoleColor.Magenta,
                "Tierra" => ConsoleColor.DarkYellow,
                "Volador" => ConsoleColor.Cyan,
                "Psíquico" => ConsoleColor.Magenta,
                "Bicho" => ConsoleColor.DarkGreen,
                "Roca" => ConsoleColor.DarkGray,
                "Fantasma" => ConsoleColor.DarkMagenta,
                "Dragón" => ConsoleColor.DarkBlue,
                "Siniestro" => ConsoleColor.DarkGray,
                "Acero" => ConsoleColor.Gray,
                "Hada" => ConsoleColor.Magenta,
                "Normal" => ConsoleColor.White,
                _ => ConsoleColor.White,
            };
        }
    }
}

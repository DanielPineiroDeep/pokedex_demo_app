namespace PruebaPokeApi.Model
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoPrincipal { get; set; }
        public string TipoSecundario { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }

        // Constructor
        public Pokemon(int id, string nombre, string tipoPrincipal, string? tipoSecundario, string altura, string peso)
        {
            Id = id;
            Nombre = nombre;
            TipoPrincipal = tipoPrincipal;
            TipoSecundario = tipoSecundario ?? "";
            Altura = altura;
            Peso = peso;
        }
    }
}

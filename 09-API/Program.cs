using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _09_API
{
    internal class Program
    {
        static readonly HttpClient client = new();

        public static async Task Main()
        {
            string url = "https://pokeapi.co/api/v2/pokemon/";

            Console.WriteLine("Puedes buscar información de un pokémon por medio de ID o Nombre.");
            int opcion;
            do
            {
                Console.WriteLine("¿Quieres hacer una búsqueda?");
                Console.WriteLine("\t1-Si");
                Console.WriteLine("\t2-No");
                opcion = Int32.Parse(Console.ReadLine());
                if(opcion == 2)
                {
                    break;
                }
                Console.WriteLine("¿De qué pokémon necesitas tener información?");
                string? input = Console.ReadLine();
                if (input != null)
                {
                    string nombrePokemon = input;
                    string urlCompleta = (url + nombrePokemon).ToLower();
                    await HacerSolicitudGet(urlCompleta);
                    Console.WriteLine("\n\n");
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, introduce el nombre de un Pokémon.");
                }
            }
            while(opcion != 2);

        }

        public static async Task HacerSolicitudGet(string urlCompleta)
        {
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(urlCompleta))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonSerializer.Deserialize<Pokemon>(responseBody);
                    if (pokemon != null)
                    {
                        ImprimirPokemon(pokemon);
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("¡Error encontrado! " + e.Message);
            }
        }

        public static void ImprimirPokemon(Pokemon pokemon)
        {
            Console.WriteLine($"Nombre: {pokemon.name}");
            Console.WriteLine($"ID: {pokemon.id}");
            Console.WriteLine($"Altura: {pokemon.height / 10.0} m"); // Convertir decímetros a metros
            Console.WriteLine($"Peso: {pokemon.weight / 10.0} kg"); // Convertir hectogramos a kilogramos

            Console.WriteLine("Tipos:");
            if (pokemon.types != null && pokemon.types.Count > 0)
            {
                foreach (var type in pokemon.types)
                {
                    Console.WriteLine($"- {type.type.name}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron tipos.");
            }

            Console.WriteLine("Habilidades:");
            if (pokemon.abilities != null && pokemon.abilities.Count > 0)
            {
                foreach (var ability in pokemon.abilities)
                {
                    Console.WriteLine($"- {ability.ability.name} (Oculta: {ability.isHidden})");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron habilidades.");
            }
        }
    }

    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int height { get; set; }
        public int weight { get; set; }
        public List<PokemonType> types { get; set; } = [];
        public List<PokemonAbility> abilities { get; set; } = [];
    }

    public class PokemonType
    {
        public int slot { get; set; }
        public TypeDetail type { get; set; } = new();
    }

    public class TypeDetail
    {
        public string name { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
    }

    public class PokemonAbility
    {
        public AbilityDetail ability { get; set; } = new();
        public bool isHidden { get; set; }
        public int slot { get; set; }
    }

    public class AbilityDetail
    {
        public string name { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
    }
}

using System.Globalization;

namespace MJU23v_DTP_T1
{
    public class Language
    {
        public string family, group;
        public string language, area, link;
        public int pop;
        public Language(string line)
        {
            string[] field = line.Split("|");
            family = field[0];
            group = field[1];
            language = field[2];
            pop = (int)double.Parse(field[3], CultureInfo.InvariantCulture);
            area = field[4];
            link = field[5];
        }
        public void Print()
        {
            Console.WriteLine($"Language {language}:");
            Console.Write($"  family: {family}");
            if (group != "")
                Console.Write($">{group}");
            Console.WriteLine($"\n  population: {pop}");
            Console.WriteLine($"  area: {area}");
        }
    }

    public class Program
    {
        // Plan:
        // 1. Lägg till planeringskommentarer. (KLAR)
        // 2. Skapa en kommandoloop med stöd för 'help' och 'quit'. (KLAR)
        // 3. Implementera kommandot 'list group <groupname>'. (KLAR)
        // 4. Implementera kommandot 'list country <countryname>'. (PÅGÅR)
        // 5. Implementera kommandot 'show language <languagename>'.
        // 6. Lägg till kommentarer för NYI-kommandon.
        // 7. Lägg till felhanteringskommentarer (FIXME).
        // 8. Testa och säkerställ att de fyra grundläggande kommandona fungerar.
        // 9. Refaktorera om det behövs (TBD).
        // 10. Gör slutlig testning och dokumentation.

        static string dir = @"..\..\..";
        static List<Language> eulangs = new List<Language>();

        static void Main(string[] arg)
        {
            using (StreamReader sr = new StreamReader($"{dir}\\lang.txt"))
            {
                Language lang;
                string line = sr.ReadLine();
                while (line != null)
                {
                    lang = new Language(line);
                    eulangs.Add(lang);
                    line = sr.ReadLine();
                }
            }

            CommandLoop();
        }

        static void CommandLoop()
        {
            Console.WriteLine("Welcome to the European Languages Program!");
            Console.WriteLine("Type 'help' for a list of commands or 'quit' to exit.");

            string command;
            do
            {
                Console.Write("> ");
                command = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(command)) continue;

                ProcessCommand(command);
            } while (!command.Equals("quit", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("Goodbye!");
        }

        static void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            switch (parts[0].ToLower())
            {
                case "help":
                    PrintHelp();
                    break;
                case "quit":
                    // Do nothing; loop will exit.
                    break;
                case "list":
                    if (parts.Length > 2 && parts[1].ToLower() == "group")
                        ListGroup(parts[2]);
                    else if (parts.Length > 2 && parts[1].ToLower() == "country")
                        ListCountry(parts[2]);
                    else
                        Console.WriteLine("Invalid 'list' command.");
                    break;
                default:
                    Console.WriteLine($"Unknown command: {parts[0]}");
                    break;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  help - Show this help text");
            Console.WriteLine("  quit - Exit the program");
            Console.WriteLine("  list group <groupname> - List all languages in a specific group");
            Console.WriteLine("  list country <countryname> - List all languages spoken in a specific country");
        }

        static void ListGroup(string groupName)
        {
            var languages = eulangs.Where(l => l.group.Contains(groupName, StringComparison.OrdinalIgnoreCase));

            if (!languages.Any())
            {
                Console.WriteLine($"No languages found in the group '{groupName}'.");
                return;
            }

            Console.WriteLine($"Languages in the group '{groupName}':");

            foreach (var lang in languages)
                Console.WriteLine($"- {lang.language}");
        }

        static void ListCountry(string countryName)
        {
            var languages = eulangs.Where(l => l.area.Contains(countryName, StringComparison.OrdinalIgnoreCase));

            if (!languages.Any())
            {
                Console.WriteLine($"No languages found in the country '{countryName}'.");
                return;
            }

            Console.WriteLine($"Languages spoken in '{countryName}':");

            foreach (var lang in languages)
                Console.WriteLine($"- {lang.language}");
        }
    }
}

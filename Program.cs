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
        // 4. Implementera kommandot 'list country <countryname>'. (KLAR)
        // 5. Implementera kommandot 'show language <languagename>'. (KLAR)
        // 6. Lägg till kommentarer för NYI-kommandon. (KLAR)
        // 7. Lägg till felhanteringskommentarer (FIXME). (PÅGÅR)
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

                if (string.IsNullOrEmpty(command))
                {
                    // FIXME: Handle empty input gracefully.
                    continue;
                }

                ProcessCommand(command);
            } while (!command.Equals("quit", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("Goodbye!");
        }

        static void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                // FIXME: Handle case where command is empty or invalid.
                return;
            }

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
                        Console.WriteLine("Invalid 'list' command."); // FIXME: Provide more specific error messages.
                    break;
                case "show":
                    if (parts.Length > 1 && parts[1].ToLower() == "language")
                        ShowLanguage(parts.Skip(2).FirstOrDefault());
                    else
                        Console.WriteLine("Invalid 'show' command."); // FIXME: Provide more specific error messages.
                    break;

                default:
                    Console.WriteLine($"Unknown command: {parts[0]}"); // FIXME: Suggest valid commands to the user.
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
            Console.WriteLine("  show language <languagename> - Show details about a specific language");

            // NYI: Add descriptions for the following commands when implemented:
            //      list between <lownum> and <hinum>
            //      show group <groupname>
            //      show country <countryname>
            //      show between <lownum> and <hinum>
            //      population group <groupname>
        }

        static void ListGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                Console.WriteLine("Please specify a group name.");
                // FIXME: Handle missing or invalid group names.
                return;
            }

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
            if (string.IsNullOrEmpty(countryName))
            {
                Console.WriteLine("Please specify a country name.");
                // FIXME: Handle missing or invalid country names.
                return;
            }

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

        static void ShowLanguage(string languageName)
        {
            if (string.IsNullOrEmpty(languageName))
            {
                Console.WriteLine("Please specify a language name.");
                // FIXME: Handle missing or invalid language names.
                return;
            }

            var language = eulangs.FirstOrDefault(l => l.language.Equals(languageName, StringComparison.OrdinalIgnoreCase));

            if (language != null)
                language.Print();
            else
                Console.WriteLine($"Language '{languageName}' not found.");
            // FIXME: Suggest similar language names if not found.
        }
    }
}

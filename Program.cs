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
        // 7. Lägg till felhanteringskommentarer (FIXME). (KLAR)
        // 8. Testa och säkerställ att de fyra grundläggande kommandona fungerar. (KLAR)
        // 9. Refaktorera om det behövs. (KLAR)
        // 10. Gör slutlig testning och dokumentation. (PÅGÅR)

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

        /// <summary>
        /// Starts the interactive command loop for the program.
        /// Users can type commands to query information about European languages.
        /// </summary>
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
                    Console.WriteLine("No command entered. Please try again.");
                    continue;
                }

                ProcessCommand(command);
            } while (!command.Equals("quit", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("Goodbye!");
        }

        /// <summary>
        /// Processes a user command and executes the corresponding functionality.
        /// </summary>
        /// <param name="command">The user's input command.</param>
        static void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                Console.WriteLine("Invalid command format.");
                return;
            }

            switch (parts[0].ToLower())
            {
                case "help":
                    PrintHelp();
                    break;
                case "quit":
                    break; // Exit loop.
                case "list":
                    HandleListCommand(parts);
                    break;
                case "show":
                    HandleShowCommand(parts);
                    break;
                default:
                    Console.WriteLine($"Unknown command: {parts[0]}");
                    break;
            }
        }

        /// <summary>
        /// Handles commands starting with "list".
        /// </summary>
        static void HandleListCommand(string[] parts)
        {
            if (parts.Length > 2 && parts[1].ToLower() == "group")
                ListGroup(parts[2]);
            else if (parts.Length > 2 && parts[1].ToLower() == "country")
                ListCountry(parts[2]);
            else
                Console.WriteLine("Invalid 'list' command.");
        }

        /// <summary>
        /// Handles commands starting with "show".
        /// </summary>
        static void HandleShowCommand(string[] parts)
        {
            if (parts.Length > 1 && parts[1].ToLower() == "language")
                ShowLanguage(parts.Skip(2).FirstOrDefault());
            else
                Console.WriteLine("Invalid 'show' command.");
        }

        /// <summary>
        /// Prints a list of available commands.
        /// </summary>
        static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  help - Show this help text");
            Console.WriteLine("  quit - Exit the program");
            Console.WriteLine("  list group <groupname> - List all languages in a specific group");
            Console.WriteLine("  list country <countryname> - List all languages spoken in a specific country");
            Console.WriteLine("  show language <languagename> - Show details about a specific language");

            // NYI: Add descriptions for future commands when implemented.
        }

        static void ListGroup(string groupName)
        {
            if (!ValidateInput(groupName, "group name")) return;

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
            if (!ValidateInput(countryName, "country name")) return;

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
            if (!ValidateInput(languageName, "language name")) return;

            var language = eulangs.FirstOrDefault(l => l.language.Equals(languageName, StringComparison.OrdinalIgnoreCase));

            if (language != null)
                language.Print();
            else
                Console.WriteLine($"Language '{languageName}' not found.");
        }

        static bool ValidateInput(string input, string inputType)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine($"Please specify a valid {inputType}.");
                return false;
            }

            return true;
        }
    }
}

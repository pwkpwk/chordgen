using System.Xml.Linq;

namespace chordgen
{
    internal class Options
    {
        private static readonly IDictionary<string, ParameterProcessor> _processors = MakeParameterProcessors();
        private const int FRETS_LIMIT = 24;
        private readonly Random _random;
        private delegate int ParameterProcessor(string[] args, int index, Options options);
        private readonly ISet<string> _characters = new HashSet<string>();
        private readonly IList<string> _stringNames = new List<string>();
        private readonly IList<GuitarString> _strings = new List<GuitarString>();
        private readonly Lazy<string[]> _charactersArray;

        public Options(string[] args, Random random)
        {
            _random = random;
            ChordCount = 0;
            FretCount = 12;
            bool failed = false;

            _charactersArray = new Lazy<string[]>(() =>
            {
                if (_characters.Count == 0)
                {
                    _characters.Add(string.Empty);
                }
                return _characters.ToArray();
            });

            for (int i = 0; i < args.Length; )
            {
                int skip = ProcessParameter(args, i);

                if (skip > 0)
                {
                    i += skip;
                }
                else
                {
                    ChordCount = 0;
                    failed = true;
                    break;
                }
            }

            if (failed)
            {
                ChordCount = 0;
            }
            else
            {
                if (ChordCount == 0)
                {
                    ChordCount = 10;
                }

                if (_stringNames.Count == 0)
                {
                    AddStringName("E");
                    AddStringName("A");
                    AddStringName("D");
                }

                PopulateStrings();
            }
        }

        public int ChordCount { get; private set; }

        public int FretCount { get; private set; }

        public GuitarString GetRandomString()
        {
            return _strings[_random.Next(_strings.Count)];
        }

        public string GetRandomCharacter()
        {
            if (_characters.Count == 0)
            {
                return string.Empty;
            }

            string[] characters = _charactersArray.Value;
            return characters[_random.Next(characters.Length)];
        }

        private int AddCharacter(string character)
        {
            _characters.Add(character);
            return 1;
        }

        private int AddFretCount(string[] args, int index)
        {
            if (index < args.Length - 1 && int.TryParse(args[index + 1], out int count) && count > 0 && count <= FRETS_LIMIT)
            {
                FretCount = count;
                return 2;
            }

            Console.Error.WriteLine($"Number of frets nust be an integer number from 1 to {FRETS_LIMIT}");
            return 0;
        }

        private int AddStringName(string name)
        {
            if (ChromaticScale.FindPitch(name) >= 0)
            {
                _stringNames.Add(name);
                return 1;
            }

            return 0;
        }

        private void PopulateStrings()
        {
            IDictionary<string, GuitarString> strings = new Dictionary<string, GuitarString>();

            foreach (string name in _stringNames)
            {
                strings.Add(name, new GuitarString(name, FretCount));
            }

            foreach (GuitarString s in strings.Values)
            {
                _strings.Add(s);
            }
        }

        private int ProcessParameter(string[] args, int index)
        {
            int skip = 0;

            if (_processors.TryGetValue(args[index], out var processor))
            {
                skip = processor(args, index, this);
            }
            else if (AddStringName(args[index]) > 0)
            {
                skip = 1;
            }
            else if (int.TryParse(args[index], out var count))
            {
                if (count > 0 && ChordCount == 0)
                {
                    ChordCount = count;
                    skip = 1;
                }
            }

            return skip;
        }

        private static IDictionary<string, ParameterProcessor> MakeParameterProcessors()
        {
            return new SortedDictionary<string, ParameterProcessor>(StringComparer.OrdinalIgnoreCase)
            {
                { "--help", PrintHelp },
                { "--?", PrintHelp },
                { "--major", (args, index, options) => options.AddCharacter(string.Empty) },
                { "--m", (args, index, options) => options.AddCharacter("m") },
                { "--minor", (args, index, options) => options.AddCharacter("m") },
                { "--7", (args, index, options) => options.AddCharacter("7") },
                { "--sus2", (args, index, options) => options.AddCharacter("sus2") },
                { "--sus4", (args, index, options) => options.AddCharacter("sus4") },
                { "--frets", (args, index, options) => options.AddFretCount(args, index) },
            };
        }

        private static int PrintHelp(string[] args, int index, Options options)
        {
            Console.WriteLine(Resources.help);
            return 0; // fail the command line parsing immediately
        }
    }
}

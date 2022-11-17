using System.Diagnostics.Contracts;
using System.Linq;

namespace chordgen
{
    internal class Options
    {
        private static readonly IDictionary<string, ParameterProcessor> _processors = MakeParameterProcessors();
        private readonly Random _random;
        private delegate int ParameterProcessor(string[] args, int index, Options options);
        private readonly ISet<string> _characters = new HashSet<string>();
        private readonly IDictionary<string, GuitarString> _strings = new Dictionary<string, GuitarString>();
        private readonly Lazy<GuitarString[]> _stringsArray;
        private readonly Lazy<string[]> _charactersArray;

        public Options(string[] args, Random random)
        {
            _random = random;
            ChordCount = 0;
            bool failed = false;

            _stringsArray = new Lazy<GuitarString[]>(() => _strings.Values.ToArray());
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

                if (_strings.Count == 0)
                {
                    AddString("E");
                    AddString("A");
                    AddString("D");
                }
            }
        }

        public int ChordCount { get; private set; }

        public GuitarString GetRandomString()
        {
            GuitarString[] strings = _stringsArray.Value;
            return strings[_random.Next(_stringsArray.Value.Length)];
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

        private int AddString(string name)
        {
            GuitarString s = new GuitarString(name);
            if (s.IsEmpty)
            {
                return 0;
            }
            _strings.Add(name, s);
            return 1;
        }

        private int ProcessParameter(string[] args, int index)
        {
            int skip = 0;

            if (_processors.TryGetValue(args[index], out var processor))
            {
                skip = processor(args, index, this);
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
                { "--major", (args, index, options) => options.AddCharacter(string.Empty) },
                { "--m", (args, index, options) => options.AddCharacter("m") },
                { "--7", (args, index, options) => options.AddCharacter("7") },
                { "--sus2", (args, index, options) => options.AddCharacter("sus2") },
                { "--sus4", (args, index, options) => options.AddCharacter("sus4") },
                { "E", (args, index, options) => options.AddString("E") },
                { "A", (args, index, options) => options.AddString("A") },
                { "D", (args, index, options) => options.AddString("D") },
            };
        }
    }
}

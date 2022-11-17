namespace chordgen
{
    internal class GuitarString
    {
        private static readonly string[] _scale = { "A", "A#", "Bb", "B", "C", "C#", "Db", "D", "D#", "Eb", "E", "F", "F#", "Gb", "G", "G#", "Ab" };
        private readonly static string[] _emptyString = { };
        private readonly string _name;
        private readonly string[] _notes;

        public GuitarString(string name)
        {
            _name = name;
            _notes = PickNotes(name);
        }

        public string Name { get { return _name; } }

        public bool IsEmpty { get { return _notes.Length == 0; } }

        public string GetRandomNote(Random random)
        {
            return _notes[random.Next(_notes.Length)];
        }

        private string[] PickNotes(string name)
        {
            for (int index = 0; index < _scale.Length; ++index)
            {
                if (_scale[index].Equals(name))
                {
                    string[] notes = new string[_scale.Length];
                    int readIndex = index;

                    for (int writeIndex = 0; writeIndex < _scale.Length; ++writeIndex)
                    {
                        notes[writeIndex] = _scale[readIndex++];

                        if (readIndex >= _scale.Length)
                        {
                            readIndex = 0;
                        }
                    }

                    return notes;
                }
            }

            return _emptyString;
        }
    }
}

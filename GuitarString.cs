namespace chordgen
{
    internal class GuitarString
    {
        private readonly static IList<Pitch> _emptyString = new List<Pitch>();
        private readonly string _name;
        private readonly IList<Pitch> _notes;

        public GuitarString(string name, int fretCount)
        {
            _name = name;
            _notes = PickNotes(name, fretCount + 1); // Open string through the highest fret
        }

        public string Name { get { return _name; } }

        public bool IsEmpty { get { return _notes.Count == 0; } }

        public string GetRandomNote(Random random)
        {
            return _notes[random.Next(_notes.Count)].GetRandomName(random);
        }

        private IList<Pitch> PickNotes(string name, int fretCount)
        {
            int readIndex = ChromaticScale.FindPitch(name);

            if (readIndex >= 0)
            {
                IList<Pitch> pitches = new List<Pitch>(ChromaticScale.Pitches.Length);

                for (int writeIndex = 0; writeIndex < fretCount; ++writeIndex)
                {
                    pitches.Add(ChromaticScale.Pitches[readIndex++]);

                    if (readIndex >= ChromaticScale.Pitches.Length)
                    {
                        readIndex = 0;
                    }
                }

                return pitches;
            }

            return _emptyString;
        }
    }
}

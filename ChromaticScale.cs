namespace chordgen
{
    internal class ChromaticScale
    {
        private static readonly Pitch[] _pitches = {
            new Pitch("A"),
            new Pitch("A#", "Bb"),
            new Pitch("B"),
            new Pitch("C"),
            new Pitch("C#", "Db"),
            new Pitch("D"),
            new Pitch("D#", "Eb"),
            new Pitch("E"),
            new Pitch("F"),
            new Pitch("F#", "Gb"),
            new Pitch("G"),
            new Pitch("G#", "Ab")
        };

        public static Pitch[] Pitches { get { return _pitches; } }

        public static int FindPitch(string note)
        {
            int index = 0;

            foreach (Pitch pitch in _pitches)
            {
                if (pitch.IsSamePitch(note))
                {
                    return index;
                }
                ++index;
            }

            return -1;
        }
    }
}

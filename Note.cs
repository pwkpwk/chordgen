namespace chordgen
{
    internal class Note
    {
        private readonly char _name;
        private readonly bool _hasSharp;
        private readonly bool _hasFlat;

        public Note(char name, bool hasSharp, bool hasFlat)
        {
            _name = name;
            _hasSharp = hasSharp;
            _hasFlat = hasFlat;
        }
    }
}

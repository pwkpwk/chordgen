namespace chordgen
{
    internal class Pitch
    {
        private readonly string[] _names;

        public Pitch(params string[] names)
        {
            _names = names;
        }

        public bool IsSamePitch(string note)
        {
            foreach (string name in _names)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(name, note))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetRandomName(Random random)
        {
            if (_names.Length == 1)
            {
                return _names[0];
            }

            return _names[random.Next(_names.Length)];
        }
    }
}

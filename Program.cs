namespace chordgen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Options options = new Options(args, random);

            for (int i = 0; i < options.ChordCount; i++)
            {
                GuitarString s = options.GetRandomString();
                string note = s.GetRandomNote(random);
                string character = options.GetRandomCharacter();

                Console.WriteLine($"{s.Name}\t{note}{character}");
            }
        }
    }
}

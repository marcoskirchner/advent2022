using System.Diagnostics;

internal class Input25
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("../../../input25.txt");
        RunPart1(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var sum = lines.Select(Snafu.Parse).Select(s => (long)s).Sum();
        var s = (Snafu)sum;
        Console.WriteLine(s.ToString());
    }

    struct Snafu
    {
        long value;

        public static Snafu Parse(string s)
        {
            var v = 0L;
            foreach (var c in s)
            {
                v *= 5;
                v += c switch
                {
                    '-' => -1,
                    '=' => -2,
                    _ => c - '0',
                };
            }

            return new Snafu() { value = v };
        }

        public override string? ToString()
        {
            var digits = new List<char>();
            var v = value;
            while (v > 0)
            {
                var r = v % 5;
                v = v / 5;
                if (r > 2) v++;

                digits.Add(r switch
                {
                    4 => '-',
                    3 => '=',
                    2 => '2',
                    1 => '1',
                    0 => '0',
                    _ => throw new UnreachableException(),
                });
            }

            digits.Reverse();
            return new string(digits.ToArray().AsSpan());
        }

        public static implicit operator long(Snafu s) { return s.value;}
        public static implicit operator Snafu(long v) { return new Snafu() { value = v}; }
    }
}
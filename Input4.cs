using System.Diagnostics;

class Input4
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input4.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var pairs = lines[i];
            var s1 = pairs.IndexOf("-");
            var s2 = pairs.IndexOf(",", s1);
            var s3 = pairs.IndexOf("-", s2);

            var e1 = int.Parse(pairs[0..s1]);
            var e2 = int.Parse(pairs[(s1 + 1)..s2]);
            var e3 = int.Parse(pairs[(s2 + 1)..s3]);
            var e4 = int.Parse(pairs[(s3 + 1)..]);

            if ((e1 <= e3 && e2 >= e4) || (e1 >= e3 && e2 <= e4))
            {
                sum++;
            }
        }
        System.Console.WriteLine(sum);
    }

    private static void RunPart2(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var pairs = lines[i];
            var s1 = pairs.IndexOf("-");
            var s2 = pairs.IndexOf(",", s1);
            var s3 = pairs.IndexOf("-", s2);

            var e1 = int.Parse(pairs[0..s1]);
            var e2 = int.Parse(pairs[(s1 + 1)..s2]);
            var e3 = int.Parse(pairs[(s2 + 1)..s3]);
            var e4 = int.Parse(pairs[(s3 + 1)..]);

            if ((e1 <= e4 && e2 >= e3) || (e1 <= e4 && e2 >= e3))
            {
                sum++;
            }
        }
        System.Console.WriteLine(sum);
    }
}

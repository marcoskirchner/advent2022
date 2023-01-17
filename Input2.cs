using System.Diagnostics;

class Input2
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input2.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var l = lines[i];
            var p1 = l[0] - 'A';
            var p2 = l[2] - 'X';
            /*
            0 - rock
            1 - paper
            2 - scissors
            */

            if (p1 == p2)
            {
                // draw
                sum += 3;
            }
            else if (p2 == ((p1 + 1) % 3))
            {
                sum += 6;
            }
            sum += p2 + 1; // indexes are 0 based
        }

        System.Console.WriteLine(sum);
    }

    private static void RunPart2(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var l = lines[i];
            var p1 = l[0] - 'A';
            var expected_result = l[2] - 'X';
            /*
            0 - rock
            1 - paper
            2 - scissors
            */

            var (p2_offset, points) = expected_result switch
            {
                0 => (2, 0),
                1 => (0, 3),
                2 => (1, 6),
                _ => throw new UnreachableException(),
            };

            var p2 = (p1 + p2_offset) % 3;
            sum += points + p2 + 1;
        }

        System.Console.WriteLine(sum);
    }
}

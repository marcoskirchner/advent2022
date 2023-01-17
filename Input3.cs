using System.Diagnostics;

class Input3
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input3.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var rucksack = lines[i];
            var splitPoint = rucksack.Length / 2;
            var c1 = rucksack[0..(splitPoint)].ToCharArray();
            var c2 = rucksack[splitPoint..].ToCharArray();
            var common = c1.First(c => c2.Contains(c));
            var priority = 0;
            if (common >= 'a' && common <= 'z')
            {
                priority = common - 'a' + 1;
            }
            else
            {
                priority = common - 'A' + 27;
            }
            sum += priority;
        }
        System.Console.WriteLine(sum);
    }

    private static void RunPart2(string[] lines)
    {
        var sum = 0;
        for (int i = 0; i < lines.Length; i += 3)
        {
            var rucksack1 = lines[i + 0];
            var rucksack2 = lines[i + 1];
            var rucksack3 = lines[i + 2];
            var common = rucksack1.Intersect(rucksack2).Intersect(rucksack3).Single();

            var priority = 0;
            if (common >= 'a' && common <= 'z')
            {
                priority = common - 'a' + 1;
            }
            else
            {
                priority = common - 'A' + 27;
            }
            sum += priority;
        }
        System.Console.WriteLine(sum);
    }
}

using System.Diagnostics;

class Input6
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input6.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            var data = lines[i].AsSpan();
            int startPos;
            for (startPos = 0; startPos < data.Length; startPos++)
            {
                if (data[startPos..(startPos + 4)].ToArray().Distinct().Count() == 4)
                    break;
            }
            System.Console.WriteLine(startPos + 4);
        }
        System.Console.WriteLine();
    }

    private static void RunPart2(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            var data = lines[i].AsSpan();
            int startPos;
            for (startPos = 0; startPos < data.Length; startPos++)
            {
                if (data[startPos..(startPos + 14)].ToArray().Distinct().Count() == 14)
                    break;
            }
            System.Console.WriteLine(startPos + 14);
        }
        System.Console.WriteLine();

    }
}

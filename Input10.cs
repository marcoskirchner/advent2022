using System.Diagnostics;

class Input10
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input10.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var x = 1;
        var cycle = 0;
        var instruction = -1;
        var instructionCycles = 0;
        var instEffect = 0;

        var sumOfSignalStrength = 0;

        while (true)
        {
            cycle++;

            if (instructionCycles == 0)
            {
                x += instEffect;
                instruction++;
                if (instruction == lines.Length)
                {
                    break;
                }

                if (lines[instruction][0] == 'n')
                {
                    instructionCycles = 1;
                    instEffect = 0;
                }
                else
                {
                    instructionCycles = 2;
                    instEffect = int.Parse(lines[instruction][5..]);
                }
            }

            instructionCycles--;

            if (new[] { 20, 60, 100, 140, 180, 220 }.Contains(cycle))
            {
                sumOfSignalStrength += x * cycle;
            }
        }

        System.Console.WriteLine(sumOfSignalStrength);
    }

    private static void RunPart2(string[] lines)
    {
        var x = 1;
        var cycle = 0;
        var instruction = -1;
        var instructionCycles = 0;
        var instEffect = 0;
        var crtPos = 0;

        while (true)
        {
            cycle++;

            if (instructionCycles == 0)
            {
                x += instEffect;
                instruction++;
                if (instruction == lines.Length)
                {
                    break;
                }

                if (lines[instruction][0] == 'n')
                {
                    instructionCycles = 1;
                    instEffect = 0;
                }
                else
                {
                    instructionCycles = 2;
                    instEffect = int.Parse(lines[instruction][5..]);
                }
            }

            instructionCycles--;

            if (crtPos == x || crtPos == x - 1 || crtPos == x + 1)
            {
                System.Console.Write('#');
            }
            else
            {
                System.Console.Write('.');
            }
            crtPos++;
            if (crtPos == 40)
            {
                System.Console.WriteLine();
                crtPos = 0;
            }
        }

    }
}

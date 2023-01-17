internal class Input18
{
    const int AIR = 0;
    const int LAVA = 1;
    const int STEAM = 2;

    internal static void Run()
    {
        var lines = File.ReadAllLines("input18.txt");
        var input = ReadInput(lines);
        RunPart1(input);
        RunPart2(input);
    }

    private static byte[,,] ReadInput(string[] lines)
    {
        var input = new byte[23, 23, 23];
        foreach (var line in lines)
        {
            var pos = line.Split(',').Select(int.Parse).ToArray();
            input[pos[0] + 1, pos[1] + 1, pos[2] + 1] = LAVA;
        }
        return input;
    }

    private static void RunPart1(byte[,,] input)
    {
        Console.WriteLine(CountSurfaces(input, AIR));
    }

    private static void RunPart2(byte[,,] input)
    {
        var limit = input.GetLength(0) - 1;

        ExpandSteam(0, 0, 0);
        Console.WriteLine(CountSurfaces(input, STEAM));

        void ExpandSteam(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0)
                return;
            if (x > limit || y > limit || z > limit)
                return;

            if (input[x, y, z] != AIR)
                return;

            input[x, y, z] = STEAM;
            ExpandSteam(x - 1, y, z);
            ExpandSteam(x + 1, y, z);
            ExpandSteam(x, y - 1, z);
            ExpandSteam(x, y + 1, z);
            ExpandSteam(x, y, z - 1);
            ExpandSteam(x, y, z + 1);
        }
    }

    private static int CountSurfaces(byte[,,] input, int type)
    {
        var limit = input.GetLength(0) - 1;
        var sum = 0;
        for (int x = 0; x <= limit; x++)
        {
            for (int y = 0; y <= limit; y++)
            {
                for (int z = 0; z <= limit; z++)
                {
                    if (input[x, y, z] == LAVA)
                    {
                        if (input[x - 1, y, z] == type)
                        {
                            sum++;
                        }
                        if (input[x + 1, y, z] == type)
                        {
                            sum++;
                        }
                        if (input[x, y - 1, z] == type)
                        {
                            sum++;
                        }
                        if (input[x, y + 1, z] == type)
                        {
                            sum++;
                        }
                        if (input[x, y, z - 1] == type)
                        {
                            sum++;
                        }
                        if (input[x, y, z + 1] == type)
                        {
                            sum++;
                        }
                    }
                }
            }
        }
        return sum;
    }
}

using System.Diagnostics;

class Input14
{
    const int AIR = 0;
    const int ROCK = 1;
    const int SOURCE = 2;
    const int SAND = 3;

    internal static void Run()
    {
        var lines = File.ReadAllLines("input14.txt");
        var cave = ReadInput(lines);
        //Draw(cave);
        RunPart1(cave);
        cave = ReadInput(lines);
        RunPart2(cave);
    }

    private static void Draw(byte[,] cave)
    {
        Thread.Sleep(50);
        Console.Clear();
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = 0, maxY = 0;
        for (int i = 0; i < cave.GetLength(0); i++)
        {
            for (int j = 0; j < cave.GetLength(1); j++)
            {
                if (cave[i, j] != AIR)
                {
                    if (i < minX) minX = i;
                    if (i > maxX) maxX = i;
                    if (j < minY) minY = j;
                    if (j > maxY) maxY = j;
                }
            }
        }

        for (int j = minY; j <= maxY; j++)
        {
            for (int i = minX; i <= maxX; i++)
            {
                var form = cave[i, j] switch
                {
                    AIR => '.',
                    ROCK => '#',
                    SOURCE => '+',
                    SAND => 'o',
                };
                System.Console.Write(form);
            }
            System.Console.WriteLine();
        }
    }

    private static byte[,] ReadInput(string[] lines)
    {
        byte[,] cave = new byte[1000, 200];
        for (int i = 0; i < lines.Length; i++)
        {
            var coords = lines[i].Split(new[] { " -> ", "," }, StringSplitOptions.None).Select(int.Parse).ToArray();
            var prevX = coords[0];
            var prevY = coords[1];
            for (int j = 2; j < coords.Length; j += 2)
            {
                var currentX = coords[j];
                var currentY = coords[j + 1];
                var signX = Math.Sign(currentX - prevX);
                var signY = Math.Sign(currentY - prevY);

                cave[prevX, prevY] = ROCK;
                while (prevX != currentX || prevY != currentY)
                {
                    prevX += signX;
                    prevY += signY;
                    cave[prevX, prevY] = ROCK;
                }
            }
        }
        cave[500, 0] = SOURCE;
        return cave;
    }

    private static void RunPart1(byte[,] cave)
    {
        var isSandFlowing = false;
        var restedSand = 0;

        while (!isSandFlowing)
        {
            var sandPos = (x: 500, y: 0);
            var rested = false;
            while (!rested)
            {
                if (sandPos.y + 1 == cave.GetLength(1))
                {
                    isSandFlowing = true;
                    break;
                }
                if (cave[sandPos.x, sandPos.y + 1] == AIR)
                {
                    sandPos.y++;
                }
                else if (cave[sandPos.x - 1, sandPos.y + 1] == AIR)
                {
                    sandPos.x--;
                    sandPos.y++;
                }
                else if (cave[sandPos.x + 1, sandPos.y + 1] == AIR)
                {
                    sandPos.x++;
                    sandPos.y++;
                }
                else
                {
                    rested = true;
                    restedSand++;
                    cave[sandPos.x, sandPos.y] = SAND;
                }
            }
            //Draw(cave);
        }
        System.Console.WriteLine(restedSand);
    }

    private static void RunPart2(byte[,] cave)
    {
        int maxY = 0;
        for (int i = 0; i < cave.GetLength(0); i++)
        {
            for (int j = 0; j < cave.GetLength(1); j++)
            {
                if (cave[i, j] != AIR)
                {
                    if (j > maxY) maxY = j;
                }
            }
        }
        maxY += 2;
        for (int i = 0; i < cave.GetLength(0); i++)
        {
            cave[i, maxY] = ROCK;
        }
        

        var isSandBlocked = false;
        var restedSand = 0;

        while (!isSandBlocked)
        {
            var sandPos = (x: 500, y: 0);
            var rested = false;
            while (!rested)
            {
                if (cave[sandPos.x, sandPos.y + 1] == AIR)
                {
                    sandPos.y++;
                }
                else if (cave[sandPos.x - 1, sandPos.y + 1] == AIR)
                {
                    sandPos.x--;
                    sandPos.y++;
                }
                else if (cave[sandPos.x + 1, sandPos.y + 1] == AIR)
                {
                    sandPos.x++;
                    sandPos.y++;
                }
                else
                {
                    rested = true;
                    restedSand++;
                    cave[sandPos.x, sandPos.y] = SAND;
                    if (sandPos.x == 500 && sandPos.y == 0) {
                        isSandBlocked = true;
                    }
                }
            }
            //Draw(cave);
        }
        System.Console.WriteLine(restedSand);
    }
}

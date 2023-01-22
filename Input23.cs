internal class Input23
{
    const int BORDER_PADDING = 52;

    const byte ELF = 1 << 7;
    const byte MOVING = 1 << 6;
    const byte PROPOSES_MASK = 0b11;

    const byte POS_NW = 1 << 7;
    const byte POS_N = 1 << 6;
    const byte POS_NE = 1 << 5;
    const byte POS_W = 1 << 4;
    const byte POS_E = 1 << 3;
    const byte POS_SW = 1 << 2;
    const byte POS_S = 1 << 1;
    const byte POS_SE = 1 << 0;

    internal static void Run()
    {
        var lines = File.ReadAllLines("../../../input23.txt");
        var map = ReadInput(lines);

        List<(byte checkMask, int moveX, int moveY)> checks = new()
        {
            (POS_N | POS_NE | POS_NW, 0, -1),
            (POS_S | POS_SE | POS_SW, 0, 1),
            (POS_W | POS_NW | POS_SW, -1, 0),
            (POS_E | POS_NE | POS_SE, 1, 0),
        };

        RunPart1(map, checks);
        RunPart2(map, checks);
    }

    private static byte[,] ReadInput(string[] lines)
    {
        var elves = new byte[
            lines.Length + BORDER_PADDING + BORDER_PADDING,
            lines[0].Length + BORDER_PADDING + BORDER_PADDING];

        for (var line = 0; line < lines.Length; line++)
        {
            for (var col = 0; col < lines[line].Length; col++)
            {
                if (lines[line][col] == '#')
                {
                    elves[line + BORDER_PADDING, col + BORDER_PADDING] = ELF;
                }
            }
        }
        return elves;
    }

    private static void RunPart1(byte[,] map, List<(byte checkMask, int moveX, int moveY)> checks)
    {
        for (int round = 0; round < 10; round++)
        {
            DoRound(map, checks);
        }

        var numElfs = 0;
        var top = map.GetLength(0);
        var bottom = 0;
        var left = map.GetLength(1);
        var right = 0;

        var numLines = map.GetLength(0);
        var numCols = map.GetLength(1);
        for (var line = 0; line < numLines; line++)
        {
            for (var col = 0; col < numCols; col++)
            {
                var cell = map[line, col];
                if ((cell & ELF) > 0)
                {
                    numElfs++;
                    if (line < top) top = line;
                    if (line > bottom) bottom = line;
                    if (col < left) left = col;
                    if (col > right) right = col;
                }
            }
        }
        Console.WriteLine((bottom - top + 1) * (right - left + 1) - numElfs);
    }

    private static void RunPart2(byte[,] map, List<(byte checkMask, int moveX, int moveY)> checks)
    {
        var round = 11;
        while (DoRound(map, checks))
        {
            round++;
        }
        Console.WriteLine(round);
    }

    private static bool DoRound(byte[,] map, List<(byte checkMask, int moveX, int moveY)> checks)
    {
        var numLines = map.GetLength(0);
        var numCols = map.GetLength(1);
        var hasMoved = false;

        // phase 1 - propose moves
        for (var line = 0; line < numLines; line++)
        {
            for (var col = 0; col < numCols; col++)
            {
                ref var cell = ref map[line, col];
                if ((cell & ELF) > 0)
                {
                    var neighbours = FindElfNeighbours(col, line, map);
                    if (neighbours > 0)
                    {
                        hasMoved = true;
                        for (int i = 0; i < checks.Count; i++)
                        {
                            if ((neighbours & checks[i].checkMask) == 0)
                            {
                                var numMovesDest = ++map[line + checks[i].moveY, col + checks[i].moveX];
                                if (numMovesDest == 1)
                                {
                                    cell += (byte)i;
                                    cell += MOVING;
                                }
                                break;
                            }
                        }

                    }
                }
            }
        }

        // phase 2 - moves and cleanup
        for (var line = 0; line < numLines; line++)
        {
            for (var col = 0; col < numCols; col++)
            {
                ref var cell = ref map[line, col];
                if ((cell & ELF) > 0)
                {
                    if ((cell & MOVING) > 0)
                    {
                        var moveMask = cell & PROPOSES_MASK;
                        var (_, moveX, moveY) = checks[moveMask];
                        ref var destCell = ref map[line + moveY, col + moveX];
                        if (destCell == 1)
                        {
                            destCell = ELF;
                            cell = 0;
                        }
                        else
                        {
                            cell = ELF;
                        }
                    }
                }
                else if (cell > 1)
                {
                    cell = 0;
                }
            }
        }

        var c = checks[0];
        checks.RemoveAt(0);
        checks.Insert(3, c);

        return hasMoved;
    }

    private static int FindElfNeighbours(int elfX, int elfY, byte[,] map)
    {
        int neighbours = 0;
        int bitNumber = 7;

        for (int line = elfY - 1; line <= elfY + 1; line++)
        {
            for (int col = elfX - 1; col <= elfX + 1; col++)
            {
                if (line != elfY || col != elfX)
                {
                    if ((map[line, col] & ELF) == ELF)
                    {
                        neighbours |= 1 << bitNumber;
                    }
                    bitNumber--;
                }
            }
        }

        return neighbours;
    }
}

//#define DEBUG_VISUAL

using System.Runtime.CompilerServices;

internal class Input17
{
    // 380ms / 620ms - 529990

    static int[] ROCK_TEMPLATES = new[] {
        (0b0011110 << 0),

        (0b0001000 << 16) +
        (0b0011100 << 8) +
        (0b0001000 << 0),

        (0b0000100 << 16) +
        (0b0000100 << 8) +
        (0b0011100 << 0),

        (0b0010000 << 24) +
        (0b0010000 << 16) +
        (0b0010000 << 8) +
        (0b0010000 << 0),

        (0b0011000 << 8) +
        (0b0011000 << 0),
    };
    static int LEFT_LIMIT = 0x40404040;
    static int RIGHT_LIMIT = 0x01010101;

    internal static void Run()
    {
        var line = File.ReadAllLines("../../../input17.txt")[0];
        RunPart1(line);
    }

    private static void RunPart1(string jetsSequence)
    {
        var chamber = new byte[6000];
        var nextRock = 0;
        var nextJet = 0;
        var highestLine = -1;
        var cachedView = new Dictionary<(int, int, long), (long, int)>();
        var foundRepeat = false;
        long repeatHeight = 0;
        //const long TOTAL_ROCKS = 2022;
        const long TOTAL_ROCKS = 1_000_000_000_000;

        for (long numRocks = 0; numRocks < TOTAL_ROCKS; numRocks++)
        {
            var yPos = highestLine + 4;
            var rock = ROCK_TEMPLATES[nextRock++];
            if (nextRock == ROCK_TEMPLATES.Length) nextRock = 0;

            var fieldView = 0;

            var canFall = true;
            while (canFall)
            {
                var jet = jetsSequence[nextJet++];
                if (nextJet == jetsSequence.Length) nextJet = 0;

                var newRockPos = rock;
                if (jet == '<')
                {
                    if ((newRockPos & LEFT_LIMIT) == 0)
                        newRockPos <<= 1;
                }
                else if ((newRockPos & RIGHT_LIMIT) == 0)
                    newRockPos >>= 1;

                if ((newRockPos & fieldView) == 0)
                {
                    // didn't hit anything, use new pos
                    rock = newRockPos;
                }

                int newFieldView = fieldView << 8;
                if (yPos > 0 &&
                    ((newFieldView |= chamber[yPos - 1]) & rock) == 0)
                {
                    // didn't hit anything, use new pos
                    fieldView = newFieldView;
                    yPos--;
                }
                else
                {
                    fieldView |= rock;
                    Unsafe.As<byte, int>(ref chamber[yPos]) = fieldView;
                    canFall = false;
                }
            }

            while (chamber[highestLine + 1] != 0) highestLine++;

            if (!foundRepeat && highestLine > 8)
            {
                var topView = BitConverter.ToInt64(chamber, highestLine - 8 + 1);
                if (cachedView.ContainsKey((nextRock, nextJet, topView)))
                {
                    var (oldRocks, oldHighestLine) = cachedView[(nextRock, nextJet, topView)];
                    foundRepeat = true;
                    var diffRocks = numRocks - oldRocks;
                    var numCycles = (TOTAL_ROCKS - numRocks) / diffRocks;
                    numRocks += diffRocks * numCycles;
                    repeatHeight = numCycles * (highestLine - oldHighestLine);
                }
                else
                {
                    cachedView[(nextRock, nextJet, topView)] = (numRocks, highestLine);
                }
            }

            //DrawChamber(chamber, highestLine);
        }
        Console.WriteLine(highestLine + 1 + repeatHeight);
    }

    private static void DrawChamber(byte[] chamber, int highestLine)
    {
        for (int i = highestLine; i >= 0; i--)
        {
            Console.Write('|');
            var b = chamber[i];
            for (int j = 64; j > 0; j >>= 1)
            {
                Console.Write((b & j) > 0 ? '#' : '.');
            }
            Console.Write('|');
            Console.WriteLine();
        }
        Console.WriteLine("+-------+");
        Console.ReadLine();
    }

    //static byte[][,] rocks_old = new byte[][,]
    //{
    //    new byte[,]
    //    {
    //        { 1, 1, 1, 1 },
    //    },
    //    new byte[,]
    //    {
    //        { 0, 1, 0 },
    //        { 1, 1, 1 },
    //        { 0, 1, 0 },
    //    },
    //    new byte[,]
    //    {
    //        { 1, 1, 1 },
    //        { 0, 0, 1 },
    //        { 0, 0, 1 },
    //    },
    //    new byte[,]
    //    {
    //        { 1 },
    //        { 1 },
    //        { 1 },
    //        { 1 },
    //    },
    //    new byte[,]
    //    {
    //        { 1, 1 },
    //        { 1, 1 },
    //    },
    //};

    //    private static void RunPart1(string jetsLine)
    //    {
    //        var chamber = new byte[530000, 7];
    //        var nextRock = 0;
    //        var nextJet = 0;

    //        var highestLine = -1;
    //        List<char[,]> drawings;

    //#if DEBUG_VISUAL
    //        Dictionary<string, (int, List<char[,]>, int)> cached = new();
    //#endif

    //        for (int numRocks = 0; numRocks < 349990; numRocks++)
    //        {
    //#if DEBUG_VISUAL
    //            drawings = new List<char[,]>();
    //#endif
    //            var lastGoodPos = (x: 2, y: highestLine + 4);
    //            var usedRock = nextRock;
    //            var rock = rocks[nextRock++];
    //            if (nextRock == rocks.Length) nextRock = 0;

    //            var topDrawLine = lastGoodPos.y + rock.GetLength(0) - 1;
    //            var moves = 0;
    //            DrawToBuffer(drawings, chamber,
    //                topDrawLine,
    //                highestLine - 10,
    //                rock, lastGoodPos,
    //                moves, ' ', '+');

    //            var canFall = true;
    //            while (canFall)
    //            {
    //                var usedJet = nextJet;
    //                var jetChar = jetsLine[nextJet++];
    //                var jet = jetChar == '<' ? -1 : 1;
    //                if (nextJet == jetsLine.Length)
    //                {
    //                    nextJet = 0;
    //                }

    //                if (IsValidNewPos(chamber, rock, lastGoodPos, jet, 0))
    //                {
    //                    lastGoodPos.x += jet;
    //                }

    //                if (IsValidNewPos(chamber, rock, lastGoodPos, 0, -1))
    //                {
    //                    lastGoodPos.y--;
    //                    moves++;
    //                    DrawToBuffer(drawings, chamber,
    //                        topDrawLine,
    //                        Math.Min(highestLine, lastGoodPos.y) - 10,
    //                        rock, lastGoodPos,
    //                        moves, jetChar, 'v');
    //                }
    //                else
    //                {
    //                    canFall = false;
    //                    highestLine = Math.Max(highestLine,
    //                        lastGoodPos.y + rock.GetLength(0) - 1);

    //                    for (int line = 0; line < rock.GetLength(0); line++)
    //                    {
    //                        for (int col = 0; col < rock.GetLength(1); col++)
    //                        {
    //                            chamber[line + lastGoodPos.y, col + lastGoodPos.x] +=
    //                                rock[line, col];
    //                        }
    //                    }
    //                    DrawToBuffer(drawings, chamber,
    //                        topDrawLine,
    //                        Math.Min(highestLine, lastGoodPos.y) - 10,
    //                        null, lastGoodPos,
    //                        moves, 'v', '-');
    //                    var cacheKey = $"r{usedRock} - j{usedJet:000}";
    //#if DEBUG_VISUAL
    //                    if (cached.ContainsKey(cacheKey))
    //                    {
    //                        Console.WriteLine($"{numRocks,5:#,##0} - {cacheKey} - {cached[cacheKey].Item1}");
    //                        Console.WriteLine();

    //                        Console.Write($"Rock {cached[cacheKey].Item1}: ");
    //                        Console.WriteLine();
    //                        DrawToConsole(cached[cacheKey].Item2, cached[cacheKey].Item3);

    //                        Console.Write($"Rock {numRocks + 1}: ");
    //                        Console.WriteLine();
    //                        DrawToConsole(drawings, topDrawLine);
    //                        Console.ReadLine();
    //                    }
    //                    cached[cacheKey] = (numRocks, drawings, topDrawLine);
    //#endif
    //                }
    //            }
    //        }
    //        Console.WriteLine(highestLine + 1);
    //    }

    //    private static void DrawToConsole(List<char[,]> drawings, int topDrawLine)
    //    {
    //        var highestDrawing = drawings.Max(d => d.GetLength(0));
    //        var drawingsPerLine = 15;
    //        var drawingLines = (int)Math.Ceiling(drawings.Count / (double)drawingsPerLine);
    //        var startDrawingAt = 0;

    //        for (int i = 0; i < drawingLines; i++)
    //        {
    //            for (int j = 0; j < highestDrawing; j++)
    //            {
    //                Console.Write("{0,5:#,##0}: ", topDrawLine - j + 1);

    //                for (int k = 0; k < drawingsPerLine; k++)
    //                {
    //                    if (startDrawingAt + k < drawings.Count)
    //                    {
    //                        var drawing = drawings[startDrawingAt + k];
    //                        var drawingHeight = drawing.GetLength(0);
    //                        for (int l = 0; l < drawing.GetLength(1); l++)
    //                        {
    //                            if (j < drawingHeight)
    //                            {
    //                                Console.Write(drawing[j, l]);
    //                            }
    //                            else
    //                            {
    //                                Console.Write('.');
    //                            }
    //                        }
    //                        Console.Write(' ');
    //                    }
    //                }
    //                Console.WriteLine();
    //            }
    //            startDrawingAt += drawingsPerLine;
    //            Console.WriteLine();
    //        }
    //    }

    //    [Conditional("DEBUG_VISUAL")]
    //    private static void DrawToBuffer(List<char[,]> drawings, byte[,] chamber,
    //        int fromLine, int toLine,
    //        byte[,]? rock, (int x, int y) rockPos,
    //        int moves, char action1, char action2)
    //    {
    //        if (toLine < 0) toLine = 0;
    //        var drawing = new char[fromLine - toLine + 2, 9];
    //        var movesStr = moves.ToString();
    //        for (int i = 0; i < drawing.GetLength(1); i++)
    //        {
    //            drawing[0, i] = ' ';
    //        }
    //        for (int i = 0; i < movesStr.Length; i++)
    //        {
    //            drawing[0, i] = movesStr[i];
    //        }
    //        drawing[0, 3] = action1;
    //        drawing[0, 5] = action2;
    //        var drawingLine = 1;

    //        for (int line = fromLine; line >= toLine; line--)
    //        {
    //            drawing[drawingLine, 0] = '|';
    //            for (int col = 0; col < chamber.GetLength(1); col++)
    //            {
    //                var drawChar = '.';
    //                if (chamber[line, col] > 0)
    //                {
    //                    drawChar = '#';
    //                }
    //                else if (rock != null)
    //                {
    //                    if (col >= rockPos.x && col <= rockPos.x + rock.GetLength(1) - 1
    //                        && line >= rockPos.y && line <= rockPos.y + rock.GetLength(0) - 1
    //                        && rock[line - rockPos.y, col - rockPos.x] > 0)
    //                    {
    //                        drawChar = '@';
    //                    }
    //                }
    //                drawing[drawingLine, col + 1] = drawChar;
    //            }
    //            drawing[drawingLine, 8] = '|';

    //            drawingLine++;
    //        }

    //        drawings.Add(drawing);
    //    }

    //    private static bool IsValidNewPos(byte[,] chamber, byte[,] rock,
    //        (int x, int y) lastGoodPos, int xMove, int yMove)
    //    {
    //        var newX = lastGoodPos.x + xMove;
    //        if (newX < 0 ||
    //            newX + rock.GetLength(1) > chamber.GetLength(1))
    //        {
    //            return false;
    //        }

    //        var newY = lastGoodPos.y + yMove;
    //        if (newY < 0)
    //        {
    //            return false;
    //        }

    //        for (int line = 0; line < rock.GetLength(0); line++)
    //        {
    //            for (int col = 0; col < rock.GetLength(1); col++)
    //            {
    //                if (rock[line, col] > 0 &&
    //                    chamber[line + newY, col + newX] > 0)
    //                {
    //                    return false;
    //                }
    //            }
    //        }
    //        return true;
    //    }
}

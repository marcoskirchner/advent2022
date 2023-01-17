using System.Diagnostics;

internal class Input22
{
    const byte OPEN_TILE = 1;
    const byte SOLID_WALL = 2;

    const int RIGHT = 0;
    const int DOWN = 1;
    const int LEFT = 2;
    const int UP = 3;

    internal static void Run()
    {
        var lines = File.ReadAllLines("../../../input22.txt");
        var parameters = ReadInput(lines);
        RunPart1(parameters);
        RunPart2(parameters);
    }

    private static MapParameters ReadInput(string[] lines)
    {
        var cb = lines.Length < 20 ? 4 : 50;

        var p = new MapParameters
        {
            CubeSize = cb,
            Maps = new byte[6, cb, cb],
        };

        if (cb == 4)
        {
            p.Offsets = new Point[]
            {
                new Point(2 * cb, 0 * cb),
                new Point(0 * cb, 1 * cb),
                new Point(1 * cb, 1 * cb),
                new Point(2 * cb, 1 * cb),
                new Point(2 * cb, 2 * cb),
                new Point(3 * cb, 2 * cb),
            };
        }
        else
        {
            p.Offsets = new Point[]
            {
                new Point(1 * cb, 0 * cb),
                new Point(2 * cb, 0 * cb),
                new Point(1 * cb, 1 * cb),
                new Point(0 * cb, 2 * cb),
                new Point(1 * cb, 2 * cb),
                new Point(0 * cb, 3 * cb),
            };
        }

        for (int i = 0; i < p.Offsets.Length; i++)
        {
            var readMap = p.Offsets[i];
            for (int writeLine = 0, readLine = readMap.Y; writeLine < cb; writeLine++, readLine++)
            {
                for (int writeCol = 0, readCol = readMap.X; writeCol < cb; writeCol++, readCol++)
                {
                    p.Maps[i, writeLine, writeCol] = lines[readLine][readCol] switch
                    {
                        '.' => OPEN_TILE,
                        '#' => SOLID_WALL,
                        _ => throw new UnreachableException(),
                    };
                }
            }
        }

        p.Directions = lines[lines.Length - 1];
        return p;
    }

    private static void RunPart1(MapParameters parameters)
    {
        var cb = parameters.CubeSize;
        if (cb == 4)
        {
            parameters.NewMapPosDir = (map, pos, dir) => (map, dir) switch
            {
                (0, RIGHT) => (0, new(0, pos.Y), RIGHT),
                (0, DOWN) => (3, new(pos.X, 0), DOWN),
                (0, LEFT) => (0, new(cb - 1, pos.Y), LEFT),
                (0, UP) => (4, new(pos.X, cb - 1), UP),

                (1, RIGHT) => (2, new(0, pos.Y), RIGHT),
                (1, DOWN) => (1, new(pos.X, 0), DOWN),
                (1, LEFT) => (3, new(cb - 1, pos.Y), LEFT),
                (1, UP) => (1, new(pos.X, cb - 1), UP),

                (2, RIGHT) => (3, new(0, pos.Y), RIGHT),
                (2, DOWN) => (2, new(pos.X, 0), DOWN),
                (2, LEFT) => (1, new(cb - 1, pos.Y), LEFT),
                (2, UP) => (2, new(pos.X, cb - 1), UP),

                (3, RIGHT) => (1, new(0, pos.Y), RIGHT),
                (3, DOWN) => (4, new(pos.X, 0), DOWN),
                (3, LEFT) => (2, new(cb - 1, pos.Y), LEFT),
                (3, UP) => (0, new(pos.X, cb - 1), UP),

                (4, RIGHT) => (5, new(0, pos.Y), RIGHT),
                (4, DOWN) => (0, new(pos.X, 0), DOWN),
                (4, LEFT) => (5, new(cb - 1, pos.Y), LEFT),
                (4, UP) => (3, new(pos.X, cb - 1), UP),

                (5, RIGHT) => (4, new(0, pos.Y), RIGHT),
                (5, DOWN) => (5, new(pos.X, 0), DOWN),
                (5, LEFT) => (4, new(cb - 1, pos.Y), LEFT),
                (5, UP) => (5, new(pos.X, cb - 1), UP),

                _ => throw new UnreachableException(),
            };
        }
        else
        {
            parameters.NewMapPosDir = (map, pos, dir) => (map, dir) switch
            {
                (0, RIGHT) => (1, new(0, pos.Y), RIGHT),
                (0, DOWN) => (2, new(pos.X, 0), DOWN),
                (0, LEFT) => (1, new(cb - 1, pos.Y), LEFT),
                (0, UP) => (4, new(pos.X, cb - 1), UP),

                (1, RIGHT) => (0, new(0, pos.Y), RIGHT),
                (1, DOWN) => (1, new(pos.X, 0), DOWN),
                (1, LEFT) => (0, new(cb - 1, pos.Y), LEFT),
                (1, UP) => (1, new(pos.X, cb - 1), UP),

                (2, RIGHT) => (2, new(0, pos.Y), RIGHT),
                (2, DOWN) => (4, new(pos.X, 0), DOWN),
                (2, LEFT) => (2, new(cb - 1, pos.Y), LEFT),
                (2, UP) => (0, new(pos.X, cb - 1), UP),

                (3, RIGHT) => (4, new(0, pos.Y), RIGHT),
                (3, DOWN) => (5, new(pos.X, 0), DOWN),
                (3, LEFT) => (4, new(cb - 1, pos.Y), LEFT),
                (3, UP) => (5, new(pos.X, cb - 1), UP),

                (4, RIGHT) => (3, new(0, pos.Y), RIGHT),
                (4, DOWN) => (0, new(pos.X, 0), DOWN),
                (4, LEFT) => (3, new(cb - 1, pos.Y), LEFT),
                (4, UP) => (2, new(pos.X, cb - 1), UP),

                (5, RIGHT) => (5, new(0, pos.Y), RIGHT),
                (5, DOWN) => (3, new(pos.X, 0), DOWN),
                (5, LEFT) => (5, new(cb - 1, pos.Y), LEFT),
                (5, UP) => (3, new(pos.X, cb - 1), UP),

                _ => throw new UnreachableException(),
            };
        }
        ProcessMap(parameters);
    }
    private static void RunPart2(MapParameters parameters)
    {
        var cb = parameters.CubeSize;
        if (cb == 4)
        {
            parameters.NewMapPosDir = (map, pos, dir) => (map, dir) switch
            {
                //(0, RIGHT) => (5, new(0, pos.Y), LEFT),
                (0, DOWN) => (3, new(pos.X, 0), DOWN),
                //(0, LEFT) => (0, new(cb - 1, pos.Y), LEFT),
                //(0, UP) => (4, new(pos.X, cb - 1), UP),

                (1, RIGHT) => (2, new(0, pos.Y), RIGHT),
                //(1, DOWN) => (1, new(pos.X, 0), DOWN),
                //(1, LEFT) => (3, new(cb - 1, pos.Y), LEFT),
                //(1, UP) => (1, new(pos.X, cb - 1), UP),

                //(2, RIGHT) => (3, new(0, pos.Y), RIGHT),
                //(2, DOWN) => (2, new(pos.X, 0), DOWN),
                //(2, LEFT) => (1, new(cb - 1, pos.Y), LEFT),
                (2, UP) => (0, new(0, pos.X), RIGHT),

                (3, RIGHT) => (5, new(cb - pos.Y - 1, 0), DOWN),
                //(3, DOWN) => (4, new(pos.X, 0), DOWN),
                //(3, LEFT) => (2, new(cb - 1, pos.Y), LEFT),
                //(3, UP) => (0, new(pos.X, cb - 1), UP),

                //(4, RIGHT) => (5, new(0, pos.Y), RIGHT),
                (4, DOWN) => (1, new(cb - pos.X - 1, cb - 1), UP),
                //(4, LEFT) => (5, new(cb - 1, pos.Y), LEFT),
                //(4, UP) => (3, new(pos.X, cb - 1), UP),

                //(5, RIGHT) => (4, new(0, pos.Y), RIGHT),
                //(5, DOWN) => (5, new(pos.X, 0), DOWN),
                (5, LEFT) => (4, new(cb - 1, pos.Y), LEFT),
                //(5, UP) => (5, new(pos.X, cb - 1), UP),

                _ => throw new UnreachableException(),
            };
        }
        else
        {
            parameters.NewMapPosDir = (map, pos, dir) => (map, dir) switch
            {
                (0, RIGHT) => (1, new(0, pos.Y), RIGHT),
                (0, DOWN) => (2, new(pos.X, 0), DOWN),
                (0, LEFT) => (3, new(0, cb - pos.Y - 1), RIGHT),
                (0, UP) => (5, new(0, pos.X), RIGHT),

                (1, RIGHT) => (4, new(cb - 1, cb - pos.Y - 1), LEFT),
                (1, DOWN) => (2, new(cb - 1, pos.X), LEFT),
                (1, LEFT) => (0, new(cb - 1, pos.Y), LEFT),
                (1, UP) => (5, new(pos.X, cb - 1), UP),

                (2, RIGHT) => (1, new(pos.Y, cb - 1), UP),
                (2, DOWN) => (4, new(pos.X, 0), DOWN),
                (2, LEFT) => (3, new(pos.Y, 0), DOWN),
                (2, UP) => (0, new(pos.X, cb - 1), UP),

                (3, RIGHT) => (4, new(0, pos.Y), RIGHT),
                (3, DOWN) => (5, new(pos.X, 0), DOWN),
                (3, LEFT) => (0, new(0, cb - pos.Y - 1), RIGHT),
                (3, UP) => (2, new(0, pos.X), RIGHT),

                (4, RIGHT) => (1, new(cb - 1, cb - pos.Y - 1), LEFT),
                (4, DOWN) => (5, new(cb - 1, pos.X), LEFT),
                (4, LEFT) => (3, new(cb - 1, pos.Y), LEFT),
                (4, UP) => (2, new(pos.X, cb - 1), UP),

                (5, RIGHT) => (4, new(pos.Y, cb - 1), UP),
                (5, DOWN) => (1, new(pos.X, 0), DOWN),
                (5, LEFT) => (0, new(pos.Y, 0), DOWN),
                (5, UP) => (3, new(pos.X, cb - 1), UP),

                _ => throw new UnreachableException(),
            };
        }
        ProcessMap(parameters);
    }

    private static void ProcessMap(MapParameters parameters)
    {
        var map = 0;
        var pos = new Point(0, 0);
        var walk = new Point(1, 0);
        var dir = 0;

        var pMaps = parameters.Maps;
        while (pMaps[map, pos.Y, pos.X] != OPEN_TILE)
        {
            pos.X++;
        }

        var pDirs = parameters.Directions;
        var dirPos = 0;
        while (dirPos < pDirs.Length)
        {
            if (pDirs[dirPos] <= '9')
            {
                var startPos = dirPos++;
                while (dirPos < pDirs.Length && pDirs[dirPos] <= '9')
                {
                    dirPos++;
                }
                var distance = int.Parse(pDirs[startPos..dirPos]);
                dirPos--;

                DoWalking(distance);
            }
            else
            {
                dir += pDirs[dirPos] == 'L' ? -1 : 1;
                if (dir < 0) { dir = 3; } else if (dir > 3) { dir = 0; }
                walk = NewWalkDirection(dir);
            }
            dirPos++;
        }
        var password = (parameters.Offsets[map].Y + pos.Y + 1) * 1000
            + (parameters.Offsets[map].X + pos.X + 1) * 4
            + dir;
        Console.WriteLine(password);

        Point NewWalkDirection(int direction)
        {
            return direction switch
            {
                RIGHT => new Point(1, 0),
                DOWN => new Point(0, 1),
                LEFT => new Point(-1, 0),
                UP => new Point(0, -1),
                _ => throw new UnreachableException(),
            };
        }

        void DoWalking(int distanceLeft)
        {
            var newMap = map;
            var newPos = pos;
            var newDir = dir;
            while (distanceLeft > 0)
            {
                newPos.X += walk.X;
                newPos.Y += walk.Y;
                if (newPos.X < 0
                    || newPos.X == parameters.CubeSize
                    || newPos.Y < 0
                    || newPos.Y == parameters.CubeSize)
                {
                    (newMap, newPos, newDir) = parameters.NewMapPosDir(map, pos, dir);
                }

                //Console.WriteLine($"{map} - {pos} - {dir}");
                if (pMaps[newMap, newPos.Y, newPos.X] == SOLID_WALL)
                {
                    return;
                }
                else if (pMaps[newMap, newPos.Y, newPos.X] == OPEN_TILE)
                {
                    map = newMap;
                    pos = newPos;
                    if (dir != newDir)
                    {
                        dir = newDir;
                        walk = NewWalkDirection(dir);
                    }
                    distanceLeft--;
                }
            }
        }
    }

}

internal record struct Point(int X, int Y);

internal class MapParameters
{
    public int CubeSize;
    public byte[,,] Maps;
    public Point[] Offsets;
    public Func<int, Point, int, (int, Point, int)> NewMapPosDir;
    public string Directions;
}
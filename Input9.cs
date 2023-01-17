using System.Diagnostics;
using System.Drawing;

class Input9
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input9.txt");
        SimulateRope(lines, new Point[2]);
        SimulateRope(lines, new Point[10]);
    }
    private static void SimulateRope(string[] lines, Point[] knots)
    {
        var visitedPoints = new HashSet<Point>();
        ref var tail = ref knots[^1];
        visitedPoints.Add(tail);

        for (int i = 0; i < lines.Length; i++)
        {
            var direction = lines[i][0];
            var distance = int.Parse(lines[i][2..]);

            var (movX, movY) = direction switch
            {
                'R' => (1, 0),
                'L' => (-1, 0),
                'U' => (0, -1),
                'D' => (0, 1),
                _ => throw new UnreachableException(),
            };

            while (distance-- > 0)
            {
                // head movement
                ref var prevKnot = ref knots[0];
                prevKnot.X += movX;
                prevKnot.Y += movY;

                // other knots
                for (int j = 1; j < knots.Length; j++)
                {
                    ref var currentKnot = ref knots[j];

                    var diffX = prevKnot.X - currentKnot.X;
                    var diffY = prevKnot.Y - currentKnot.Y;
                    if (Math.Abs(diffX) > 1 || Math.Abs(diffY) > 1)
                    {
                        currentKnot.X += Math.Sign(diffX);
                        currentKnot.Y += Math.Sign(diffY);
                    }

                    prevKnot = ref currentKnot;
                }

                if (!visitedPoints.Contains(tail))
                {
                    visitedPoints.Add(tail);
                }
            }
        }

        System.Console.WriteLine(visitedPoints.Count);
    }
}
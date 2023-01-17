using System.Diagnostics;

class Input12
{
    struct Node
    {
        public short Line;
        public short Column;

        public Node(int line, int column)
        {
            Line = (short)line;
            Column = (short)column;
        }

        public int Num
        {
            get { return Line * LineLength + Column; }
        }

        public static int LineLength;
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input12.txt");
        RunPart1(lines);
        lines = File.ReadAllLines("input12.txt");
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        Node.LineLength = lines[0].Length;
        var numLines = lines.Length;
        var numColumns = lines[0].Length;

        var totalNodes = numLines * numColumns;
        var dist = new int[totalNodes];
        var Q = new HashSet<Node>(totalNodes);

        Node startNode = new Node(-1, -1), endNode = startNode;
        for (int i = 0; i < numLines; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                var n = new Node(i, j);
                if (lines[i][j] == 'S')
                {
                    startNode = n;
                    lines[i] = lines[i].Replace('S', 'a');
                }
                else if (lines[i][j] == 'E')
                {
                    endNode = n;
                    lines[i] = lines[i].Replace('E', 'z');
                }
                Q.Add(n);
                dist[n.Num] = Int32.MaxValue;
            }
        }
        dist[startNode.Num] = 0;
        while (Q.Count > 0)
        {
            var u = Q.MinBy(q => dist[q.Num]);
            if (dist[u.Num] == Int32.MaxValue)
            {
                throw new Exception("No path");
            }

            if (u.Line == endNode.Line && u.Column == endNode.Column)
            {
                System.Console.WriteLine(dist[u.Num]);
                return;
            }
            Q.Remove(u);
            var maxHeight = (char)(lines[u.Line][u.Column] + 1);

            Node temp;

            temp = new Node(u.Line, u.Column - 1);
            if (u.Column > 0 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line, u.Column + 1);
            if (u.Column < numColumns - 1 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line - 1, u.Column);
            if (u.Line > 0 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line + 1, u.Column);
            if (u.Line < numLines - 1 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }
        };
    }

    private static void RunPart2(string[] lines)
    {
        Node.LineLength = lines[0].Length;
        var numLines = lines.Length;
        var numColumns = lines[0].Length;

        var totalNodes = numLines * numColumns;
        var dist = new int[totalNodes];
        var Q = new HashSet<Node>(totalNodes);

        Node startNode = new Node(-1, -1), endNode = startNode;
        for (int i = 0; i < numLines; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                var n = new Node(i, j);
                Q.Add(n);
                dist[n.Num] = Int32.MaxValue;

                switch (lines[i][j])
                {
                    case 'a':
                        dist[n.Num] = 0;
                        break;
                    case 'S':
                        dist[n.Num] = 0;
                        lines[i] = lines[i].Replace('S', 'a');
                        break;
                    case 'E':
                        endNode = n;
                        lines[i] = lines[i].Replace('E', 'z');
                        break;
                }
            }
        }
        while (Q.Count > 0)
        {
            var u = Q.MinBy(q => dist[q.Num]);
            if (dist[u.Num] == Int32.MaxValue)
            {
                throw new Exception("No path");
            }

            if (u.Line == endNode.Line && u.Column == endNode.Column)
            {
                System.Console.WriteLine(dist[u.Num]);
                return;
            }
            Q.Remove(u);
            var maxHeight = (char)(lines[u.Line][u.Column] + 1);

            Node temp;

            temp = new Node(u.Line, u.Column - 1);
            if (u.Column > 0 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line, u.Column + 1);
            if (u.Column < numColumns - 1 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line - 1, u.Column);
            if (u.Line > 0 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }

            temp = new Node(u.Line + 1, u.Column);
            if (u.Line < numLines - 1 && lines[temp.Line][temp.Column] <= maxHeight && Q.Contains(temp))
            {
                var alt = dist[u.Num] + 1;
                if (alt < dist[temp.Num])
                {
                    dist[temp.Num] = alt;
                }
            }
        }
    }
}

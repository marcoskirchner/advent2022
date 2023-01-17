using System.Diagnostics;

class Input13
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input13.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    class ListOrInt : IComparable<ListOrInt>
    {
        public int? Value;
        public List<ListOrInt>? List;

        public bool IsList => List != null;

        public int CompareTo(ListOrInt other)
        {
            if (IsList)
            {
                if (other.IsList)
                {
                    var limit = Math.Min(List.Count, other.List.Count);
                    for (int i = 0; i < limit; i++)
                    {
                        var c = List[i].CompareTo(other.List[i]);
                        if (c != 0)
                        {
                            return c;
                        }
                    }
                    return List.Count.CompareTo(other.List.Count);
                }
                else
                {
                    var t = new ListOrInt();
                    t.List = new() { other };
                    return CompareTo(t);
                }
            }
            else
            {
                // is int
                if (other.IsList)
                {
                    var t = new ListOrInt();
                    t.List = new() { this };
                    return t.CompareTo(other);
                }
                else
                {
                    return Value.Value.CompareTo(other.Value.Value);
                }
            }
        }
    }

    private static ListOrInt ParseList(string line)
    {
        ListOrInt InternalParse(string line, ref int pos)
        {
            var el = new ListOrInt();
            if (line[pos] == '[')
            {
                el.List = new();
                while (++pos < line.Length && line[pos] != ']')
                {
                    if (line[pos] != ',')
                    {
                        el.List.Add(InternalParse(line, ref pos));
                    }
                }
            }
            else
            {
                var numStart = pos;
                while (++pos < line.Length && char.IsDigit(line[pos])) ;
                el.Value = int.Parse(line.Substring(numStart, pos - numStart));
            }

            return el;
        }

        var start = 0;
        return InternalParse(line, ref start);
    }

    private static void RunPart1(string[] lines)
    {
        var pair = 0;
        var sum = 0;
        for (int i = 0; i < lines.Length; i += 3)
        {
            var line1 = ParseList(lines[i]);
            var line2 = ParseList(lines[i + 1]);

            pair++;
            if (line1.CompareTo(line2) < 0)
            {
                sum += pair;
            }
        }
        System.Console.WriteLine(sum);
    }

    private static void RunPart2(string[] lines)
    {
        var s1 = ParseList("[[2]]");
        var s2 = ParseList("[[6]]");
        List<ListOrInt> packets = new();
        packets.Add(s1);
        packets.Add(s2);

        for (int i = 0; i < lines.Length; i += 3)
        {
            packets.Add(ParseList(lines[i]));
            packets.Add(ParseList(lines[i + 1]));
        }

        packets.Sort();

        var key = 1;
        var index = 0;
        foreach (var p in packets)
        {
            index++;
            if (p.CompareTo(s1) == 0 || p.CompareTo(s2) == 0)
            {
                key *= index;
            }
        }
        System.Console.WriteLine(key);
    }
}

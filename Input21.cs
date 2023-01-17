internal class Input21
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("../../../input21.txt");
        var monkeys = ReadInput(lines);

        RunPart1(monkeys);
        RunPart2(monkeys);
    }

    private static Dictionary<string, Monkey> ReadInput(string[] lines)
    {
        var ops = new Dictionary<char, Func<long, long, long>>(4)
        {
            { '+', (p1, p2) => p1 + p2 },
            { '-', (p1, p2) => p1 - p2 },
            { '*', (p1, p2) => p1 * p2 },
            { '/', (p1, p2) => p1 / p2 },
        };
        var monkeys = new Dictionary<string, Monkey>(lines.Length);
        foreach (var line in lines)
        {
            var name = line[0..4];
            var m = new Monkey(name);
            if (line.Length == 17)
            {
                m.Monkey1 = line[6..10];
                m.Op = ops[line[11]];
                m.OpChar = line[11];
                m.Monkey2 = line[13..];
            }
            else
            {
                m.Value = long.Parse(line[6..]);
            }
            monkeys[name] = m;
        }
        return monkeys;
    }

    private static void RunPart1(Dictionary<string, Monkey> monkeys)
    {
        var root = monkeys["root"];
        SolveMonkey(root);
        Console.WriteLine(root.Value!.Value);

        void SolveMonkey(Monkey monkey)
        {
            if (monkey.Value == null)
            {
                var m1 = monkeys[monkey.Monkey1!];
                var m2 = monkeys[monkey.Monkey2!];
                SolveMonkey(m1);
                SolveMonkey(m2);
                monkey.Value = monkey.Op!(m1.Value!.Value, m2.Value!.Value);
            }
        }
    }

    private static void RunPart2(Dictionary<string, Monkey> monkeys)
    {
        foreach (var monkey in monkeys.Values)
        {
            if (monkey.Monkey1 != null)
            {
                monkey.Value = null;
            }
        }

        const string ME_MONKEY = "humn";
        monkeys[ME_MONKEY].Value = null;
        var root = monkeys["root"];
        SolveMonkey(root);
        var rm1 = monkeys[root.Monkey1!];
        var rm2 = monkeys[root.Monkey2!];
        if (rm1.Value.HasValue)
        {
            rm2.Value = rm1.Value;
            ReverseSolveMonkey(rm2);
        }
        else
        {
            rm1.Value = rm2.Value;
            ReverseSolveMonkey(rm1);
        }
        Console.WriteLine(monkeys[ME_MONKEY].Value);

        void SolveMonkey(Monkey monkey)
        {
            if (monkey.Value == null && monkey.Name != ME_MONKEY)
            {
                var m1 = monkeys[monkey.Monkey1!];
                var m2 = monkeys[monkey.Monkey2!];
                SolveMonkey(m1);
                SolveMonkey(m2);
                if (m1.Value.HasValue && m2.Value.HasValue)
                {
                    monkey.Value = monkey.Op!(m1.Value!.Value, m2.Value!.Value);
                }
            }
        }

        void ReverseSolveMonkey(Monkey monkey)
        {
            if (monkey.Name == ME_MONKEY)
                return;

            var m1 = monkeys[monkey.Monkey1!];
            var m2 = monkeys[monkey.Monkey2!];

            var solvedValue = (monkey.OpChar, m1.Value, m2.Value) switch
            {
                ('+', _, null) => monkey.Value - m1.Value,
                ('+', null, _) => monkey.Value - m2.Value,
                ('-', _, null) => m1.Value - monkey.Value,
                ('-', null, _) => m2.Value + monkey.Value,
                ('*', _, null) => monkey.Value / m1.Value,
                ('*', null, _) => monkey.Value / m2.Value,
                ('/', _, null) => m1.Value / monkey.Value,
                ('/', null, _) => m2.Value * monkey.Value,
                (_, _, _) => throw new Exception(),
            };

            if (m1.Value.HasValue)
            {
                m2.Value = solvedValue;
                ReverseSolveMonkey(m2);
            }
            else
            {
                m1.Value = solvedValue;
                ReverseSolveMonkey(m1);
            }
        }
    }
}

internal class Monkey
{

    public string Name;
    public long? Value;
    public Func<long, long, long>? Op;
    public char OpChar;
    public string? Monkey1;
    public string? Monkey2;

    public Monkey(string name)
    {
        Name = name;
    }
}
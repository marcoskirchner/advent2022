using System.Diagnostics;

class Input11
{
    class Monkey
    {
        public long Inspects;
        public Queue<long> Items = new();
        public char Op;
        public int OpSize;
        public int DivisibleTest;
        public int TrueMonkey;
        public int FalseMonkey;
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input11.txt");
        var monkeys = LoadMonkeys(lines);
        RunPart1(monkeys);
        monkeys = LoadMonkeys(lines);
        RunPart2(monkeys);
    }

    private static Monkey[] LoadMonkeys(string[] lines)
    {
        var monkeys = new Monkey[(int)Math.Ceiling(lines.Length / 7m)];

        for (int i = 0, m = 0; i < lines.Length; i += 7, m++)
        {
            Monkey monkey = new();
            foreach (var item in lines[i + 1][18..].Split(',').Select(int.Parse))
            {
                monkey.Items.Enqueue(item);
            }
            monkey.Op = lines[i + 2][23];
            var opSize = lines[i + 2][25..];
            if (opSize == "old")
            {
                monkey.Op = '^';
                monkey.OpSize = 2;
            }
            else
            {
                monkey.OpSize = int.Parse(opSize);
            }
            monkey.DivisibleTest = int.Parse(lines[i + 3][21..]);
            monkey.TrueMonkey = int.Parse(lines[i + 4][29..]);
            monkey.FalseMonkey = int.Parse(lines[i + 5][30..]);
            monkeys[m] = monkey;
        }

        return monkeys;
    }

    private static void RunPart1(Monkey[] monkeys)
    {
        for (int i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    monkey.Inspects++;
                    var item = monkey.Items.Dequeue();
                    item = monkey.Op switch
                    {
                        '+' => item + monkey.OpSize,
                        '*' => item * monkey.OpSize,
                        '^' => (long)Math.Pow(item, monkey.OpSize),
                        _ => throw new UnreachableException(),
                    };
                    item /= 3;
                    if (item % monkey.DivisibleTest == 0)
                    {
                        monkeys[monkey.TrueMonkey].Items.Enqueue(item);
                    }
                    else
                    {
                        monkeys[monkey.FalseMonkey].Items.Enqueue(item);
                    }
                }
            }
        }
        var r = monkeys
            .OrderByDescending(m => m.Inspects)
            .Take(2)
            .Aggregate(1L, (p, m) => p * m.Inspects);
        System.Console.WriteLine(r);
    }

    private static void RunPart2(Monkey[] monkeys)
    {
        long divs = 1;
        foreach (var monkey in monkeys)
        {
            divs *= monkey.DivisibleTest;
        }

        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    monkey.Inspects++;
                    var item = monkey.Items.Dequeue();
                    checked
                    {
                        item = monkey.Op switch
                        {
                            '+' => item + monkey.OpSize,
                            '*' => item * monkey.OpSize,
                            '^' => (long)Math.Pow(item, monkey.OpSize),
                            _ => throw new UnreachableException(),
                        };
                        item %= divs;
                    }
                    if (item % monkey.DivisibleTest == 0)
                    {
                        monkeys[monkey.TrueMonkey].Items.Enqueue(item);
                    }
                    else
                    {
                        monkeys[monkey.FalseMonkey].Items.Enqueue(item);
                    }
                }
            }
        }
        var r = monkeys
            .OrderByDescending(m => m.Inspects)
            .Take(2)
            .Aggregate(1L, (p, m) => p * m.Inspects);
        System.Console.WriteLine(r);
    }
}

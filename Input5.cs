using System.Diagnostics;

class Input5
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("input5.txt");
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(string[] lines)
    {
        var blankLine = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0)
            {
                blankLine = i;
                break;
            }
        }

        var stacksLine = lines[blankLine - 1];
        var stacks = new List<Stack<char>>();
        stacks.Add(new()); // stacks[0]
        for (int linePos = 1; linePos < stacksLine.Length; linePos += 4)
        {
            stacks.Add(new()); // stacks[i]
        }

        for (int i = blankLine - 2; i >= 0; i--)
        {
            var cratesLine = lines[i];

            for (int linePos = 1, stackNum = 1; linePos < stacksLine.Length; linePos += 4, stackNum++)
            {
                var crate = cratesLine[linePos];
                if (crate != ' ') {
                    stacks[stackNum].Push(crate);
                }
            }
        }

        for (int i = blankLine + 1; i < lines.Length; i++)
        {
            var moveLine = lines[i].Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",")
                .Split(',').Select(int.Parse).ToArray();

            var qty = moveLine[0];
            var from = moveLine[1];
            var to = moveLine[2];
            while (qty > 0) {
                var crate = stacks[from].Pop();
                stacks[to].Push(crate);
                qty--;
            }
        }

        for (int i = 1; i < stacks.Count; i++)
        {
            System.Console.Write(stacks[i].Peek());
        }
        System.Console.WriteLine();
    }

    private static void RunPart2(string[] lines)
    {
        var blankLine = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0)
            {
                blankLine = i;
                break;
            }
        }

        var stacksLine = lines[blankLine - 1];
        var stacks = new List<Stack<char>>();
        stacks.Add(new()); // stacks[0]
        for (int linePos = 1; linePos < stacksLine.Length; linePos += 4)
        {
            stacks.Add(new()); // stacks[i]
        }

        for (int i = blankLine - 2; i >= 0; i--)
        {
            var cratesLine = lines[i];

            for (int linePos = 1, stackNum = 1; linePos < stacksLine.Length; linePos += 4, stackNum++)
            {
                var crate = cratesLine[linePos];
                if (crate != ' ') {
                    stacks[stackNum].Push(crate);
                }
            }
        }

        var tempStack = new Stack<char>();
        for (int i = blankLine + 1; i < lines.Length; i++)
        {
            var moveLine = lines[i].Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",")
                .Split(',').Select(int.Parse).ToArray();

            var qty = moveLine[0];
            var from = moveLine[1];
            var to = moveLine[2];
            while (qty > 0) {
                var crate = stacks[from].Pop();
                tempStack.Push(crate);
                qty--;
            }
            while (tempStack.Count > 0) {
                var crate = tempStack.Pop();
                stacks[to].Push(crate);
            }
        }

        for (int i = 1; i < stacks.Count; i++)
        {
            System.Console.Write(stacks[i].Peek());
        }
        System.Console.WriteLine();
    }
}

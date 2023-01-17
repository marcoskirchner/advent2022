using System.ComponentModel.DataAnnotations;

internal class Input20
{
    internal static void Run()
    {
        var lines = File.ReadAllLines("../../../input20.txt").Select(long.Parse);
        RunPart1(lines);
        RunPart2(lines);
    }

    private static void RunPart1(IEnumerable<long> lines)
    {
        var (workingList, originalList) = LoadLists(lines, 1);
        MixinRound(workingList, originalList);
        PrintCoordinates(workingList);
    }

    private static void RunPart2(IEnumerable<long> lines)
    {
        var (workingList, originalList) = LoadLists(lines, 811589153);
        for (int i = 0; i < 10; i++)
        {
            MixinRound(workingList, originalList);
        }
        PrintCoordinates(workingList);
    }

    private static void MixinRound(
        LinkedList<long> workingList,
        LinkedListNode<long>[] originalList)
    {
        var modSize = originalList.Length - 1;
        var halfLength = modSize >> 1;
        var nHalfLength = halfLength * -1;
        for (int i = 0; i < originalList.Length; i++)
        {
            var itemToMove = originalList[i];
            var v = itemToMove.Value % modSize;

            if (v != 0)
            {
                if (v < 0 && v < nHalfLength)
                {
                    v += modSize;
                }
                else if (v > 0 && v > halfLength)
                {
                    v -= modSize;
                }

                var prev = itemToMove.Previous ?? workingList.Last!;
                workingList.Remove(itemToMove);

                while (v > 0)
                {
                    prev = prev.Next ?? workingList.First!;
                    v--;
                }
                while (v < 0)
                {
                    prev = prev.Previous ?? workingList.Last!;
                    v++;
                }

                workingList.AddAfter(prev, itemToMove);
            }
        }

    }

    private static
        (LinkedList<long> workingList, LinkedListNode<long>[] originalList)
        LoadLists(IEnumerable<long> lines, long decryptionKey)
    {
        LinkedList<long> workingList = new(lines.Select(v => v * decryptionKey));
        var originalList = new LinkedListNode<long>[workingList.Count];

        var current = workingList.First;
        var pos = 0;
        while (current != null)
        {
            originalList[pos++] = current;
            current = current!.Next;
        }
        return (workingList, originalList);
    }

    private static void PrintCoordinates(LinkedList<long> workingList)
    {
        var current = workingList.Find(0)!;
        var sum = 0L;
        for (int i = 1; i <= 3000; i++)
        {
            current = current.Next ?? workingList.First!;
            if (i % 1000 == 0)
            {
                sum += current.Value;
            }
        }

        Console.WriteLine(sum);
    }
}
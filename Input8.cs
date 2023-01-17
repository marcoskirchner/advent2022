using System.Diagnostics;

class Input8
{
    struct Tree
    {
        public int Height;
        public bool Visible;
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input8.txt");
        var trees = LoadTrees(lines);
        RunPart1(trees);
        RunPart2(trees);
    }

    private static Tree[,] LoadTrees(string[] lines)
    {
        var trees = new Tree[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                trees[i, j].Height = (line[j] - '0');
            }
        }
        return trees;
    }

    private static void RunPart1(Tree[,] trees)
    {
        for (int i = 0; i < trees.GetLength(0); i++)
        {
            int maxHeightFromLeft = -1;
            int maxHeightFromRight = -1;
            for (int j = 0; j < trees.GetLength(1); j++)
            {
                if (trees[i, j].Height > maxHeightFromLeft)
                {
                    trees[i, j].Visible = true;
                    maxHeightFromLeft = trees[i, j].Height;
                }
            }
            for (int j = trees.GetLength(1) - 1; j >= 0; j--)
            {
                if (trees[i, j].Height > maxHeightFromRight)
                {
                    trees[i, j].Visible = true;
                    maxHeightFromRight = trees[i, j].Height;
                }
            }
        }

        for (int k = 0; k < trees.GetLength(1); k++)
        {
            int maxHeightFromTop = -1;
            int maxHeightFromBottom = -1;
            for (int l = 0; l < trees.GetLength(0); l++)
            {
                if (trees[l, k].Height > maxHeightFromTop)
                {
                    trees[l, k].Visible = true;
                    maxHeightFromTop = trees[l, k].Height;
                }
            }
            for (int l = trees.GetLength(0) - 1; l >= 0; l--)
            {
                if (trees[l, k].Height > maxHeightFromBottom)
                {
                    trees[l, k].Visible = true;
                    maxHeightFromBottom = trees[l, k].Height;
                }
            }
        }

        var count = 0;
        foreach (var tree in trees)
        {
            if (tree.Visible)
            {
                count++;
            }
        }
        System.Console.WriteLine(count);
    }

    private static void RunPart2(Tree[,] trees)
    {
        var highest = 0;
        for (int i = 0; i < trees.GetLength(0); i++)
        {
            for (int j = 0; j < trees.GetLength(1); j++)
            {
                var treeScore = CalculateTreeScenicScore(i, j, trees);
                if (treeScore > highest)
                {
                    highest = treeScore;
                }
            }
        }
        System.Console.WriteLine(highest);
    }

    private static int CalculateTreeScenicScore(int treeRow, int treeColumn, Tree[,] trees)
    {
        var treeHeight = trees[treeRow, treeColumn].Height;

        var viewLeft = 0;
        for (int i = treeColumn - 1; i >= 0; i--)
        {
            viewLeft++;
            if (trees[treeRow, i].Height >= treeHeight)
            {
                break;
            }
        }

        var viewRight = 0;
        for (int i = treeColumn + 1; i < trees.GetLength(1); i++)
        {
            viewRight++;
            if (trees[treeRow, i].Height >= treeHeight)
            {
                break;
            }
        }

        var viewUp = 0;
        for (int i = treeRow - 1; i >= 0; i--)
        {
            viewUp++;
            if (trees[i, treeColumn].Height >= treeHeight)
            {
                break;
            }
        }

        var viewDown = 0;
        for (int i = treeRow + 1; i < trees.GetLength(0); i++)
        {
            viewDown++;
            if (trees[i, treeColumn].Height >= treeHeight)
            {
                break;
            }
        }

        return viewLeft * viewRight * viewUp * viewDown;
    }
}

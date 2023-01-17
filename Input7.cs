using System.Diagnostics;

class Input7
{
    class DirEntry
    {
        public DirEntry(string name)
        {
            Name = name;
            Directories = new();
            Parent = this;
        }
        public string Name { get; init; }
        public int LocalSize { get; set; }
        public int TotalSize { get; set; }
        public int NumFiles { get; set; }
        public DirEntry Parent { get; set; }
        public Dictionary<string, DirEntry> Directories { get; }
    }

    internal static void Run()
    {
        var lines = File.ReadAllLines("input7.txt");
        var rootDir = ProcessCommands(lines);
        RunPart1(rootDir);
        RunPart2(rootDir);
    }

    private static DirEntry ProcessCommands(string[] lines)
    {
        DirEntry rootEntry = new("/");
        rootEntry.Parent = rootEntry;
        DirEntry currentDir = rootEntry;

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.StartsWith("$ cd "))
            {
                if (line[5..] == "/")
                {
                    currentDir = rootEntry;
                }
                else if (line[5..] == "..")
                {
                    var currentDirSize = currentDir.TotalSize;
                    currentDir = currentDir.Parent;
                    currentDir.TotalSize += currentDirSize;
                }
                else
                {
                    currentDir = currentDir.Directories[line[5..].ToString()];
                }
            }
            else if (line.StartsWith("$ ls"))
            {
                while (++i < lines.Length && !lines[i].StartsWith("$ "))
                {
                    line = lines[i];
                    if (line.StartsWith("dir "))
                    {
                        var subDir = new DirEntry(line.Substring(4));
                        subDir.Parent = currentDir;
                        currentDir.Directories.Add(subDir.Name, subDir);
                    }
                    else
                    {
                        var fileSize = int.Parse(line.Split(' ')[0]);
                        currentDir.LocalSize += fileSize;
                        currentDir.TotalSize += fileSize;
                        currentDir.NumFiles++;
                    }
                }
                i--;
            }
            else
            {
                throw new UnreachableException();
            }
        }
        while (currentDir != rootEntry)
        {
            var currentDirSize = currentDir.TotalSize;
            currentDir = currentDir.Parent;
            currentDir.TotalSize += currentDirSize;
        }

        return rootEntry;
    }

    private static void RunPart1(DirEntry rootEntry)
    {
        int SumSmallDirs(DirEntry dir)
        {
            var size = dir.TotalSize;
            if (size > 100000)
                size = 0;
            foreach (var subDir in dir.Directories.Values)
            {
                size += SumSmallDirs(subDir);
            }
            return size;
        }
        System.Console.WriteLine(SumSmallDirs(rootEntry));
    }

    private static void RunPart2(DirEntry rootEntry)
    {
        var fsSize = 70000000;
        var freeSpace = fsSize - rootEntry.TotalSize;
        var neededSpace = 30000000 - freeSpace;
        
        int SmallestDirToDelete(DirEntry dir)
        {
            var size = dir.TotalSize;
            foreach (var subDir in dir.Directories.Values)
            {
                var subDirSize = SmallestDirToDelete(subDir);
                if (subDirSize >= neededSpace && subDirSize < size)
                    size = subDirSize;
            }
            return size;
        }
        System.Console.WriteLine(SmallestDirToDelete(rootEntry));
    }
}

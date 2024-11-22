using System.Diagnostics;
using System.IO;

namespace LemonPlatform.Core.Helpers
{
    public class PathHelper
    {
        public static void OpenPath(string path)
        {
            Process.Start("explorer.exe", path);
        }

        public static List<string> GetFiles(string directory, bool isRecursive, params string[] patterns)
        {
            var files = new List<string>();
            if (!patterns.Any())
            {
                patterns = new[] { "*.*" };
            }

            foreach (var pattern in patterns)
            {
                files.AddRange(Directory.GetFiles(directory, pattern));
            }

            if (!isRecursive) return files;
            foreach (var item in Directory.GetDirectories(directory))
            {
                files.AddRange(GetFiles(item, true, patterns));
            }

            return files;
        }

        public static List<string> GetDirectories(string directory)
        {
            var directories = new List<string> { directory };
            var subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                directories.AddRange(GetDirectories(subDirectory));
            }

            return directories;
        }
    }
}
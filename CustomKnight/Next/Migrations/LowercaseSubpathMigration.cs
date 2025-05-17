using System.IO;
using System.Linq;

namespace CustomKnight.Next.Migrations
{
    internal class LowercaseSubpathMigration : BaseMigration
    {
        public string[] allowedDirectories = ["knight", "areabackgrounds", "charms", "inventory", "savehud", "knight", "spells", "particles", "ui", "minions"];
        public void FixDirectory(string path)
        {
            var files = Directory.EnumerateFiles(path);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (!fileName.Equals(fileName.ToLowerInvariant()))
                {
                    File.Move(file, Path.Combine(path, fileName.ToLowerInvariant()));
                }
            }
            var directories = Directory.EnumerateDirectories(path);
            foreach (var directory in directories)
            {
                var directoryName = new DirectoryInfo(directory).Name;
                if (allowedDirectories.Contains(directoryName.ToLower()))
                {
                    FixDirectory(directory);
                    if (!directoryName.Equals(directoryName.ToLowerInvariant()))
                    {
                        Directory.Move(directory, Path.Combine(path, directoryName.ToLowerInvariant()));
                    }
                }
            }
        }
        public override void Run(MigrationContext context)
        {
            FixDirectory(context.SkinPath);
        }
    }
}
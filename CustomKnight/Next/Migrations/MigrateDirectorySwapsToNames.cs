using System.IO;
using System.Linq;

namespace CustomKnight.Next.Migrations
{
    internal static class NameDbExtension {
        internal static void SafeAdd(this Dictionary<string, List<string>> NameDb, string objectPath, string hash)
        {
            if (!NameDb.TryGetValue(hash, out var objectPaths))
            {
                NameDb[hash] = new List<string>();
            }
            if (!NameDb[hash].Contains(objectPath))
            {
                NameDb[hash].Add(objectPath);
            }
        }
    }
    internal class MigrateDirectorySwapsToNames : BaseMigration
    {
        SettingsLoader<Dictionary<string, List<string>>> NamesLoader = new();
        
        public override void Run(MigrationContext context)
        {
            var swapPath = Path.Combine(context.SkinPath, "Swap");
            var dirs = Directory.GetDirectories(swapPath);
            foreach (var dir in dirs)
            {
                if (File.Exists(Path.Combine(dir, "names.json"))) {
                    continue;
                }
                if (File.Exists(Path.Combine(dir, "manifest.json"))) {
                    continue;
                }
                // time to search the paths and collect the structure but we will need hashcache.json and aliases.json to resolve to usable names.json if we want defined alias only
                var filePaths = Directory.EnumerateFiles(dir,".png",SearchOption.AllDirectories);

                var namesData = NamesLoader.Load(Path.Combine(dir, "names.json"));
                foreach (var file in filePaths)
                {
                    var relativeFilePathWithoutExtension = Path.Combine(Path.GetDirectoryName(file),Path.GetFileNameWithoutExtension(file)).Replace(dir, "");
                    //var hash = HashWithCache.GetHashFromPath(relativeFilePathWithoutExtension);
                    //var alias = HashWithCache.GetAliasFromHash(hash);
                    namesData.SafeAdd(relativeFilePathWithoutExtension, relativeFilePathWithoutExtension);

                }
                NamesLoader.Save(Path.Combine(dir, "names.json"), namesData);
            }
        }
    }
}
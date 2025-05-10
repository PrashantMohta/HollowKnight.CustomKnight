using System.IO;
using System.Linq;
using CustomKnight.Next.Skin;
using CustomKnight.Next.Skin.Enum;

namespace CustomKnight.Next.Migrations
{
    class GenerateManifestMigration : BaseMigration
    {
        public override void Run(MigrationContext context)
        {

            var dirs = Directory.GetDirectories(context.SkinPath);
            foreach (var dir in dirs)
            {
                string directoryName = new DirectoryInfo(dir).Name;
                if (directoryName.Equals("swap", StringComparison.InvariantCultureIgnoreCase))
                {
                    // swap has many sub directories and so must be handled separately.
                    WriteSwapItemGroups(dir);
                }
                else
                {
                    ItemGroup group;
                    if (directoryName.Equals("cinematics", StringComparison.InvariantCultureIgnoreCase))
                    {
                        group = GetCustomItemGroup(dir, SkinItemGroupType.Cinematic, SkinItemType.Cinematic);
                    }
                    else if (
                        directoryName.Equals("areabackgrounds", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("charms", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("inventory", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("savehud", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("knight", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("spells", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("particles", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("ui", StringComparison.InvariantCultureIgnoreCase) ||
                        directoryName.Equals("minions", StringComparison.InvariantCultureIgnoreCase))
                    {
                        group = GetCustomItemGroup(dir, SkinItemGroupType.Traditional, SkinItemType.Traditional);
                    }
                    else
                    {
                        group = GetCustomItemGroup(dir, SkinItemGroupType.Custom, SkinItemType.Custom);
                    }
                    WriteGroupManifest(dir, group);
                }

            }

            //Directory.EnumerateFiles(, "manifest.json", SearchOption.AllDirectories);
        }
        private void WriteSwapItemGroups(string directoryPath)
        {
            string directoryName = new DirectoryInfo(directoryPath).Name;
            if (File.Exists(Path.Combine(directoryPath, "replace.txt")))
            {
                ItemGroup group = new ItemGroup
                {
                    Id = directoryName,
                    Type = SkinItemGroupType.Text,
                    Items = [new SkinItem
                                        {
                                Id = "replace.txt",
                                Type = SkinItemType.TextFindReplace,
                                Variants = [
                                    new Variant {
                                        Id = "default" ,
                                        Assets = [
                                            new AssetDefination {
                                                FileName = "replace.txt" ,
                                                Paths = []
                                            }]
                                    }]
                            }]
                };
                WriteGroupManifest(directoryPath, group);
            }
            var directories = Directory.GetDirectories(directoryPath);
            foreach (var directory in directories)
            {
                var group = GetSwapItemGroup(directory);
                WriteGroupManifest(directory, group);
            }
        }
        private ItemGroup GetSwapItemGroup(string directoryPath)
        {

            string directoryName = new DirectoryInfo(directoryPath).Name;
            List<SkinItem> items = new List<SkinItem>();
            SettingsLoader<Dictionary<string, List<string>>> NamesLoader = new();
            var namesFile = Path.Combine(directoryPath, "names.json");
            if (File.Exists(namesFile)) {
                var namesData = NamesLoader.Load(Path.Combine(directoryPath, "names.json"));
                // migrate names.json to manifest.json
                foreach (var name in namesData) {
                    SkinItem skinItem = new SkinItem
                    {
                        Id = name.Key,
                        Type = SkinItemType.SceneUnknown,
                        CustomSubType = "",
                        Variants = [ new Variant {
                                Id = "default" ,
                                Assets = [new AssetDefination {
                                    FileName = name.Key + ".png" ,
                                    Paths = name.Value.ToArray()
                                }]
                            }]
                    };
                    items.Add(skinItem);
                }
            }
            ItemGroup group = new ItemGroup
            {
                Id = directoryName.ToLowerInvariant(),
                Type = SkinItemGroupType.Scene,
                Items = items.ToArray()
            };
            return group;
        }

        private void WriteGroupManifest(string BasePath, ItemGroup group)
        {
            SettingsLoader<ItemGroup> ItemGroupLoader = new SettingsLoader<ItemGroup>();
            ItemGroupLoader.Save(Path.Combine(BasePath, "manifest.json"), group);
        }

        public ItemGroup GetCustomItemGroup(string directoryPath, SkinItemGroupType skinItemGroupType, SkinItemType skinItemType)
        {
            string directoryName = new DirectoryInfo(directoryPath).Name;
            List<SkinItem> items = new List<SkinItem>();

            //read all files
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                // create a default skin item for traditional knight items with possible alts
                var possibleAlts = files.Where(candidate => !Path.GetFileName(candidate).Equals(fileName) && Path.GetFileName(candidate).Contains(Path.GetFileNameWithoutExtension(file)));
                var altVariants = possibleAlts.Select(possibleAlt => new Variant
                {
                    Id = Path.GetFileNameWithoutExtension(possibleAlt),
                    Assets = [new AssetDefination {
                                    FileName = Path.GetFileName(possibleAlt) ,
                                    Paths = []
                                }]
                });
                SkinItem skinItem = new SkinItem
                {
                    Id = Path.GetFileNameWithoutExtension(file),
                    Type = skinItemType,
                    CustomSubType = directoryName,
                    Variants = [ new Variant {
                                Id = "default" ,
                                Assets = [new AssetDefination {
                                    FileName = fileName ,
                                    Paths = []
                                }]
                            },..altVariants]
                };
                items.Add(skinItem);
            }

            ItemGroup group = new ItemGroup
            {
                Id = directoryName.ToLowerInvariant(),
                Type = skinItemGroupType,
                Items = items.ToArray()
            };
            return group;
        }
    }
}

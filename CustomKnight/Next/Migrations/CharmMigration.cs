using System.IO;
using static Satchel.IoUtils;

namespace CustomKnight.Next.Migrations
{
    class CharmMigration : BaseMigration
    {
        public override void Run(MigrationContext context)
        {
            var charmsFolder = Path.Combine(context.SkinPath, "charms");
            string[] files = Directory.GetFiles(context.SkinPath);
            foreach (string file in files)
            {
                if (!Path.GetFileName(file).StartsWith("charm_"))
                {
                    continue;
                }
                try
                {
                    EnsureDirectory(charmsFolder);
                    File.Move(file, Path.Combine(charmsFolder, Path.GetFileName(file)));
                }
                catch (Exception e)
                {
                    CustomKnight.Instance.LogError("A File could not be Copied : " + e.ToString());
                }
            }
        }
    }
}

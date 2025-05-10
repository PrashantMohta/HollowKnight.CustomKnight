using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Satchel.IoUtils;

namespace CustomKnight.Next.Migrations
{
    class CharmMigration : BaseMigration
    {
        public override void Run(MigrationContext context)
        {
            var charmsFolder = Path.Combine(context.SkinPath, "Charms");
            string[] files = Directory.GetFiles(context.SkinPath);
            foreach (string file in files)
            {
                if (!Path.GetFileName(file).StartsWith("Charm_"))
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

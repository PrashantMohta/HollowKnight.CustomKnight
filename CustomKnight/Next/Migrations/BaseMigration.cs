

using System.IO;
using static Satchel.IoUtils;

namespace CustomKnight.Next.Migrations
{
    internal abstract class BaseMigration
    {

        public void SmartMigrate(string sourceFile, string targetPath, string targetName)
        {
            var destinationFile = Path.Combine(targetPath, targetName);
            if (File.Exists(sourceFile))
            {
                try
                {
                    EnsureDirectory(targetPath);
                    File.Move(sourceFile, destinationFile);
                }
                catch (Exception e)
                {
                    CustomKnight.Instance.LogError("A File could not be Copied : " + e.ToString());
                }
            }
        }
        public abstract void Run(MigrationContext context);
    }
}

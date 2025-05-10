using System.IO;

namespace CustomKnight.Next.Migrations
{
    internal class KnownSwapIntegrationMigration : BaseMigration
    {
        public void SmartMoveDirectory(string source, string destination, string directoryName)
        {
            if (Directory.Exists(Path.Combine(source, directoryName)))
            {
                Directory.Move(Path.Combine(source, directoryName), Path.Combine(destination, directoryName));
            }
        }
        public override void Run(MigrationContext context)
        {
            var swapPath = Path.Combine(context.SkinPath, "Swap");
            SmartMoveDirectory(swapPath, context.SkinPath, "CustomAudio");
            SmartMoveDirectory(swapPath, context.SkinPath, "Journal");
            SmartMoveDirectory(swapPath, context.SkinPath, "CustomImage");
        }
    }
}
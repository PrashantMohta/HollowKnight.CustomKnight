namespace CustomKnight.Next.Migrations
{
    static class MigrationManager
    {
        private static readonly List<BaseMigration> migrations = new List<BaseMigration> {
            new LowercaseSubpathMigration(),
            new CharmMigration(),
            new GroupMigration(),
            new KnownSwapIntegrationMigration(),
            new MigrateDirectorySwapsToNames(),
            new GenerateManifestMigration(),
        };

        public static void RunMigrations(MigrationContext context)
        {
            // todo maybe keep a record of migrations that have already run on this skin and don't re run them if these make the skin switching too slow.
            // fix skins button can re run them?
            foreach (var migration in migrations)
            {
                migration.Run(context);
            }
        }
    }
}

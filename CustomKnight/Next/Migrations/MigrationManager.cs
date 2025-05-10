using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight.Next.Migrations
{
    static class MigrationManager
    {
        private static readonly List<BaseMigration> migrations = new List<BaseMigration> { 
            new CharmMigration(),
            new GroupMigration(),
            new GenerateManifestMigration(),
        };

        public static void RunMigrations(MigrationContext context)
        {
            foreach (var migration in migrations)
            {
                migration.Run(context);
            }
        }
    }
}

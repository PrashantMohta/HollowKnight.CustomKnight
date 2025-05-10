using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight.Next.Migrations
{
    class GroupMigration : BaseMigration
    {
        public static Dictionary<string, Skinable> KnightGroup = new()
        {
            {Knight.NAME,new Knight()},
            {Sprint.NAME,new Sprint()},
            {Unn.NAME,new Unn()},
            {Shade.NAME,new Shade()},

            {"Cloak",new Preload("Cloak",() => CustomKnight.GameObjects["Cloak"])},
            {"Shriek",new Preload("Shriek",() => CustomKnight.GameObjects["Shriek"])},
            {"Wings",new Preload("Wings",() => CustomKnight.GameObjects["Wings"])},
            {"Quirrel",new Preload("Quirrel",() => CustomKnight.GameObjects["Quirrel"])},
            {"Webbed",new Preload("Webbed",() => CustomKnight.GameObjects["Webbed"])},
            {"DreamArrival",new Preload("DreamArrival",() => CustomKnight.GameObjects["DreamArrival"])},
            {"Dreamnail",new Preload("Dreamnail",() => CustomKnight.GameObjects["Dreamnail"])},
            {"Hornet",new Preload("Hornet",() => CustomKnight.GameObjects["Hornet"])},
            {"Birthplace",new Preload("Birthplace",() => CustomKnight.GameObjects["Birthplace"])},
        };

        public static Dictionary<string, Skinable> ParticlesGroup = new()
        {

            {ShadeOrb.NAME,new ShadeOrb()},

            {QOrbs.NAME,new QOrbs()},
            {QOrbs2.NAME,new QOrbs2()},
            {ScrOrbs.NAME,new ScrOrbs()},
            {ScrOrbs2.NAME,new ScrOrbs2()},
            {DungRecharge.NAME, new DungRecharge()},
            {SDCrystalBurst.NAME,new SDCrystalBurst()},
            {DoubleJFeather.NAME,new DoubleJFeather()},
            {Leak.NAME,new Leak()},
            {HitPt.NAME,new HitPt()},
            {ShadowDashBlobs.NAME,new ShadowDashBlobs()},
            {Deathpt.NAME,new Deathpt()},
            {DDeathpt.NAME,new DDeathpt()},

            {Grubberfly.NAME,new Grubberfly()},

            {DeathNail.NAME,new DeathNail() },
            {DeathAsh.NAME,new DeathAsh() },
            {BrummWave.NAME,new BrummWave() },
            {BrummShield.NAME,new BrummShield() },
            {FlowerBreak.NAME,new FlowerBreak() }
        };

        public static Dictionary<string, Skinable> SpellSkinables = new()
        {

            {Wraiths.NAME,new Wraiths()},
            {VoidSpells.NAME,new VoidSpells()},
            {VS.NAME,new VS()},

        };

        public static Dictionary<string, Skinable> InterfaceSkinables = new()
        {
            {Geo.NAME,new Geo()},
            {Hud.NAME,new Hud()},
            {OrbFull.NAME,new OrbFull()},
            {Liquid.NAME,new Liquid()},
            {Compass.NAME,new Compass()},
        };

        public static Dictionary<string, Skinable> MinionsSkinables = new()
        {
            {Baldur.NAME,new Baldur()},
            {Fluke.NAME,new Fluke()},
            {Grimm.NAME,new Grimm()},
            {Shield.NAME,new Shield()},
            {Weaver.NAME,new Weaver()},
            {Hatchling.NAME,new Hatchling()},
            {Salubra.NAME,new Salubra() }
        };

        public override void Run(MigrationContext context)
        {
            foreach (var kvp in KnightGroup)
            {
                var fileName = kvp.Value.ckTex.fileName;
                var sourceFile = Path.Combine(context.SkinPath, fileName);
                var targetPath = Path.Combine(context.SkinPath, "knight");
                SmartMigrate(sourceFile, targetPath, fileName);
            }

            foreach (var kvp in ParticlesGroup)
            {
                var fileName = kvp.Value.ckTex.fileName;
                var sourceFile = Path.Combine(context.SkinPath, fileName);
                var targetPath = Path.Combine(context.SkinPath, "particles");
                SmartMigrate(sourceFile, targetPath, fileName);
            }

            foreach (var kvp in SpellSkinables)
            {
                var fileName = kvp.Value.ckTex.fileName;
                var sourceFile = Path.Combine(context.SkinPath, fileName);
                var targetPath = Path.Combine(context.SkinPath, "spells");
                SmartMigrate(sourceFile, targetPath, fileName);
            }

            foreach (var kvp in InterfaceSkinables)
            {
                var fileName = kvp.Value.ckTex.fileName;
                var sourceFile = Path.Combine(context.SkinPath, fileName);
                var targetPath = Path.Combine(context.SkinPath, "ui");
                SmartMigrate(sourceFile, targetPath, fileName);
            }

            foreach (var kvp in MinionsSkinables)
            {
                var fileName = kvp.Value.ckTex.fileName;
                var sourceFile = Path.Combine(context.SkinPath, fileName);
                var targetPath = Path.Combine(context.SkinPath, "minions");
                SmartMigrate(sourceFile, targetPath, fileName);
            }
        }
    }
}

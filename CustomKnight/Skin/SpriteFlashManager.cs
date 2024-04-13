namespace CustomKnight
{
    internal static class SpriteFlashManager
    {

        internal static void Hook()
        {
            CustomKnight.OnInit += CustomKnight_OnInit;
            CustomKnight.OnUnload += CustomKnight_OnUnload;
        }

        private static void CustomKnight_OnInit(object sender, EventArgs e)
        {
            On.SpriteFlash.flashDung += SpriteFlash_flashDung;
            On.SpriteFlash.flashDungQuick += SpriteFlash_flashDungQuick;
            On.SpriteFlash.flashFocusHeal += SpriteFlash_flashFocusHeal;
        }
        private static void CustomKnight_OnUnload(object sender, EventArgs e)
        {
            On.SpriteFlash.flashDung -= SpriteFlash_flashDung;
            On.SpriteFlash.flashDungQuick -= SpriteFlash_flashDungQuick;
            On.SpriteFlash.flashFocusHeal -= SpriteFlash_flashFocusHeal;
        }

        private static void SpriteFlash_flashFocusHeal(On.SpriteFlash.orig_flashFocusHeal orig, global::SpriteFlash self)
        {
            orig(self);
            if (SkinManager.GetCurrentSkin() is StaticSkin skin)
            {
                var block = ReflectionHelper.GetField<SpriteFlash, MaterialPropertyBlock>(self, "block");
                block.SetColor("_FlashColor", skin.skinConfig.flashColor);
                ReflectionHelper.SetField<SpriteFlash, MaterialPropertyBlock>(self, "block", block);
            }
        }

        private static void SpriteFlash_flashDungQuick(On.SpriteFlash.orig_flashDungQuick orig, global::SpriteFlash self)
        {
            orig(self);
            if (SkinManager.GetCurrentSkin() is StaticSkin skin)
            {
                var block = ReflectionHelper.GetField<SpriteFlash, MaterialPropertyBlock>(self, "block");
                block.SetColor("_FlashColor", skin.skinConfig.dungFlash);
                ReflectionHelper.SetField<SpriteFlash, MaterialPropertyBlock>(self, "block", block);
            }
        }

        private static void SpriteFlash_flashDung(On.SpriteFlash.orig_flashDung orig, global::SpriteFlash self)
        {
            orig(self);
            if (SkinManager.GetCurrentSkin() is StaticSkin skin)
            {
                var block = ReflectionHelper.GetField<SpriteFlash, MaterialPropertyBlock>(self, "block");
                block.SetColor("_FlashColor", skin.skinConfig.dungFlash);
                ReflectionHelper.SetField<SpriteFlash, MaterialPropertyBlock>(self, "block", block);
            }
        }

    }
}

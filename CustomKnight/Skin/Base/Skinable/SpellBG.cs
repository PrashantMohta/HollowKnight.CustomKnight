namespace CustomKnight
{
    internal class SpellBG:Skinable_Sprite
    {
        public static string Name = "SpellBG";
        public SpellBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject spellbg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Spell Quake").FindGameObjectInChildren("Quake Bg");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(spellbg.GetComponent<SpriteRenderer>().sprite), spellbg.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject quakebg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Spell Quake").FindGameObjectInChildren("Quake Bg");
            quakebg.GetComponent<SpriteRenderer>().sprite = sprite;
            GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Spell Fireball").FindGameObjectInChildren("Fireball Bg").GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
            GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Spell Scream").FindGameObjectInChildren("Scream Bg").GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
        }
    }
}

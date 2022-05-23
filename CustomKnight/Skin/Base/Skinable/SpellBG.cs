namespace CustomKnight
{
    internal class SpellBG:Skinable_Sprite
    {
        public static string Name = "Inventory/SpellBG";
        public SpellBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject spellbg = SkinManager.invitem.FindGameObjectInChildren("Spell Quake").FindGameObjectInChildren("Quake Bg");
            ckTex.defaultSprite = spellbg.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject quakebg = SkinManager.invitem.FindGameObjectInChildren("Spell Quake").FindGameObjectInChildren("Quake Bg");
            quakebg.GetComponent<SpriteRenderer>().sprite = sprite;
            SkinManager.invitem.FindGameObjectInChildren("Spell Fireball").FindGameObjectInChildren("Fireball Bg").GetComponent<SpriteRenderer>().sprite = sprite;
            SkinManager.invitem.FindGameObjectInChildren("Spell Scream").FindGameObjectInChildren("Scream Bg").GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}

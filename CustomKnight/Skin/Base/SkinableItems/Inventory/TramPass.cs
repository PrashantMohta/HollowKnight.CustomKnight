namespace CustomKnight
{
    internal class TramPass : Skinable_Sprite
    {
        public static string Name = "Inventory/TramPass";
        public TramPass() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Tram Pass");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Tram Pass");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
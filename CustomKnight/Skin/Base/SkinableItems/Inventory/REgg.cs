using CustomKnight;
internal class REgg : Skinable_Sprite
{
    public static string Name = "Inventory/RancidEgg";
    public REgg() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Rancid Egg");
        ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Rancid Egg");
        sd.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

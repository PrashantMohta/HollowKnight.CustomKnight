using CustomKnight;
public class Brand : Skinable_Sprite
{
    public static string Name = "Inventory/Brand";
    public Brand() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Kings Brand");
        ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Kings Brand");
        sd.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

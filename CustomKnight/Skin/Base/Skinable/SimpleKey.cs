using CustomKnight;
public class SimpleKey : Skinable_Sprite
{
    public static string Name = "Inventory/SimpleKey";
    public SimpleKey() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Simple Key");
        ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Simple Key");
        sd.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

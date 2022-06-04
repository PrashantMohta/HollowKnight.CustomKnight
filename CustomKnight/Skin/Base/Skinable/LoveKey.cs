using CustomKnight;
public class LoveKey : Skinable_Sprite
{
    public static string Name = "Inventory/LoveKey";
    public LoveKey() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Love Key");
        ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite ;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Love Key");
        sd.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
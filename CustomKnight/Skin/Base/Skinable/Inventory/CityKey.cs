using CustomKnight;
public class CityKey : Skinable_Sprite
{
    public static string Name = "Inventory/CityKey";
    public CityKey() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("City Key");
        ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = SkinManager.equipment.FindGameObjectInChildren("City Key");
        sd.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

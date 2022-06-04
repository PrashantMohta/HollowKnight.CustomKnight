
namespace CustomKnight
{
    public class InvClaw : Skinable_Sprite
    {
        public static string Name = "Inventory/Claw";
        public InvClaw() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject cw = SkinManager.equipment.FindGameObjectInChildren("Mantis Claw");
            ckTex.defaultSprite = cw.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject cw = SkinManager.equipment.FindGameObjectInChildren("Mantis Claw");
            cw.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}

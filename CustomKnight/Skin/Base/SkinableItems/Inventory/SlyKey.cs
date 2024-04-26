namespace CustomKnight
{
    internal class SlyKey : Skinable_Sprite
    {
        public static string Name = "Inventory/SlyKey";
        public SlyKey() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Store Key");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Store Key");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}

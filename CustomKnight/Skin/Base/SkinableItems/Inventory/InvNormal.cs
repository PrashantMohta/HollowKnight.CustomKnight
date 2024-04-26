namespace CustomKnight
{
    internal class InvNormal : Skinable_Sprite
    {
        public string ItemName;
        public InvNormal(string name) : base(name)
        {
            ItemName = name;
        }
        public override void SaveDefaultTexture()
        {
            switch (ItemName)
            {
                case "Inventory/DSlash":
                    ckTex.defaultSprite = SkinManager.invitem.FindGameObjectInChildren("Art Uppercut").GetComponent<InvItemDisplay>().activeSprite;//so confused
                    break;
                case "Inventory/CSlash":
                    ckTex.defaultSprite = SkinManager.invitem.FindGameObjectInChildren("Art Cyclone").GetComponent<InvItemDisplay>().activeSprite;
                    break;
                case "Inventory/GSlash":
                    ckTex.defaultSprite = SkinManager.invitem.FindGameObjectInChildren("Art Dash").GetComponent<InvItemDisplay>().activeSprite;
                    break;
                case "Inventory/DreamGate":
                    ckTex.defaultSprite = SkinManager.invitem.FindGameObjectInChildren("Dream Gate").GetComponent<InvItemDisplay>().activeSprite;
                    break;
                case "Inventory/Geo":
                    ckTex.defaultSprite = SkinManager.invitem.FindGameObjectInChildren("Geo").GetComponent<SpriteRenderer>().sprite;
                    break;
                default:
                    break;

            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            switch (ItemName)
            {
                case "Inventory/DSlash":
                    SkinManager.invitem.FindGameObjectInChildren("Art Uppercut").GetComponent<InvItemDisplay>().activeSprite = sprite;
                    break;
                case "Inventory/CSlash":
                    SkinManager.invitem.FindGameObjectInChildren("Art Cyclone").GetComponent<InvItemDisplay>().activeSprite = sprite;
                    break;
                case "Inventory/GSlash":
                    SkinManager.invitem.FindGameObjectInChildren("Art Dash").GetComponent<InvItemDisplay>().activeSprite = sprite;
                    break;
                case "Inventory/DreamGate":
                    SkinManager.invitem.FindGameObjectInChildren("Dream Gate").GetComponent<InvItemDisplay>().activeSprite = sprite;
                    break;
                case "Inventory/Geo":
                    SkinManager.invitem.FindGameObjectInChildren("Geo").GetComponent<SpriteRenderer>().sprite = sprite;
                    break;
                default:
                    break;

            }
        }
    }
}

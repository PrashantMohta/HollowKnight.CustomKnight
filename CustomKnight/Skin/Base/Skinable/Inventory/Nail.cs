namespace CustomKnight
{
    public class Nail : Skinable_Sprite
    {
        public string NailLevel;
        public Nail(string naillevel) : base(naillevel)
        {
            NailLevel = naillevel;
        }
        public override void SaveDefaultTexture()
        {
            InvNailSprite nailsp = SkinManager.inv.FindGameObjectInChildren("Nail").GetComponent<InvNailSprite>();
            switch (NailLevel)
            {
                case "Inventory/Nail_1":
                    ckTex.defaultSprite = nailsp.level1;
                    break;
                case "Inventory/Nail_2":
                    ckTex.defaultSprite = nailsp.level2;
                    break;
                case "Inventory/Nail_3":
                    ckTex.defaultSprite = nailsp.level3;
                    break;
                case "Inventory/Nail_4":
                    ckTex.defaultSprite = nailsp.level4;
                    break;
                case "Inventory/Nail_5":
                    ckTex.defaultSprite = nailsp.level5;
                    break;
                default:
                    break;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            InvNailSprite nailsp = SkinManager.inv.FindGameObjectInChildren("Nail").GetComponent<InvNailSprite>();
            switch (NailLevel)
            {
                case "Inventory/Nail_1":
                    nailsp.level1 = sprite;
                    break;
                case "Inventory/Nail_2":
                    nailsp.level2 = sprite;
                    break;
                case "Inventory/Nail_3":
                    nailsp.level3 = sprite;
                    break;
                case "Inventory/Nail_4":
                    nailsp.level4 = sprite;
                    break;
                case "Inventory/Nail_5":
                    nailsp.level5 = sprite;
                    break;
                default:
                    break;
            }
        }
    }
}

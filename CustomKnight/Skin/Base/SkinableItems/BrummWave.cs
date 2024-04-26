namespace CustomKnight
{
    internal class BrummWave : Skinable_Sprite
    {
        public static string NAME = "BrummWave";
        public BrummWave() : base(BrummWave.NAME) { }

        public override void ApplySprite(Sprite sprite)
        {
            GameObject hd = HeroController.instance.transform.Find("Brumm_shield").gameObject;
            hd.Find("brumm_shield_pieces_0000_Layer-2").GetAddComponent<SpriteRenderer>().sprite = sprite;
            hd.Find("brumm_shield_pieces_0000_Layer-2 (1)").GetAddComponent<SpriteRenderer>().sprite = sprite;
            if (SkinManager.GetCurrentSkin() is ISupportsConfig skin)
            {
                hd.Find("Shield_core").GetComponent<SpriteRenderer>().color = skin.GetConfig().brummColor;
            }

        }

        public override void SaveDefaultTexture()
        {
            GameObject hd = HeroController.instance.transform.Find("Brumm_shield").gameObject;
            ckTex.defaultSprite = hd.Find("brumm_shield_pieces_0000_Layer-2").GetAddComponent<SpriteRenderer>().sprite;
        }
    }
}

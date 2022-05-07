namespace CustomKnight
{
    public class Flower : Skinable_Sprite
    {
        public static string Name = "Flower";
        public int broken;
        public Flower(int broken) : base(Name+$"_{broken}") {
            this.broken = broken;

        }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Xun Flower");
            if(broken==1)
            {
                ckTex.defaultSprite = sd.GetComponent<InvItemDisplay>().inactiveSprite;
            }
            if(broken==0)
            {
                ckTex.defaultSprite = sd.GetComponent<InvItemDisplay>().activeSprite;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Xun Flower");
            if (broken == 1)
            {
                sd.GetComponent<InvItemDisplay>().inactiveSprite= CreateSpritewithppu(sprite.texture, sd.GetComponent<InvItemDisplay>().inactiveSprite.pixelsPerUnit);
            }
            if (broken == 0)
            {
                 sd.GetComponent<InvItemDisplay>().activeSprite= CreateSpritewithppu(sprite.texture, sd.GetComponent<InvItemDisplay>().activeSprite.pixelsPerUnit);
            }
        }
    }
}

namespace CustomKnight
{
    public class Flower : Skinable_Sprite
    {
        public static string Name = "Inventory/Flower";
        public int broken;
        public Flower(int broken) : base(Name+$"_{broken}") {
            this.broken = broken;

        }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Xun Flower");
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
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Xun Flower");
            if (broken == 1)
            {
                sd.GetComponent<InvItemDisplay>().inactiveSprite = sprite;
            }
            if (broken == 0)
            {
                 sd.GetComponent<InvItemDisplay>().activeSprite= sprite;
            }
        }
    }
}

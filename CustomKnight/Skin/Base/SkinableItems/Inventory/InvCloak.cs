
namespace CustomKnight
{
    internal class InvCloak : Skinable_Sprite
    {
        public static string BaseName = "Inventory/Cloak";
        public int level;
        public InvCloak(int Level) : base(BaseName + $"_{Level}")
        {
            this.level = Level;
        }
        public override void SaveDefaultTexture()
        {

            PlayMakerFSM eqfsm = SkinManager.equipment.LocateMyFSM("Build Equipment List");
            if (level == 1)//I hate this
            {
                ckTex.defaultSprite = SkinManager.equipment.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite;
            }
            if (level == 2)
            {
                ckTex.defaultSprite = eqfsm.GetAction<SetSpriteRendererSprite>("Dash", 16).sprite.Value as Sprite;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {

            PlayMakerFSM eqfsm = SkinManager.equipment.LocateMyFSM("Build Equipment List");
            if (level == 1 && !PlayerData.instance.hasShadowDash)
            {
                SkinManager.equipment.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite = sprite;
            }
            if (level == 2)
            {
                eqfsm.GetAction<SetSpriteRendererSprite>("Dash", 16).sprite.Value = sprite;
            }
        }
    }
}

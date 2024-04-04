namespace CustomKnight
{
    public class DeathNail : Skinable_Hook
    {
        public static string NAME = "DeathNail";
        public DeathNail() : base(NAME) { }

        public override void Hook()
        {
            On.AutoRecycleSelf.OnEnable += AutoRecycleSelf_OnEnable;
        }
        public override void UnHook()
        {
            On.AutoRecycleSelf.OnEnable -= AutoRecycleSelf_OnEnable;
        }
        private void AutoRecycleSelf_OnEnable(On.AutoRecycleSelf.orig_OnEnable orig, AutoRecycleSelf self)
        {
            if (self.name.Contains("Corpse Nail Hero"))
            {
                self.gameObject.GetComponent<SpriteRenderer>().sprite = cachedSprite;
            }
            orig(self);
        }

        public override void SaveDefaultTexture()
        {
            var go = HeroController.instance.transform.Find("Hero Death").gameObject;
            go = go.LocateMyFSM("Hero Death Anim").GetAction<FlingObjectsFromGlobalPool>("Blow", 0).gameObject.Value;
            ckTex.defaultSprite = go.GetComponent<SpriteRenderer>().sprite;
        }

    }
}

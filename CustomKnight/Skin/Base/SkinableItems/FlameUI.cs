namespace CustomKnight.Skin.Base.SkinableItems
{
    class FlameUI : Skinable_Sprite
    {
        public const string FRONT = "front";
        public const string BACK = "back";

        private string flameType;

        public FlameUI(string flameType) : base("Charms/" + flameType)
        {
            this.flameType = flameType;
        }

        public override void ApplySprite(Sprite sprite)
        {
            PlayMakerFSM FlameUIFsm = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Grimm Flame UI").LocateMyFSM("Control");
            switch (flameType)
            {
                case FRONT:
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 1", 0).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 2", 0).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 2", 1).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 3", 0).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 3", 1).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 3", 2).sprite.Value = sprite;
                    break;
                case BACK:
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("None", 0).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("None", 1).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("None", 2).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 1", 1).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 1", 2).sprite.Value = sprite;
                    FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 2", 2).sprite.Value = sprite;
                    break;
            }
        }

        public override void SaveDefaultTexture()
        {
            PlayMakerFSM FlameUIFsm = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Grimm Flame UI").LocateMyFSM("Control");
            switch (flameType)
            {
                case FRONT:
                    ckTex.defaultSprite = FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 1", 0).sprite.Value as Sprite;
                    break;
                case BACK:
                    ckTex.defaultSprite = FlameUIFsm.GetAction<SetSpriteRendererSprite>("S1 F 1", 1).sprite.Value as Sprite;
                    break;
            }
        }
    }
}

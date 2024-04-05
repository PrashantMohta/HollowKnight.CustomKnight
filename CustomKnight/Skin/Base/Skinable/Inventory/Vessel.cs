namespace CustomKnight
{
    public class Vessel : Skinable_Sprite
    {
        public string VesselName;
        public int Vesselamount;
        public Vessel(string vesselname, int pieces) : base(vesselname)
        {
            Vesselamount = pieces;
            VesselName = vesselname;
        }
        public override void SaveDefaultTexture()
        {
            InvVesselFragments vesselFragments = SkinManager.inv.FindGameObjectInChildren("Soul Orb").GetComponent<InvVesselFragments>();
            switch (Vesselamount)
            {
                case 0:
                    ckTex.defaultSprite = vesselFragments.backboardSprite;
                    break;
                case 1:
                    ckTex.defaultSprite = vesselFragments.singlePieceSprite;
                    break;
                case 2:
                    ckTex.defaultSprite = vesselFragments.doublePieceSprite;
                    break;
                case 3:
                    ckTex.defaultSprite = vesselFragments.fullSprite;
                    break;
                default:
                    break;
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            InvVesselFragments vesselFragments = SkinManager.inv.FindGameObjectInChildren("Soul Orb").GetComponent<InvVesselFragments>();
            switch (Vesselamount)
            {
                case 0:
                    vesselFragments.backboardSprite = sprite;
                    break;
                case 1:
                    vesselFragments.singlePieceSprite = sprite;
                    break;
                case 2:
                    vesselFragments.doublePieceSprite = sprite;
                    break;
                case 3:
                    vesselFragments.fullSprite = sprite;
                    break;
                default:
                    break;
            }
        }
    }
}

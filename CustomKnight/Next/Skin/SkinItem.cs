using System.Linq;
using CustomKnight.Next.Skin.Enum;

namespace CustomKnight.Next.Skin
{
    public class SkinItem
    {
        public string Id { get; set; }
        public SkinItemType Type { get; set; }
        public string CustomSubType { get; set; }
        public Variant[] Variants { get; set; }

        public AssetItem[] GetAssetItems(Context context, string variantId)
        {
            var assets = Variants.First(i => i.Id == variantId).Assets;
            return [.. assets.Select(asset => new AssetItem(context, this, variantId, asset))];
        }

    }
}

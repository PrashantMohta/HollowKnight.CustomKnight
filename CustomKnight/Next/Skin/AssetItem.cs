
using System.IO;

namespace CustomKnight.Next.Skin
{
    // runtime model
    class AssetItem(Context context, SkinItem skinItem, string variantId, AssetDefination defination)
    {
        public SkinItem SkinItem { get; private set; } = skinItem;
        public string VariantId { get; set; } = variantId;
        public AssetDefination Defination { get; private set; } = defination;
        public Context AssetContext { get; private set; } = context;

        public virtual byte[] GetRawData() => File.ReadAllBytes(Path.Combine(AssetContext.GroupBasePath, Defination.FileName));
        public virtual Texture2D GetTexture2D() => TextureUtils.LoadTextureFromFile(Path.Combine(AssetContext.GroupBasePath, Defination.FileName));
        public virtual Sprite GetSprite() => throw new NotSupportedException();
        public virtual string GetAssetUrl() => throw new NotSupportedException();
    }
}

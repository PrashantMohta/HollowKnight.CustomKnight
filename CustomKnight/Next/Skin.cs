
using System.IO;

namespace CustomKnight.Next
{
    enum Feature
    {
        Tradional,
        Charms,
        SceneSwaps,
        GlobalSwaps,
        Cinematics,
        SaveSelection,
        UserInterface,
        MainMenu
    }

    enum SkinItemGroupType
    {
        Traditional,
        Scene,
        UserInterface,
        Menu,
        Cinematic,
        Custom
    }

    enum SkinItemType
    {
        Traditional, //use ID to determine :/
        SceneTk2d,
        SceneSprite,
        SceneParticle,
        Cinematic,
        Custom
    }
    class Context
    {
        public string RelativePath { get; set; }
    }
    abstract class AssetResolver
    {
        public string SkinItemId { get; set; }
        public SkinItemType Type { get; set; }
        public AssetDefination Defination { get; set; }
        public virtual byte[] GetRawData() => throw new NotSupportedException();
        public virtual Texture2D GetTexture2D() => throw new NotSupportedException();
        public virtual Sprite GetSprite() => throw new NotSupportedException();
        public virtual string GetAssetUrl() => throw new NotSupportedException();
    }

    class StaticTextureAssetResolver : AssetResolver
    {
        public Context context { get; set; }
        public override Texture2D GetTexture2D()
        {
            return TextureUtils.LoadTextureFromFile(Path.Combine(context.RelativePath,Defination.FileName));
        }
    }
    class AssetDefination
    {
        public string FileName;
        public string[] Paths;
    }
    class SkinItemVariant
    {
        public string Id {  get; set; }
        public AssetDefination[] Assets { get; set; }
    }
    class SkinItem
    {
        public string Id { get; set; }
        public SkinItemType Type { get; set; }
        public string CustomSubType { get; set; }
        public SkinItemVariant[] Variants { get; set; }
    }

    class SkinItemGroup
    {
        public string Id { get; set; }
        public SkinItemGroupType Type { get; set; }
        public SkinItem[] Items { get; set; }
    }

    abstract class Skin
    {
        /// <summary>
        /// List of features supported by the skin
        /// </summary>
        public readonly Feature[] features;

        /// <summary>
        ///  GetId
        /// </summary>
        /// <returns>The unique id of the skin as a <c>string</c></returns>
        public abstract string GetId();

        /// <summary>
        ///  GetName
        /// </summary>
        /// <returns>The Name to be displayed in the menu as a <c>string</c></returns>
        public abstract string GetName();

        /// <summary>
        ///  shouldCache
        /// </summary>
        /// <returns>A <c>bool</c> representing if the texture can be cached in memory or not.</returns>
        public virtual bool ShouldCache() => false;

        public SkinItemGroup[] SkinItemGroups { get; set; }


        public  AssetResolver[] GetAssetResolver(SkinItem skinItem, string variantId)
        {
            var variant = Array.Find(skinItem.Variants, item => item.Id == variantId);
            List<AssetResolver> assetResolvers = new List<AssetResolver>();
            for (int i = 0; i < variant.Assets.Length; i++)
            {
                switch (skinItem.Type) //todo actually populate all the switchcase types
                {
                    case SkinItemType.Traditional:
                    case SkinItemType.SceneTk2d:
                        assetResolvers.Add(new StaticTextureAssetResolver {  }); //todo fix this to properly init this
                        break;
                    default:
                        break;
                }
            }
            return assetResolvers.ToArray();
        }
    }
}

using CustomKnight.Next.Skin.Enum;

namespace CustomKnight.Next.Skin
{
    class ItemGroup
    {
        public string Id { get; set; }
        public SkinItemGroupType Type { get; set; }
        public SkinItem[] Items { get; set; }
        internal Context Context { get;  set; }

    }
}

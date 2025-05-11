namespace CustomKnight.Next.Skin.Enum
{
    public enum SkinItemType
    {
        Traditional, //use ID to determine :/
        SceneTk2d,
        SceneSprite,
        SceneParticle,
        SceneUnknown, // when auto migrating the swap directory we can't know the GO details.
        Cinematic,
        TextKeyReplace,
        TextFindReplace,
        Custom
    }
}

using MonoMod.RuntimeDetour.HookGen;
using UnityEngine.Video;

namespace CustomKnight.Skin.Cinematics
{
    internal static class CinematicHelper
    {
        /// <summary>
        ///     Contains necessary information to create Hooks. Does not contain any hooks
        /// </summary>
        public static class Delegates
        {
            public delegate VideoClip get_EmbeddedVideoClip_AfterArgs(Params_get_EmbeddedVideoClip args, VideoClip ret);

            public delegate void get_EmbeddedVideoClip_BeforeArgs(Params_get_EmbeddedVideoClip args);

            public delegate VideoClip get_EmbeddedVideoClip_WithArgs(Func<CinematicVideoReference, VideoClip> orig,
                CinematicVideoReference self);

            public sealed class Params_get_EmbeddedVideoClip
            {
                public CinematicVideoReference self;
            }
        }
        public static event Delegates.get_EmbeddedVideoClip_WithArgs get_EmbeddedVideoClip
        {
            add => HookEndpointManager.Add<Delegates.get_EmbeddedVideoClip_WithArgs>(
                ReflectionHelper.GetMethodInfo(typeof(CinematicVideoReference), "get_EmbeddedVideoClip"), value);
            remove => HookEndpointManager.Remove<Delegates.get_EmbeddedVideoClip_WithArgs>(
                ReflectionHelper.GetMethodInfo(typeof(CinematicVideoReference), "get_EmbeddedVideoClip"), value);
        }
    }

    internal class CinematicSequenceR
    {
        internal T GetField<T>(string name) {
            return ReflectionHelper.GetField<CinematicSequence,T>(this.orig, name);
        }
        internal void SetField<T>(string name,T value)
        {
            ReflectionHelper.SetField<CinematicSequence, T>(this.orig, name,value);
        }
        internal CinematicSequence orig;
        internal CinematicSequenceR(CinematicSequence orig)
        {
            this.orig = orig;
        }

        public CinematicVideoReference videoReference
        {
            get => GetField<CinematicVideoReference>("videoReference");
            set => SetField("videoReference",value);
        }

        public int framesSinceBegan
        {
            get => GetField<int>("framesSinceBegan");
            set => SetField("framesSinceBegan", value);
        }
    }
}

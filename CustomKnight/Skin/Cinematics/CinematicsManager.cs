using System.IO;
using System.Linq;
using static Satchel.GameObjectUtils;
using static Satchel.IoUtils;
using HKMirror.Reflection;
using HKMirror.Hooks.OnHooks;
using UnityEngine.Video;
/*
video : StagTunnelRun:Assets/Cinematics/Stag_tunnel_run_v03.mp4
video : CharmSlugKiss:Assets/Cinematics/Charm_Slug_Kiss_30fps.mov
video : Intro:Assets/Cinematics/Intro_cinematic_v01.mov
video : Prologue:Assets/Cinematics/Prologue_Cutscene_30fps.mov
video : Nailsmith:Assets/Cinematics/Nailsmith_cinematic.mov
video : NailsmithPaint:Assets/Cinematics/Nailsmith_Paint_Cinematic.mov
video : Blacksmith:Assets/Cinematics/Blacksmith_cinematic.mov

video : FinalA:Assets/Cinematics/Final_Cutscene_A_v01.mov
video : FinalB:Assets/Cinematics/Final_Cutscene_B_v01.mov 
video : FinalC:Assets/Cinematics/Final_Cutscene_C_v01.mov
video : FinalD:Assets/Cinematics/Cutscene_ending_D_with_Sound_v02
video : FinalE:Assets/Cinematics/Cutscene_ending_E_with_Sound_v03
video : MrMushroom:Assets/Cinematics/mr_mushroom_v02.mov
video : Telescope:Assets/Cinematics/Telescope_cinematic_30fps.mov
*/
namespace CustomKnight
{
    public class CinematicsManager{
        internal VideoPlayer stagClip;
        internal Dictionary<string,Cinematic> Cinematics = new Dictionary<string,Cinematic>(){
            {"Prologue", new Cinematic("Prologue")},
            {"Intro", new Cinematic("Intro")},
            {"StagTunnelRun", new Cinematic("StagTunnelRun")},
            {"CharmSlugKiss", new Cinematic("CharmSlugKiss")},
            {"Nailsmith", new Cinematic("Nailsmith")},
            {"NailsmithPaint", new Cinematic("NailsmithPaint")},
            {"Blacksmith", new Cinematic("Blacksmith")},
            {"FinalA", new Cinematic("FinalA")},
            {"FinalB", new Cinematic("FinalB")},
            {"FinalC", new Cinematic("FinalC")},
            {"FinalD", new Cinematic("FinalD")},
            {"FinalE", new Cinematic("FinalE")},
            {"MrMushroom", new Cinematic("MrMushroom")},
            {"Telescope", new Cinematic("Telescope")}
        };
        
        private Dictionary<string,string> cinematicFileUrlCache = new();
        internal CinematicsManager(){
            if(CustomKnight.isSatchelInstalled()){
                On.CinematicSequence.Update += CinematicSequence_Update;
                OnCinematicVideoReference.WithOrig.get_EmbeddedVideoClip += WithOrig_get_EmbeddedVideoClip;
                On.XB1CinematicVideoPlayer.ctor += XB1CinematicVideoPlayer_ctor;
            }

        }

        private bool GetCiematicSafely(string name, out Cinematic cinematic)
        {
            if (Cinematics.TryGetValue(name, out cinematic))
            {
                return true;
            }
            else
            {
                CustomKnight.Instance.Log($"Cinematic '{name}' not implemented in CustomKnight");
                return false;
            }
        }


        public bool HasCinematic(string cinematicName)
        {
            if (cinematicFileUrlCache.TryGetValue(cinematicName, out var url))
            {
                return url.Length > 0;
            }
            else
            {
                EnsureDirectory($"{SkinManager.DATA_DIR}/Cinematics/");
                string file = ($"{SkinManager.DATA_DIR}/Cinematics/{cinematicName}").Replace("\\", "/");
                cinematicFileUrlCache[cinematicName] = GetCinematicUrl(cinematicName);
                return cinematicFileUrlCache[cinematicName].Length > 0;
            }

        }

        private void XB1CinematicVideoPlayer_ctor(On.XB1CinematicVideoPlayer.orig_ctor orig, XB1CinematicVideoPlayer self, CinematicVideoPlayerConfig config)
        {
            orig(self, config);
            VideoPlayer source = ReflectionHelper.GetField<XB1CinematicVideoPlayer, VideoPlayer>(self, "videoPlayer"); ;
            if (source.clip != null)
            {
                foreach (var cinematicKvp in Cinematics)
                {
                    if (cinematicKvp.Value.OriginalVideo != null && (cinematicKvp.Value.OriginalVideo.originalPath == source.clip.originalPath))
                    {
                        if (SkinManager.GetCurrentSkin().HasCinematic(cinematicKvp.Value.ClipName))
                        {
                            source.clip = null;
                            source.url = SkinManager.GetCurrentSkin().GetCinematicUrl(cinematicKvp.Value.ClipName);
                            source.Prepare();
                        }
                        else if (HasCinematic(cinematicKvp.Value.ClipName))
                        {
                            source.clip = null;
                            source.url = GetCinematicUrl(cinematicKvp.Value.ClipName);
                            source.Prepare();
                        }
                    }
                }
            }
        }


        public string GetCinematicUrl(string cinematicName){
            string path = "";
            string file = ($"{SkinManager.DATA_DIR}/Cinematics/{cinematicName}").Replace("\\", "/");
            if(File.Exists(file+".webm")){
                path = file+".webm";
            }
            CustomKnight.Instance.Log("[getCine]"+cinematicName+":"+path);
            return path;
        }

        private void CinematicSequence_Update(On.CinematicSequence.orig_Update orig, CinematicSequence self)
        {
            var fles = self.Reflect();
            if(GetCiematicSafely(fles.videoReference.VideoFileName,out var cinematic)){
                if(SkinManager.GetCurrentSkin().HasCinematic(cinematic.ClipName) || HasCinematic(cinematic.ClipName)){
                    fles.framesSinceBegan = 0;
                }
            } 
            orig(self);
        }

        private VideoClip WithOrig_get_EmbeddedVideoClip(Func<CinematicVideoReference, UnityEngine.Video.VideoClip> orig, CinematicVideoReference self)
        {
            if(GetCiematicSafely(self.VideoFileName,out var cinematic)){
                var originalVideo = orig(self);
                if(originalVideo != null){
                    cinematic.OriginalVideo = originalVideo;
                }
                return cinematic.OriginalVideo;
            }
            return orig(self);
        }
    }
}
using System;
using System.Collections;
using System.IO;
using HutongGames.PlayMaker.Actions;
using InControl;
using ModCommon;
using ModCommon.Util;
using Modding;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CustomKnight.Canvas
{
    public class SkinSwapperPanel
    {
        public static CanvasPanel Panel;

        private static float y;

        public static void BuildMenu(GameObject canvas)
        {
            Log("Building Skin Swapper Panel");
            y = 30.0f;
            int imageWidth = 300;
            int panelHeight = 1080;
            int imageHeight = 60;
            Texture2D texture2D = new Texture2D(imageWidth, panelHeight);
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < panelHeight; j++)
                {
                    texture2D.SetPixel(i, j, Color.clear);
                }
            }
            texture2D.Apply();
            Texture2D texture2D2 = new Texture2D(imageWidth, imageHeight);
            for (int k = 0; k < imageWidth; k++)
            {
                for (int l = 0; l < imageHeight; l++)
                {
                    texture2D2.SetPixel(k, l, Color.clear);
                }
            }
            texture2D2.Apply();
            Panel = new CanvasPanel(
                canvas,
                texture2D,
                new Vector2(0, y), 
                Vector2.zero,
                new Rect(0, 0, imageWidth, 60)
            );

            float textHeight = 90;
            Panel.AddText(
                "Change Skin Text",
                "Change Skin",
                new Vector2(0, y),
                new Vector2(imageWidth, textHeight), 
                GUIController.Instance.trajanNormal,
                24,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            y += textHeight;

            GC.Collect();

            foreach (string path in Directory.GetDirectories(CustomKnight.DATA_DIR))
            {
                string directoryName = new DirectoryInfo(path).Name;
                
                
                Panel.AddButton(
                    directoryName,
                    texture2D2,
                    new Vector2(0, y),
                    Vector2.zero,
                    ChangeSkin,
                    new Rect(0, y, imageWidth, imageHeight),
                    GUIController.Instance.trajanNormal,
                    directoryName,
                    24
                );
                y += imageHeight;
                
                GC.Collect();
            }

            Panel.SetActive(false, true);
            
            Vector2 newPanelSize = new Vector2(imageWidth, y);
            Panel.ResizeBG(newPanelSize);
            
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }

        private static void ChangeSkin(string buttonName)
        {
            CustomKnight.SKIN_FOLDER = buttonName;
            GameManager.instance.StartCoroutine(ChangeSkinRoutine());
        }

        private static IEnumerator ChangeSkinRoutine()
        {
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            Panel.SetActive(false, true);
            CustomKnight.Instance.Initialize(null);

            yield return new WaitUntil(() => SpriteLoader.LoadComplete);
            
            Panel.SetActive(true, false);
        }
        
        private static void OnPause(On.HeroController.orig_Pause orig, HeroController hc)
        {
            Panel.SetActive(true, false);
            
            orig(hc);
        }
        
        private static void OnUnpause(On.HeroController.orig_UnPause orig, HeroController hc)
        {
            Panel.SetActive(false, true);
            
            orig(hc);
        }

        private static void OnSceneChange(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Menu_Title")
            {
                Panel.SetActive(false, true);
            }
        }

        // Taken from https://stackoverflow.com/questions/56949217/how-to-resize-a-texture2d-using-height-and-width
        private static Texture2D Resize(Texture2D texture2D ,int targetX,int targetY)
        {
            RenderTexture rt=new RenderTexture(targetX, targetY,24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D,rt);
            Texture2D result=new Texture2D(targetX,targetY);
            result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
            result.Apply();
            return result;
        }
        
        private static void Log(object message) => Modding.Logger.Log("[Skin Swapper Panel] " + message);
    }
}
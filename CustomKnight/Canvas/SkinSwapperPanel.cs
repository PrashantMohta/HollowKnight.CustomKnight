using System;
using System.Collections;
using System.IO;
using ModCommon;
using ModCommon.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CustomKnight.Canvas
{
    public class SkinSwapperPanel
    {
        private static CanvasPanel _panel;

        public static float Y;

        public static void BuildMenu(GameObject canvas)
        {
            Log("Building Skin Swapper Panel");
            Y = 30.0f;

            _panel = new CanvasPanel(
                canvas,
                GUIController.Instance.images["Panel_BG"],
                new Vector2(0, Y), 
                Vector2.zero,
                new Rect(0, 0, GUIController.Instance.images["Panel_BG"].width, 60)
            );
            
            float textHeight = 90;
            _panel.AddText(
                "Change Skin Text",
                "Change Skin",
                new Vector2(0, Y),
                new Vector2(GUIController.Instance.images["Panel_BG"].width, textHeight), 
                GUIController.Instance.trajanNormal,
                24,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            Y += textHeight;
            
            GC.Collect();

            foreach (string path in Directory.GetDirectories(CustomKnight.DATA_DIR))
            {
                string directoryName = new DirectoryInfo(path).Name;
                WWW knight = new WWW(("file:///" + CustomKnight.DATA_DIR + "/" + directoryName + "/" +  CustomKnight.KNIGHT_PNG).Replace("\\", "/"));
                Texture2D knightTex = knight.texture;
                int imageHeight = 128;
                int imageWidth = 300;
                Color[] colors = knightTex.GetPixels(2890, 2523, imageWidth, imageHeight);
                Texture2D tex = new Texture2D(imageWidth, imageHeight);
                tex.SetPixels(colors);
                tex.Apply();

                _panel.AddButton(
                    directoryName,
                    tex,
                    new Vector2(0, Y),
                    Vector2.zero,
                    ChangeSkin,
                    new Rect(0, Y, imageWidth, imageHeight),
                    GUIController.Instance.trajanNormal,
                    directoryName,
                    24
                );
                Y += imageHeight;
                
                GC.Collect();
            }

            Vector2 newPanelSize = new Vector2(GUIController.Instance.images["Panel_BG"].width, Y);
            _panel.ResizeBG(newPanelSize);
        }

        private static void ChangeSkin(string buttonName)
        {
            Log("Button Name: " + buttonName);
            CustomKnight.SKIN_FOLDER = buttonName;
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            _panel.SetActive(false, true);
            CustomKnight.Instance.Initialize(null);
            _panel.SetActive(true, false);
        }
        
        public static void Update()
        {
            if (_panel == null)
            {
                return;
            }

            if (GameManager.instance.IsGamePaused())
            {
                if (!_panel.active)
                {
                    _panel.SetActive(true, false);    
                }
            }
            else
            {
                if (_panel.active)
                {
                    _panel.SetActive(false, true);   
                }
            }
        }

        private static void Log(object message) => Modding.Logger.Log("[Skin Swapper Panel] " + message);
    }
}
using BepInEx;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RoR2;
using RoR2.UI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System;

namespace RoR2Vietnamese
{
    // This is an example plugin that can be put in
    // BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    // It's a small plugin that adds a relatively simple item to the game,
    // and gives you that item whenever you press F2.

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    // This is the main declaration of our plugin class.
    // BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    // BaseUnityPlugin itself inherits from MonoBehaviour,
    // so you can use this as a reference for what you can declare and use in your plugin class
    // More information in the Unity Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class MainPlugin : BaseUnityPlugin
    {
        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Helscarthe";
        public const string PluginName = "RoR2Vietnamese";
        public const string PluginVersion = "1.0.0";
        
        // Having the Info property setup so it can be referenced in font loading
        public static PluginInfo PInfo { get; private set; }

        // The font to be used
        public static TMP_FontAsset vietFont;
        public static TMP_FontAsset defaultFont;
        public static TMP_FontAsset currentFont;

        // The assetbundle
        public static AssetBundle mainBundle;

        // The assetbundle path
        public static string assetBundlePath;

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            // Init the Info property
            PInfo = Info;

            // Add translation
            Language.collectLanguageRootFolders += LanguageOnCollectLanguageRootFolders;

            assetBundlePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(MainPlugin.PInfo.Location), "AssetBundles", "bombadiervn");

            mainBundle = AssetBundle.LoadFromFile(assetBundlePath);

            if ((bool) mainBundle)
            {
                Log.Info("Font bundle loaded!");
            }

            // Init font
            TMP_FontAsset[] array = mainBundle.LoadAllAssets<TMP_FontAsset>();
            vietFont = array[0]; // insanely jank but this is my first mod forgive me
            if ((bool) vietFont)
            {
                Log.Info("Vietnamese Font loaded!");
            }
            defaultFont = Resources.Load<TMP_FontAsset>("TmpFonts/Bombardier/tmpBombDropShadow");
            if ((bool) defaultFont)
            {
                Log.Info("Default Font loaded!");
            }

            // Hook for changing to Vietnamese
            TMP_Settings.fallbackFontAssets.Add(vietFont);
            //On.RoR2.UI.HGTextMeshProUGUI.OnCurrentLanguageChanged += new On.RoR2.UI.HGTextMeshProUGUI.hook_OnCurrentLanguageChanged(this.HGTextMeshProUGUI_OnCurrentLanguageChanged);
            /*new Hook((MethodBase)typeof(TextMeshProUGUI).GetMethod("LoadFontAsset", (BindingFlags)(-1)), (Delegate)(Action<Action<TextMeshProUGUI>, TextMeshProUGUI>)delegate (Action<TextMeshProUGUI> orig, TextMeshProUGUI self)
            {
                orig(self);
                if ((self.font == vietFont || self.font == defaultFont) && (bool)currentFont)
                {
                    self.font = currentFont;
                }
            });*/

            Log.Info($"Plugin RoR2Vietnamese is loaded!");
        }

        /*public void HGTextMeshProUGUI_OnCurrentLanguageChanged(On.RoR2.UI.HGTextMeshProUGUI.orig_OnCurrentLanguageChanged orig)
        {
            if (Language.currentLanguageName == "vi")
            {
                Language.currentLanguage.SetStringByToken("DEFAULT_FONT", "TmpFonts/Bombardier/tmpBombDropShadow");
                TMP_Settings.fallbackFontAssets.Add(vietFont);
                Log.Info("FallbackFont Added");
            }
            else if (TMP_Settings.fallbackFontAssets.Count == 2)
            {
                TMP_Settings.fallbackFontAssets.RemoveAt(1);
                Log.Info("FallbackFont Removed");
            }
            orig.Invoke();
            if (Language.currentLanguageName == "vi")
            {
                HGTextMeshProUGUI.defaultLanguageFont = vietFont;
                currentFont = vietFont;
                Log.Info("RoR2Vietnamese :: Vietnamese Font Added");
            } else
            {
                HGTextMeshProUGUI.defaultLanguageFont = defaultFont;
                currentFont = defaultFont;
                Log.Info("RoR2Vietnamese :: Vietnamese Font Removed");
            };
        }*/

        private void LanguageOnCollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(PInfo.Location)!, "Languages"));
        }
    }
}

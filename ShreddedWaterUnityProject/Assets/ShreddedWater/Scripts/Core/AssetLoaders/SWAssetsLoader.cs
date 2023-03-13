using Moonstorm.Loaders;
using System;
using System.IO;
using System.Linq;
using Moonstorm.Starstorm2;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace ShreddedWater
{
    public enum SWBundleEnum
    {
        Invalid,
        Main,
        // Base,
        // Artifacts,
        // Executioner,
        // Equipments,
        // Items,
        // Events,
        // Vanilla,
        // InDev,
        // Shared
    }


    public sealed class SWAssetsLoader : CommonAssetsLoader<SWAssetsLoader, SWBundleEnum>
    {
        private const string AssetBundleFolderName = "assetbundles";

        private const string BundleMain = "swmain";
        // private const string BASE = "ss2base";
        // private const string ARTIFACTS = "ss2artifacts";
        // private const string EXECUTIONER = "ss2executioner";
        // private const string EQUIPS = "ss2equipments";
        // private const string ITEMS = "ss2items";
        // private const string EVENTS = "ss2events";
        // private const string VANILLA = "ss2vanilla";
        // private const string DEV = "ss2dev";
        // private const string SHARED = "ss2shared";

        public override AssetBundle MainAssetBundle => GetAssetBundle(SWBundleEnum.Main);

        internal void Init()
        {
            string[] bundlePaths = Directory
                .GetFiles(Path.Combine(SWPlugin.Instance.AssemblyDir, AssetBundleFolderName))
                .Where(filePath => !filePath.EndsWith(".manifest", StringComparison.Ordinal))
                .ToArray();

            foreach (string path in bundlePaths)
            {
                string fileName = Path.GetFileName(path);
                switch (fileName)
                {
                    case BundleMain:
                        LoadAssetBundleFromFile(path, SWBundleEnum.Main);
                        break;

                    // case BASE: LoadBundle(path, SWBundleEnum.Base); break;
                    // case ARTIFACTS: LoadBundle(path, SWBundleEnum.Artifacts); break;
                    // case EXECUTIONER: LoadBundle(path, SWBundleEnum.Executioner); break;
                    // case EQUIPS: LoadBundle(path, SWBundleEnum.Equipments); break;
                    // case ITEMS: LoadBundle(path, SWBundleEnum.Items); break;
                    // case EVENTS: LoadBundle(path, SWBundleEnum.Events); break;
                    // case VANILLA: LoadBundle(path, SWBundleEnum.Vanilla); break;
                    // case DEV: LoadBundle(path, SWBundleEnum.InDev); break;
                    // case SHARED: LoadBundle(path, SWBundleEnum.Shared); break;
                    default:
                        SS2Log.Warning($"Invalid or Unexpected file in the AssetBundles folder (File name: {fileName}, Path: {path})");
                        break;
                }
            }
        }

        //Not the most pleasant workaround but that's what we get
        //private static PostProcessProfile[] ppProfiles;
        private void LoadPostProcessing()
        {
            // PostProcessProfile[] ppProfiles = LoadAllAssetsByTypeFromAnyBundle<PostProcessProfile>();
            // foreach (PostProcessProfile ppProfile in ppProfiles)
            // {
            //     SS2Log.Error(ppProfile);
            //     bool modified = false;
            //
            //     if (ppProfile.TryGetSettings(out SS2RampFog tempFog))
            //     {
            //         var fog = ppProfile.AddSettings<RampFog>();
            //         fog.enabled = tempFog.enabled;
            //         fog.active = tempFog.active;
            //         fog.fogIntensity = tempFog.fogIntensity;
            //         fog.fogPower = tempFog.fogPower;
            //         fog.fogZero = tempFog.fogZero;
            //         fog.fogOne = tempFog.fogOne;
            //         fog.fogHeightStart = tempFog.fogHeightStart;
            //         fog.fogHeightEnd = tempFog.fogHeightEnd;
            //         fog.fogHeightIntensity = tempFog.fogHeightIntensity;
            //         fog.fogColorStart = tempFog.fogColorStart;
            //         fog.fogColorMid = tempFog.fogColorMid;
            //         fog.fogColorEnd = tempFog.fogColorEnd;
            //         fog.skyboxStrength = tempFog.skyboxStrength;
            //         ppProfile.RemoveSettings(typeof(SS2RampFog));
            //         modified = true;
            //     }
            //     if (ppProfile.TryGetSettings(out SS2SobelOutline tempOutline))
            //     {
            //         var outline = ppProfile.AddSettings<SobelOutline>();
            //         outline.enabled = tempOutline.enabled;
            //         outline.active = tempOutline.active;
            //         outline.outlineIntensity = tempOutline.outlineIntensity;
            //         outline.outlineScale = tempOutline.outlineScale;
            //         ppProfile.RemoveSettings(typeof(SS2SobelOutline));
            //         modified = true;
            //     }
            //     if (ppProfile.TryGetSettings(out SS2SobelRain tempRain))
            //     {
            //         var rain = ppProfile.AddSettings<SobelRain>();
            //         rain.enabled = tempRain.enabled;
            //         rain.active = tempRain.active;
            //         rain.rainIntensity = tempRain.rainIntensity;
            //         rain.outlineScale = tempRain.outlineScale;
            //         rain.rainDensity = tempRain.rainDensity;
            //         rain.rainTexture = tempRain.rainTexture;
            //         rain.rainColor = tempRain.rainColor;
            //         ppProfile.RemoveSettings(typeof(SS2SobelRain));
            //         modified = true;
            //     }
            //     ppProfile.isDirty = modified;
            // }
        }
    }
}
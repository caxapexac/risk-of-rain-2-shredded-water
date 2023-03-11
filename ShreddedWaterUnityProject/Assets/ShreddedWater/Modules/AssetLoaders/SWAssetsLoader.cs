using Moonstorm.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Moonstorm;
using Moonstorm.Starstorm2;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Path = System.IO.Path;

namespace ShreddedWater
{
    public enum SWBundleEnum
    {
        Invalid,
        All,
        Main,
        // Base,
        // Artifacts,
        // Executioner,
        // Nemmando,
        // Equipments,
        // Items,
        // Events,
        // Vanilla,
        // Indev,
        // Shared
    }
    
    public class SWAssetsLoader : AssetsLoader<SWAssetsLoader>
    {
        private const string ASSET_BUNDLE_FOLDER_NAME = "assetbundles";
        private const string MAIN = "swmain";
        // private const string BASE = "ss2base";
        // private const string ARTIFACTS = "ss2artifacts";
        // private const string EXECUTIONER = "ss2executioner";
        // private const string NEMMANDO = "ss2nemmando";
        // private const string EQUIPS = "ss2equipments";
        // private const string ITEMS = "ss2items";
        // private const string EVENTS = "ss2events";
        // private const string VANILLA = "ss2vanilla";
        // private const string DEV = "ss2dev";
        // private const string SHARED = "ss2shared";

        private static Dictionary<SWBundleEnum, AssetBundle> assetBundles = new Dictionary<SWBundleEnum, AssetBundle>();
        
        //         [Obsolete("LoadAsset should not be used without specifying the SS2Bundle")]
        //         public new static TAsset LoadAsset<TAsset>(string name) where TAsset : UnityEngine.Object
        //         {
        // #if DEBUG
        //             SS2Log.Warning($"Method {SS2Util.GetCallingMethod<SWAssets>()} is trying to load an asset of name {name} and type {typeof(TAsset).Name} without specifying what bundle to use for loading. This causes large performance loss as SS2Assets has to search thru the entire bundle collection. Avoid calling LoadAsset without specifying the AssetBundle.");
        // #endif
        //             return LoadAsset<TAsset>(name, SWBundleEnum.All);
        //         }
        //         [Obsolete("LoadAllAssetsOfType should not be used without specifying the SS2Bundle")]
        //         public new static TAsset[] LoadAllAssetsOfType<TAsset>() where TAsset : UnityEngine.Object
        //         {
        // #if DEBUG
        //             SS2Log.Warning($"Method {SS2Util.GetCallingMethod<SWAssets>()} is trying to load all assets of type {typeof(TAsset).Name} without specifying what bundle to use for loading. This causes large performance loss as SS2Assets has to search thru the entire bundle collection. Avoid calling LoadAsset without specifying the AssetBundle.");
        // #endif
        //             return LoadAllAssetsOfType<TAsset>(SWBundleEnum.All);
        //         } 

        public static TAsset LoadAsset<TAsset>(string name, SWBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            if(Instance == null)
            {
                SS2Log.Error("Cannot load asset when there's no isntance of SS2Assets!");
                return null;
            }
            return Instance.LoadAssetInternal<TAsset>(name, bundle);
        }
        public static TAsset[] LoadAllAssetsOfType<TAsset>(SWBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            if(Instance == null)
            {
                SS2Log.Error("Cannot load asset when there's no instance of SS2Assets!");
                return null;
            }
            return Instance.LoadAllAssetsOfTypeInternal<TAsset>(bundle);
        }
        
        public override AssetBundle MainAssetBundle => GetAssetBundle(SWBundleEnum.Main);
        public string AssemblyDir => Path.GetDirectoryName(SWMain.PluginInfo.Location);
        public AssetBundle GetAssetBundle(SWBundleEnum bundle)
        {
            return assetBundles[bundle];
        }
        internal void Init()
        {
            var bundlePaths = GetAssetBundlePaths();
            foreach(string path in bundlePaths)
            {
                var fileName = Path.GetFileName(path);
                switch(fileName)
                {
                    case MAIN: LoadBundle(path, SWBundleEnum.Main); break;
                    // case BASE: LoadBundle(path, SWBundleEnum.Base); break;
                    // case ARTIFACTS: LoadBundle(path, SWBundleEnum.Artifacts); break;
                    // case EXECUTIONER: LoadBundle(path, SWBundleEnum.Executioner); break;
                    // case NEMMANDO: LoadBundle(path, SWBundleEnum.Nemmando); break;
                    // case EQUIPS: LoadBundle(path, SWBundleEnum.Equipments); break;
                    // case ITEMS: LoadBundle(path, SWBundleEnum.Items); break;
                    // case EVENTS: LoadBundle(path, SWBundleEnum.Events); break;
                    // case VANILLA: LoadBundle(path, SWBundleEnum.Vanilla); break;
                    // case DEV: LoadBundle(path, SWBundleEnum.Indev); break;
                    // case SHARED: LoadBundle(path, SWBundleEnum.Shared); break;
                    default: SS2Log.Warning($"Invalid or Unexpected file in the AssetBundles folder (File name: {fileName}, Path: {path})"); break;
                }
            }

            void LoadBundle(string path, SWBundleEnum bundleEnum)
            {
                try
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(path);
                    if(!bundle)
                    {
                        throw new FileLoadException("AssetBundle.LoadFromFile did not return an asset bundle");
                    }

                    if(assetBundles.ContainsKey(bundleEnum))
                    {
                        throw new InvalidOperationException($"AssetBundle in path loaded succesfully, but the assetBundles dictionary already contains an entry for {bundleEnum}.");
                    }

                    assetBundles[bundleEnum] = bundle;
                }
                catch(Exception e)
                {
                    SS2Log.Error($"Could not load assetbundle at path {path} and assign to enum {bundleEnum}. {e}");
                }
            }
        }

        private TAsset LoadAssetInternal<TAsset>(string name, SWBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            TAsset asset = null;
            if(bundle == SWBundleEnum.All)
            {
                asset = FindAsset<TAsset>(name, out SWBundleEnum foundInBundle);
#if DEBUG
                if(!asset)
                {
                    SS2Log.Warning($"Could not find asset of type {typeof(TAsset).Name} with name {name} in any of the bundles.");
                }
                else
                {
                    SS2Log.Info($"Asset of type {typeof(TAsset).Name} was found inside bundle {foundInBundle}, it is recommended that you load the asset directly");
                }
#endif
                return asset;
            }

            asset = assetBundles[bundle].LoadAsset<TAsset>(name);
#if DEBUG
            if(!asset)
            {
                SS2Log.Warning($"The  method \"{SS2Util.GetCallingMethod<SWAssetsLoader>()}\" is calling \"LoadAsset<TAsset>(string, SS2Bundle)\" with the arguments \"{typeof(TAsset).Name}\", \"{name}\" and \"{bundle}\", however, the asset could not be found.\n" +
                    $"A complete search of all the bundles will be done and the correct bundle enum will be logged.");
                return LoadAssetInternal<TAsset>(name, SWBundleEnum.All);
            }
#endif
            return asset;

            TAsset FindAsset<TAsset>(string assetName, out SWBundleEnum foundInBundle) where TAsset : UnityEngine.Object
            {
                foreach((var enumVal, var assetBundle) in assetBundles)
                {
                    var loadedAsset = assetBundle.LoadAsset<TAsset>(assetName);
                    if (loadedAsset)
                    {
                        foundInBundle = enumVal;
                        return loadedAsset;
                    }
                }
                foundInBundle = SWBundleEnum.Invalid;
                return null;
            }
        }

        private TAsset[] LoadAllAssetsOfTypeInternal<TAsset>(SWBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            List<TAsset> loadedAssets = new List<TAsset>();
            if(bundle == SWBundleEnum.All)
            {
                FindAssets<TAsset>(loadedAssets);
#if DEBUG
                if(loadedAssets.Count == 0)
                {
                    SS2Log.Warning($"Could not find any asset of type {typeof(TAsset)} inside any of the bundles");
                }
#endif
                return loadedAssets.ToArray();
            }

            loadedAssets = assetBundles[bundle].LoadAllAssets<TAsset>().ToList();
#if DEBUG
            if (loadedAssets.Count == 0)
            {
                SS2Log.Warning($"Could not find any asset of type {typeof(TAsset)} inside the bundle {bundle}");
            }
#endif
            return loadedAssets.ToArray();

            void FindAssets<TAsset>(List<TAsset> output) where TAsset: UnityEngine.Object
            {
                foreach((var _, var bndl) in assetBundles)
                {
                    output.AddRange(bndl.LoadAllAssets<TAsset>());
                }
                return;
            }
        }

        internal void SwapMaterialShaders()
        {
            SwapShadersFromMaterials(LoadAllAssetsOfType<Material>(SWBundleEnum.All).Where(mat => mat.shader.name.StartsWith("Stubbed")));
        }

        internal void FinalizeCopiedMaterials()
        {
            foreach(var (_, bundle) in assetBundles)
            {
                FinalizeMaterialsWithAddressableMaterialShader(bundle);
            }
        }

        private string[] GetAssetBundlePaths()
        {
            return Directory.GetFiles(Path.Combine(AssemblyDir, ASSET_BUNDLE_FOLDER_NAME))
               .Where(filePath => !filePath.EndsWith(".manifest"))
               .ToArray();
        }

        //Not the most pleasant workaround but that's what we get
        //private static PostProcessProfile[] ppProfiles;
        private void LoadPostProcessing()
        {
            var ppProfiles = LoadAllAssetsOfType<PostProcessProfile>(SWBundleEnum.All);
            foreach (var ppProfile in ppProfiles)
            {
                SS2Log.Error(ppProfile);
                bool modified = false;
                // if (ppProfile.TryGetSettings(out SS2RampFog tempFog))
                // {
                //     var fog = ppProfile.AddSettings<RampFog>();
                //     fog.enabled = tempFog.enabled;
                //     fog.active = tempFog.active;
                //     fog.fogIntensity = tempFog.fogIntensity;
                //     fog.fogPower = tempFog.fogPower;
                //     fog.fogZero = tempFog.fogZero;
                //     fog.fogOne = tempFog.fogOne;
                //     fog.fogHeightStart = tempFog.fogHeightStart;
                //     fog.fogHeightEnd = tempFog.fogHeightEnd;
                //     fog.fogHeightIntensity = tempFog.fogHeightIntensity;
                //     fog.fogColorStart = tempFog.fogColorStart;
                //     fog.fogColorMid = tempFog.fogColorMid;
                //     fog.fogColorEnd = tempFog.fogColorEnd;
                //     fog.skyboxStrength = tempFog.skyboxStrength;
                //     ppProfile.RemoveSettings(typeof(SS2RampFog));
                //     modified = true;
                // }
                // if (ppProfile.TryGetSettings(out SS2SobelOutline tempOutline))
                // {
                //     var outline = ppProfile.AddSettings<SobelOutline>();
                //     outline.enabled = tempOutline.enabled;
                //     outline.active = tempOutline.active;
                //     outline.outlineIntensity = tempOutline.outlineIntensity;
                //     outline.outlineScale = tempOutline.outlineScale;
                //     ppProfile.RemoveSettings(typeof(SS2SobelOutline));
                //     modified = true;
                // }
                // if (ppProfile.TryGetSettings(out SS2SobelRain tempRain))
                // {
                //     var rain = ppProfile.AddSettings<SobelRain>();
                //     rain.enabled = tempRain.enabled;
                //     rain.active = tempRain.active;
                //     rain.rainIntensity = tempRain.rainIntensity;
                //     rain.outlineScale = tempRain.outlineScale;
                //     rain.rainDensity = tempRain.rainDensity;
                //     rain.rainTexture = tempRain.rainTexture;
                //     rain.rainColor = tempRain.rainColor;
                //     ppProfile.RemoveSettings(typeof(SS2SobelRain));
                //     modified = true;
                // }
                ppProfile.isDirty = modified;
            }
        }
    }
}
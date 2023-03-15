using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Moonstorm.Starstorm2;
using UnityEngine;


namespace Moonstorm.Loaders
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="TBundleEnum">Value 0 is Invalid</typeparam>
    public abstract class CommonAssetsLoader<TSelf, TBundleEnum> : AssetsLoader<CommonAssetsLoader<TSelf, TBundleEnum>>
        where TSelf : CommonAssetsLoader<TSelf, TBundleEnum>
        where TBundleEnum : Enum
    {
        protected readonly Dictionary<TBundleEnum, AssetBundle> AssetBundles = new Dictionary<TBundleEnum, AssetBundle>();

        #region Bundles

        public AssetBundle GetAssetBundle(TBundleEnum bundle)
        {
            return AssetBundles[bundle];
        }
        
        protected void LoadAssetBundleFromFile(string path, TBundleEnum bundleEnum)
        {
            try
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(path);
                if(!bundle)
                {
                    throw new FileLoadException("AssetBundle.LoadFromFile did not return an asset bundle");
                }

                if(AssetBundles.ContainsKey(bundleEnum))
                {
                    throw new InvalidOperationException($"AssetBundle in path loaded successfully, but the assetBundles dictionary already contains an entry for {bundleEnum}.");
                }

                AssetBundles[bundleEnum] = bundle;
            }
            catch(Exception e)
            {
                SS2Log.Error($"Could not load asset bundle at path {path} and assign to enum {bundleEnum}. {e}");
            }
        }

        #endregion

        
        #region Assets

                [CanBeNull]
        public TAsset LoadAsset<TAsset>(string name, TBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            if (Instance == null)
            {
                SS2Log.Error($"Cannot load asset when there's no instance of {nameof(CommonAssetsLoader<TSelf, TBundleEnum>)}");
                return null;
            }
            TAsset asset = AssetBundles[bundle].LoadAsset<TAsset>(name);
#if DEBUG
            if (!asset)
            {
                SS2Log.Warning(
                    $"The  method \"{SS2Util.GetCallingMethod<CommonAssetsLoader<TSelf, TBundleEnum>>()}\" is calling \"LoadAsset<TAsset>(string, SS2Bundle)\" with the arguments \"{typeof(TAsset).Name}\", \"{name}\" and \"{bundle}\", however, the asset could not be found.\n"
                    + $"A complete search of all the bundles will be done and the correct bundle enum will be logged.");

                // ReSharper disable once TailRecursiveCall
                return LoadAssetFromAnyBundle<TAsset>(name);
            }
#endif
            return asset;
        }

        /// <summary>
        /// Debug only! Use when asset bundle in unknown
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public TAsset LoadAssetFromAnyBundle<TAsset>(string name) where TAsset : UnityEngine.Object
        {
            TAsset asset = FindAssetInAllBundles<TAsset>(name, out TBundleEnum foundInBundle);
#if DEBUG
            if (!asset)
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

        [CanBeNull]
        public TAsset[] LoadAllAssetsByType<TAsset>(TBundleEnum bundle) where TAsset : UnityEngine.Object
        {
            if (Instance == null)
            {
                SS2Log.Error($"Cannot load asset when there's no instance of {nameof(CommonAssetsLoader<TSelf, TBundleEnum>)}");
                return null;
            }

            List<TAsset> loadedAssets = AssetBundles[bundle].LoadAllAssets<TAsset>().ToList(); // TODO optimize
#if DEBUG
            if (loadedAssets.Count == 0)
            {
                SS2Log.Warning($"Could not find any asset of type {typeof(TAsset)} inside the bundle {bundle}");
            }
#endif
            return loadedAssets.ToArray();
        }

        /// <summary>
        /// Debug only! Use when asset bundle in unknown
        /// </summary>
        /// <param name="bundle"></param>
        /// <typeparam name="TAsset"></typeparam>
        /// <returns></returns>
        [NotNull]
        public TAsset[] LoadAllAssetsByTypeFromAnyBundle<TAsset>() where TAsset : UnityEngine.Object
        {
            List<TAsset> loadedAssets = new List<TAsset>();
            foreach ((TBundleEnum _, AssetBundle b) in AssetBundles)
            {
                loadedAssets.AddRange(b.LoadAllAssets<TAsset>());
            }
#if DEBUG
            if (loadedAssets.Count == 0)
            {
                SS2Log.Warning($"Could not find any asset of type {typeof(TAsset)} inside any of the bundles");
            }
#endif
            return loadedAssets.ToArray();
        }

        [CanBeNull]
        private TAsset FindAssetInAllBundles<TAsset>(string assetName, out TBundleEnum foundInBundle) where TAsset : UnityEngine.Object
        {
            foreach ((TBundleEnum enumVal, AssetBundle assetBundle) in AssetBundles)
            {
                TAsset loadedAsset = assetBundle.LoadAsset<TAsset>(assetName);
                if (loadedAsset)
                {
                    foundInBundle = enumVal;
                    return loadedAsset;
                }
            }
            foundInBundle = default;
            return null;
        }

        #endregion

        
        #region Materials

        internal void SwapMaterialShaders()
        {
            SwapShadersFromMaterials(LoadAllAssetsByTypeFromAnyBundle<Material>().Where(mat => mat.shader.name.StartsWith("Stubbed", StringComparison.Ordinal)));
        }

        internal void FinalizeCopiedMaterials()
        {
            foreach (var (_, bundle) in AssetBundles)
            {
                FinalizeMaterialsWithAddressableMaterialShader(bundle);
            }
        }

        #endregion

    }
}
using Moonstorm;
using R2API.ScriptableObjects;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace ShreddedWater.ItemTiers
{
    public class ItemTierCult : ItemTierBase
    {
        public override ItemTierDef ItemTierDef => SWAssetsLoader.Instance.LoadAsset<ItemTierDef>("ItemTierCult", SWBundleEnum.Main);
        
        public override GameObject PickupDisplayVFX => SWAssetsLoader.Instance.LoadAsset<GameObject>("PickupDisplayVFXItemTierCult", SWBundleEnum.Main);

        public override SerializableColorCatalogEntry ColorIndex => SWAssetsLoader.Instance.LoadAsset<SerializableColorCatalogEntry>("ColorCult", SWBundleEnum.Main);
        
        public override SerializableColorCatalogEntry DarkColorIndex => SWAssetsLoader.Instance.LoadAsset<SerializableColorCatalogEntry>("ColorCultDark", SWBundleEnum.Main);

        public override void Initialize()
        {
            base.Initialize();
            // TODO
            ItemTierDef.highlightPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HighlightTier2Item.prefab").WaitForCompletion();
            ItemTierDef.dropletDisplayPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Common/VoidOrb.prefab").WaitForCompletion();
        }
    }
}
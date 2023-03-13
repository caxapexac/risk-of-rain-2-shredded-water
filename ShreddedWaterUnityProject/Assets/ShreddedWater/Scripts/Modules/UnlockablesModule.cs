using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moonstorm;
using Moonstorm.Starstorm2;
using ShreddedWater;
using UObject = UnityEngine.Object;


namespace ShreddedWater
{
    public sealed class UnlockablesModule : UnlockablesModuleBase
    {
        public static UnlockablesModule Instance { get; private set; }

        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Unlockables.");
            GetUnlockableBases();
        }

        protected override IEnumerable<UnlockableBase> GetUnlockableBases()
        {
            IEnumerable<UnlockableBase> allUnlocks = base.GetUnlockableBases();
            
            SS2Log.Info($"Unlock all config is {SWConfigLoader.Instance.EntryUnlockAll.Value}");
            if (SWConfigLoader.Instance.EntryUnlockAll.Value)
            {
                RemoveAllNonSkinUnlocks();
                allUnlocks = allUnlocks.Where(unlock => unlock.UnlockableDef.cachedName.Contains("skin"));
            }
            allUnlocks.ToList().ForEach(unlock => AddUnlockable(unlock));

            return null;
        }

        private void RemoveAllNonSkinUnlocks()
        {
            //This should load all the assets we have that:
            //Are not skin defs
            //Have a field that has an UnlockableDef field.
            IEnumerable<UObject> allAssets = SWAssetsLoader.Instance.MainAssetBundle.LoadAllAssets()
                .Where(asset => !(asset is SkinDef))
                .Where(asset => asset
                        .GetType()
                        .GetFields()
                        .Count(fieldInfo => fieldInfo.FieldType == typeof(UnlockableDef))
                    > 0);

            foreach (UObject asset in allAssets)
            {
                IEnumerable<FieldInfo> fieldsInAsset = asset.GetType()
                    .GetFields()
                    .Where(fieldInfo => fieldInfo.FieldType == typeof(UnlockableDef));

                foreach (FieldInfo field in fieldsInAsset)
                {
                    field.SetValue(asset, null);
                }
            }
        }
    }
}
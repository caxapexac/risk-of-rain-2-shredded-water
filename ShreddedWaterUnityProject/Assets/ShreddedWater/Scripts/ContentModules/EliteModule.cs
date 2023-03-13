using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;
using UnityEngine;


namespace ShreddedWater
{
    public class EliteModule : EliteModuleBase
    {
        public static EliteModule Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;
        public override AssetBundle AssetBundle { get; } = SWAssetsLoader.Instance.MainAssetBundle;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Elites...");
            GetInitializedEliteEquipmentBases();
        }

        protected override IEnumerable<EliteEquipmentBase> GetInitializedEliteEquipmentBases()
        {
            base.GetInitializedEliteEquipmentBases()
                .ToList()
                .ForEach(elite => AddElite(elite));
            return null;
        }
    }
}
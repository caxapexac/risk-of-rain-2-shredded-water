using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class ItemTierModule : ItemTierModuleBase
    {
        public static ItemTierModule Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info("Initializing Item Tiers...");
            GetItemTierBases();
        }

        protected override IEnumerable<ItemTierBase> GetItemTierBases()
        {
            base.GetItemTierBases()
                .ToList()
                .ForEach(itemTier => AddItemTier(itemTier));
            return null;
        }
    }
}
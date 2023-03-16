using BepInEx;
using BepInEx.Configuration;
using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class ItemModule : ItemModuleBase
    {
        public static ItemModule Instance { get; private set; }

        public override R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info("Initializing Items...");
            GetItemBases();
        }

        protected override IEnumerable<ItemBase> GetItemBases()
        {
            base.GetItemBases()
                .ToList()
                .ForEach(item => AddItem(item));

            base.GetItemBases().ToList().ForEach(CheckEnabledStatus);

            return null;
        }

        private void CheckEnabledStatus(ItemBase item)
        {
            if (item.ItemDef.deprecatedTier != RoR2.ItemTier.NoTier)
            {
                string niceName = MSUtil.NicifyString(item.GetType().Name);
                ConfigEntry<bool> enabled = SWConfigLoader.Instance.ConfigItems.Bind(
                    niceName,
                    "Enabled",
                    true,
                    "Should this item be enabled?");

                if (!enabled.Value)
                {
                    item.ItemDef.deprecatedTier = RoR2.ItemTier.NoTier;
                }
            }
        }
    }
}
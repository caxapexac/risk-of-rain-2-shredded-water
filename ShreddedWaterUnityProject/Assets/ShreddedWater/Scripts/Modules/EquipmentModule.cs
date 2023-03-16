using BepInEx.Configuration;
using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class EquipmentModule : EquipmentModuleBase
    {
        public static EquipmentModule Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info("Initializing Equipments...");
            GetEquipmentBases();
            GetEliteEquipmentBases();
        }

        protected override IEnumerable<EquipmentBase> GetEquipmentBases()
        {
            base.GetEquipmentBases()
                .ToList()
                .ForEach(eqp => AddEquipment(eqp));

            base.GetEquipmentBases().ToList().ForEach(CheckEnabledStatus);

            return null;
        }

        protected override IEnumerable<EliteEquipmentBase> GetEliteEquipmentBases()
        {
            base.GetEliteEquipmentBases()
                .ToList()
                .ForEach(equipment => AddEliteEquipment(equipment));
            return null;
        }

        private void CheckEnabledStatus(EquipmentBase eqp)
        {
            string niceName = MSUtil.NicifyString(eqp.GetType().Name);
            if (!(eqp.EquipmentDef.dropOnDeathChance > 0 || eqp.EquipmentDef.passiveBuffDef || niceName.ToLower().Contains("affix")))
            {
                //string niceName = MSUtil.NicifyString(eqp.GetType().Name);
                ConfigEntry<bool> enabled = SWConfigLoader.Instance.ConfigEquips.Bind(
                    niceName,
                    "Enabled",
                    true,
                    "Should this item be enabled?");

                if (!enabled.Value)
                {
                    eqp.EquipmentDef.appearsInSinglePlayer = false;
                    eqp.EquipmentDef.appearsInMultiPlayer = false;
                    eqp.EquipmentDef.canDrop = false;
                }
            }
        }
    }
}
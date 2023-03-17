using Moonstorm;
using Moonstorm.Starstorm2;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShreddedWater.Items
{
    public class ItemInjector : EquipmentBase
    {
        private const string token = "SS2_EQUIP_ITEMINJECTOR_DESC";
        public override EquipmentDef EquipmentDef { get; } = SWAssetsLoader.Instance.LoadAsset<EquipmentDef>("ItemInjector", SWBundleEnum.Main);

        //SS2Log.Warning($"Spawned debug item {name}");


        public override bool FireAction(EquipmentSlot slot)
        {
            SS2Log.Info($"ItemInjector activated");

            return true;
        }
    }
}

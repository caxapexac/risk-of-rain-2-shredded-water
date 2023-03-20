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
        private const string token = "SW_EQUIP_ITEMINJECTOR_DESC";
        public override EquipmentDef EquipmentDef { get; } = SWAssetsLoader.Instance.LoadAsset<EquipmentDef>("ItemInjector", SWBundleEnum.Main);

        private Behavior _behavior;
        
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            _behavior = body.AddItemBehavior<Behavior>(stack);
        }

        public override bool FireAction(EquipmentSlot slot)
        {
            return _behavior.FireAction(slot);
        }

        public sealed class Behavior : CharacterBody.ItemBehavior
        {
            private TargetSelector _targetSelector;
            private InputBankTest _inputBank;

            private void Start()
            {

                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = $"{nameof(ItemInjector)}.{nameof(Start)}" });
                _targetSelector = body.gameObject.AddComponent<TargetSelector>();
                _targetSelector.TeamMask = TeamMask.GetEnemyTeams(body.teamComponent.teamIndex);
                _targetSelector.TeamMask.a ^= TeamMask.all.a;
                _inputBank = body.GetComponent<InputBankTest>();
            }

            private void FixedUpdate()
            {
                if (_inputBank != null)
                {
                    _targetSelector.Ray = new Ray(_inputBank.aimOrigin, _inputBank.aimDirection);
                }
                else
                {
                    _targetSelector.Ray = new Ray(body.corePosition, body.characterDirection.forward); // Not checked
                }
            }

            private void OnDestroy()
            {
                Destroy(_targetSelector);
            }

            public bool FireAction(EquipmentSlot slot)
            {
                CharacterBody target = _targetSelector.Selected;
                if (target == null)
                    return false;



                Inventory inventoryTo = target.inventory;
                Inventory inventoryFrom = body.inventory;

                SS2Log.Warning($"{nameof(ItemInjector)} -> {string.Join(",", inventoryFrom)}");


                List<ItemIndex> list = new List<ItemIndex>();
                
                foreach(ItemIndex itemIndex in inventoryFrom.itemAcquisitionOrder)
                {
                    ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
                    if (!itemDef.canRemove)
                        continue;
                    list.Add(itemIndex);
                }
                //inventory.itemAcquisitionOrder;
                if (list.Count <= 0)
                    return false;

                ItemIndex selectedItemIndex = list[Random.Range(0, list.Count)];

                if (inventoryFrom.GetItemCount(selectedItemIndex) <= 0) // should not be needed
                    return false;

                inventoryFrom.RemoveItem(selectedItemIndex, 1);
                inventoryTo.GiveItem(selectedItemIndex, 1);

                ItemDef selectedItemDef = ItemCatalog.GetItemDef(selectedItemIndex);
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = $"{nameof(ItemInjector)}.{nameof(FireAction)} transfer {selectedItemDef.name}" });
                //SS2Log.Warning($"{nameof(ItemInjector)}.{nameof(FireAction)}");
                return true;
            }
        }
    }
}

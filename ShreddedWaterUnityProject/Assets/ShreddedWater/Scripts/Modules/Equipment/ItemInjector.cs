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
                CharacterBody body = _targetSelector.Selected;
                if(body == null)
                    return false;

                //Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = $"{nameof(ItemInjector)}.{nameof(FireAction)}" });
                //SS2Log.Warning($"{nameof(ItemInjector)}.{nameof(FireAction)}");
                return true;
            }
        }
    }
}

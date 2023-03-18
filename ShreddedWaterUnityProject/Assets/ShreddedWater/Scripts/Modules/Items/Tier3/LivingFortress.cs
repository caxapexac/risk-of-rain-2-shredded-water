using Moonstorm;
using R2API;
using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Networking;


namespace ShreddedWater.Items
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class LivingFortress : ItemBase
    {
        public override ItemDef ItemDef { get; } = SWAssetsLoader.Instance.LoadAsset<ItemDef>("LivingFortress", SWBundleEnum.Main);

        public override void Initialize()
        {
            
        }
        
        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SWContentLoader.Items.LivingFortress;
            
            private static BuffDef GetBuffDef() => SWContentLoader.Buffs.BuffLivingFortress;

            private float _stopWatch = 0;
            private float _stopWatchInterval = 0.15f;
            private float _moveSpeedThreshold = 0.1f;
            
            private void FixedUpdate()
            {
                if (body.moveSpeed > _moveSpeedThreshold)
                {
                    _stopWatch += Time.fixedDeltaTime;
                    float stackedStopWatchInterval = _stopWatchInterval * Mathf.Pow(0.99f, stack);
                    if (_stopWatch > stackedStopWatchInterval)
                    {
                        _stopWatch -= stackedStopWatchInterval;
                        AddBuffStack();
                    }
                }
                else
                {
                    _stopWatch = 0;
                    RemoveBuffStacks();
                }
            }
            
            public void OnDestroy()
            {
                body.RecalculateStats();
            }

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += 1.0f * body.GetBuffCount(GetBuffDef().buffIndex);
            }

            private void AddBuffStack()
            {
                if (NetworkServer.active)
                    return;
                
                body.AddBuff(GetBuffDef().buffIndex);
                body.RecalculateStats();
            }
            
            private void RemoveBuffStacks()
            {
                if (NetworkServer.active)
                    return;

                body.SetBuffCount(GetBuffDef().buffIndex, 0);
                body.RecalculateStats();
            }
        }
    }
}
﻿using RoR2;
using RoR2.Items;

namespace Moonstorm.Starstorm2.Items
{
    
    public sealed class MoltenCoin : ItemBase
    {
        public override ItemDef ItemDef { get; } = SS2Assets.LoadAsset<ItemDef>("MoltenCoin", SS2Bundle.Items);

        [ConfigurableField(ConfigDesc = "Chance for Molten Coin to Proc. (100 = 100%)")]
        [TokenModifier("SS2_ITEM_MOLTENCOIN_DESC", StatTypes.Default, 0)]
        public static float procChance = 6f;

        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SS2Content.Items.MoltenCoin;
            public void OnDamageDealtServer(DamageReport report)
            {
                if (Util.CheckRoll(procChance, body.master))
                {
                    if (report.damageInfo.procCoefficient > 0)
                    {
                        var dotInfo = new InflictDotInfo()
                        {
                            attackerObject = body.gameObject,
                            victimObject = report.victim.gameObject,
                            dotIndex = DotController.DotIndex.Burn,
                            duration = report.damageInfo.procCoefficient * 4f,
                            damageMultiplier = stack
                        };
                        StrengthenBurnUtils.CheckDotForUpgrade(report.attackerBody.inventory, ref dotInfo);
                        DotController.InflictDot(ref dotInfo);

                        body.master.GiveMoney((uint)(stack * (Run.instance.stageClearCount + (1 * 1f))));
                        MSUtil.PlayNetworkedSFX("MoltenCoin", report.victim.gameObject.transform.position);
                        EffectManager.SimpleImpactEffect(HealthComponent.AssetReferences.gainCoinsImpactEffectPrefab, report.victimBody.transform.position, UnityEngine.Vector3.up, true);
                    }
                }
            }
        }
    }
}

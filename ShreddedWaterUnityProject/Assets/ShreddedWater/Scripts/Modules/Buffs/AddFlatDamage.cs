using Moonstorm;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ShreddedWater.Buffs
{
    public class BuffLivingFortress : BuffBase
    {
        public override BuffDef BuffDef { get; } = SWAssetsLoader.Instance.LoadAsset<BuffDef>("BuffLivingFortress", SWBundleEnum.Main);

        public override Material OverlayMaterial { get; } = null;
        
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            index = DotAPI.RegisterDotDef(0.25f, 0.25f, DamageColorIndex.SuperBleed, BuffDef);
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            bool triggerGougeProc = false;
            if (NetworkServer.active)
            {
                if (damageInfo.dotIndex == index && damageInfo.procCoefficient == 0f)
                {
                    if (damageInfo.attacker)
                    {
                        CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                        if (attackerBody)
                        {
                            damageInfo.crit = Util.CheckRoll(attackerBody.crit, attackerBody.master);
                        }
                    }
                    damageInfo.procCoefficient = 0.3f;
                    triggerGougeProc = true;
                }
            }

            orig(self, damageInfo);

            if (NetworkServer.active && !damageInfo.rejected)
            {
                if (triggerGougeProc)
                {
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, self.gameObject);
                }
            }
        }
    }
}

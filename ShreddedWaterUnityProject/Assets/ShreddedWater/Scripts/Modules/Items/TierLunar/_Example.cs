﻿// ReSharper disable All

// using Mono.Cecil.Cil;
// using MonoMod.Cil;
// using R2API;
// using RoR2;
// using RoR2.Items;
// using System;
// using System.Collections;
// using UnityEngine;
//
// namespace Moonstorm.Starstorm2.Items
// {
//     public sealed class RelicOfForce : ItemBase
//     {
//         public override ItemDef ItemDef { get; } = SS2Assets.LoadAsset<ItemDef>("RelicOfForce", SS2Bundle.Items);
//
//         [ConfigurableField(ConfigDesc = "Damage coefficient for subsequent hits. (1 = 100% of total damage)")]
//         [TokenModifier("SS2_ITEM_RELICOFFORCE_DESC", StatTypes.MultiplyByN, 0, "100")]
//         public static float damageMultiplier = 1;
//
//         [ConfigurableField(ConfigDesc = "Attack speed reduction and cooldown increase per stack. (1 = 100% slower attack speed and longer cooldowns)")]
//         [TokenModifier("SS2_ITEM_RELICOFFORCE_DESC", StatTypes.MultiplyByN, 1, "100")]
//         public static float forcePenalty = .4f;
//
//         [ConfigurableField(ConfigDesc = "Delay between additional hits. (1 = 1 second)")]
//         [TokenModifier("SS2_ITEM_RELICOFFORCE_DESC", StatTypes.MultiplyByN, 2, "100")]
//         public static float hitDelay = .2f;
//
//         public static DamageAPI.ModdedDamageType relicForceDamageType;
//
//         public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier, IOnDamageDealtServerReceiver
//         {
//             [ItemDefAssociation]
//             private static ItemDef GetItemDef() => SS2Content.Items.RelicOfForce;
//
//             ForceHitToken EnemyToken;
//
//             public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
//             {
//                 //args.damageMultAdd += damageMultiplier;
//                 float penalty = MSUtil.InverseHyperbolicScaling(forcePenalty, forcePenalty, 0.9f, stack);
//                 args.attackSpeedMultAdd -= penalty;
//
//                 //args.primaryCooldownMultAdd += penalty; //this got removed as it makes some primaries feel like absolute ass
//                 args.secondaryCooldownMultAdd += penalty;
//                 args.utilityCooldownMultAdd += penalty;
//                 args.specialCooldownMultAdd += penalty;
//             }
//
//             private void OnEnable()
//             {
//                 IL.RoR2.GenericSkill.CalculateFinalRechargeInterval += ForceSkillFinalRecharge;
//             }
//
//             private void OnDisable()
//             {
//                 IL.RoR2.GenericSkill.CalculateFinalRechargeInterval -= ForceSkillFinalRecharge;
//             }
//
//             public void OnDamageDealtServer(DamageReport damageReport)
//             {
//                 if (!damageReport.damageInfo.HasModdedDamageType(relicForceDamageType) && !damageReport.damageInfo.HasModdedDamageType(Malice.maliceDamageType) && damageReport.damageInfo.procCoefficient > 0 && damageReport.attacker && damageReport.victimBody)
//                 {
//                     int count = damageReport.attackerBody.inventory.GetItemCount(SS2Content.Items.RelicOfForce); //im pretty sure using stack here made the mod break and im just not having it rn, this works
//                     if (count > 0)
//                     {
//                         var token = damageReport.victim.body.gameObject.GetComponent<ForceHitToken>();
//                         if (token)
//                         {
//                             token.CallMoreHits(damageReport, count);
//                         }
//                         else
//                         {
//                             if (EnemyToken)
//                             {
//                                 Destroy(EnemyToken);
//                             }
//                             token = damageReport.victim.body.gameObject.AddComponent<ForceHitToken>();
//                             EnemyToken = token;
//                             token.CallMoreHits(damageReport, count);
//                             //token = damageReport.victim.body.gameObject.GetComponent<ForceHitToken>();
//                             //token.count = count;
//                             //token.damageReport = damageReport;
//                             //token.enabled = true;
//                         }
//
//                     }
//                 }
//             }
//
//             private void ForceSkillFinalRecharge(ILContext il)
//             {
//                 ILCursor c = new ILCursor(il);
//                 if (c.TryGotoNext(
//                     x => x.MatchLdarg(0),
//                     x => x.MatchCallOrCallvirt<RoR2.GenericSkill>("get_baseRechargeInterval")
//                     ))
//                 {
//                     c.Remove();
//                     c.Remove();
//                 }
//                 else
//                 {
//                     SS2Log.Error("Failed to apply Relic of Force First IL Hook");
//                 }
//
//                 if (c.TryGotoNext(
//                     x => x.MatchCallOrCallvirt<UnityEngine.Mathf>("Min")
//                     ))
//                 {
//                     c.Remove();
//                     //c.EmitDelegate<Func<float, float, float>>((v1, v2) => v2);
//                     //c.Emit(OpCodes.Ret);
//                 }
//                 else
//                 {
//                     SS2Log.Error("Failed to apply Relic of Force IL Second Hook");
//                 }
//             }
//         }
//
//         public class ForceHitToken : MonoBehaviour
//         {
//             //public int count = 0;
//             //public DamageReport damageReport;
//             public int hitCount = 0;
//
//             private void Start()
//             {
//                 //StartCoroutine(RelicForceDelayedHits(damageReport, count));
//             }
//
//             public void CallMoreHits(DamageReport damageReport, int count)
//             {
//                 if(hitCount * .05f < 1)
//                 {
//                     hitCount++;
//                 }
//                 //hitCount++;
//                 StartCoroutine(RelicForceDelayedHits(damageReport, count));
//             }
//
//             IEnumerator RelicForceDelayedHits(DamageReport damageReport, int count)
//             {
//                 var attacker = damageReport.attacker;
//                 var victim = damageReport.victimBody;
//                 var victimHealthComp = damageReport.victimBody.healthComponent;
//                 var initalHit = damageReport.damageInfo;
//
//                 float hitMult = hitCount * .05f;
//                 
//                 for (int i = 0; i < count; i++)
//                 {
//                     DamageInfo damageInfo = new DamageInfo();
//                     damageInfo.damage = damageReport.damageDealt * damageMultiplier * hitMult;
//                     damageInfo.attacker = attacker;
//                     damageInfo.inflictor = initalHit.inflictor;
//                     damageInfo.force = Vector3.zero;
//                     damageInfo.crit = initalHit.crit;
//                     damageInfo.procChainMask = initalHit.procChainMask;
//                     damageInfo.procCoefficient = initalHit.procCoefficient / 2f;
//                     damageInfo.position = victim.transform.position;
//                     damageInfo.damageColorIndex = DamageColorIndex.Item;
//                     damageInfo.damageType = initalHit.damageType;
//                     damageInfo.AddModdedDamageType(RelicOfForce.relicForceDamageType);
//
//                     yield return new WaitForSeconds(hitDelay);
//                     damageInfo.position = victim.transform.position;
//                     if (victim.healthComponent.alive)
//                     {
//                         damageInfo.position = victim.transform.position;
//                         victimHealthComp.TakeDamage(damageInfo);
//                         GlobalEventManager.instance.OnHitEnemy(damageInfo, victimHealthComp.gameObject);
//                         GlobalEventManager.instance.OnHitAll(damageInfo, victimHealthComp.gameObject);
//                         EffectData effectData = new EffectData
//                         {
//                             origin = victim.transform.position
//                         };
//                         effectData.SetNetworkedObjectReference(victim.gameObject);
//                         EffectManager.SpawnEffect(SS2Assets.LoadAsset<GameObject>("RelicOfForceHitEffect", SS2Bundle
//                             .Items), effectData, transmit: true);
//                     }
//                     
//                 }
//             }
//
//         }
//     }
// }
//

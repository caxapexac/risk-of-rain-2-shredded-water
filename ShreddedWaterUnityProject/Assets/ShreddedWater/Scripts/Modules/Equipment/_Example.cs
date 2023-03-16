﻿// ReSharper disable All

// using RoR2;
// using RoR2.Audio;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
//
// namespace Moonstorm.Starstorm2.Equipments
// {
//     //TODO: make warbanner
//     public sealed class GreaterWarbanner : EquipmentBase
//     {
//         private const string token = "SS2_EQUIP_GREATERWARBANNER_DESC";
//         public override EquipmentDef EquipmentDef { get; } = SS2Assets.LoadAsset<EquipmentDef>("GreaterWarbanner", SS2Bundle.Equipments);
//         public GameObject WarbannerObject { get; set; } = SS2Assets.LoadAsset<GameObject>("GreaterWarbannerWard", SS2Bundle.Equipments);
//
//         [ConfigurableField(ConfigDesc = "Amount of Extra Regeneration. (1 = 100%)")]
//         [TokenModifier(token, StatTypes.MultiplyByN, 0, "100")]
//         public static float extraRegeneration = 0.5f;
//
//         [ConfigurableField(ConfigDesc = "Amount of Extra Crit Chance. (100 = 100%)")]
//         [TokenModifier(token, StatTypes.Default, 1)]
//         public static float extraCrit = 20f;
//
//         [ConfigurableField(ConfigDesc = "Amount of Cooldown Reduction. (1 = 100%)")]
//         [TokenModifier(token, StatTypes.MultiplyByN, 2, "100")]
//         public static float cooldownReduction = 0.5f;
//
//         [ConfigurableField(ConfigDesc = "Max active warbanners for each character.")]
//         [TokenModifier(token, StatTypes.Default, 3)]
//         public static int maxGreaterBanners = 5;
//
//         public override void AddBehavior(ref CharacterBody body, int stack)
//         {
//             body.AddItemBehavior<GreaterWarbannerBehavior>(stack);
//         }
//
//         public override bool FireAction(EquipmentSlot slot)
//         {
//             var GBToken = slot.characterBody.gameObject.GetComponent<GreaterBannerToken>();
//             if (!GBToken) {
//                 slot.characterBody.gameObject.AddComponent<GreaterBannerToken>();
//                 GBToken = slot.characterBody.gameObject.GetComponent<GreaterBannerToken>();
//             }
//             //To do: make better placement system
//             Vector3 position = slot.inputBank.aimOrigin + slot.inputBank.aimDirection;
//             GameObject gameObject = UnityEngine.Object.Instantiate(WarbannerObject, position, Quaternion.identity);
//
//             gameObject.GetComponent<TeamFilter>().teamIndex = slot.teamComponent.teamIndex;
//             var behavior = slot.gameObject.GetComponent<GreaterWarbannerBehavior>();
//             //if (behavior.warBannerInstance)
//             //NetworkServer.Destroy(behavior.warBannerInstance);
//             behavior.warBannerInstance = gameObject;
//             NetworkServer.Spawn(behavior.warBannerInstance);
//
//             GBToken.ownedBanners.Add(behavior.warBannerInstance);
//             if(GBToken.ownedBanners.Count > maxGreaterBanners)
//             {
//                 SS2Log.Debug("Removing oldest Warbanner");
//                 var oldBanner = GBToken.ownedBanners[0];
//                 GBToken.ownedBanners.RemoveAt(0);
//                 EffectData effectData = new EffectData
//                 {
//                     origin = oldBanner.transform.position
//                 };
//                 effectData.SetNetworkedObjectReference(oldBanner);
//                 EffectManager.SpawnEffect(HealthComponent.AssetReferences.executeEffectPrefab, effectData, transmit: true);
//
//                 UnityEngine.Object.Destroy(oldBanner);
//             }
//
//             if (behavior.soundCooldown >= 5f)
//             {
//                 var sound = NetworkSoundEventCatalog.FindNetworkSoundEventIndex("GreaterWarbanner");
//                 EffectManager.SimpleSoundEffect(sound, behavior.warBannerInstance.transform.position, true);
//                 behavior.soundCooldown = 0f;
//             }
//             return true;
//         }
//
//         public sealed class GreaterWarbannerBehavior : CharacterBody.ItemBehavior
//         {
//             public GameObject warBannerInstance;
//             public float soundCooldown = 5f;
//
//             private void FixedUpdate()
//             {
//                 soundCooldown += Time.fixedDeltaTime;
//             }
//
//             private void OnDisable()
//             {
//                 //if (warBannerInstance)
//                 //Destroy(warBannerInstance);
//             }
//         }
//
//         public class GreaterBannerToken : MonoBehaviour
//         {
//             //public GameObject[] ownedBanners = new GameObject[0];
//             public List<GameObject> ownedBanners = new List<GameObject>(0);
//             //prevents hilarity from happening
//         }
//     }
//
// }

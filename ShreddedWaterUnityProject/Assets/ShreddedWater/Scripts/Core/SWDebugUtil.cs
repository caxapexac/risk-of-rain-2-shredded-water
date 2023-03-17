using System;
using System.Collections.Generic;
using Moonstorm;
using Moonstorm.Components;
using Moonstorm.Starstorm2;
using RoR2;
using RoR2.UI;
using UnityEngine;


namespace ShreddedWater
{
    public sealed class SWDebugUtil : MonoBehaviour
    {
        private List<ScriptableObject> _toSpawnDefs = new List<ScriptableObject>();

        private void Start()
        {
            SS2Log.Warning("SWDebugUtil Start");
            Run.onRunStartGlobal += OnRunStart;
            RoR2Application.onLoad += OnRoR2Load;
        }

        private void OnRoR2Load()
        {
            #region Item display helper adder

            //Adds the item display helper to all the character bodies.
            foreach (GameObject prefab in BodyCatalog.allBodyPrefabs)
            {
                try
                {
                    ModelLocator modelLocator = prefab.GetComponent<ModelLocator>();
                    if (!modelLocator)
                        continue;

                    GameObject mdlPrefab = modelLocator.modelTransform.gameObject;
                    if (!mdlPrefab)
                        continue;

                    CharacterModel charModel = mdlPrefab.GetComponent<CharacterModel>();
                    if (!charModel)
                        continue;

                    if (charModel.itemDisplayRuleSet == null)
                        continue;

                    mdlPrefab.EnsureComponent<MoonstormIDH>();
                }
                catch (Exception e)
                {
                    SS2Log.Error(e);
                }
            }

            #endregion
        }

        private void OnRunStart(Run obj)
        {
            SS2Log.Warning("SWDebugUtil OnRunStart");

            #region Command Invoking

            if (MSUtil.IsModInstalled("iHarbHD.DebugToolkit"))
            {
                // InvokeCommand("stage1_pod", "0");
                // InvokeCommand("no_enemies");
            }

            #endregion
        }

        // ReSharper disable once UnusedMember.Local
        private void InvokeCommand(string commandName, params string[] arguments)
        {
            DebugToolkit.DebugToolkit.InvokeCMD(NetworkUser.instancesList[0], commandName, arguments);
        }

        private void Update()
        {
            GameObject playerObject = GetPlayerObject();
            if (playerObject == null)
                return;

            Transform playerTransform = playerObject.transform;
            InputBankTest playerInputBank = playerObject.GetComponent<InputBankTest>();

            #region MaterialTester

            if (Input.GetKeyDown(KeyCode.Alpha0) && Run.instance)
            {
                InputBankTest inputBank = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<InputBankTest>();
                Vector3 position = inputBank.aimOrigin + inputBank.aimDirection * 5;
                Quaternion quaternion = Quaternion.LookRotation(inputBank.GetAimRay().direction, Vector3.up);
                GameObject materialTester = MoonstormSharedUtils.MSUAssetBundle.LoadAsset<GameObject>("MaterialTester");
                Instantiate(materialTester, position, quaternion);
            }

            #endregion

            #region EventHelpers

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                EventCard eventCard = MoonstormSharedUtils.MSUAssetBundle.LoadAsset<EventCard>("DummyEventCard");
                EventHelpers.EventAnnounceInfo eventInfo = new EventHelpers.EventAnnounceInfo(eventCard, 15, true) { fadeOnStart = false };
                GameObject go = EventHelpers.AnnounceEvent(eventInfo);
                go.GetComponent<HGTextMeshProUGUI>().alpha = 1f;
            }

            #endregion

            #region Chat

            if (Input.GetKeyDown(KeyCode.Home))
            {
                SS2Log.Warning("Ahoy");
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = "Ahoy" });
            }

            #endregion

            #region Items

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _toSpawnDefs.Add(SWContentLoader.Items.SandCannon);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _toSpawnDefs.Add(SWContentLoader.Equipments.ItemInjector);
            }

            foreach (ScriptableObject def in _toSpawnDefs)
            {
                try
                {
                    string name = "?";
                    PickupIndex pickupIndex = PickupIndex.none;

                    if (def is ItemDef itemDef)
                    {
                        name = itemDef.nameToken;
                        pickupIndex = PickupCatalog.FindPickupIndex(itemDef.itemIndex);
                    }
                    else if (def is EquipmentDef equipmentDef)
                    {
                        name = equipmentDef.nameToken;
                        pickupIndex = PickupCatalog.FindPickupIndex(equipmentDef.equipmentIndex);
                    }

                    if (pickupIndex != PickupIndex.none)
                    {
                        PickupDropletController.CreatePickupDroplet(
                            pickupIndex,
                            playerTransform.position,
                            playerInputBank.aimDirection * 20f);
                        SS2Log.Warning($"Spawned debug item {name}");
                    }
                    else
                    {
                        SS2Log.Warning($"Cent spawn by def {def}");
                    }
                }
                catch (Exception e)
                {
                    SS2Log.Warning(e);
                }
            }

            _toSpawnDefs.Clear();

            #endregion

            #region Prefabs

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                try
                {
                    GameObject prefab = SWAssetsLoader.Instance.LoadAsset<GameObject>("ContentPack/Items/Tier1/SandCannon/DisplaySandCannon", SWBundleEnum.Main);
                    Instantiate(prefab, playerTransform.position + playerInputBank.aimDirection * 5f, Quaternion.identity);
                }
                catch (Exception e)
                {
                    SS2Log.Warning(e);
                }
            }

            #endregion
        }

        private static GameObject GetPlayerObject()
        {
            if (PlayerCharacterMasterController.instances.Count == 0)
                return null;
            return PlayerCharacterMasterController.instances[0].master.GetBodyObject();
        }
    }
}


/*
TODO

BaseItemMasterBehavior
ContentModules
 Buffs
 Characters
 DamageTypes
 EntityStates
 Equip
 Items
 Projectiles
 Unlockables checks

TODO for mobs https://thunderstore.io/package/Nebby/VarianceAPI/
*/
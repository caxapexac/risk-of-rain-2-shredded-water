using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Moonstorm.Starstorm2
{
    public static class SS2Util
    {
        public static void DropShipCall(Transform origin, int tierWeight, uint teamLevel = 1, int amount = 1, ItemTier forcetier = 0, string theWorstCodeOfTheYear = null)
        {
            List<PickupIndex> dropList;
            //float rarityscale = tierWeight * (float)(Math.Sqrt(teamLevel * 12) - 4); //I have absolutely no fucking idea what this is
            //float rarityscale = tierWeight * ((float)MSUtil.InverseHyperbolicScaling(5, .25f, 10, (int)teamLevel) - 5); // this is still gross but i think will be fine
            int templevel = (int)teamLevel;
            float rarityMult = tierWeight * (MSUtil.InverseHyperbolicScaling(1, .1f, 6, templevel) / ((templevel + 14f)/ templevel)); //trust me this almost makes sense - at level 10, non white items are 1.3x more likely, and at 20, they're 2.5x more likely. additionally, reds are impossible at low enough levels
            //SS2Log.Debug(rarityMult);

            if (forcetier == ItemTier.Boss)
            {
                dropList = Run.instance.availableBossDropList;
            }
            else if (forcetier == ItemTier.Lunar)
            {
                dropList = Run.instance.availableLunarCombinedDropList;
            }
            else if (Util.CheckRoll(1 * rarityMult))
            {
                dropList = Run.instance.availableTier3DropList;

                //Util.CheckRoll(.01f * raritymult)
            }
            else if (Util.CheckRoll(20 * rarityMult))
            {
                dropList = Run.instance.availableTier2DropList;
            }
            else
            {
                dropList = Run.instance.availableTier1DropList;
            }
            //else if (Util.CheckRoll(0.5f * rarityscale - 1))
            //    dropList = Run.instance.availableTier3DropList;
            //else if (Util.CheckRoll(4 * rarityscale))
            //    dropList = Run.instance.availableTier2DropList;
            //else
            //    dropList = Run.instance.availableTier1DropList;


            int item = Run.instance.treasureRng.RangeInt(0, dropList.Count);

            if (amount > 1)
            {
                float angle = 360f / (float)amount;
                Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 15f + Vector3.forward * (4.75f + (.25f * amount)));

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                for (int i = 0; i < amount; i++)
                {
                    //if (theWorstCodeOfTheYear != null)
                    //    CreateVFXDroplet(dropList[item], origin.position, vector, theWorstCodeOfTheYear);
                    //    
                    //else
                    //    PickupDropletController.CreatePickupDroplet(dropList[item], origin.position, vector);
                    PickupDropletController.CreatePickupDroplet(dropList[item], origin.position, vector);
                    vector = rotation * vector;
                }
                return;
            }

            // if (dropList[item].itemIndex == SWContentLoader.Items.NkotasHeritage.itemIndex)
            // {
            //     if (item != 0)
            //     {
            //         item--;
            //     }
            //     else if (item != dropList.Count)
            //     {
            //         item++;
            //     }
            // }

            if (theWorstCodeOfTheYear != null)
            {
                PickupDropletController.CreatePickupDroplet(dropList[item], origin.position, new Vector3(0, 15, 0));
                //CreateVFXDroplet(dropList[item], origin.position, new Vector3(0, 15, 0), theWorstCodeOfTheYear);
                return;
            }
            PickupDropletController.CreatePickupDroplet(dropList[item], origin.position, new Vector3(0, 15, 0));
        }

        public static void RemoveDotStacks(CharacterBody victim, DotController.DotIndex TargetedIndex, int NumberOfStacksToRemove)
        {
            DotController VictimController = DotController.FindDotController(victim.gameObject);
            if (!VictimController)
            {
                return;
            }

            for (int i = VictimController.dotStackList.Count - 1; i >= 0; i--)
            {
                DotController.DotStack dotStack = VictimController.dotStackList[i];
                if (dotStack.dotIndex == TargetedIndex)
                {
                    VictimController.RemoveDotStackAtServer(i);
                    NumberOfStacksToRemove--;

                    if (NumberOfStacksToRemove <= 0)
                    {
                        return;
                    }
                }
            }
        }

        public static bool CheckIsValidInteractable(IInteractable interactable, GameObject interactableObject)
        {
            var procFilter = interactableObject.GetComponent<InteractionProcFilter>();
            MonoBehaviour interactableAsMonobehavior = (MonoBehaviour)interactable;

            if ((bool)procFilter)
            {
                return procFilter.shouldAllowOnInteractionBeginProc;
            }
            if ((bool)interactableAsMonobehavior.GetComponent<GenericPickupController>())
            {
                return false;
            }
            if ((bool)interactableAsMonobehavior.GetComponent<VehicleSeat>())
            {
                return false;
            }
            if ((bool)interactableAsMonobehavior.GetComponent<NetworkUIPromptController>())
            {
                return false;
            }
            return true;
        }

        public static ItemDef NkotasRiggedItemDrop(int tierWeight, uint teamLevel = 1, int forcetier = 0)
        {
            List<PickupIndex> dropList;
            float rarityscale = tierWeight * (float)(Math.Sqrt(teamLevel * 13) - 4); //I have absolutely no fucking idea what this is // me neither
            if (Util.CheckRoll(0.5f * rarityscale - 1)  || (forcetier == 3 && forcetier != 0))
                dropList = Run.instance.availableTier3DropList;
            else if (Util.CheckRoll(4 * rarityscale) || (forcetier == 2 && forcetier != 0))
                dropList = Run.instance.availableTier2DropList;
            else
                dropList = Run.instance.availableTier1DropList;
            int item = Run.instance.treasureRng.RangeInt(0, dropList.Count);
            return ItemCatalog.GetItemDef(PickupCatalog.GetPickupDef(dropList[item]).itemIndex);
        }

        public static void CreateVFXDroplet(PickupIndex pickupIndex, Vector3 position, Vector3 velocity, string vfxPrefab)
        {
            //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PickupDropletController.pickupDropletPrefab, position, Quaternion.identity);
            //gameObject.GetComponent<PickupDropletController>().NetworkpickupIndex = pickupIndex;
            //Rigidbody component = gameObject.GetComponent<Rigidbody>();
            //component.velocity = velocity;
            //component.AddTorque(UnityEngine.Random.Range(150f, 120f) * UnityEngine.Random.onUnitSphere);
            //NetworkServer.Spawn(gameObject);

            var pickup = new GenericPickupController.CreatePickupInfo();

            pickup.prefabOverride = PickupCatalog.GetPickupDef(pickupIndex).dropletDisplayPrefab;
            //pickup.prefabOverride.AddComponent<NetworkIdentity>();

            //pickup.prefabOverride.AddComponent<ParticleSystem>();
            //var particleSys = pickup.prefabOverride.GetComponent<ParticleSystem>();
            //particleSys.

            pickup.pickupIndex = pickupIndex;
            PickupDropletController.CreatePickupDroplet(pickup, position, velocity);

            //PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
            
            // TODO uncomment
            // EffectManager.SpawnEffect(SWAssetsLoader.LoadAsset<GameObject>(vfxPrefab, SWBundleEnum.All), new EffectData
            // {
            //     rootObject = pickup.prefabOverride,
            //     origin = position,
            //     scale = 1f,
            // }, true);
        }

        public static IEnumerator BroadcastChat(string token)
        {
            yield return new WaitForSeconds(1);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = token });
            yield break;
        }

        internal static string ScepterDescription(string desc)
        {
            return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
        }
        
#if DEBUG
        internal static string GetCallingMethod<T>()
        {
            var stackTrace = new StackTrace();

            for(int stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
            {
                var frame = stackTrace.GetFrame(stackFrameIndex);
                var method = frame.GetMethod();
                
                if (method == null)
                    continue;

                var declaringType = method.DeclaringType;
                if (declaringType == typeof(T))
                    continue;

                var fileName = frame.GetFileName();
                var fileLineNumber = frame.GetFileLineNumber();
                var fileColumnNumber = frame.GetFileColumnNumber();

                return $"{declaringType.FullName}.{method.Name}({GetMethodParams(method)}) (fileName: {fileName}, Location: L{fileLineNumber} C{fileColumnNumber})";
            }

            return "[COULD NOT GET CALLING METHOD]";
        }

        internal static string GetMethodParams(MethodBase methodBase)
        {
            var parameters = methodBase.GetParameters();
            if (parameters.Length == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach(var parameter in parameters)
            {
                stringBuilder.Append(parameter.ToString() + ", ");
            }
            return stringBuilder.ToString();
        }
#endif
    }
}
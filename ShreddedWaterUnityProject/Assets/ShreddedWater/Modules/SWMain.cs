using BepInEx;
using Moonstorm;
// using Moonstorm.Starstorm2;
using R2API;
using R2API.ScriptableObjects;
using R2API.Utils;
using R2API.ContentManagement;
using R2API.Networking;
using UnityEngine;

namespace ShreddedWater
{
    #region R2API
    [BepInDependency("com.bepis.r2api.artifactcode")]
    [BepInDependency("com.bepis.r2api.colors")]
    [BepInDependency("com.bepis.r2api.commandhelper")]
    [BepInDependency("com.bepis.r2api.content_management")]
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.bepis.r2api.damagetype")]
    [BepInDependency("com.bepis.r2api.deployable")]
    [BepInDependency("com.bepis.r2api.difficulty")]
    [BepInDependency("com.bepis.r2api.director")]
    [BepInDependency("com.bepis.r2api.dot")]
    [BepInDependency("com.bepis.r2api.elites")]
    [BepInDependency("com.bepis.r2api.items")]
    [BepInDependency("com.bepis.r2api.language")]
    [BepInDependency("com.bepis.r2api.loadout")]
    [BepInDependency("com.bepis.r2api.lobbyconfig")]
    [BepInDependency("com.bepis.r2api.networking")]
    [BepInDependency("com.bepis.r2api.orb")]
    [BepInDependency("com.bepis.r2api.prefab")]
    [BepInDependency("com.bepis.r2api.recalculatestats")]
    [BepInDependency("com.bepis.r2api.rules")]
    [BepInDependency("com.bepis.r2api.sceneasset")]
    [BepInDependency("com.bepis.r2api.sound")]
    [BepInDependency("com.bepis.r2api.tempvisualeffect")]
    [BepInDependency("com.bepis.r2api.unlockable")]
    #endregion
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.RiskyLives.RiskyMod", BepInDependency.DependencyFlags.SoftDependency)]
    // [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(Guid, ModName, Version)]
	public class SWMain : BaseUnityPlugin
	{
		public const string Guid = "com.caxapexac.ShreddedWater";
		public const string ModName = "Shredded Water";
		public const string Version = "0.0.1";

		public static SWMain Instance { get; private set; }
		public static PluginInfo PluginInfo;
		public static bool DEBUG = true;

		public static bool ScepterInstalled = false;
		public static bool RiskyModInstalled = false;


		private void Awake()
		{
			Instance = this;
			PluginInfo = Info;
// 			SS2Log.logger = Logger;
// #if DEBUG
// 			gameObject.AddComponent<SWDebugUtil>();
// #endif
// 			new SWAssetsLoader().Init();
// 			new SWConfigLoader().Init();
// 			new SWContentLoader().Init();
// 			new SWLanguage().Init();
			ConfigurableFieldManager.AddMod(this);

			//N: i have no idea if SystemInitializer would be too late for this, so it stays here for now.
			// R2API.Networking.NetworkingAPI.RegisterMessageType<ScriptableObjects.NemesisSpawnCard.SyncBaseStats>();
		}	
		
		private void Start()
		{
			// SoundBankManager.Init();
			SetupModCompat();
			//On.RoR2.EquipmentCatalog.Init += RemoveUnfitEquipmentsFromChaos;
		}
		
		//private void RemoveUnfitEquipmentsFromChaos(EquipmentCatalog.orig_Init orig)
		//{
		//    orig();
		//    SS2Content.Equipments.BackThruster.canBeRandomlyTriggered = false;
		//    SS2Content.Equipments.PressurizedCanister.canBeRandomlyTriggered = false;
		//    SS2Content.Equipments.MIDAS.canBeRandomlyTriggered = false;
		//    //EquipmentCatalog.randomTriggerEquipmentList.Remove(SS2Content.Equipments.BackThruster.equipmentIndex);
		//    //EquipmentCatalog.randomTriggerEquipmentList.Remove(SS2Content.Equipments.PressurizedCanister.equipmentIndex);
		//}

		private void SetupModCompat()
		{
			ScepterInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
			RiskyModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
		}
	}
}

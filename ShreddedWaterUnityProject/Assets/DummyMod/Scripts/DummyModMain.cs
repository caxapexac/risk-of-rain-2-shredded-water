using BepInEx;
using UnityEngine;

namespace DummyMod
{
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

	[BepInPlugin(Guid, ModName, Version)]
	public sealed class DummyModMain : BaseUnityPlugin
	{
		private const string Guid = "com.caxapexac.DummyMod";
		private const string ModName = "DummyMod LongName";
		private const string Version = "0.0.1";

		public static DummyModMain Instance { get; private set; }

		private void Awake()
		{
			Instance = this;

			Vector3 dummyUnityStuff = Vector3.one;
			dummyUnityStuff += dummyUnityStuff;
			Debug.Log(dummyUnityStuff);
		}	
	}
}
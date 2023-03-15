using System.Linq;
using Moonstorm.Starstorm2;
using R2API.ScriptableObjects;
using RoR2;
using UnityEngine;


namespace ShreddedWater
{
    public class EffectModule
    {
        private static R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public static void Init()
        {
            SS2Log.Info($"Populating effect prefabs");
            SerializableContentPack.effectPrefabs = SerializableContentPack.effectPrefabs
                .Concat(SWAssetsLoader.Instance.LoadAllAssetsByTypeFromAnyBundle<GameObject>().Where(go => go.GetComponent<EffectComponent>()))
                .ToArray();
        }
    }
}
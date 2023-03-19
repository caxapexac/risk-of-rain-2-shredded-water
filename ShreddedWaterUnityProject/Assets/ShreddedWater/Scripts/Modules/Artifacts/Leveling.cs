﻿using System.Collections.Generic;
using System.Globalization;
using R2API.ScriptableObjects;
using RoR2;
using Moonstorm;
using Moonstorm.Starstorm2;
using UnityEngine;
using UnityEngine.Networking;


namespace ShreddedWater.Artifacts
{
    // ReSharper disable once UnusedType.Global
    public class Leveling : ArtifactBase
    {
        public override ArtifactDef ArtifactDef { get; } = SWAssetsLoader.Instance.LoadAsset<ArtifactDef>("Leveling", SWBundleEnum.Main);
        public override ArtifactCode ArtifactCode { get; } = SWAssetsLoader.Instance.LoadAsset<ArtifactCode>("ArtifactCodeLeveling", SWBundleEnum.Main);

        private Xoroshiro128Plus _random;

        public override void Initialize()
        {
        }

        public override void OnArtifactEnabled()
        {
            _random = new Xoroshiro128Plus(Run.instance.seed);
            GlobalEventManager.onCharacterLevelUp += OnCharacterLevelUp;
        }

        public override void OnArtifactDisabled()
        {
            GlobalEventManager.onCharacterLevelUp += OnCharacterLevelUp;
        }

        private void OnCharacterLevelUp(CharacterBody characterBody)
        {
            if (!NetworkServer.active)
                return;

            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = $"{characterBody.name} Leveled to {characterBody.level}" });

            List<PickupIndex> pickupIndexList;
            if (characterBody.level % 10 == 0)
            {
                pickupIndexList = new List<PickupIndex>(Run.instance.availableTier3DropList);
            }
            else if (characterBody.level % 3 == 0)
            {
                pickupIndexList = new List<PickupIndex>(Run.instance.availableTier2DropList);
            }
            else
            {
                pickupIndexList = new List<PickupIndex>(Run.instance.availableTier1DropList);
            }

            if (pickupIndexList.Count <= 0)
            {
                SS2Log.Error($"{nameof(pickupIndexList)} is empty");
                return;
            }
            Util.ShuffleList(pickupIndexList, _random);

            if (characterBody.inputBank == null)
            {
                SS2Log.Error($"{nameof(characterBody.inputBank)} is null on {characterBody.name}");
                return;
            }

            PickupIndex pickupIndex = pickupIndexList[0];

            // Chat.SendBroadcastChat(new Chat.NamedObjectChatMessage()
            // {
            //     namedObject = characterBody.gameObject,
            //     baseToken = "Levels up to level {0} and gets {1}", // TODO l10n
            //     paramTokens = new[]
            //     {
            //         characterBody.level.ToString(CultureInfo.InvariantCulture),
            //         ItemCatalog.GetItemDef(pickupIndex.pickupDef.itemIndex).nameToken
            //     }
            // });

            PickupDropletController.CreatePickupDroplet(pickupIndex, characterBody.transform.position, Vector3.up * 15.0f);
        }
    }
}
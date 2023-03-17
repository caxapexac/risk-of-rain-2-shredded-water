using R2API.ScriptableObjects;
using RoR2;
using Moonstorm;
using UnityEngine;
using UnityEngine.Networking;


namespace ShreddedWater.Artifacts
{
    // ReSharper disable once UnusedType.Global
    public class Leveling : ArtifactBase
    {
        public override ArtifactDef ArtifactDef { get; } = SWAssetsLoader.Instance.LoadAsset<ArtifactDef>("Leveling", SWBundleEnum.Main);
        public override ArtifactCode ArtifactCode { get; } = SWAssetsLoader.Instance.LoadAsset<ArtifactCode>("ArtifactCodeLeveling", SWBundleEnum.Main);

        public override void Initialize()
        {
        }

        public override void OnArtifactEnabled()
        {
            GlobalEventManager.onCharacterLevelUp += OnCharacterLevelUp;
        }

        public override void OnArtifactDisabled()
        {
            GlobalEventManager.onCharacterLevelUp += OnCharacterLevelUp;
        }
        
        private void OnCharacterLevelUp(CharacterBody characterBody)
        {
            // if (!NetworkServer.active)
            //     return;
            //
            // Inventory inventory = characterBody.inventory;
            // if (inventory == null)
            //     return;
            //
            // int itemCount = inventory.GetItemCount(RoR2Content.Items.WardOnLevel);
            // if (itemCount <= 0)
            //     return;
            //
            // GameObject gameObject = Object.Instantiate<GameObject>(WardOnLevelManager.wardPrefab, ((Component) characterBody).transform.position, Quaternion.identity);
            // gameObject.GetComponent<TeamFilter>().teamIndex = characterBody.teamComponent.teamIndex;
            // gameObject.GetComponent<BuffWard>().Networkradius = (float) (8.0 + 8.0 * itemCount);
            // NetworkServer.Spawn(gameObject);
            //
            //
            // if (body.teamComponent.teamIndex == TeamIndex.Player)
            // {
            //     // TODO
            //     body.inventory.GiveItem(SWContentLoader.Items.SandCannon, 1);
            // }
        }
    }
}
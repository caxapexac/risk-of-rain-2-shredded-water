using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class InteractableModule : InteractableModuleBase
    {
        public static InteractableModule Instance { get; private set; }

        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Interactables.");
            GetInteractableBases();
        }

        protected override IEnumerable<InteractableBase> GetInteractableBases()
        {
            base.GetInteractableBases()
                .Where(interactable => SWConfigLoader.Instance.ConfigInteractables.Bind(
                    $"{SWPlugin.Instance.Info.Metadata.Name} :: Interactables", 
                    $"{interactable.Interactable}", // TODO check all sections naminig
                    true, 
                    "Enable/Disable this Interactable").Value)
                .ToList()
                .ForEach(interactable => AddInteractable(interactable));
            return null;
        }
    }
}
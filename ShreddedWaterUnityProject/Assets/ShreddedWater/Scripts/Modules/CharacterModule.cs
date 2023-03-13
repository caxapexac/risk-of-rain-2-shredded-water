using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;

namespace ShreddedWater
{
    public sealed class CharacterModule : CharacterModuleBase
    {
        public static CharacterModule Instance { get; set; }

        public override R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Bodies.");
            GetCharacterBases();
        }

        protected override IEnumerable<CharacterBase> GetCharacterBases()
        {
            base.GetCharacterBases()
                .ToList()
                .ForEach(character => AddCharacter(character));
            return null;
        }
    }
}
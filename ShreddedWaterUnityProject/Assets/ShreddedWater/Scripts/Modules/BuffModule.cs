using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class BuffModule : BuffModuleBase
    {
        public static BuffModule Instance { get; set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info("Initializing Buffs.");
            GetBuffBases();
        }

        protected override IEnumerable<BuffBase> GetBuffBases()
        {
            base.GetBuffBases()
                .ToList()
                .ForEach(buff => AddBuff(buff));
            return null;
        }
    }
}
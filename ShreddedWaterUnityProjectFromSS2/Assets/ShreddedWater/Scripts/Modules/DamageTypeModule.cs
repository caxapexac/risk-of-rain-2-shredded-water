using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public class DamageTypeModule : DamageTypeModuleBase
    {
        public static DamageTypeModule Instance { get; private set; }

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing DamageTypes.");
            GetDamageTypeBases();
        }

        protected override IEnumerable<DamageTypeBase> GetDamageTypeBases()
        {
            base.GetDamageTypeBases()
                .ToList()
                .ForEach(dType => AddDamageType(dType));
            return null;
        }
    }
}
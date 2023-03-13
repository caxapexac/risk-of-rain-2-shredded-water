using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class ProjectileModule : ProjectileModuleBase
    {
        public static ProjectileModule Instance { get; set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Projectiles.");
            GetProjectileBases();
        }

        protected override IEnumerable<ProjectileBase> GetProjectileBases()
        {
            base.GetProjectileBases()
                .ToList()
                .ForEach(projectile => AddProjectile(projectile));
            return null;
        }
    }
}
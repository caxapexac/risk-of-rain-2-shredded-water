using System.IO;
using BepInEx;


namespace ShreddedWater
{
    // [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)] // TODO q
    public abstract class CommonUnityPlugin<T> : BaseUnityPlugin where T : CommonUnityPlugin<T>
    {
        public static T Instance { get; private set; }

        public string AssemblyDir => Path.GetDirectoryName(Info.Location) ?? throw new DirectoryNotFoundException();

        private void Awake()
        {
            Instance = this as T;
            OnAwake();
        }

        protected abstract void OnAwake();

        private void Start()
        {
            OnStart();
        }

        protected abstract void OnStart();
    }
}
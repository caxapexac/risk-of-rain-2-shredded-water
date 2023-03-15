using Moonstorm.Loaders;


namespace ShreddedWater
{
    public abstract class CommonContentLoader<T> : ContentLoader<T> where T : CommonContentLoader<T>
    {
    }
}
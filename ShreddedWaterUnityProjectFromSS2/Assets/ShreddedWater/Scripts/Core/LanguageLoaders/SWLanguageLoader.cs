using Moonstorm.Loaders;
using ShreddedWater;


namespace ShreddedWater
{
    public class SWLanguageLoader : LanguageLoader<SWLanguageLoader>
    {
        public override string AssemblyDir => SWPlugin.Instance.AssemblyDir;

        public override string LanguagesFolderName => "languages";

        ///Due to the nature of the language system in ror, we cannot load our language file using system initializer, as its too late.
        internal void Init()
        {
            LoadLanguages();
            // TMProEffects.Init();
        }
    }
}
using Moonstorm.Loaders;
using ShreddedWater;


namespace ShreddedWater
{
    public class SWLanguage : LanguageLoader<SWLanguage>
    {
        public override string AssemblyDir => SWAssetsLoader.Instance.AssemblyDir;

        public override string LanguagesFolderName => "SWLang";

        ///Due to the nature of the language system in ror, we cannot load our language file using system initializer, as its too late.
        internal void Init()
        {
            LoadLanguages();
            // TMProEffects.Init();
        }
    }
}
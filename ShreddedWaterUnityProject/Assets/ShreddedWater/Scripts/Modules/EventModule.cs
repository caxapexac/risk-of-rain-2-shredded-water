using UnityEngine.SceneManagement;

namespace ShreddedWater
{
    public static class EventModule
    {
        public static void Init()
        {
            if (SWConfigLoader.Instance.EntryEnableEvents.Value)
            {
                // EventCatalog.AddCards(SWAssetsLoader.LoadAllAssetsOfType<EventCard>(SWBundleEnum.Events));
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Equals("title"))
            {
                // E.g.:
                // System.DateTime today = System.DateTime.Today;
                // if ((today.Month == 12) && ((today.Day == 25) || (today.Day == 24)))
                // {
                //     Object.Instantiate(SS2Assets.LoadAsset<GameObject>("ChristmasMenuEffect", SS2Bundle.Events), Vector3.zero, Quaternion.identity);
                //     Debug.Log("Merry Christmas from TeamMoonstorm!! :)");
                // }
                // else
                // {
                //     Object.Instantiate(SS2Assets.LoadAsset<GameObject>("StormMainMenuEffect", SS2Bundle.Events), Vector3.zero, Quaternion.identity);
                // }
            }   
        }
    }
}

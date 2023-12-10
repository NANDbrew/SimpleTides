using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using SailwindModdingHelper;
namespace SimpleTides
{
    public class ModSettings : UnityModManager.ModSettings, IDrawable
    {
        // place settings here
        [Draw("Antipodal tides: ")] public bool antipode = true;
        [Draw("Manual settings: ")] public bool manualSet = false;
        [Draw("Magnitude: ")] public float tideMagnitude = 2f;
        [Draw("Offset: ")] public float tideOffset = 0f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange() { }
    }

    internal static class Main
    {
        public static ModSettings settings;
        //public static SailwindModdingHelper.ModLogger logger;
        public static UnityModManager.ModEntry mod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            settings = UnityModManager.ModSettings.Load<ModSettings>(modEntry);

            // uncomment if using settings
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnFixedUpdate = OnFixedUpdate;
            mod = modEntry;
            return true;
        }
        static void OnFixedUpdate(UnityModManager.ModEntry modEntry, float dt)
        {

        }
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
    }
}
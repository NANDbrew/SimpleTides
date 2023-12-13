using Crest;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using SailwindModdingHelper;
using UnityEngine.Playables;
namespace SimpleTides
{
    public class ModSettings : UnityModManager.ModSettings, IDrawable
    {
        // place settings here
        [Draw("Antipodal tides:")] public bool antipode = true;
        [Draw("Solar tides:")] public bool solarTides = true;

/*        [Draw("Al'ankh:")] public float aaMag = 1.8f;
        [Draw("Aestrin:")] public float aeMag = 2.0f;
        [Draw("Emerald:")] public float eaMag = 1.7f;
        [Draw("Firefish:")] public float ffMag = 1.6f;
        [Draw("Happy bay:")] public float hbMag = 1f;
        [Draw("Oasis:")] public float oaMag = 1.4f;
        [Draw("Chronos:")] public float chMag = 2.5f;*/

        [Draw("Manual settings:")] public bool manualSet = false;
        [Draw("Magnitude:")] public float tideMagnitude = 2f;
        [Draw("Offset:")] public float tideOffset = 0f;

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
            Patches.OnFixedUpdate();
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
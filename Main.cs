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
        [Draw("Cheats")] public bool cheats = false;
        [Draw("Cheat speed", VisibleOn = "cheats|true")] public float cheatSpeed = 10;

        [Draw("Antipodal tides")] public bool antipode = true;
        [Draw("Solar tides")] public bool solarTides = true;
        [Draw("Regional tides", Collapsible = true)] public RegionTides regionTides = new RegionTides();
        [Draw("Regional offsets", DrawType.Slider, Min = -1f, Max = 5f, Collapsible = true)] public RegionOffsets regionOffsets = new RegionOffsets();
        public class RegionTides
        {
            [Draw("Al'Ankh", DrawType.Slider, Min = 0f, Max = 5f)] public float alankh = 2.0f;
            [Draw("Aestrin", DrawType.Slider, Min = 0f, Max = 5f)] public float aestrin = 1.8f;
            [Draw("Emerald", DrawType.Slider, Min = 0f, Max = 5f)] public float emerald = 1.4f;
            [Draw("Fire Fish", DrawType.Slider, Min = 0f, Max = 5f)] public float firefish = 1.3f;
            [Draw("Chronos", DrawType.Slider, Min = 0f, Max = 5f)] public float chronos = 4.0f;
        }
        public class RegionOffsets
        {
            [Draw("Al'Ankh", DrawType.Slider, Min = -2f, Max = 2f)] public float alankh = 0.5f;
            [Draw("Aestrin", DrawType.Slider, Min = -2f, Max = 2f)] public float aestrin = 0.35f;
            [Draw("Emerald", DrawType.Slider, Min = -2f, Max = 2f)] public float emerald = 0.45f;
            [Draw("Fire Fish", DrawType.Slider, Min = -2f, Max = 2f)] public float firefish = 0.4f;
            [Draw("Chronos", DrawType.Slider, Min = -2f, Max = 2f)] public float chronos = 0.95f;
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange() 
        {
            Tides.OnChange();
        }
    }

    internal static class Main
    {
        public static ModSettings settings;
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
            Tides.OnFixedUpdate();
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
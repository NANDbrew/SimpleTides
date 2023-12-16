using Crest;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using SailwindModdingHelper;
using UnityEngine.Playables;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;


namespace SimpleTides
{

    internal class RegionList
    {
        internal ConfigEntry<float> alankh;
        internal ConfigEntry<float> aestrin;
        internal ConfigEntry<float> emerald;
        internal ConfigEntry<float> firefish;
        internal ConfigEntry<float> chronos;
    }

    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Main : BaseUnityPlugin
    {
        public const string GUID = "com.nandbrew.simpletides";
        public const string NAME = "Simple Tides";
        public const string VERSION = "1.0.0";

        internal static Main instance;

        internal static ManualLogSource logSource;
        //public event EventHandler SettingChanged;

        // settings
        internal static ConfigEntry<bool> solarTides;
        internal static ConfigEntry<bool> antipode;
        internal static RegionList regionTides = new RegionList();
        internal static RegionList regionOffsets = new RegionList();

        private void Awake()
        {
            instance = this;
            //logSource = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            solarTides = Config.Bind("Options", "Solar tides", false);
            antipode = Config.Bind("Options", "Antipodal tides", true);

            regionTides.alankh = Config.Bind("Regional tides", "Al Ankh", 2.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionTides.aestrin = Config.Bind("Regional tides", "Aestrin", 1.8f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionTides.emerald = Config.Bind("Regional tides", "Emerald", 1.4f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionTides.firefish = Config.Bind("Regional tides", "Fire Fish", 1.3f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionTides.chronos = Config.Bind("Regional tides", "Chronos", 4.0f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            regionOffsets.alankh = Config.Bind("Regional offsets", "Al Ankh", 0.50f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionOffsets.aestrin = Config.Bind("Regional offsets", "Aestrin", 0.35f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionOffsets.emerald = Config.Bind("Regional offsets", "Emerald", 0.45f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionOffsets.firefish = Config.Bind("Regional offsets", "Fire Fish", 0.40f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            regionOffsets.chronos = Config.Bind("Regional offsets", "Chronos", 0.95f, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            Tides.Awake();
    }
        private void FixedUpdate()
        {
            Tides.OnFixedUpdate();
        }
    }
}
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using OVRSimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

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
        public const string VERSION = "1.1.0";

        public static string defPath;

        internal static Main instance;

        internal static ManualLogSource logSource;

        // settings
        internal static ConfigEntry<bool> solarTides;
        internal static ConfigEntry<bool> antipode;
        internal static RegionList regionTides = new RegionList();
        internal static RegionList regionOffsets = new RegionList();
        internal static ConfigEntry<bool> debugRegionals;

        private void Awake()
        {
            instance = this;
            defPath = Path.Combine(Directory.GetParent(Main.instance.Info.Location).FullName, $"definitions.json");
            //logSource = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            solarTides = Config.Bind("Options", "Solar tides", false, new ConfigDescription("Sun affects tides (40% as much as moon)"));
            antipode = Config.Bind("Options", "Antipodal tides", true, new ConfigDescription("Two high tides per day (off: one per day)"));
            debugRegionals = Config.Bind("Options", "Refresh definitions", false, new ConfigDescription("Update magnitudes and offsets from definitions.json", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            debugRegionals.SettingChanged += (sender, args) => instance.StartCoroutine(RefreshConfigs());
            antipode.SettingChanged += (sender, args) => Tides.UpdateMults();
            solarTides.SettingChanged += (sender, args) => Tides.UpdateMults();

            LoadDefs(defPath);
        }

        private void FixedUpdate()
        {
            Tides.OnFixedUpdate();
        }
        private void Update()
        {
            if (GameState.playing && !GameState.wasInSettingsMenu)
            {
                Tides.UpdateBlend();
            }
        }

        private System.Collections.IEnumerator RefreshConfigs()
        {
            if (debugRegionals.Value)
            {
                LoadDefs(defPath);
                yield return new WaitForSeconds(0.5f);
                Debug.Log("SimpleTides: refreshed from file");
                debugRegionals.Value = false;
            }
        }

        public Tuple<Dictionary<string, TideRegion>, Dictionary<int, float>> WriteDefaults()
        {
            Dictionary<string, TideRegion> tideRegions = new Dictionary<string, TideRegion>
            {
                { "Region Al'ankh", new TideRegion{ magnitude = 2f, offset = 0.5f } },
                { "Region Medi",  new TideRegion{ magnitude = 1.8f, offset = 0.35f } },
                { "Region Emerald (new smaller)", new TideRegion{ magnitude = 1.4f, offset = 0.45f } },
                { "Region Emerald Lagoon", new TideRegion{ magnitude = 1.3f, offset = 0.4f } },
                { "Region Medi East", new TideRegion{  magnitude = 4f, offset = 0.95f } }
            };

            JSONNode json = new JSONObject();
            JSONNode arr = new JSONArray();

            Dictionary<int, float> islandOffsets = new Dictionary<int, float>()
            {
                // islandIndex, offset in meters
                {8, -0.50f}, // al'ankh academy
                {15, 0.25f }, // fort aestrin
                {20, -0.50f}, // oasis
                {26, -0.42f}, // temple (fire fish town)
            };

            foreach (var def in tideRegions)
            {
                JSONNode reg = new JSONObject();
                reg.Add("name", def.Key);
                reg.Add("magnitude", def.Value.magnitude);
                reg.Add("offset", def.Value.offset);
                arr.Add(def.Key, reg);
            }
            json.Add("regions", arr);

            JSONNode arr2 = new JSONArray();
            foreach (var isle in islandOffsets)
            {
                JSONNode isl = new JSONObject();
                isl.Add("index", isle.Key);
                isl.Add("offset", isle.Value);
                arr2.Add(isl);
            }
            json.Add("island_offsets", arr2);

            File.WriteAllText(defPath, json.ToString());
            return new Tuple<Dictionary<string, TideRegion>, Dictionary<int, float>>(tideRegions, islandOffsets);
        }

        public void LoadDefs(string path)
        {
            Dictionary<string, TideRegion> output = new Dictionary<string, TideRegion>();
            Dictionary<int, float> map = new Dictionary<int, float>();

            if (!File.Exists(path))
            {
                Tuple<Dictionary<string, TideRegion>, Dictionary<int, float>> tuple = WriteDefaults();
                output = tuple.Item1;
                map = tuple.Item2;
            }
            else
            {
                string json = File.ReadAllText(path);

                foreach (var thing in JSON.Parse(json))
                {
                    if (thing.Key == "regions")
                    {
                        var blah = thing.Value.AsArray;
                        foreach (var b in blah)
                        {
                            var reg = new TideRegion();
                            var f = b.Value.Linq;
                            var name = "";
                            foreach (var f2 in f)
                            {
                                if (f2.Key == "name") name = f2.Value;
                                else if (f2.Key == "magnitude") reg.magnitude = f2.Value.AsFloat;
                                else if (f2.Key == "offset") reg.offset = f2.Value.AsFloat;

                            }
                            output.Add(name, reg);
                        }
                    }
                    else if (thing.Key == "island_offsets")
                    {
                        var bb = thing.Value.AsArray;
                        foreach (var b in bb)
                        {
                            int ind = 0;
                            float off = 0;
                            foreach (var b2 in b.Value)
                            {
                                if (b2.Key == "index") ind = b2.Value.AsInt;
                                else if (b2.Key == "offset") off = b2.Value.AsFloat;
                            }
                            map.Add(ind, off);
                        }
                    }
                }
            }
            if (Dictionaries.regionalDefaults.Count <= output.Count)
            {
                Dictionaries.regionalDefaults = output;
            }
            else
            {
                foreach (var region in output)
                {
                    Dictionaries.regionalDefaults[region.Key] = region.Value;
                }
            }
            if (Dictionaries.islandOffsets.Count <= map.Count)
            {
                Dictionaries.islandOffsets = map;
            }
            else
            {
                foreach (var island in map)
                {
                    Dictionaries.islandOffsets[island.Key] = island.Value;
                }
            }
        }
    }

    public struct TideRegion
    {
        public float magnitude;
        public float offset;

        public static TideRegion zero = new TideRegion { magnitude = 0f, offset = 0f };
    }
}
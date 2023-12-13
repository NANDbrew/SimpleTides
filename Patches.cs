using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using SailwindModdingHelper;
using UnityModManagerNet;
using UnityEngine;
using Crest;
using UnityEngine.Playables;

namespace SimpleTides
{
    internal class Patches
    {
        public static OceanRenderer ocean = RefsDirectory.instance.oceanRenderer;
        public static float defaultSeaLevel = ocean.transform.position.y;
        private static Transform shiftingWorld = GameObject.Find("_shifting world").transform;
        public static float GetTide(int islandIndex)
        {
            int period = 12;
            float phaseMult = 2;
            float offset;
            float magnitude;
            double solarTide = 0;
            double lunarTide;
            Dictionaries.islandRegions.TryGetValue(islandIndex, out string region);
            if (Main.settings.manualSet)
            {
                magnitude = Main.settings.tideMagnitude;
                offset = Main.settings.tideOffset;
            }
            else
            {
                Dictionaries.regionalTides.TryGetValue(region, out magnitude);
                Dictionaries.islandOffsets.TryGetValue(islandIndex, out offset);
            }

            if (Main.settings.antipode)
            {
                period = 6;
                phaseMult = 4;
            }

            if (Main.settings.solarTides)
            {
                solarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) + ((phaseMult / 2) * Math.PI));
                magnitude /= 2.8f;

            }
            lunarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) - Moon.instance.currentPhase * (phaseMult * Math.PI));

            return (float)(solarTide / 2.5 + lunarTide * (magnitude / 2) + offset);
            //return (float)Math.Cos((Sun.sun.localTime / (period / Math.PI)) - Moon.instance.currentPhase * Math.PI * phaseMult) * (magnitude / 2) + offset;
        }

        public static void OnFixedUpdate()
        {
            ocean.transform.position = new Vector3(ocean.transform.position.x, defaultSeaLevel + GetTide(1), ocean.transform.position.z);
            //RegionBlender.instance.GetPrivateField<Region>("currentTargetRegion");
        }

/*        [HarmonyPatch(typeof(RegionBlender))]
        private static class RegionBlenderPatch
        {
            [HarmonyPatch("SwitchRegion")]
        }*/

        /*        [HarmonyPatch(typeof(IslandHorizon))]
                private static class IslandHorizonPatches
                {
                    [HarmonyPrefix]
                    [HarmonyPatch("SetHeight")]
                    public static void IslandSetHeightPatch(int ___islandIndex, ref float y)
                    {
                        y -= GetTide(___islandIndex);
                    }*/
        /*
                    [HarmonyPostfix]
                    [HarmonyPatch("Start")]
                    private static void StartPatch(IslandHorizon __instance)
                    {
                        ModLogger.Log(Main.mod, __instance.islandIndex.ToString() + " " + __instance.name);

                    }

        }*/
    }
}

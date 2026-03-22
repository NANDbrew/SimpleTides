
using HarmonyLib;
using UnityEngine;


namespace SimpleTides
{
    internal class Patches
    {
        [HarmonyPatch(typeof(RegionBlender))]
        private static class RegionBlenderPatch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            public static void StartPatch()
            {
                Tides.Setup();
            }

            [HarmonyPatch("SwitchRegion")]
            [HarmonyPostfix]
            public static void SwitchRegionPatch(Region newRegion)
            {
                Tides.SwitchRegion(newRegion);
            }
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void UpdatePatch(Region ___currentTargetRegion)
            {
                if (!Main.debugRegionals.Value) return;
                Tides.SwitchRegion(___currentTargetRegion);
            }        
        }

        [HarmonyPatch(typeof(IslandHorizon))]
        private static class IslandHorizonPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetHeight")]
            public static void IslandSetHeightPatch(int ___islandIndex, ref float y)
            {
                if (Dictionaries.islandOffsets.TryGetValue(___islandIndex, out float offset)) y -= offset;

            }
        }
    }
}

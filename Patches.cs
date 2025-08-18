
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
            public static void StartPatch(Region ___currentTargetRegion)
            {
                Tides.Setup(RefsDirectory.instance.oceanRenderer, ___currentTargetRegion);
            }

            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void UpdateBlendPatch(Region ___currentTargetRegion, Transform ___player)
            {
                float distance = Vector3.Distance(___player.position, ___currentTargetRegion.transform.position);
                Tides.UpdateBlend(___currentTargetRegion, distance);
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

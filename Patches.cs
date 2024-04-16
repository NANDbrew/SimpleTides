
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
            public static void StartPatch(Region ___currentTargetRegion, Region ___initialRegion)
            {
                Tides.ocean = RefsDirectory.instance.oceanRenderer;
                Tides.defaultSeaLevel = RefsDirectory.instance.oceanRenderer.transform.position.y;

                Tides.currentRegion = ___currentTargetRegion;
                Tides.magnitude = Tides.GetRegionalTide(___currentTargetRegion);

            }

            [HarmonyPatch("UpdateBlend")]
            [HarmonyPostfix]
            public static void UpdateBlendPatch(Region ___currentTargetRegion, Transform ___player)
            {
                float value = Vector3.Distance(___player.position, ___currentTargetRegion.transform.position);
                float num = Mathf.InverseLerp(45000f, 43000f, value);
                Tides.magnitude = Mathf.Lerp(Tides.magnitude, Tides.GetRegionalTide(___currentTargetRegion), num);
                Tides.offset = Mathf.Lerp(Tides.offset, Tides.GetRegionalOffset(___currentTargetRegion), num);
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

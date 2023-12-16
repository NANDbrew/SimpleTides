
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
        private static class IslandHorizonPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetHeight")]
            public static void IslandSetHeightPatch(int ___islandIndex, ref float y)
            {
                float offset = 0;
                Dictionaries.islandOffsets.TryGetValue(___islandIndex, out offset);

                y -= offset;
            }
        }
        //test patches
 /*       [HarmonyPatch(typeof(GPButtonSteeringWheel), "Update")]
        private static class CheatySpeed
        {
            private static void Postfix(GPButtonSteeringWheel __instance, GoPointer ___stickyClickedBy) 
            {
                if (!Main.cheats.Value) return;
                if(___stickyClickedBy)
                {
                    if (GameInput.GetKey(InputName.MoveUp))
                    {
                        if (GameInput.GetKey(InputName.Run))
                        {
                            GameState.currentBoat.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.forward * Main.settings.cheatSpeed * 3, ForceMode.Acceleration);
                        }
                        else
                        {
                            GameState.currentBoat.GetComponentInParent<Rigidbody>().AddRelativeForce(Vector3.forward * Main.settings.cheatSpeed, ForceMode.Acceleration);
                        }

                        //ModLogger.Log(Main.mod, "recieved input");
                    }
                    else if (GameInput.GetKey(InputName.MoveDown))
                    {
                        if (GameInput.GetKey(InputName.Run))
                        {
                            GameState.currentBoat.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * Main.settings.cheatSpeed * 1.5f, ForceMode.Acceleration);
                        }
                        else
                        {
                            GameState.currentBoat.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * Main.settings.cheatSpeed * 0.5f, ForceMode.Acceleration);
                        }
                    }
                }
            }
        }*/
    }
}

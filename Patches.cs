﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using SailwindModdingHelper;
using UnityModManagerNet;
namespace SimpleTides
{
    internal class Patches
    {
        private static readonly Dictionary<int, string> islandRegions = new Dictionary<int, string>()
        {
            // ---al'ankh---
            {1, "alankh"}, // gold rock city
            {2, "alankh"}, // al'nilem
            {3, "alankh"}, // neverdin
            {4, "alankh"}, // albacore town
            {5, "alankh"}, // isle of clear mind
            {6, "alankh"}, // lion's fang
            {7, "alankh"}, // alchemist's island
            {8, "alankh"}, // al'ankh academy

            {20, "oasis"}, // oasis, might want seperate

            // ---emerald archipelago---
            {9, "emerald"},  // dragon cliffs
            {10, "emerald"}, // sanctuary
            {11, "emerald"}, // crab beach
            {12, "emerald"}, // new port
            {13, "emerald"}, // sage hills
            {22, "emerald"}, // serpent isle

            // ---aestrin---
            {15, "aestrin"}, // fort aestrin
            {16, "aestrin"}, // sunspire
            {17, "aestrin"}, // mount malefic
            {19, "aestrin"}, // eastwind
            {21, "aestrin"}, // siren song
            {23, "aestrin"}, // oracle
            {14, "aestrin"}, // nightcall, don't know where this is, it has m in the name so it must be in aestrin

            // ---happy bay---
            {18, "happy"}, // happy bay
            {30, "happy"}, // rock of despair, assumed to be new thing near happy bay

            //---chronos---
            {25, "chronos"},

            //--- fire fish lagoon---
            {26, "firefish"}, // temple, (fire fish town)
            {27, "firefish"}, // shipyard (kicia bay)
            {28, "firefish"}, // sen'na
            {29, "firefish"}, // on'na
            {31, "firefish"}  // fisherman (uninhabited north isle)
        };
        private static readonly Dictionary<string, float> regionalTides = new Dictionary<string, float>()
        {
            // meters total between high and low tides
            {"alankh", 1.8f},
            {"emerald", 1.70f},
            {"aestrin", 2.0f},
            {"happy", 1.0f},
            {"chronos", 2.5f},
            {"firefish", 1.6f},
            {"oasis", 1.4f}
        };
        private static readonly Dictionary<int, float> islandOffsets = new Dictionary<int, float>()
        {
            // islandIndex, offset in meters
            {8, -0.70f}, // al'ankh academy
            {20, -0.50f}, // oasis
            {26, -0.60f}, // temple, (fire fish town)
        };
        public static float GetTide(int islandIndex)
        {
            int period;
            float phaseMult;
            float offset;
            float magnitude;
            if (Main.settings.antipode)
            {
                period = 6;
                phaseMult = 4f;
            }
            else
            {
                period = 12;
                phaseMult = 2f;
            }
            islandRegions.TryGetValue(islandIndex, out string region);
            if (Main.settings.manualSet)
            {
                magnitude = Main.settings.tideMagnitude;
                offset = Main.settings.tideOffset;
            }
            else
            {
                regionalTides.TryGetValue(region, out magnitude);
                if(!islandOffsets.TryGetValue(islandIndex, out offset)) offset = 0;
            }
            return (float)Math.Cos((double)(Sun.sun.localTime / (period / Math.PI)) - (double)Moon.instance.currentPhase * Math.PI * phaseMult) * (magnitude / 2) + offset;
        }

        [HarmonyPatch(typeof(IslandHorizon))]
        private static class IslandHorizonPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetHeight")]
            public static void IslandSetHeightPatch(int ___islandIndex, ref float y)
            {
                y -= GetTide(___islandIndex);
            }

            [HarmonyPostfix]
            [HarmonyPatch("Start")]
            private static void StartPatch(IslandHorizon __instance)
            {
                ModLogger.Log(Main.mod, __instance.islandIndex.ToString() + " " + __instance.name);

            }
        }
    }
}

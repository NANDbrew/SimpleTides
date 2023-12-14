using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTides
{
    internal class Dictionaries
    {
        public static readonly Dictionary<int, string> islandRegions = new Dictionary<int, string>()
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
        public static Dictionary<string, float> regionalTides = new Dictionary<string, float>()
        {
            // meters total between high and low tides
            {"alankh", 1.8f},
            {"emerald", 1.7f},
            {"aestrin", 2.0f},
            {"happy", 1f},
            {"chronos", 2.5f},
            {"firefish", 1.6f},
            {"oasis", 1.4f}
        };
        public static Dictionary<int, float> islandOffsets = new Dictionary<int, float>()
        {
            // islandIndex, offset in meters
            {8, -0.50f}, // al'ankh academy
            {20, -0.50f}, // oasis
            {26, -0.42f}, // temple, (fire fish town)
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTides
{
    internal class Dictionaries
    {
        public static Dictionary<int, float> islandOffsets = new Dictionary<int, float>()
        {
            // islandIndex, offset in meters
            {8, -0.50f}, // al'ankh academy
            {20, -0.50f}, // oasis
            {26, -0.42f}, // temple (fire fish town)
        };
        /*
                    // ---al'ankh---
                    {1, 0.0f}, // gold rock city
                    {2, 0.0f}, // al'nilem
                    {3, 0.0f}, // neverdin
                    {4, 0.0f}, // albacore town
                    {5, 0.0f}, // isle of clear mind
                    {6, 0.0f}, // lion's fang
                    {7, 0.0f}, // alchemist's island
                    {8, -0.5f}, // al'ankh academy
                    {20, -0.5f}, // oasis

                    // ---emerald archipelago---
                    {9, 0.0f},  // dragon cliffs
                    {10, 0.0f}, // sanctuary
                    {11, 0.0f}, // crab beach
                    {12, 0.0f}, // new port
                    {13, 0.0f}, // sage hills
                    {22, 0.0f}, // serpent isle

                    // ---aestrin---
                    {15, 0.0f}, // fort aestrin
                    {16, 0.0f}, // sunspire
                    {17, 0.0f}, // mount malefic
                    {19, 0.0f}, // eastwind
                    {21, 0.0f}, // siren song
                    {23, 0.0f}, // oracle
                    {14, 0.0f}, // nightcall, don't know where this is, it has m in the name so it must be in aestrin
                    {18, 0.0f}, // happy bay
                    {30, 0.0f}, // rock of despair, assumed to be new thing near happy bay

                    //---chronos---
                    {25, 0.0f},

                    //--- fire fish lagoon---
                    {26, -0.42f}, // temple (fire fish town)
                    {27, 0.0f}, // shipyard (kicia bay)
                    {28, 0.0f}, // sen'na
                    {29, 0.0f}, // on'na
                    {31, 0.0f}  // fisherman (uninhabited north isle)
        */
    }
}
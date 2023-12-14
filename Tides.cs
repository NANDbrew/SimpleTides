using Crest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleTides
{
    internal class Tides
    {
        private static OceanRenderer ocean = RefsDirectory.instance.oceanRenderer;
        private static float defaultSeaLevel = ocean.transform.position.y;
        public static Region currentRegion;
        public static float magnitude;
        public static float offset;

        //private static Transform shiftingWorld = GameObject.Find("_shifting world").transform;
        public static float SetTide()
        {
            int period = 12;
            float phaseMult = 2;
            double solarTide = 0;
            double lunarTide;
            float solarMult = 1;

            if (Main.settings.antipode)
            {
                period = 6;
                phaseMult = 4;
            }

            if (Main.settings.solarTides)
            {
                solarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) + ((phaseMult / 2) * Math.PI)) * 0.4f;
                solarMult = 0.71428f;
            }

            lunarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) - Moon.instance.currentPhase * (phaseMult * Math.PI));

            return (float)((solarTide + lunarTide) * (solarMult * magnitude / 2) - (magnitude / 2 - offset));
        }

        public static void OnFixedUpdate()
        {
            ocean.transform.position = new Vector3(ocean.transform.position.x, defaultSeaLevel + SetTide(), ocean.transform.position.z);
        }
        public static void OnChange()
        {
            magnitude = GetRegionalTide(currentRegion);
            offset = GetRegionalOffset(currentRegion);
            //RegionBlender.instance.InvokePrivateMethod("UpdateBlend");
        }

        public static float GetRegionalTide(Region region)
        {
            if (region.name.Contains("ankh")) return Main.settings.regionTides.alankh;
            if (region.name.Contains("Medi"))
            {
                if (region.name.Contains("East")) return Main.settings.regionTides.chronos;
                return Main.settings.regionTides.aestrin;
            }
            if (region.name.Contains("Emerald"))
            {
                if (region.name.Contains("Lagoon")) return Main.settings.regionTides.firefish;
                return Main.settings.regionTides.emerald;
            }
            return 1f;
        }
        public static float GetRegionalOffset(Region region)
        {
            if (region.name.Contains("ankh")) return Main.settings.regionOffsets.alankh;
            if (region.name.Contains("Medi"))
            {
                if (region.name.Contains("East")) return Main.settings.regionOffsets.chronos;
                return Main.settings.regionOffsets.aestrin;
            }
            if (region.name.Contains("Emerald"))
            {
                if (region.name.Contains("Lagoon")) return Main.settings.regionOffsets.firefish;
                return Main.settings.regionOffsets.emerald;
            }
            return 1f;
        }
    }
}

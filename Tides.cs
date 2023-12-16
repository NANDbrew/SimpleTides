using Crest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleTides
{
    public static class Tides
    {
        public static OceanRenderer ocean;
        public static float defaultSeaLevel;
        public static Region currentRegion;
        public static float magnitude;
        public static float offset;

        public static void Awake()
        {
            //ocean = RefsDirectory.instance.oceanRenderer;
            //defaultSeaLevel = ocean.transform.position.y;
        }
        //private static Transform shiftingWorld = GameObject.Find("_shifting world").transform;
        public static float SetTide()
        {
            int period = 12;
            float phaseMult = 2;
            double solarTide = 0;
            double lunarTide;
            float solarMult = 1;

            if (Main.antipode.Value)
            {
                period = 6;
                phaseMult = 4;
            }

            if (Main.solarTides.Value)
            {
                solarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) + ((phaseMult / 2) * Math.PI)) * 0.4f;
                solarMult = 0.71428f;
            }

            lunarTide = Math.Cos(Sun.sun.localTime / (period / Math.PI) - Moon.instance.currentPhase * (phaseMult * Math.PI));

            return (float)((solarTide + lunarTide) * (solarMult * magnitude / 2) - (magnitude / 2 - offset));
        }

        public static void OnFixedUpdate()
        {
            if (ocean != null) ocean.transform.position = new Vector3(ocean.transform.position.x, defaultSeaLevel + SetTide(), ocean.transform.position.z);
        }
        public static void OnChange()
        {
            magnitude = GetRegionalTide(currentRegion);
            offset = GetRegionalOffset(currentRegion);
            //RegionBlender.instance.InvokePrivateMethod("UpdateBlend");
        }

        public static float GetRegionalTide(Region region)
        {
            if (region.name.Contains("ankh")) return Main.regionTides.alankh.Value;
            if (region.name.Contains("Medi"))
            {
                if (region.name.Contains("East")) return Main.regionTides.chronos.Value;
                return Main.regionTides.aestrin.Value;
            }
            if (region.name.Contains("Emerald"))
            {
                if (region.name.Contains("Lagoon")) return Main.regionTides.firefish.Value;
                return Main.regionTides.emerald.Value;
            }
            return 1f;
        }
        public static float GetRegionalOffset(Region region)
        {
            if (region.name.Contains("ankh")) return Main.regionOffsets.alankh.Value;
            if (region.name.Contains("Medi"))
            {
                if (region.name.Contains("East")) return Main.regionOffsets.chronos.Value;
                return Main.regionOffsets.aestrin.Value;
            }
            if (region.name.Contains("Emerald"))
            {
                if (region.name.Contains("Lagoon")) return Main.regionOffsets.firefish.Value;
                return Main.regionOffsets.emerald.Value;
            }
            return 1f;
        }
    }
}

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
        public static float magnitude;
        public static float offset;
        private const float solarInfluence = 0.4f;
        private const float solarComp = 1 / (1 + solarInfluence);// number by which to multiply output magnitude to normalize totals

        public static void Setup(OceanRenderer newOcean, Region initialRegion)
        {
            ocean = newOcean;
            defaultSeaLevel = ocean.transform.position.y;
            magnitude = GetRegionalTide(initialRegion);
        }

        public static float GetTide()
        {
            int period = 12;
            float phaseMult = 2;
            float solarTide = 0;
            float lunarTide;
            float solarMult = 1;

            if (Main.antipode.Value)
            {
                period = 6;
                phaseMult = 4;
            }

            if (Main.solarTides.Value)
            {
                solarTide = Mathf.Cos(Sun.sun.localTime / (period / Mathf.PI) + (phaseMult / 2) * Mathf.PI) * solarInfluence;
                solarMult = solarComp; 
                //0.71428f; // if solar influence is 40%, multiply by (1 / 1.4) to normalize (sun + moon) to 1
            }

            lunarTide = Mathf.Cos(Sun.sun.localTime / (period / Mathf.PI) - Moon.instance.currentPhase * (phaseMult * Mathf.PI));

            return (solarTide + lunarTide) * (solarMult * magnitude / 2) - (magnitude / 2 - offset);
        }

        public static void OnFixedUpdate()
        {
            if (ocean != null) ocean.transform.position = new Vector3(ocean.transform.position.x, defaultSeaLevel + GetTide(), ocean.transform.position.z);
        }

        public static void UpdateBlend(Region region, float distance)
        {
            float lerpValue = Mathf.InverseLerp(45000f, 43000f, distance);
            magnitude = Mathf.Lerp(magnitude, GetRegionalTide(region), lerpValue);
            offset = Mathf.Lerp(offset, GetRegionalOffset(region), lerpValue);

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

using Crest;
using UnityEngine;

namespace SimpleTides
{
    public static class Tides
    {
        public static OceanRenderer ocean;
        public static float defaultSeaLevel;
        public static float solarInfluence = 0.4f;

        private static float magnitude = 0f;
        private static float offset = 0f;
        private static float solarComp = 1 / (1 + solarInfluence);// number by which to multiply output magnitude to normalize totals
        private static TideRegion currentRegionals = TideRegion.zero;
        private static float periodPI = 12 / Mathf.PI;
        private static float phaseMult = 2;
        
        internal static void Setup()
        {
            ocean = RefsDirectory.instance.oceanRenderer;
            defaultSeaLevel = ocean.transform.position.y;
            UpdateMults();
        }

        internal static void UpdateMults()
        {
            var period = Main.antipode.Value ? 6 : 12;
            phaseMult = 24 / period;
            periodPI = period / Mathf.PI;
            solarComp = Main.solarTides.Value ? 1 / (1 + solarInfluence) : 1;
        }

        public static float GetTide()
        {
            float lunarTide = Mathf.Cos((Sun.sun.localTime / periodPI) - Moon.instance.currentPhase * (phaseMult * Mathf.PI));

            float solarTide = 0;
            if (Main.solarTides.Value)
            {
                solarTide = Mathf.Cos(Sun.sun.localTime / periodPI + (phaseMult / 2) * Mathf.PI) * solarInfluence;
            }

            return ((solarTide + lunarTide) * (solarComp * magnitude / 2)) - ((magnitude / 2) - offset);
        }

        internal static void OnFixedUpdate()
        {
            if (ocean != null) ocean.transform.position = new Vector3(ocean.transform.position.x, defaultSeaLevel + GetTide(), ocean.transform.position.z);
            //ocean.transform.Translate(0f, (GetTide() - ocean.transform.position.y), 0f);
        }

        internal static void UpdateBlend()
        {
            //float blendRate = 0.001f;//(GameState.recovering || GameState.currentlyLoading) ? 0.1f : 0.0001f;
            if (GameState.distanceToLand < 10000)
            {
                var blendRate = 1 / GameState.distanceToLand;
                magnitude = Mathf.MoveTowards(magnitude, currentRegionals.magnitude, blendRate * 3);
                offset = Mathf.MoveTowards(offset, currentRegionals.offset, blendRate);
            }
            else
            {
                var blendRate = 0.0002f;
                magnitude = Mathf.MoveTowards(magnitude, 0f, blendRate * 3);
                offset = Mathf.MoveTowards(offset, 0f, blendRate);
            }
        }
        public static void SwitchRegion(Region newRegion)
        {
            currentRegionals = GetRegionals(newRegion);
        }

        public static TideRegion GetRegionals(Region region)
        {
            // hopefully other mods can add their regions to this
            if (Dictionaries.regionalDefaults.TryGetValue(region.name, out TideRegion regionValues))
            {
                return regionValues;
            }
            return TideRegion.zero;
        }
        public static void AddRegion(Region region, float magnitude, float offset)
        {
            Dictionaries.regionalDefaults.Add(region.name, new TideRegion { magnitude = magnitude, offset = offset });
        }
    }
}

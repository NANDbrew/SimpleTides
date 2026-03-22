using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace SimpleTides
{
    [HarmonyPatch(typeof(Sail), "GetWindHeightMult")]
    internal class SailHeightPatch
    {
        public static void Prefix(ref float worldY)
        {
            worldY -= RefsDirectory.instance.oceanRenderer.transform.position.y;
        }
    }
}

using HarmonyLib;
using Kitchen;
using KitchenGoldfishMemory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenGoldfishMemory.Patches
{
    [HarmonyPatch]
    static class LocalViewRouter_Patch
    {
        [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
        [HarmonyPostfix]
        static void GetPrefab_Postfix(ViewType view_type, ref GameObject __result)
        {
            if (view_type == ViewType.CustomerIndicator && __result.GetComponent<GroupMenuPhaseView>() == null)
            {
                __result.AddComponent<GroupMenuPhaseView>();
            }
        }
    }
}

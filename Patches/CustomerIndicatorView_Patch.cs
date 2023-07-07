using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenGoldfishMemory.Views;
using System.Collections.Generic;
using TMPro;

namespace KitchenGoldfishMemory.Patches
{
    [HarmonyPatch]
    static class CustomerIndicatorView_Patch
    {
        static Dictionary<MenuPhase, string> _menuPhaseSprites = new Dictionary<MenuPhase, string>()
        {
            { MenuPhase.Starter, "food_card" },
            { MenuPhase.Main, "service" },
            { MenuPhase.Dessert, "fill_coffee" }
        };
        static Dictionary<MenuPhase, string> MenuPhaseSprites => _menuPhaseSprites;

        [HarmonyPatch(typeof(CustomerIndicatorView), "UpdateData")]
        [HarmonyPostfix]
        static void UpdateData_Postfix(CustomerIndicatorView.ViewData view_data, ref TextMeshPro ___Icon, ref CustomerIndicatorView __instance)
        {

            if (view_data.IsHidden ||
                view_data.PatienceReason != PatienceReason.Service ||
                ___Icon == null ||
                __instance.GetComponent<GroupMenuPhaseView>() == null)
                return;

            GroupMenuPhaseView groupMealPhaseView = __instance.GetComponent<GroupMenuPhaseView>();
            if (groupMealPhaseView != null && TryGetIcon(groupMealPhaseView.MenuPhase, out string icon))
            {
                ___Icon.text = icon;
            }
        }

        static bool TryGetIcon(MenuPhase menuPhase, out string icon)
        {
            if (MenuPhaseSprites.TryGetValue(menuPhase, out string iconName))
            {
                icon = $"<sprite name=\"{iconName}\">";
                return true;
            }
            icon = default;
            return false;
        }
    }
}

using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Oc;
using Oc.Item.UI;

namespace BigInventory
{
    [BepInPlugin("com.github.xiaoye97.plugin.Craftopia.BigInventory", "BigInventory", "1.4")]
    public class BigInventory : BaseUnityPlugin
    {
        public static int InventorySize = 100;
        public static ManualLogSource logger;

        public void Awake()
        {
            BigInventory.logger = base.Logger;
            base.Logger.LogMessage("大背包Mod启动");
            Harmony.CreateAndPatchAll(typeof(BigInventory), null);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(OcItemUI_Cell_List), "InitOnAwake", new Type[] { typeof(int) })]
        public static bool GamePatch(OcItemUI_Cell_List __instance, ref int size)
        {
            OcItemUI_InventoryMng instUISS = UISceneSingleton<OcItemUI_InventoryMng>.InstUISS;
            if ((int)__instance.itemType < instUISS.advancedExtendInventory.Length)
            {
                BigInventory.logger.LogMessage($"将{__instance.name} {__instance.itemType}的初始化尺寸从{size}设为{BigInventory.InventorySize}");
                size = BigInventory.InventorySize;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(OcItemUI_Cell_List), "InitOnAwake", new Type[] { })]
        public static bool GamePatch2(OcItemUI_Cell_List __instance)
        {
            if (!__instance.list.IsNullOrEmpty<OcItemUI_Cell>())
            {
                if (__instance.list.Count < __instance.Size)
                {
                    BigInventory.logger.LogMessage($"{__instance.name} {__instance.itemType}的list.Count {__instance.list.Count} < Size {__instance.Size}，进行补充");
                    for (int i = __instance.list.Count; i < __instance.Size; i++)
                    {
                        __instance.list.Add(__instance.InstCell(i));
                    }
                }
            }
            return true;
        }
    }
}
using System;
using BepInEx;
using HarmonyLib;

[BepInPlugin("ant2357.tithe_tax_mod", "Tithe Tax Mod", "1.1.1")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony("ant2357.tithe_tax_mod");
        harmony.PatchAll();
    }
}

// 基礎税金を常に 0 にするパッチ
[HarmonyPatch(typeof(Faction), "GetBaseTax")]
public static class GetBaseTaxPatch
{
    private static void Postfix(ref int __result, bool evasion)
    {
        __result = 0;
    }
}

// 有名税をプレイヤー所持金の10分の1に変更するパッチ
[HarmonyPatch(typeof(Faction), "GetFameTax")]
public static class GetFameTaxPatch
{
    private static void Postfix(ref int __result, bool evasion)
    {
        float playerMoney = EClass.pc.GetCurrency();
        __result = Math.Max(1, (int)Math.Floor(playerMoney / 10f));
    }
}

[HarmonyPatch(typeof(Scene))]
[HarmonyPatch(nameof(Scene.Init))]
class ScenePatch
{
    static void Postfix(Scene.Mode newMode)
    {
        if (newMode == Scene.Mode.StartGame)
        {
            Msg.NewLine();
            Msg.SayRaw("=== Tithe Tax Mod ===");
        }
    }
}

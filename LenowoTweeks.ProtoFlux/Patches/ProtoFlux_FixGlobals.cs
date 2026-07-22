using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;

using HarmonyLib;

using LenowoTweeks.Core;


namespace LenowoTweeks.ProtoFlux.Patches;

[HarmonyPatch]
public class ProtoFlux_FixGlobals
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxGlobalRefProxy), "BuildUI")]
	public static void FixGlobalStringField(ProtoFluxGlobalRefProxy __instance, SyncRef<Button> ____proxyVisual)
	{
		if (!Helpers.ModShouldRun(__instance)) return;
		if (!LenowoTweeks_ProtoFlux.initializeProtofluxGlobals.Value) return;
		Type valueType = __instance.ValueType.Value;
		Type baseType = valueType.IsGenericType ? valueType.GetGenericTypeDefinition() : valueType;
		// DO NOT DO METHOD/FUNCTION PROXIES!!!
		if (valueType.IsGenericType && baseType.Name.Contains("Func") || baseType.Name.Contains("Action")) return;
		// LIKE SERIOUSLY DO NOT DO THEM
		if (__instance.Node.Target.NodeName.Contains("MethodProxy") || __instance.Node.Target.NodeName.Contains("FunctionProxy")) return;

		var proxy = __instance.EnsureProxy();

		Slot proxySlot = ____proxyVisual.Target.Slot;
		if (valueType == typeof(string))
		{
			TextField textField = proxySlot.EnsureSingleComponent<TextField>();
			textField.Text = proxySlot.GetComponentInChildren<Text>();
		}
		else
		{
			__instance.InvokeMethod("OnProxyButtonPressed", [____proxyVisual.Target, new ButtonEventData()]);
		}

		__instance.InvokeMethod("OnChanges");
	}
}

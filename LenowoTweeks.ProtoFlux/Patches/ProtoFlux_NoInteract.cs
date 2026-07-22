using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;

using HarmonyLib;


namespace LenowoTweeks.ProtoFlux.Patches;

[HarmonyPatch]
public class ProtoFlux_NoInteract
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void DisablePhysicalInteract(ProtoFluxNodeVisual __instance)
	{
		if (!LenowoTweeks_ProtoFlux.disablePhysicalInteraction.Value) return;

		__instance.Slot.GetComponent<Canvas>().AcceptPhysicalTouch.Value = false;
	}
}

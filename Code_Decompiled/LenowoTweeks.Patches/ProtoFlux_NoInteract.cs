using System;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFlux_NoInteract
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void DisablePhysicalInteract(ProtoFluxNodeVisual __instance)
	{
		if (LenowoTweeks.disablePhysicalInteraction.Value)
		{
			((SyncField<bool>)(object)((ContainerWorker<Component>)(object)((Component)__instance).Slot).GetComponent<Canvas>((Predicate<Canvas>)null, false).AcceptPhysicalTouch).Value = false;
		}
	}
}

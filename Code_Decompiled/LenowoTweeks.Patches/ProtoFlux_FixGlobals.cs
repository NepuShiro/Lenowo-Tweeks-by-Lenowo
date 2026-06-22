using System;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFlux_FixGlobals
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxGlobalRefProxy), "BuildUI")]
	public static void FixGlobalStringField(ProtoFluxGlobalRefProxy __instance, SyncRef<Button> ____proxyVisual)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		if (!Helpers.ModShouldRun((Component)(object)__instance) || !LenowoTweeks.initializeProtofluxGlobals.Value)
		{
			return;
		}
		Type value = ((SyncField<Type>)(object)((ProtoFluxRefProxy)__instance).ValueType).Value;
		Type type = (value.IsGenericType ? value.GetGenericTypeDefinition() : value);
		if ((!value.IsGenericType || !type.Name.Contains("Func")) && !type.Name.Contains("Action") && !((ProtoFluxRefProxy)__instance).Node.Target.NodeName.Contains("MethodProxy") && !((ProtoFluxRefProxy)__instance).Node.Target.NodeName.Contains("FunctionProxy"))
		{
			IGlobalValueProxy val = __instance.EnsureProxy();
			Slot slot = ((Component)____proxyVisual.Target).Slot;
			if (value == typeof(string))
			{
				TextField val2 = ((ContainerWorker<Component>)(object)slot).EnsureSingleComponent<TextField>((Predicate<TextField>)null);
				val2.Text = slot.GetComponentInChildren<Text>((Predicate<Text>)null, false, false);
			}
			else
			{
				__instance.InvokeMethod("OnProxyButtonPressed", ____proxyVisual.Target, (object)default(ButtonEventData));
			}
			__instance.InvokeMethod("OnChanges");
		}
	}
}

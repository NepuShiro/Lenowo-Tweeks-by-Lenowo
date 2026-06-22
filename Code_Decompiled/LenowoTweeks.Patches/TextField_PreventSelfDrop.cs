using System;
using System.Collections.Generic;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class TextField_PreventSelfDrop
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(TextField), "TryReceive")]
	public static bool TextField_PreventSelfDrop_Patch(TextField __instance, ref bool __result, IEnumerable<IGrabbable> items)
	{
		ValueFieldDropMode value = LenowoTweeks.valueFieldDroppingMode.Value;
		if (value == ValueFieldDropMode.AlwaysAllow)
		{
			return true;
		}
		Slot objectRoot = ObjectRootExtensions.GetObjectRoot(((Component)__instance).Slot, false);
		foreach (IGrabbable item in items)
		{
			ValueProxy<string> componentInChildren = ((IComponent)item).Slot.GetComponentInChildren<ValueProxy<string>>((Predicate<ValueProxy<string>>)null, false, false);
			if (componentInChildren != null)
			{
				((SyncField<string>)(object)__instance.Text.Content).Value = Sync<string>.op_Implicit(componentInChildren.Value);
				__instance.Editor.Target.ForceEditingChangedEvent();
				__result = true;
				return false;
			}
		}
		__result = false;
		if (value == ValueFieldDropMode.NeverAllow)
		{
			return false;
		}
		foreach (IGrabbable item2 in items)
		{
			if (ObjectRootExtensions.GetObjectRoot(((IComponent)item2).Slot, false) != objectRoot)
			{
				IValueSource componentInChildren2 = ((IComponent)item2).Slot.GetComponentInChildren<IValueSource>((Predicate<IValueSource>)null, false, false);
				if (componentInChildren2 != null)
				{
					((SyncField<string>)(object)__instance.Text.Content).Value = componentInChildren2.BoxedValue?.ToString();
					__instance.Editor.Target.ForceEditingChangedEvent();
					__result = true;
				}
			}
		}
		return false;
	}
}

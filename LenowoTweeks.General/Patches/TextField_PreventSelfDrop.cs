using FrooxEngine;
using FrooxEngine.UIX;

using HarmonyLib;

namespace LenowoTweeks.General.Patches;

[HarmonyPatch]
public class TextField_PreventSelfDrop
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(TextField), nameof(TextField.TryReceive))]
	public static bool TextField_PreventSelfDrop_Patch(TextField __instance, ref bool __result, IEnumerable<IGrabbable> items)
	{
		ValueFieldDropMode dropMode = LenowoTweeks_General.valueFieldDroppingMode.Value;
		if (dropMode == ValueFieldDropMode.AlwaysAllow) return true;

		Slot thisRoot = __instance.Slot.GetObjectRoot();
		foreach (IGrabbable item in items)
		{
			// this one doesnt matter, so it can stay as is
			ValueProxy<string> proxiesInChildren = item.Slot.GetComponentInChildren<ValueProxy<string>>();
			if (proxiesInChildren != null)
			{
				__instance.Text.Content.Value = proxiesInChildren.Value;
				__instance.Editor.Target.ForceEditingChangedEvent();
				__result = true;
				return false;
			}
		}

		__result = false;
		if (dropMode == ValueFieldDropMode.NeverAllow) return false;

		foreach (IGrabbable item2 in items)
		{
			if (item2.Slot.GetObjectRoot() != thisRoot)
			{
				IValueSource fieldsInChildren = item2.Slot.GetComponentInChildren<IValueSource>();
				if (fieldsInChildren != null)
				{
					__instance.Text.Content.Value = fieldsInChildren.BoxedValue?.ToString();
					__instance.Editor.Target.ForceEditingChangedEvent();
					__result = true;
				}
			}
		}

		return false;
	}
}

using FrooxEngine;

using HarmonyLib;

namespace LenowoTweeks.Core.Patches;

[HarmonyPatch]
public class EnsureConfigSpace
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(UserRoot), "OnStart")]
	public static void EnsureConfigSpaceExists(UserRoot __instance)
	{
		if (!LenowoTweeks_Core.ensureConfigSpace.Value) return;
		User activeUser = __instance.ActiveUser;
		if (!activeUser.IsLocalUser) return;

		Helpers.GetConfigSpace(activeUser);
	}
}

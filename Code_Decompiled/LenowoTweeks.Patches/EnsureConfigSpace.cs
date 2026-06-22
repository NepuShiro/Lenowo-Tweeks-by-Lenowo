using FrooxEngine;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class EnsureConfigSpace
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(UserRoot), "OnStart")]
	public static void EnsureConfigSpaceExists(UserRoot __instance)
	{
		if (LenowoTweeks.ensureConfigSpace.Value)
		{
			User activeUser = __instance.ActiveUser;
			if (activeUser.IsLocalUser)
			{
				Helpers.GetConfigSpace(activeUser);
			}
		}
	}
}

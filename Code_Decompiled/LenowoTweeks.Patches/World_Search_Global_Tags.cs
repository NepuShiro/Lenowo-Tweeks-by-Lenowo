using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using SkyFrost.Base;

namespace LenowoTweeks.Patches;

[HarmonyPatch(typeof(LegacyWorldListManager))]
public class World_Search_Global_Tags
{
	private static bool needsInitialized = true;

	private static List<string> optionalTags = new List<string>();

	private static List<string> requiredTags = new List<string>();

	private static List<string> excludedTags = new List<string>();

	private static void WorldSearchOnKeyConfigurationChanged(object? new_value)
	{
		optionalTags.Clear();
		requiredTags.Clear();
		excludedTags.Clear();
		if (new_value is string text)
		{
			SearchQueryParser.Parse(text, optionalTags, requiredTags, excludedTags);
		}
	}

	public static bool Prepare()
	{
		if (needsInitialized)
		{
			LenowoTweeks.worldSearchGlobalTags.ConfigKey.OnChanged += WorldSearchOnKeyConfigurationChanged;
			WorldSearchOnKeyConfigurationChanged(LenowoTweeks.worldSearchGlobalTags.Value);
			needsInitialized = false;
		}
		return true;
	}

	private static MethodBase? TargetMethod()
	{
		MethodInfo element = AccessTools.Method(typeof(LegacyWorldListManager), "UpdateList");
		AsyncStateMachineAttribute customAttribute = element.GetCustomAttribute<AsyncStateMachineAttribute>();
		return customAttribute.StateMachineType.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.NonPublic);
	}

	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].opcode != OpCodes.Ldfld)
			{
				continue;
			}
			string text = list[i].operand.ToString();
			if (text.Substring(text.Length - 5) == "Terms" && list[i + 1].opcode == OpCodes.Callvirt && list[i + 1].operand.ToString().Contains("Clear()"))
			{
				list[i + 1].opcode = OpCodes.Call;
				switch (text.Substring(text.Length - 13, 8))
				{
				case "optional":
					list[i + 1].operand = AccessTools.Method(typeof(World_Search_Global_Tags), "Optional_Fill");
					break;
				case "required":
					list[i + 1].operand = AccessTools.Method(typeof(World_Search_Global_Tags), "Required_Fill");
					break;
				case "excluded":
					list[i + 1].operand = AccessTools.Method(typeof(World_Search_Global_Tags), "Excluded_Fill");
					return list.AsEnumerable();
				default:
					UniLog.Error("World_Search_Global_Tags:Transpiler ERROR: \"" + text + "\" is not expected!", false);
					break;
				}
			}
		}
		return list.AsEnumerable();
	}

	private static void Optional_Fill(List<string> _optionalTerms)
	{
		_optionalTerms.Clear();
		_optionalTerms.AddRange(optionalTags);
	}

	private static void Required_Fill(List<string> _requiredTerms)
	{
		_requiredTerms.Clear();
		_requiredTerms.AddRange(requiredTags);
	}

	private static void Excluded_Fill(List<string> _excludedTerms)
	{
		_excludedTerms.Clear();
		_excludedTerms.AddRange(excludedTags);
	}
}

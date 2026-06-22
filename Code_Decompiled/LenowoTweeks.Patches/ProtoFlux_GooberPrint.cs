using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using SkyFrost.Base;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFlux_GooberPrint
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxTool), "OnUnpack")]
	public static bool CheckUnpackInPrint(Slot root)
	{
		if (!LenowoTweeks.AllowGooberUnpack.Value)
		{
			return true;
		}
		string tag = root.Tag;
		if (string.IsNullOrWhiteSpace(tag))
		{
			return true;
		}
		if (!tag.StartsWith("[") || !tag.Contains("&"))
		{
			return true;
		}
		((Worker)root).StartTask((Func<Task>)async delegate
		{
			Uri spawnUri = ((SkyFrostInterface)((Worker)root).Cloud).Variables.Read<Uri>("U-1O0jo6sFCi0", "U-1O0jo6sFCi0.Rosa.GooberPrint", (string)null).GetAwaiter().GetResult()
				.Entity;
			Slot GooberPrint = ((Worker)root).LocalUserSpace.AddSlot("GooberPrint Spawner", false);
			await Helpers.CloudSpawn(spawnUri, GooberPrint);
			SlotPositioning.PositionInFrontOfUser(GooberPrint, (float3?)null, (float3?)null, 0f, (User)null, false, true, false);
			floatQ viewRotation = ((Worker)root).LocalUserRoot.ViewRotation;
			float3 val = new float3(0f, 0f, 1f);
			GooberPrint.Forward = (ref viewRotation) * (ref val);
			GooberPrint.Up = ((Component)((Worker)root).LocalUserRoot).Slot.Up;
			val = GooberPrint.GlobalPosition;
			float3 forward = GooberPrint.Forward;
			float3 val2 = 0.5f * (ref forward);
			GooberPrint.GlobalPosition = (ref val) + (ref val2);
			if (GooberPrint != null && !((Worker)GooberPrint).IsRemoved)
			{
				Slot GooberProcessing = ((IEnumerable<Slot>)(object)GooberPrint.Children).First();
				DynamicVariableHelper.WriteDynamicVariable<Slot>(GooberProcessing, "GooberPrint/UnpackRoot", root);
				ProtoFluxHelper.DynamicImpulseHandler.TriggerDynamicImpulse(GooberProcessing, "GPUnpack", false, (FrooxEngineContext)null);
				await new Updates(6);
				ProtoFluxHelper.DynamicImpulseHandler.TriggerDynamicImpulse(GooberProcessing, "GPSnapNodes", false, (FrooxEngineContext)null);
			}
		});
		return false;
	}
}

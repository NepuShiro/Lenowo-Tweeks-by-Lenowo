using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements.Core;
using FrooxEngine;
using SkyFrost.Base;

namespace LenowoTweeks;

public static class Helpers
{
	private const RunMode DefaultRunMode = RunMode.ElementAllocating | RunMode.SlotAllocating;

	public static bool ModShouldRun(Component instance)
	{
		return ModShouldRun(instance, RunMode.ElementAllocating | RunMode.SlotAllocating);
	}

	public static bool ModShouldRun(Component instance, RunMode runMode)
	{
		return Internal_ModShouldRun((IWorldElement)(object)instance, instance.Slot, runMode);
	}

	public static bool ModShouldRun(Slot instance)
	{
		return ModShouldRun(instance, RunMode.ElementAllocating | RunMode.SlotAllocating);
	}

	public static bool ModShouldRun(Slot instance, RunMode runMode)
	{
		return Internal_ModShouldRun((IWorldElement)(object)instance, instance, runMode);
	}

	private static bool Internal_ModShouldRun(IWorldElement element, Slot targetSlot, RunMode runMode)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		switch (runMode)
		{
		case RunMode.None:
			return false;
		case RunMode.Always:
			return true;
		default:
		{
			if (!runMode.HasFlag(RunMode.AllowRemoved) && (element.IsRemoved || ((Worker)targetSlot).IsRemoved))
			{
				return false;
			}
			RefID referenceID = element.ReferenceID;
			ulong num = default(ulong);
			byte b = default(byte);
			((RefID)(ref referenceID)).ExtractIDs(ref num, ref b);
			User userByAllocationID = element.World.GetUserByAllocationID(b);
			if (runMode.HasFlag(RunMode.ElementAllocating))
			{
				if (userByAllocationID == ((Worker)targetSlot).LocalUser)
				{
					return true;
				}
				if (runMode.HasFlag(RunMode.AllowNonAllocating) && userByAllocationID == null)
				{
					return true;
				}
			}
			if (runMode.HasFlag(RunMode.SlotAllocating) && (userByAllocationID == null || num < userByAllocationID.AllocationIDStart))
			{
				referenceID = ((Worker)targetSlot).ReferenceID;
				ulong num2 = default(ulong);
				byte b2 = default(byte);
				((RefID)(ref referenceID)).ExtractIDs(ref num2, ref b2);
				User userByAllocationID2 = element.World.GetUserByAllocationID(b2);
				if (runMode.HasFlag(RunMode.AllowNonAllocating) && userByAllocationID2 == null)
				{
					return true;
				}
				if (userByAllocationID2 == null || num2 < userByAllocationID2.AllocationIDStart || userByAllocationID2 != ((Worker)targetSlot).LocalUser)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		}
	}

	public static User AllocatingUser(this Worker worker)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RefID referenceID = worker.ReferenceID;
		ulong num = default(ulong);
		byte b = default(byte);
		((RefID)(ref referenceID)).ExtractIDs(ref num, ref b);
		return worker.World.GetUserByAllocationID(b);
	}

	public static Slot GetModSharedSlot(User runner)
	{
		return ((Worker)runner).World.RootSlot.FindChild("__TEMP").FindChildOrAdd("LenowoTweeks Shared", false);
	}

	public static Slot GetModUserSlot(User runner)
	{
		Slot modSharedSlot = GetModSharedSlot(runner);
		return modSharedSlot.FindChildOrAdd(runner.UserName + "'s Config", false);
	}

	public static Slot GetProtoFluxManager(User runner)
	{
		Slot modUserSlot = GetModUserSlot(runner);
		return modUserSlot.FindChildOrAdd(runner.UserName + "'s Protoflux Manager", false);
	}

	public static Slot GetConnectorManager(User runner)
	{
		Slot protoFluxManager = GetProtoFluxManager(runner);
		return protoFluxManager.FindChildOrAdd(runner.UserName + "'s Connector Stuff", false);
	}

	public static Slot GetWireManager(User runner)
	{
		Slot protoFluxManager = GetProtoFluxManager(runner);
		return protoFluxManager.FindChildOrAdd(runner.UserName + "'s Wire Manager", false);
	}

	public static DynamicVariableSpace GetConfigSpace(User runner)
	{
		Slot modUserSlot = GetModUserSlot(runner);
		DynamicVariableSpace componentOrAttach = ((ContainerWorker<Component>)(object)modUserSlot).GetComponentOrAttach<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null);
		((SyncField<string>)(object)componentOrAttach.SpaceName).Value = "Config";
		return componentOrAttach;
	}

	public static T GetConfigVariable<T>(User runner, string VariableName, T defaultValue = default(T))
	{
		string key = "Config/" + VariableName;
		Slot slot = ((Component)GetConfigSpace(runner)).Slot;
		Dictionary<string, DynamicValueVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicValueVariable<T>>((Predicate<DynamicValueVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
		if (dictionary.TryGetValue(key, out var value))
		{
			return ((SyncField<T>)(object)value.Value).Value;
		}
		return defaultValue;
	}

	public static T GetConfigReference<T>(User runner, string VariableName, T defaultValue = null) where T : class, IWorldElement
	{
		string key = "Config/" + VariableName;
		Slot slot = ((Component)GetConfigSpace(runner)).Slot;
		Dictionary<string, DynamicReferenceVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicReferenceVariable<T>>((Predicate<DynamicReferenceVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
		if (dictionary.TryGetValue(key, out var value))
		{
			return value.Reference.Target;
		}
		return defaultValue;
	}

	public static IField<T> GetConfigVariableSource<T>(User runner, string VariableName, Slot createUnder = null)
	{
		string text = "Config/" + VariableName;
		Slot slot = ((Component)GetConfigSpace(runner)).Slot;
		Dictionary<string, DynamicValueVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicValueVariable<T>>((Predicate<DynamicValueVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
		if (dictionary.TryGetValue(text, out var value))
		{
			return (IField<T>)(object)value.Value;
		}
		Slot val = createUnder ?? slot;
		DynamicValueVariable<T> val2 = ((ContainerWorker<Component>)(object)val).AttachComponent<DynamicValueVariable<T>>(true, (Action<DynamicValueVariable<T>>)null);
		((SyncField<string>)(object)((DynamicVariableBase<T>)(object)val2).VariableName).Value = text;
		return (IField<T>)(object)val2.Value;
	}

	public static SyncRef<T> GetConfigReferenceSource<T>(User runner, string VariableName, Slot createUnder = null) where T : class, IWorldElement
	{
		string text = "Config/" + VariableName;
		Slot slot = ((Component)GetConfigSpace(runner)).Slot;
		Dictionary<string, DynamicReferenceVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicReferenceVariable<T>>((Predicate<DynamicReferenceVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
		if (dictionary.TryGetValue(text, out var value))
		{
			return value.Reference;
		}
		Slot val = createUnder ?? slot;
		DynamicReferenceVariable<T> val2 = ((ContainerWorker<Component>)(object)val).AttachComponent<DynamicReferenceVariable<T>>(true, (Action<DynamicReferenceVariable<T>>)null);
		((SyncField<string>)(object)((DynamicVariableBase<T>)(object)val2).VariableName).Value = text;
		return val2.Reference;
	}

	public static void SetConfigVariable<T>(User runner, string VariableName, T NewValue, Slot createUnder = null)
	{
		if (!GetConfigVariable(runner, VariableName + ".override", defaultValue: false))
		{
			string text = "Config/" + VariableName;
			Slot slot = ((Component)GetConfigSpace(runner)).Slot;
			Dictionary<string, DynamicValueVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicValueVariable<T>>((Predicate<DynamicValueVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicValueVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
			if (dictionary.TryGetValue(text, out var value))
			{
				((SyncField<T>)(object)value.Value).Value = NewValue;
				return;
			}
			Slot val = createUnder ?? slot;
			DynamicValueVariable<T> val2 = ((ContainerWorker<Component>)(object)val).AttachComponent<DynamicValueVariable<T>>(true, (Action<DynamicValueVariable<T>>)null);
			((SyncField<string>)(object)((DynamicVariableBase<T>)(object)val2).VariableName).Value = text;
			((SyncField<T>)(object)val2.Value).Value = NewValue;
		}
	}

	public static void SetConfigReference<T>(User runner, string VariableName, T NewValue, Slot createUnder = null) where T : class, IWorldElement
	{
		if (!GetConfigVariable(runner, VariableName + ".override", defaultValue: false))
		{
			string text = "Config/" + VariableName;
			Slot slot = ((Component)GetConfigSpace(runner)).Slot;
			Dictionary<string, DynamicReferenceVariable<T>> dictionary = slot.GetComponentsInChildren<DynamicReferenceVariable<T>>((Predicate<DynamicReferenceVariable<T>>)null, false, false, (Predicate<Slot>)null).DistinctBy((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value).ToDictionary((DynamicReferenceVariable<T> v) => ((SyncField<string>)(object)((DynamicVariableBase<T>)(object)v).VariableName).Value);
			if (dictionary.TryGetValue(text, out var value))
			{
				value.Reference.Target = NewValue;
				return;
			}
			Slot val = createUnder ?? slot;
			DynamicReferenceVariable<T> val2 = ((ContainerWorker<Component>)(object)val).AttachComponent<DynamicReferenceVariable<T>>(true, (Action<DynamicReferenceVariable<T>>)null);
			((SyncField<string>)(object)((DynamicVariableBase<T>)(object)val2).VariableName).Value = text;
			val2.Reference.Target = NewValue;
		}
	}

	public static void DriveFromVariable<T>(User runner, string VariableName, IField<T> output, Slot createUnder = null)
	{
		IField<T> configVariableSource = GetConfigVariableSource<T>(runner, VariableName, createUnder);
		if (configVariableSource != null)
		{
			ValueCopyExtensions.DriveFrom<T>(output, configVariableSource, false, false, true);
		}
	}

	public static void DriveFromReference<T>(User runner, string VariableName, SyncRef<T> output, Slot createUnder = null) where T : class, IWorldElement
	{
		SyncRef<T> configReferenceSource = GetConfigReferenceSource<T>(runner, VariableName, createUnder);
		if (configReferenceSource != null)
		{
			ReferenceCopyExtensions.DriveFrom<T>(output, configReferenceSource, false, false, true);
		}
	}

	public static Slot GetTimeSineDriverSlot(User runner)
	{
		return GetModUserSlot(runner).FindChildOrAdd("TSD", true);
	}

	public static IField<bool> GetTimeSineDriverSource(User runner)
	{
		Slot timeSineDriverSlot = GetTimeSineDriverSlot(runner);
		TimeSineDriver componentOrAttach = ((ContainerWorker<Component>)(object)timeSineDriverSlot).GetComponentOrAttach<TimeSineDriver>((Predicate<TimeSineDriver>)null);
		ValueField<float> componentOrAttach2 = ((ContainerWorker<Component>)(object)timeSineDriverSlot).GetComponentOrAttach<ValueField<float>>((Predicate<ValueField<float>>)null);
		ConvertibleIntDriver<float> componentOrAttach3 = ((ContainerWorker<Component>)(object)timeSineDriverSlot).GetComponentOrAttach<ConvertibleIntDriver<float>>((Predicate<ConvertibleIntDriver<float>>)null);
		ValueField<int> componentOrAttach4 = ((ContainerWorker<Component>)(object)timeSineDriverSlot).GetComponentOrAttach<ValueField<int>>((Predicate<ValueField<int>>)null);
		ValueEqualityDriver<int> componentOrAttach5 = ((ContainerWorker<Component>)(object)timeSineDriverSlot).GetComponentOrAttach<ValueEqualityDriver<int>>((Predicate<ValueEqualityDriver<int>>)null);
		((SyncField<float>)(object)componentOrAttach.Min).Value = 0f;
		((SyncField<float>)(object)componentOrAttach.Max).Value = 1f;
		((SyncField<float>)(object)componentOrAttach.Speed).Value = 4f;
		((SyncRef<IField<float>>)(object)componentOrAttach.Target).Target = (IField<float>)(object)componentOrAttach2.Value;
		((SyncRef<IField<float>>)(object)componentOrAttach3.Source).Target = (IField<float>)(object)componentOrAttach2.Value;
		((SyncRef<IField<int>>)(object)componentOrAttach3.Target).Target = (IField<int>)(object)componentOrAttach4.Value;
		((SyncRef<IField<int>>)(object)componentOrAttach5.TargetValue).Target = (IField<int>)(object)componentOrAttach4.Value;
		((SyncField<int>)(object)componentOrAttach5.Reference).Value = 1;
		IField<bool> configVariableSource = GetConfigVariableSource<bool>(runner, "TSD.Source", timeSineDriverSlot);
		((SyncRef<IField<bool>>)(object)componentOrAttach5.Target).Target = configVariableSource;
		return configVariableSource;
	}

	public static void DriveFromTSD(User runner, IField<bool> output)
	{
		Slot timeSineDriverSlot = GetTimeSineDriverSlot(runner);
		IField<bool> configVariableSource = GetConfigVariableSource<bool>(runner, "TSD.Source", timeSineDriverSlot);
		if (configVariableSource != null)
		{
			ValueCopyExtensions.DriveFrom<bool>(output, configVariableSource, false, false, true);
		}
	}

	public static async Task CloudSpawn(Uri uri, Slot root)
	{
		DataTreeDictionary loadNode = DataTreeConverter.Load(await ((Worker)root).Engine.AssetManager.GatherAssetFile(uri, 100f, (DB_Endpoint?)null), (string)null);
		root.LoadObject(loadNode, (IRecord)null, (Slot)null, (Predicate<Type>)null, (ReferenceTranslator)null, (Func<DataTreeNode, DataTreeNode>)null);
		await new Updates(6);
	}
}

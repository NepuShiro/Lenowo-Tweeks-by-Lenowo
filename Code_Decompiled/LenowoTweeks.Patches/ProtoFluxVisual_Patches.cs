using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFluxVisual_Patches
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void TweakNodeVisual(UIBuilder ui, ProtoFluxNodeVisual __instance)
	{
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.collapsibleProtoflux.Value)
		{
			return;
		}
		try
		{
			Slot UISlot = ((Component)__instance).Slot;
			ProtoFluxNode target = ((SyncRef<ProtoFluxNode>)(object)__instance.Node).Target;
			Slot slot = ((Component)target).Slot;
			int num = target.NodeInputCount + target.NodeInputLists.Sum((ISyncList l) => l.Count) + target.NodeOperationCount + target.NodeOperationLists.Sum((ISyncList l) => l.Count);
			int num2 = target.NodeOutputCount + target.NodeOutputLists.Sum((ISyncList l) => l.Count) + target.NodeImpulseCount + target.NodeImpulseLists.Sum((ISyncList l) => l.Count);
			int value = LenowoTweeks.collapseThreshold.Value;
			bool flag = num > value || num2 > value;
			ValueField<bool> toggle_field = null;
			if (flag)
			{
				toggle_field = ((ContainerWorker<Component>)(object)slot).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)((ValueField<bool> vf) => ((ComponentBase<Component>)(object)vf).UpdateOrder == 7679), false);
				if (toggle_field == null)
				{
					toggle_field = ((ContainerWorker<Component>)(object)slot).AttachComponent<ValueField<bool>>(true, (Action<ValueField<bool>>)null);
					((ComponentBase<Component>)(object)toggle_field).UpdateOrder = 7679;
					((SyncField<bool>)(object)toggle_field.Value).Value = true;
				}
			}
			Slot val = UISlot.FindChild("Overlapping Layout");
			IEnumerable<Slot> enumerable = ((IEnumerable<Slot>)(object)val.FindChild("Inputs & Operations").Children).Concat((IEnumerable<Slot>)(object)val.FindChild("Outputs & Impulses").Children);
			foreach (Slot item in enumerable)
			{
				ValueField<bool> val2 = ((ContainerWorker<Component>)(object)item).AttachComponent<ValueField<bool>>(true, (Action<ValueField<bool>>)null);
				Slot val3 = item.FindChild("Connector");
				if (val3 == null || ((Worker)val3).IsRemoved)
				{
					continue;
				}
				ProtoFluxElementProxy component = ((ContainerWorker<Component>)(object)val3).GetComponent<ProtoFluxElementProxy>((Predicate<ProtoFluxElementProxy>)null, false);
				if (component == null || ((Worker)component).IsRemoved)
				{
					continue;
				}
				Type type = ((object)component).GetType();
				if (flag)
				{
					MultiBoolConditionDriver val4 = ((ContainerWorker<Component>)(object)item).AttachComponent<MultiBoolConditionDriver>(true, (Action<MultiBoolConditionDriver>)null);
					((SyncField<ConditionMode>)(object)val4.Mode).Value = (ConditionMode)1;
					((SyncRef<IField<bool>>)(object)val4.Target).Target = (IField<bool>)(object)item.ActiveSelf_Field;
					((SyncRef<IField<bool>>)(object)((SyncElementList<Condition>)(object)val4.Conditions).Add().Field).Target = (IField<bool>)(object)toggle_field.Value;
					((SyncRef<IField<bool>>)(object)((SyncElementList<Condition>)(object)val4.Conditions).Add().Field).Target = (IField<bool>)(object)val2.Value;
					if (type == typeof(ProtoFluxInputProxy) || type == typeof(ProtoFluxImpulseProxy))
					{
						ValueCopyExtensions.DriveFrom<bool>((IField<bool>)(object)val2.Value, (IField<bool>)(object)((ContainerWorker<Component>)(object)val3).GetComponent<BooleanReferenceDriver<IAssetProvider<Sprite>>>((Predicate<BooleanReferenceDriver<IAssetProvider<Sprite>>>)null, false).State, false, false, true);
					}
				}
				if (type == typeof(ProtoFluxOutputProxy) || type == typeof(ProtoFluxOperationProxy))
				{
					ReferenceList<ProtoFluxNode> componentOrAttach = ((ContainerWorker<Component>)(object)val3).GetComponentOrAttach<ReferenceList<ProtoFluxNode>>((Predicate<ReferenceList<ProtoFluxNode>>)null);
					if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach.References).Count == 0)
					{
						componentOrAttach.References.Add((ProtoFluxNode)null);
					}
					SetupEqualityDriver<ProtoFluxNode>(item, val2, ((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach.References).GetElement(0));
				}
			}
			if (!flag)
			{
				return;
			}
			ui.NestInto(UISlot);
			RectTransform val5 = ui.Panel();
			ui.IgnoreLayout();
			((Component)val5).Slot.OrderOffset = 32769L;
			((SyncField<float2>)(object)val5.AnchorMin).Value = new float2(0f, 0f);
			((SyncField<float2>)(object)val5.AnchorMax).Value = new float2(0.4f, 0f);
			((SyncField<float2>)(object)val5.OffsetMin).Value = new float2(8f, 0f);
			((SyncField<float2>)(object)val5.OffsetMax).Value = new float2(-16f, 16f);
			Button val6 = ui.Button();
			ButtonToggle val7 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<ButtonToggle>(true, (Action<ButtonToggle>)null);
			val7.TargetValue.Target = (IField<bool>)(object)toggle_field.Value;
			ui.NestOut();
			ui.NestOut();
			BooleanValueDriver<string> val8 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<BooleanValueDriver<string>>(true, (Action<BooleanValueDriver<string>>)null);
			((SyncField<string>)(object)val8.TrueValue).Value = "-";
			((SyncField<string>)(object)val8.FalseValue).Value = "+";
			((SyncRef<IField<string>>)(object)val8.TargetField).TrySet((IWorldElement)(object)val6.LabelTextField);
			ValueCopyExtensions.DriveFrom<bool>((IField<bool>)(object)val8.State, (IField<bool>)(object)toggle_field.Value, false, false, true);
			if (((SyncField<bool>)(object)toggle_field.Value).Value)
			{
				return;
			}
			UISlot.RunInUpdates(4, (Action)delegate
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				((SyncField<bool>)(object)toggle_field.Value).Value = true;
				UISlot.RunInUpdates(2, (Action)delegate
				{
					((SyncField<bool>)(object)toggle_field.Value).Value = false;
				});
			});
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in TweakNodeVisual: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	private static ReferenceEqualityDriver<T> SetupEqualityDriver<T>(Slot s, ValueField<bool> bv, SyncRef<T> field) where T : class, IWorldElement
	{
		ReferenceEqualityDriver<T> val = ((ContainerWorker<Component>)(object)s).AttachComponent<ReferenceEqualityDriver<T>>(true, (Action<ReferenceEqualityDriver<T>>)null);
		((SyncRef<IField<bool>>)(object)val.Target).Target = (IField<bool>)(object)bv.Value;
		((SyncRef<SyncRef<T>>)(object)val.TargetReference).Target = field;
		((SyncField<bool>)(object)val.Invert).Value = true;
		Helpers.DriveFromVariable<bool>(((Worker)s).LocalUser, "TSD.Source", (IField<bool>)(object)((ComponentBase<Component>)(object)val).EnabledField);
		return val;
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxNode), "TryConnectInput")]
	public static void TryConnectInputHookPre(ISyncRef input, out IWorldElement __state)
	{
		__state = ((input != null) ? input.Target : null);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNode), "TryConnectInput")]
	public static void TryConnectInputHook(ISyncRef input, INodeOutput output, ref bool __result, IWorldElement __state)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!__result)
			{
				return;
			}
			Slot val = ((((object)((IWorldElement)output).Parent).GetType() == typeof(Slot)) ? ((Slot)((IWorldElement)output).Parent) : ((((object)((IWorldElement)output).Parent.Parent).GetType() == typeof(Slot)) ? ((Slot)((IWorldElement)output).Parent.Parent) : ((Slot)((IWorldElement)output).Parent.Parent.Parent)));
			ProtoFluxOutputProxy componentInChildren = val.GetComponentInChildren<ProtoFluxOutputProxy>((Predicate<ProtoFluxOutputProxy>)((ProtoFluxOutputProxy p) => p.NodeOutput.Target == output), false, false);
			if (componentInChildren == null)
			{
				return;
			}
			ProtoFluxNode node = ((input is ProtoFluxNode) ? ((ProtoFluxNode)input) : ((((IWorldElement)input).Parent is ProtoFluxNode) ? ((ProtoFluxNode)((IWorldElement)input).Parent) : ((ProtoFluxNode)((IWorldElement)input).Parent.Parent)));
			if (__state != null && (object)__state != output)
			{
				Slot val2 = ((((object)__state.Parent).GetType() == typeof(Slot)) ? ((Slot)__state.Parent) : ((((object)__state.Parent.Parent).GetType() == typeof(Slot)) ? ((Slot)__state.Parent.Parent) : ((Slot)__state.Parent.Parent.Parent)));
				ProtoFluxOutputProxy componentInChildren2 = val2.GetComponentInChildren<ProtoFluxOutputProxy>((Predicate<ProtoFluxOutputProxy>)((ProtoFluxOutputProxy p) => (object)p.NodeOutput.Target == __state), false, false);
				RemoveNodeFromField(((Component)componentInChildren2).Slot, node);
			}
			AddNodeToField(((Component)componentInChildren).Slot, node);
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in TryConnectInputHook: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxNode), "TryConnectImpulse")]
	public static void TryConnectImpulseHookPre(ISyncRef impulse, out IWorldElement __state)
	{
		__state = ((impulse != null) ? impulse.Target : null);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNode), "TryConnectImpulse")]
	public static void TryConnectImpulseHook(ISyncRef impulse, INodeOperation operation, ref bool __result, IWorldElement __state)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!__result)
			{
				return;
			}
			Slot val = ((((IWorldElement)operation).Parent is Slot) ? ((Slot)((IWorldElement)operation).Parent) : ((((IWorldElement)operation).Parent.Parent is Slot) ? ((Slot)((IWorldElement)operation).Parent.Parent) : ((Slot)((IWorldElement)operation).Parent.Parent.Parent)));
			ProtoFluxOperationProxy componentInChildren = val.GetComponentInChildren<ProtoFluxOperationProxy>((Predicate<ProtoFluxOperationProxy>)((ProtoFluxOperationProxy p) => p.NodeOperation.Target == operation), false, false);
			if (componentInChildren == null)
			{
				return;
			}
			ProtoFluxNode node = ((impulse is ProtoFluxNode) ? ((ProtoFluxNode)impulse) : ((((IWorldElement)impulse).Parent is ProtoFluxNode) ? ((ProtoFluxNode)((IWorldElement)impulse).Parent) : ((ProtoFluxNode)((IWorldElement)impulse).Parent.Parent)));
			if (__state != null && (object)__state != operation)
			{
				Slot val2 = ((((object)__state.Parent).GetType() == typeof(Slot)) ? ((Slot)__state.Parent) : ((((object)__state.Parent.Parent).GetType() == typeof(Slot)) ? ((Slot)__state.Parent.Parent) : ((Slot)__state.Parent.Parent.Parent)));
				ProtoFluxOperationProxy componentInChildren2 = val2.GetComponentInChildren<ProtoFluxOperationProxy>((Predicate<ProtoFluxOperationProxy>)((ProtoFluxOperationProxy p) => (object)p.NodeOperation.Target == __state), false, false);
				RemoveNodeFromField(((Component)componentInChildren2).Slot, node);
			}
			AddNodeToField(((Component)componentInChildren).Slot, node);
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in TryConnectImpulseHook: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxWireManager), "OnAwake")]
	public static void OnAwakeHook(ProtoFluxWireManager __instance)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Component)__instance).Slot.RunInUpdates(LenowoTweeks.collapseAwakeDelay.Value, (Action)delegate
			{
				ProtoFluxWireManager obj = __instance;
				if (((obj != null) ? ((Component)obj).Slot : null) != null && ((Component)__instance).Slot.Name != "TempWire" && ((SyncRef<Slot>)(object)__instance?.ConnectPoint)?.Target != null && ((ContainerWorker<Component>)(object)((Component)__instance).Slot).GetComponents<ProtoFluxWireManager>((Predicate<ProtoFluxWireManager>)null, false).Count == 1)
				{
					AddNodeToField(((SyncRef<Slot>)(object)__instance.ConnectPoint).Target.Parent, ((ContainerWorker<Component>)(object)((Component)__instance).Slot.Parent.FindParent((Predicate<Slot>)((Slot s) => s.Name == "<NODE_UI>"), -1).Parent).GetComponent<ProtoFluxNode>((Predicate<ProtoFluxNode>)null, false));
				}
			});
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in OnDestroyHook: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxWireManager), "OnDestroy")]
	public static void OnDestroyHook(ProtoFluxWireManager __instance)
	{
		try
		{
			if (((__instance != null) ? ((Component)__instance).Slot : null) != null && ((Component)__instance).Slot.Name != "TempWire" && ((SyncRef<Slot>)(object)__instance?.ConnectPoint)?.Target != null && Helpers.ModShouldRun(((Component)__instance).Slot) && ((ContainerWorker<Component>)(object)((Component)__instance).Slot).GetComponents<ProtoFluxWireManager>((Predicate<ProtoFluxWireManager>)null, false).Count == 1)
			{
				RemoveNodeFromField(((SyncRef<Slot>)(object)__instance.ConnectPoint).Target.Parent, ((ContainerWorker<Component>)(object)((Component)__instance).Slot.Parent.FindParent((Predicate<Slot>)((Slot s) => s.Name == "<NODE_UI>"), -1).Parent).GetComponent<ProtoFluxNode>((Predicate<ProtoFluxNode>)null, false));
			}
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in OnDestroyHook: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ProtoFluxTool), "OnPrimaryRelease")]
	public static void OnCutHook(ProtoFluxTool __instance, SyncRef<Slot> ____currentCutLine, HashSet<ProtoFluxWireManager> ____cutWires)
	{
		try
		{
			if (____currentCutLine.Target == null || ____cutWires == null || ____cutWires.Count <= 0)
			{
				return;
			}
			foreach (ProtoFluxWireManager ____cutWire in ____cutWires)
			{
				ProtoFluxElementProxy componentInParents = ((Component)____cutWire).Slot.GetComponentInParents<ProtoFluxElementProxy>((Predicate<ProtoFluxElementProxy>)null, true, false);
				ProtoFluxInputProxy val = (ProtoFluxInputProxy)(object)((componentInParents is ProtoFluxInputProxy) ? componentInParents : null);
				if (val != null)
				{
					RemoveNodeFromField(((SyncRef<Slot>)(object)((SyncRef<ProtoFluxWireManager>)(object)((ProtoFluxWireProxy<INodeOutput>)(object)val).Wire).Target.ConnectPoint).Target.Parent, ((ContainerWorker<Component>)(object)((Component)val).Slot.Parent.FindParent((Predicate<Slot>)((Slot s) => s.Name == "<NODE_UI>"), -1).Parent).GetComponent<ProtoFluxNode>((Predicate<ProtoFluxNode>)null, false));
				}
				ProtoFluxImpulseProxy val2 = (ProtoFluxImpulseProxy)(object)((componentInParents is ProtoFluxImpulseProxy) ? componentInParents : null);
				if (val2 != null)
				{
					RemoveNodeFromField(((SyncRef<Slot>)(object)((SyncRef<ProtoFluxWireManager>)(object)((ProtoFluxWireProxy<INodeOperation>)(object)val2).Wire).Target.ConnectPoint).Target.Parent, ((ContainerWorker<Component>)(object)((Component)val2).Slot.Parent.FindParent((Predicate<Slot>)((Slot s) => s.Name == "<NODE_UI>"), -1).Parent).GetComponent<ProtoFluxNode>((Predicate<ProtoFluxNode>)null, false));
				}
			}
		}
		catch (Exception ex)
		{
			UniLog.Error("Encountered an ERROR in OnCutHook: " + ex.Message + "\n" + ex.StackTrace, false);
		}
	}

	public static void AddNodeToField(Slot proxySlot, ProtoFluxNode node)
	{
		ReferenceList<ProtoFluxNode> component = ((ContainerWorker<Component>)(object)proxySlot).GetComponent<ReferenceList<ProtoFluxNode>>((Predicate<ReferenceList<ProtoFluxNode>>)null, false);
		if (component == null)
		{
			return;
		}
		int num = ((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).FindIndex((Predicate<SyncRef<ProtoFluxNode>>)((SyncRef<ProtoFluxNode> n) => n?.Target == node));
		if (num == -1)
		{
			if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).Count == 0)
			{
				component.References.Add((ProtoFluxNode)null);
			}
			if (component.References[0] == null)
			{
				component.References[0] = node;
			}
			else
			{
				component.References.Add(node);
			}
		}
	}

	public static void RemoveNodeFromField(Slot proxySlot, ProtoFluxNode node)
	{
		ReferenceList<ProtoFluxNode> component = ((ContainerWorker<Component>)(object)proxySlot).GetComponent<ReferenceList<ProtoFluxNode>>((Predicate<ReferenceList<ProtoFluxNode>>)null, false);
		if (component == null)
		{
			return;
		}
		int num = ((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).FindIndex((Predicate<SyncRef<ProtoFluxNode>>)((SyncRef<ProtoFluxNode> n) => n?.Target == node));
		if (num != -1)
		{
			if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).Count == 0)
			{
				component.References.Add((ProtoFluxNode)null);
			}
			else if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).Count == 1)
			{
				component.References[0] = null;
			}
			else if (num == 0)
			{
				component.References[0] = ((IEnumerable<ProtoFluxNode>)component.References).Last();
				((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).RemoveAt(((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).Count - 1);
			}
			else
			{
				((SyncElementList<SyncRef<ProtoFluxNode>>)(object)component.References).RemoveAt(num);
			}
		}
	}
}

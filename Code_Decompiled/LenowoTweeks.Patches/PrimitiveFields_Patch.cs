using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class PrimitiveFields_Patch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(PrimitiveMemberEditor), "BuildUI")]
	public static void Postfix(PrimitiveMemberEditor __instance, SyncRef<TextEditor> ____textEditor, RelayRef<IField> ____target)
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		Slot slot = ((Component)____textEditor.Target).Slot;
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null)
		{
			if (LenowoTweeks.expandedStringInputs.Value && (((SyncRef<IField>)(object)____target).Target.ValueType == typeof(string) || ((SyncRef<IField>)(object)____target).Target.ValueType == typeof(Uri)))
			{
				IText target = ____textEditor.Target.Text.Target;
				Text val = (Text)(object)((target is Text) ? target : null);
				((SyncField<float>)(object)val.Size).Value = 16f;
				((SyncField<bool>)(object)val.HorizontalAutoSize).Value = false;
				((SyncField<bool>)(object)val.VerticalAutoSize).Value = false;
				((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)____textEditor.Target).Slot).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinWidth).Value = 96f;
				OverlappingLayout componentOrAttach = ((ContainerWorker<Component>)(object)slot).GetComponentOrAttach<OverlappingLayout>((Predicate<OverlappingLayout>)null);
				((SyncField<float>)(object)componentOrAttach.PaddingBottom).Value = 2f;
				((SyncField<float>)(object)componentOrAttach.PaddingTop).Value = 2f;
				((SyncField<float>)(object)componentOrAttach.PaddingLeft).Value = 6f;
				((SyncField<float>)(object)componentOrAttach.PaddingRight).Value = 6f;
				bool flag = true;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
				((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<OverlappingLayout>((Predicate<OverlappingLayout>)null);
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)1;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)((Component)slot.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false)).Slot).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)1;
				float2 value = ((SyncField<float2>)(object)slot.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false).Size).Value;
				((float2)(ref value)).SetY(0f);
				bool flag2 = true;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).HorizontalFit).Value = (SizeFit)2;
				((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<OverlappingLayout>((Predicate<OverlappingLayout>)null);
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).HorizontalFit).Value = (SizeFit)2;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).HorizontalFit).Value = (SizeFit)1;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)((Component)slot.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false)).Slot).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).HorizontalFit).Value = (SizeFit)1;
				value = ((SyncField<float2>)(object)slot.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false).Size).Value;
				((float2)(ref value)).SetX(0f);
			}
			return;
		}
		Slot parent = slot.Parent.Parent.Parent;
		try
		{
			if (LenowoTweeks.modifiedInspectorUIX.Value)
			{
				Type valueType = ((SyncRef<IField>)(object)____target).Target.ValueType;
				if (valueType == typeof(colorX) || ((ContainerWorker<Component>)(object)((Component)__instance).Slot.Parent.Parent).GetComponent<MemberEditor>((Predicate<MemberEditor>)null, false) != null || ((ContainerWorker<Component>)(object)((Component)__instance).Slot).GetComponent<NullableMemberEditor>((Predicate<NullableMemberEditor>)null, false) != null)
				{
					return;
				}
				if (ReflectionExtensions.IsMatrixType(valueType))
				{
					parent = parent.Parent;
				}
				RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
				((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
				((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
				((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<IField>)(object)____target).Target.ValueType);
				parent.FindChildInHierarchy("Left").Destroy();
				((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			}
		}
		catch
		{
		}
		if (LenowoTweeks.expandedStringInputs.Value && (((SyncRef<IField>)(object)____target).Target.ValueType == typeof(string) || ((SyncRef<IField>)(object)____target).Target.ValueType == typeof(Uri)))
		{
			IText target2 = ____textEditor.Target.Text.Target;
			Text val2 = (Text)(object)((target2 is Text) ? target2 : null);
			((SyncField<float>)(object)val2.Size).Value = 16f;
			((SyncField<bool>)(object)val2.HorizontalAutoSize).Value = false;
			((SyncField<bool>)(object)val2.VerticalAutoSize).Value = false;
			OverlappingLayout componentOrAttach2 = ((ContainerWorker<Component>)(object)slot).GetComponentOrAttach<OverlappingLayout>((Predicate<OverlappingLayout>)null);
			((SyncField<float>)(object)componentOrAttach2.PaddingBottom).Value = 2f;
			((SyncField<float>)(object)componentOrAttach2.PaddingTop).Value = 2f;
			((SyncField<float>)(object)componentOrAttach2.PaddingLeft).Value = 2f;
			((SyncField<float>)(object)componentOrAttach2.PaddingRight).Value = 2f;
			((SyncField<bool>)(object)((DirectionalLayout)((ContainerWorker<Component>)(object)parent).GetComponentOrAttach<HorizontalLayout>((Predicate<HorizontalLayout>)null)).ForceExpandWidth).Value = false;
			((SyncField<float>)(object)((DirectionalLayout)((ContainerWorker<Component>)(object)parent).GetComponentOrAttach<HorizontalLayout>((Predicate<HorizontalLayout>)null)).Spacing).Value = 4f;
			((SyncField<float>)(object)((ContainerWorker<Component>)(object)parent).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinHeight).Value = -1f;
			Slot val3 = parent.AddSlot("Hi if you're reading this, you must be very vere lost. Don't worry friend! the way out of this hiearchy is just above me!", true);
			((ContainerWorker<Component>)(object)val3).GetComponentOrAttach<VerticalLayout>((Predicate<VerticalLayout>)null);
			slot.Parent.Parent.Parent = val3;
			Slot val4 = parent.FindChild("Text");
			val4.Parent = val3;
			val4.OrderOffset = -1L;
			LayoutElement componentOrAttach3 = ((ContainerWorker<Component>)(object)val4).GetComponentOrAttach<LayoutElement>((Predicate<LayoutElement>)null);
			((SyncField<float>)(object)componentOrAttach3.MinWidth).Value = 40f;
			((SyncField<float>)(object)componentOrAttach3.FlexibleWidth).Value = 0f;
			((SyncField<float>)(object)componentOrAttach3.MinHeight).Value = 24f;
			((SyncField<float>)(object)((ContainerWorker<Component>)(object)slot).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinHeight).Value = 24f;
			((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
			((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
			((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<OverlappingLayout>((Predicate<OverlappingLayout>)null);
			((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)2;
			((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot.Parent.Parent.Parent).GetComponentOrAttach<ContentSizeFitter>((Predicate<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)1;
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(BooleanMemberEditor), "BuildUI")]
	public static void BooleanPostfix(BooleanMemberEditor __instance, SyncRef<Button> ____button)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot slot = ((SyncElement)____button).Slot;
		Slot parent = slot.Parent.Parent;
		parent.Name = "HaHa get booleaned nerd";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(typeof(bool));
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(QuaternionMemberEditor), "BuildUI")]
	public static void QuatragidyPostfix(QuaternionMemberEditor __instance)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent.Parent;
		parent.Name = "NINE ELEVEN";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(typeof(floatQ));
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(EnumMemberEditor), "BuildUI")]
	public static void EatemPostfix(EnumMemberEditor __instance, RelayRef<IField> ____target)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent.Parent;
		if (((ContainerWorker<Component>)(object)parent).GetComponent<MemberEditor>((Predicate<MemberEditor>)null, false) != null)
		{
			return;
		}
		parent.Name = "Eats you";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<IField>)(object)____target).Target.ValueType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(SliderMemberEditor), "BuildUI")]
	public static void SlidePostfix(SliderMemberEditor __instance)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent.Parent;
		parent.Name = "YOOOO THIS SICK ASS SLIDEEEEEEE";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(typeof(float));
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(RefEditor), "Setup", new Type[]
	{
		typeof(ISyncRef),
		typeof(UIBuilder)
	})]
	public static void RefPostfix(RefEditor __instance, RelayRef<ISyncRef> ____targetRef)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent;
		parent.Name = "ref deez nuts";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<ISyncRef>)(object)____targetRef).Target.TargetType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ColorMemberEditorBase), "BuildUI")]
	public static void ColorPostfix(ColorMemberEditorBase __instance, RelayRef<IField> ____target)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent.Parent;
		parent.Name = "FUCK YOU";
		try
		{
			Slot val = parent.FindChild("Button");
			RectTransform component = ((ContainerWorker<Component>)(object)val.FindChild("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<IField>)(object)____target).Target.ValueType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			val.FindChild("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(TextureRefEditor), "Setup")]
	public static void TextureRefPostfix(TextureRefEditor __instance, RelayRef<AssetRef<ITexture2D>> ____targetRef)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent;
		parent.Name = "picture frame idfk";
		try
		{
			RectTransform component = ((ContainerWorker<Component>)(object)parent.FindChildInHierarchy("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<IAssetProvider<ITexture2D>>)(object)((SyncRef<AssetRef<ITexture2D>>)(object)____targetRef).Target).TargetType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			parent.FindChildInHierarchy("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(NullableMemberEditor), "BuildUI")]
	public static void NullablePostfix(NullableMemberEditor __instance, RelayRef<IField> ____target)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		Slot editorSlot = ((Component)__instance).Slot;
		if (editorSlot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot Panel = editorSlot.Parent.Parent;
		Panel.Name = "<alpha=#88><i>null</closeall>";
		Type valueType = ((SyncRef<IField>)(object)____target).Target.ValueType;
		try
		{
			Slot val = Panel.FindChild("Button");
			RectTransform component = ((ContainerWorker<Component>)(object)val.FindChild("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((SyncRef<IField>)(object)____target).Target.ValueType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)Panel.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			val.FindChild("Left").Destroy();
		}
		catch
		{
		}
		if (!valueType.IsGenericType || !ReflectionExtensions.IsMatrixType(valueType.GenericTypeArguments.Last()))
		{
			return;
		}
		((Worker)__instance).StartTask((Func<Task>)async delegate
		{
			await new Updates(1);
			List<PrimitiveMemberEditor> editors = ((ContainerWorker<Component>)(object)editorSlot).GetComponents<PrimitiveMemberEditor>((Predicate<PrimitiveMemberEditor>)null, false);
			List<List<PrimitiveMemberEditor>> grouped = CollectionsExtensions.SplitToGroups<PrimitiveMemberEditor>((IEnumerable<PrimitiveMemberEditor>)editors, MathX.RoundToInt(MathX.Sqrt((float)editors.Count)));
			((SyncField<float>)(object)((ContainerWorker<Component>)(object)Panel).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinHeight).Value = 24 + 28 * grouped.Count;
			((ComponentBase<Component>)(object)((ContainerWorker<Component>)(object)editorSlot).GetComponent<HorizontalLayout>((Predicate<HorizontalLayout>)null, false)).Destroy();
			editorSlot.Name = "Vertical Layout";
			VerticalLayout verticalLayout = ((ContainerWorker<Component>)(object)editorSlot).AttachComponent<VerticalLayout>(true, (Action<VerticalLayout>)null);
			((SyncField<float>)(object)((DirectionalLayout)verticalLayout).Spacing).Value = 4f;
			grouped.ForEach(delegate(List<PrimitiveMemberEditor> editorRow)
			{
				Slot horizontalSlot = editorSlot.AddSlot("Horizontal Layout", true);
				((SyncField<float>)(object)((DirectionalLayout)((ContainerWorker<Component>)(object)horizontalSlot).AttachComponent<HorizontalLayout>(true, (Action<HorizontalLayout>)null)).Spacing).Value = 4f;
				((SyncField<float>)(object)((ContainerWorker<Component>)(object)horizontalSlot).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = 24f;
				List<PrimitiveMemberEditor> list = editorRow.ToList();
				list.ForEach(delegate(PrimitiveMemberEditor e)
				{
					Traverse traverse = Traverse.Create(e);
					Button target = traverse.Field<SyncRef<Button>>("_button").Value.Target;
					((Component)target).Slot.Parent = horizontalSlot;
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)target).Slot).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinHeight).Value = 24f;
				});
			});
			((IEnumerable<Slot>)(object)editorSlot.Children).Where((Slot s) => s.Name == "Text").ToList().ForEach(delegate(Slot s)
			{
				s.Destroy();
			});
		});
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(SyncPlaybackEditor), "Setup")]
	public static void PlaybackPostfix(SyncPlaybackEditor __instance)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent;
		parent.Name = "p";
		try
		{
			Slot val = parent.FindChild("Button");
			RectTransform component = ((ContainerWorker<Component>)(object)val.FindChild("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(typeof(SyncPlayback));
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			val.FindChild("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(DelegateEditor), "Setup")]
	public static void DelegatePostfix(DelegateEditor __instance, RelayRef<ISyncDelegate> ____targetDelegate)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.modifiedInspectorUIX.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<ProtoFluxNodeVisual>((Predicate<ProtoFluxNodeVisual>)null, true, false) != null || ((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return;
		}
		Slot parent = ((Component)__instance).Slot.Parent;
		parent.Name = "deli sausage";
		try
		{
			Slot val = parent.FindChild("Button");
			RectTransform component = ((ContainerWorker<Component>)(object)val.FindChild("Right")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false);
			((SyncField<float2>)(object)component.AnchorMin).Value = new float2(0.2f, 0f);
			((SyncField<float2>)(object)component.AnchorMax).Value = new float2(0.8f, 1f);
			((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)component).Slot.Parent).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = DatatypeColorHelper.GetTypeColor(((ISyncRef)((SyncRef<ISyncDelegate>)(object)____targetDelegate).Target).TargetType);
			((SyncField<float2>)(object)((ContainerWorker<Component>)(object)parent.FindChild("Text")).GetComponent<RectTransform>((Predicate<RectTransform>)null, false).AnchorMin).Value = new float2(0.01f, 0f);
			val.FindChild("Left").Destroy();
		}
		catch
		{
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ListEditor), "Setup")]
	public static void ListCollapseing(ListEditor __instance, SyncRef<ISyncList> ____targetList)
	{
		if (!LenowoTweeks.listCollapsing.Value)
		{
			return;
		}
		if (WorldExtensions.IsUserspace(((Worker)__instance).World))
		{
			DynamicVariableSpace componentInParents = ((Component)__instance).Slot.GetComponentInParents<DynamicVariableSpace>((Predicate<DynamicVariableSpace>)null, true, false);
			if (componentInParents != null && ((SyncField<string>)(object)componentInParents.SpaceName).Value == "Config")
			{
				return;
			}
		}
		if (((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) != null)
		{
			Slot parent = ((Component)__instance).Slot.Parent;
			Slot val = parent.FindChild("Text");
			ButtonToggle val2 = ((ContainerWorker<Component>)(object)val).AttachComponent<ButtonToggle>(true, (Action<ButtonToggle>)null);
			BooleanValueDriver<string> val3 = ((ContainerWorker<Component>)(object)val).AttachComponent<BooleanValueDriver<string>>(true, (Action<BooleanValueDriver<string>>)null);
			Text component = ((ContainerWorker<Component>)(object)val).GetComponent<Text>((Predicate<Text>)null, false);
			Slot val4 = parent.FindChild("Vertical Layout");
			ValueCopy<bool> val5 = ((ContainerWorker<Component>)(object)val4).AttachComponent<ValueCopy<bool>>(true, (Action<ValueCopy<bool>>)null);
			ValueCopy<bool> val6 = ((ContainerWorker<Component>)(object)parent.FindChild("Button")).AttachComponent<ValueCopy<bool>>(true, (Action<ValueCopy<bool>>)null);
			string text = Sync<string>.op_Implicit(component.Content);
			((SyncRef<IField<bool>>)(object)val5.Source).Target = (IField<bool>)(object)val4.ActiveSelf_Field;
			((SyncRef<IField<bool>>)(object)val5.Target).Target = (IField<bool>)(object)val3.State;
			((SyncRef<IField<bool>>)(object)val6.Source).Target = (IField<bool>)(object)val4.ActiveSelf_Field;
			((SyncRef<IField<bool>>)(object)val6.Target).Target = (IField<bool>)(object)((Component)val6).Slot.ActiveSelf_Field;
			val2.TargetValue.Target = (IField<bool>)(object)val4.ActiveSelf_Field;
			((SyncRef<IField<string>>)(object)val3.TargetField).Target = (IField<string>)(object)component.Content;
			((SyncField<string>)(object)val3.FalseValue).Value = text + " (↑↑↑)";
			((SyncField<string>)(object)val3.TrueValue).Value = text + " (↓↓↓)";
			((SyncField<bool>)(object)component.ParseRichText).Value = true;
			int count = ____targetList.Target.Count;
			int value = LenowoTweeks.maxListElementsForAutoCollapse.Value;
			val4.ActiveSelf = value == -1 || count <= value;
			((SyncField<bool>)(object)val4.ActiveSelf_Field).OnValueChange += delegate
			{
				MethodInfo method = typeof(ListEditor).GetMethod("OnChanges", BindingFlags.Instance | BindingFlags.NonPublic);
				object[] parameters = new object[0];
				method.Invoke(__instance, parameters);
			};
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(ListEditor), "OnChanges")]
	public static bool ListNoLoady(ListEditor __instance)
	{
		Slot parent = ((Component)__instance).Slot.Parent;
		if (!parent.FindChild("Vertical Layout").ActiveSelf)
		{
			return false;
		}
		if (((Component)__instance).Slot.GetComponentInParents<WorkerInspector>((Predicate<WorkerInspector>)null, true, false) == null)
		{
			return true;
		}
		return true;
	}
}

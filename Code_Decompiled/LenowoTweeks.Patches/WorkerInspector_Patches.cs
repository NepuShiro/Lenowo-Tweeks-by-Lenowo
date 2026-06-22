using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Assets;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using FrooxEngine.Undo;
using HarmonyLib;
using Renderite.Shared;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class WorkerInspector_Patches
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(DevCreateNewForm), "BuildBlankUI")]
	public static bool BuildBlankUI(UIBuilder ui)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		((SyncField<float2>)(object)ui.Canvas.Size).Value = new float2(1920f, 1080f);
		colorX value = LenowoTweeks.defaultUIXPanelColor.Value;
		ui.Panel(ref value, true);
		return false;
	}

	public static colorX GetContrastingColor(colorX input)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		ColorHSV val = new ColorHSV(ref input);
		return (((ColorHSV)(ref val)).V > 0.5f) ? new colorX(0f, 0f, 0f, 1f, (ColorProfile)1) : new colorX(1f, 1f, 1f, 1f, (ColorProfile)1);
	}

	public static void SetUIColor(UIBuilder ui, colorX color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		ui.Style.TextColor = GetContrastingColor(color);
		ui.Style.ButtonColor = color;
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(SceneInspector), "OnAddChildPressed")]
	public static bool TweekedOnAddChildPressed(SceneInspector __instance, ButtonEventData eventData)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		SyncRef<Slot> componentView = __instance.ComponentView;
		if (componentView.Target != null && !LenowoTweeks.enableAddChildrenBuilder.Value)
		{
			SpawnOrDestroyExtensions.CreateSpawnUndoPoint(componentView.Target.AddSlot(componentView.Target.Name + " - Child", true), (string)null, (Action<Worker>)null);
		}
		else if (componentView.Target != null && LenowoTweeks.enableAddChildrenBuilder.Value)
		{
			Slot val = ((Worker)__instance).LocalUserSpace.AddSlot("Add Child Dialog - " + ((Worker)__instance).LocalUser.UserName, false);
			float3 localScale = val.LocalScale;
			val.LocalScale = (ref localScale) * 0.0008f;
			ref float3 globalPoint = ref eventData.globalPoint;
			localScale = ((Component)__instance).Slot.Backward;
			float3 val2 = (ref localScale) * 0.05f;
			float3 val3 = (ref globalPoint) + (ref val2);
			float3 down = ((Component)__instance).Slot.Down;
			float3 val4 = ((Component)__instance).Slot.GlobalScale;
			val4 = (ref down) * (250f * ((float3)(ref val4)).Y);
			val.GlobalPosition = (ref val3) + (ref val4);
			val.GlobalRotation = ((Component)__instance).Slot.GlobalRotation;
			UIBuilder val5 = RadiantUI_Panel.SetupPanel(val, LocaleString.op_Implicit("Add Child"), new float2(500f, 500f), true, true);
			RadiantUI_Constants.SetupEditorStyle(val5, false);
			SetUIColor(val5, LenowoTweeks.secondaryUIColor.Value);
			Slot slot = ((Component)val5.VerticalLayout(5f, 10f, (Alignment?)(Alignment)1, (bool?)true, (bool?)false)).Slot;
			((ContainerWorker<Component>)(object)slot.Parent).AttachComponent<Mask>(true, (Action<Mask>)null);
			((ContainerWorker<Component>)(object)slot).AttachComponent<ScrollRect>(true, (Action<ScrollRect>)null);
			((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)slot).AttachComponent<ContentSizeFitter>(true, (Action<ContentSizeFitter>)null).VerticalFit).Value = (SizeFit)1;
			val5.Style.MinHeight = 50f;
			LoadMainPage(val5, slot, componentView, val);
		}
		return false;
	}

	public static void LoadMainPage(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		SetUIColor(ui, LenowoTweeks.primaryUIColor.Value);
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Add Child");
		Button val2 = obj.Button(ref val);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			SpawnOrDestroyExtensions.CreateSpawnUndoPoint(ComponentView.Target.AddSlot(ComponentView.Target.Name + " - Child", true), (string)null, (Action<Worker>)null);
			PanelRoot.Destroy();
		};
		if (ComponentView.Target.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false) == null)
		{
			UIBuilder obj2 = ui;
			val = LocaleString.op_Implicit("Add Canvas");
			Button val3 = obj2.Button(ref val);
			val3.LocalPressed += (ButtonEventHandler)delegate
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				Slot val11 = ComponentView.Target.AddSlot("Canvas", true);
				val11.LocalScale = new float3(0.0008f, 0.0008f, 0.0008f);
				SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
				((ContainerWorker<Component>)(object)val11).AttachComponent<Canvas>(true, (Action<Canvas>)null);
				Image val12 = ((ContainerWorker<Component>)(object)val11.AddSlot("Image", true)).AttachComponent<Image>(true, (Action<Image>)null);
				((SyncRef<IAssetProvider<Material>>)(object)((ImageBase)val12).Material).Target = (IAssetProvider<Material>)(object)((ContainerWorker<Component>)(object)((Worker)PanelRoot).World.RootSlot).GetComponent<UI_UnlitMaterial>((Predicate<UI_UnlitMaterial>)null, false);
				((SyncField<colorX>)(object)val12.Tint).Value = LenowoTweeks.defaultUIXPanelColor.Value;
				PanelRoot.Destroy();
				ComponentView.Target = val11;
			};
		}
		if (ComponentView.Target.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false) == null)
		{
			if (((ContainerWorker<Component>)(object)ComponentView.Target).GetComponent<RootContextMenuItem>((Predicate<RootContextMenuItem>)null, false) == null)
			{
				UIBuilder obj3 = ui;
				val = LocaleString.op_Implicit("Add Root Context Menu Item");
				Button val4 = obj3.Button(ref val);
				val4.LocalPressed += (ButtonEventHandler)delegate
				{
					Slot val11 = ComponentView.Target.AddSlot("Root Context Menu Item", true);
					SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
					RootContextMenuItem val12 = ((ContainerWorker<Component>)(object)val11).AttachComponent<RootContextMenuItem>(true, (Action<RootContextMenuItem>)null);
					ContextMenuItemSource val13 = ((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuItemSource>(true, (Action<ContextMenuItemSource>)null);
					val13.LabelText = val11.Name;
					val12.Item.Target = val13;
					PanelRoot.Destroy();
					ComponentView.Target = val11;
				};
			}
			UIBuilder obj4 = ui;
			val = LocaleString.op_Implicit("Add Context Menu Item");
			Button val5 = obj4.Button(ref val);
			val5.LocalPressed += (ButtonEventHandler)delegate
			{
				Slot val11 = ComponentView.Target.AddSlot("Context Menu Item", true);
				SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
				ContextMenuItemSource val12 = ((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuItemSource>(true, (Action<ContextMenuItemSource>)null);
				val12.LabelText = val11.Name;
				PanelRoot.Destroy();
				ComponentView.Target = val11;
			};
			UIBuilder obj5 = ui;
			val = LocaleString.op_Implicit("Add Context Sub Menu");
			Button val6 = obj5.Button(ref val);
			val6.LocalPressed += (ButtonEventHandler)delegate
			{
				Slot val11 = ComponentView.Target.AddSlot("Context Sub Menu", true);
				SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
				ContextMenuItemSource val12 = ((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuItemSource>(true, (Action<ContextMenuItemSource>)null);
				val12.LabelText = val11.Name;
				((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuSubmenu>(true, (Action<ContextMenuSubmenu>)null).ItemsRoot.Target = val11;
				if (((ContainerWorker<Component>)(object)ComponentView.Target).GetComponent<ContextMenuItemSource>((Predicate<ContextMenuItemSource>)null, false) == null)
				{
					((ContainerWorker<Component>)(object)val11).AttachComponent<RootContextMenuItem>(true, (Action<RootContextMenuItem>)null).Item.Target = val12;
				}
				PanelRoot.Destroy();
				ComponentView.Target = val11;
			};
			if (((ContainerWorker<Component>)(object)ComponentView.Target).GetComponent<ContextMenuItemSource>((Predicate<ContextMenuItemSource>)null, false) != null)
			{
				UIBuilder obj6 = ui;
				val = LocaleString.op_Implicit("Add Context Back Button");
				Button val7 = obj6.Button(ref val);
				val7.LocalPressed += (ButtonEventHandler)delegate
				{
					//IL_0052: Unknown result type (might be due to invalid IL or missing references)
					Slot val11 = ComponentView.Target.AddSlot("Back", true);
					SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
					ContextMenuItemSource val12 = ((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuItemSource>(true, (Action<ContextMenuItemSource>)null);
					val12.LabelText = val11.Name;
					((SyncField<colorX>)(object)val12.Color).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
					((ContainerWorker<Component>)(object)val11).AttachComponent<ContextMenuSubmenu>(true, (Action<ContextMenuSubmenu>)null).ItemsRoot.Target = ComponentView.Target.Parent;
					PanelRoot.Destroy();
					ComponentView.Target = val11;
				};
			}
		}
		if (ComponentView.Target.GetComponentInParents<Canvas>((Predicate<Canvas>)null, true, false) != null)
		{
			UIBuilder obj7 = ui;
			val = LocaleString.op_Implicit("Add Empty UIX Slot");
			Button val8 = obj7.Button(ref val);
			val8.LocalPressed += (ButtonEventHandler)delegate
			{
				Slot val11 = ComponentView.Target.AddSlot("Panel", true);
				SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
				((ContainerWorker<Component>)(object)val11).AttachComponent<RectTransform>(true, (Action<RectTransform>)null);
				PanelRoot.Destroy();
				ComponentView.Target = val11;
			};
			UIBuilder obj8 = ui;
			val = LocaleString.op_Implicit("Add Image");
			Button val9 = obj8.Button(ref val);
			val9.LocalPressed += (ButtonEventHandler)delegate
			{
				Slot val11 = ComponentView.Target.AddSlot("Image", true);
				SpawnOrDestroyExtensions.CreateSpawnUndoPoint(val11, (string)null, (Action<Worker>)null);
				((ContainerWorker<Component>)(object)val11).AttachComponent<Image>(true, (Action<Image>)null);
				PanelRoot.Destroy();
				ComponentView.Target = val11;
			};
			SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
			UIBuilder obj9 = ui;
			val = LocaleString.op_Implicit("UIX Builder");
			Button val10 = obj9.Button(ref val);
			val10.LocalPressed += (ButtonEventHandler)delegate
			{
				LoadUIXBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
			};
		}
		ui.NestOut();
	}

	public static void LoadUIXBuilder(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		ui.NestInto(UIVerticalLayout);
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		ui.Style.TextColor = colorX.Black;
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Back");
		Button val2 = obj.Button(ref val);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)val2).Slot).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadMainPage(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		SetUIColor(ui, LenowoTweeks.primaryUIColor.Value);
		UIBuilder obj2 = ui;
		val = LocaleString.op_Implicit("Layout Builder");
		Button val3 = obj2.Button(ref val);
		val3.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadLayoutBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		UIBuilder obj3 = ui;
		val = LocaleString.op_Implicit("Field Builder");
		Button val4 = obj3.Button(ref val);
		val4.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadFieldBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		UIBuilder obj4 = ui;
		val = LocaleString.op_Implicit("Components");
		Button val5 = obj4.Button(ref val);
		val5.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadComponentAdder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
		UIBuilder obj5 = ui;
		val = LocaleString.op_Implicit("Scroll Area");
		Button val6 = obj5.Button(ref val);
		val6.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Scroll Rect");
		};
		UIBuilder obj6 = ui;
		val = LocaleString.op_Implicit("Button");
		Button val7 = obj6.Button(ref val);
		val7.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Button");
		};
		UIBuilder obj7 = ui;
		val = LocaleString.op_Implicit("Text");
		Button val8 = obj7.Button(ref val);
		val8.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Text");
		};
		UIBuilder obj8 = ui;
		val = LocaleString.op_Implicit("Mask");
		Button val9 = obj8.Button(ref val);
		val9.LocalPressed += (ButtonEventHandler)delegate
		{
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Mask");
		};
		ui.NestOut();
	}

	public static void LoadComponentAdder(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		ui.NestInto(UIVerticalLayout);
		ui.Style.TextColor = colorX.Black;
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Back");
		Button val2 = obj.Button(ref val);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)val2).Slot).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
		SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadUIXBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		UIBuilder obj2 = ui;
		val = LocaleString.op_Implicit("Layout Element");
		Button val3 = obj2.Button(ref val);
		val3.LocalPressed += (ButtonEventHandler)delegate
		{
			AddFeature(ui, UIVerticalLayout, ComponentView, PanelRoot, "Layout Element");
		};
		UIBuilder obj3 = ui;
		val = LocaleString.op_Implicit("Sprite Prvider");
		Button val4 = obj3.Button(ref val);
		val4.LocalPressed += (ButtonEventHandler)delegate
		{
			AddFeature(ui, UIVerticalLayout, ComponentView, PanelRoot, "Sprite Provider");
		};
		UIBuilder obj4 = ui;
		val = LocaleString.op_Implicit("Gradient Image");
		Button val5 = obj4.Button(ref val);
		val5.LocalPressed += (ButtonEventHandler)delegate
		{
			AddFeature(ui, UIVerticalLayout, ComponentView, PanelRoot, "Gradient Image");
		};
	}

	public static void LoadLayoutBuilder(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		ui.NestInto(UIVerticalLayout);
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Back");
		Button val2 = obj.Button(ref val);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)val2).Slot).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadUIXBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
		UIBuilder obj2 = ui;
		val = LocaleString.op_Implicit("Vertical Layout");
		Button val3 = obj2.Button(ref val);
		val3.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Vertical Layout");
		};
		UIBuilder obj3 = ui;
		val = LocaleString.op_Implicit("Horizontal Layout");
		Button val4 = obj3.Button(ref val);
		val4.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Horizontal Layout");
		};
		UIBuilder obj4 = ui;
		val = LocaleString.op_Implicit("Overlapping Layout");
		Button val5 = obj4.Button(ref val);
		val5.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Overlapping Layout");
		};
	}

	public static void LoadFieldBuilder(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		ui.NestInto(UIVerticalLayout);
		ui.Style.TextColor = colorX.Black;
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Back");
		Button val2 = obj.Button(ref val);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)val2).Slot).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadUIXBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
		};
		SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
		UIBuilder obj2 = ui;
		val = LocaleString.op_Implicit("Text Field");
		Button val3 = obj2.Button(ref val);
		val3.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "String Field");
		};
		UIBuilder obj3 = ui;
		val = LocaleString.op_Implicit("Bool Field");
		Button val4 = obj3.Button(ref val);
		val4.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Bool Field");
		};
		UIBuilder obj4 = ui;
		val = LocaleString.op_Implicit("Float Field");
		Button val5 = obj4.Button(ref val);
		val5.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Float Field");
		};
		UIBuilder obj5 = ui;
		val = LocaleString.op_Implicit("Slider");
		Button val6 = obj5.Button(ref val);
		val6.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Slider");
		};
		UIBuilder obj6 = ui;
		val = LocaleString.op_Implicit("Reference Field");
		Button val7 = obj6.Button(ref val);
		val7.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			LoadBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot, "Reference Field");
		};
	}

	public static void LoadBuilder(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot, string builderType)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		UIVerticalLayout.DestroyChildren(false, true, false, (Predicate<Slot>)null);
		ui.NestInto(UIVerticalLayout);
		ui.Style.TextColor = colorX.Black;
		UIBuilder obj = ui;
		LocaleString val = LocaleString.op_Implicit("Back");
		Button val2 = obj.Button(ref val);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)val2).Slot).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 0f, 0f, 1f, (ColorProfile)1);
		val2.LocalPressed += (ButtonEventHandler)delegate
		{
			ui.NestInto(UIVerticalLayout);
			if (builderType == "Vertical Layout" || builderType == "Horizontal Layout" || builderType == "Overlapping Layout")
			{
				LoadLayoutBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
			}
			else if (builderType == "Button" || builderType == "Text" || builderType == "Mask" || builderType == "Scroll Rect")
			{
				LoadUIXBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
			}
			else if (builderType == "String Field" || builderType == "Bool Field" || builderType == "Float Field" || builderType == "Slider" || builderType == "Reference Field")
			{
				LoadFieldBuilder(ui, UIVerticalLayout, ComponentView, PanelRoot);
			}
		};
		CreateHeader(ui, builderType);
		if (builderType == "Vertical Layout" || builderType == "Horizontal Layout" || builderType == "Overlapping Layout")
		{
			CreateFloatFieldWithText(ui, "Padding", 1f);
			ui.NestInto(UIVerticalLayout);
			if (builderType != "Overlapping Layout")
			{
				CreateFloatFieldWithText(ui, "Spacing", 5f);
				ui.NestInto(UIVerticalLayout);
			}
			CreateBoolFieldWithText(ui, "Force Expand Width", defaultVal: true);
			ui.NestInto(UIVerticalLayout);
			CreateBoolFieldWithText(ui, "Force Expand Height", defaultVal: true);
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<LayoutVerticalAlignment>(ui, "Vertical Alignment", (LayoutVerticalAlignment)1);
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<LayoutHorizontalAlignment>(ui, "Horizontal Alignment", (LayoutHorizontalAlignment)1);
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
		else if (builderType == "Scroll Rect")
		{
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<LayoutVerticalAlignment>(ui, "Vertical Alignment", (LayoutVerticalAlignment)0);
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<LayoutHorizontalAlignment>(ui, "Horizontal Alignment", (LayoutHorizontalAlignment)0);
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<SizeFit>(ui, "Horizontal Fit", (SizeFit)0);
			ui.NestInto(UIVerticalLayout);
			CreateEnumFieldWithText<SizeFit>(ui, "Vertical Fit", (SizeFit)1);
			ui.NestInto(UIVerticalLayout);
			CreateFloatFieldWithText(ui, "Padding", 1f);
			ui.NestInto(UIVerticalLayout);
			CreateFloatFieldWithText(ui, "Spacing", 5f);
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
		else if (builderType == "Button")
		{
			CreateFloatFieldWithText(ui, "Min Height", LenowoTweeks.buttonMinHeightDefault.Value);
			ui.NestInto(UIVerticalLayout);
			CreateFloatFieldWithText(ui, "Min Width", -1f);
			ui.NestInto(UIVerticalLayout);
			CreateStringFieldWithText(ui, "Text", "Button");
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
		else if (builderType == "String Field" || builderType == "Bool Field" || builderType == "Float Field" || builderType == "Slider" || builderType == "Reference Field")
		{
			CreateFloatFieldWithText(ui, "Min Height", LenowoTweeks.buttonMinHeightDefault.Value);
			ui.NestInto(UIVerticalLayout);
			if (builderType == "Slider")
			{
				ui.NestInto(UIVerticalLayout);
				CreateFloatFieldWithText(ui, "Min", 0f);
				ui.NestInto(UIVerticalLayout);
				CreateFloatFieldWithText(ui, "Max", 5f);
			}
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
		else if (builderType == "Text")
		{
			CreateFloatFieldWithText(ui, "Min Height", LenowoTweeks.buttonMinHeightDefault.Value);
			ui.NestInto(UIVerticalLayout);
			CreateFloatFieldWithText(ui, "Min Width", -1f);
			ui.NestInto(UIVerticalLayout);
			CreateStringFieldWithText(ui, "Text", "Text");
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
		else if (builderType == "Mask")
		{
			CreateBoolFieldWithText(ui, "Show Mash Graphic", defaultVal: false);
			ui.NestInto(UIVerticalLayout);
			CreateBuildButton(ui, UIVerticalLayout, ComponentView, PanelRoot, builderType);
		}
	}

	public static void CreateFloatFieldWithText(UIBuilder ui, string name, float defaultVal)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		HorizontalLayout val = ui.HorizontalLayout(5f, 5f, (Alignment?)null);
		LocaleString val2 = LocaleString.op_Implicit(name);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		FloatTextEditorParser val3 = ui.FloatField(float.MinValue, float.MaxValue, 2, (string)null, true);
		((SyncField<float>)(object)((TextEditorParser<float>)(object)val3).ParsedValue).Value = defaultVal;
		Image val4 = ((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<Image>(true, (Action<Image>)null);
		((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)val4).Sprite).Target = ui.Style.ButtonSprite;
		((SyncField<NineSliceSizing>)(object)((ImageBase)val4).NineSliceSizing).Value = (NineSliceSizing)0;
		((Component)val).Slot.Name = name;
	}

	public static void CreateStringFieldWithText(UIBuilder ui, string name, string defaultVal)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		HorizontalLayout val = ui.HorizontalLayout(5f, 5f, (Alignment?)null);
		LocaleString val2 = LocaleString.op_Implicit(name);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		ValueField<string> val3 = ((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<ValueField<string>>(true, (Action<ValueField<string>>)null);
		((SyncField<string>)(object)val3.Value).Value = defaultVal;
		Text component = ((ContainerWorker<Component>)(object)((Component)ui.TextField(Sync<string>.op_Implicit(val3.Value), false, (string)null, true, default(LocaleString))).Slot.FindChild("Text")).GetComponent<Text>((Predicate<Text>)null, false);
		ValueCopy<string> val4 = ((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<ValueCopy<string>>(true, (Action<ValueCopy<string>>)null);
		((SyncRef<IField<string>>)(object)val4.Target).Target = (IField<string>)(object)val3.Value;
		((SyncRef<IField<string>>)(object)val4.Source).Target = (IField<string>)(object)component.Content;
		((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<Image>(true, (Action<Image>)null)).Sprite).Target = ui.Style.ButtonSprite;
		((SyncField<NineSliceSizing>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).GetComponent<Image>((Predicate<Image>)null, false)).NineSliceSizing).Value = (NineSliceSizing)0;
		((Component)val).Slot.Name = name;
	}

	public static void CreateEnumFieldWithText<T>(UIBuilder ui, string name, T defaultVal)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		HorizontalLayout val = ui.HorizontalLayout(0f, 5f, (Alignment?)null);
		LocaleString val2 = LocaleString.op_Implicit(name);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		Sync<T> value = ((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<ValueField<T>>(true, (Action<ValueField<T>>)null).Value;
		((SyncField<T>)(object)value).Value = defaultVal;
		EnumMemberEditor val3 = UIBuilderEditors.EnumMemberEditor(ui, (IField)(object)value, (string)null);
		((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)val3).Slot.FindChild("Horizontal Layout")).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinWidth).Value = 200f;
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((IEnumerable<Slot>)(object)((Component)val3).Slot.FindChild("Horizontal Layout").Children).ToList()[1].FindChild("Text")).GetComponent<Text>((Predicate<Text>)null, false).Color).Value = new colorX(1f, 1f, 1f, 1f, (ColorProfile)1);
		((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<Image>(true, (Action<Image>)null)).Sprite).Target = ui.Style.ButtonSprite;
		((SyncField<NineSliceSizing>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).GetComponent<Image>((Predicate<Image>)null, false)).NineSliceSizing).Value = (NineSliceSizing)0;
		((Component)val).Slot.Name = name;
	}

	public static void CreateBoolFieldWithText(UIBuilder ui, string name, bool defaultVal)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		HorizontalLayout val = ui.HorizontalLayout(0f, 5f, (Alignment?)null);
		LocaleString val2 = LocaleString.op_Implicit(name);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		ValueField<bool> val3 = ((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<ValueField<bool>>(true, (Action<ValueField<bool>>)null);
		((SyncField<bool>)(object)val3.Value).Value = defaultVal;
		ui.NestInto(((Component)val).Slot);
		((SyncField<colorX>)(object)((ContainerWorker<Component>)(object)((Component)UIBuilderEditors.BooleanMemberEditor(ui, (IField)(object)val3.Value, (string)null)).Slot.FindChild("Panel").FindChild("Image").FindChild("Image")).GetComponent<Image>((Predicate<Image>)null, false).Tint).Value = new colorX(1f, 1f, 1f, 1f, (ColorProfile)1);
		((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).AttachComponent<Image>(true, (Action<Image>)null)).Sprite).Target = ui.Style.ButtonSprite;
		((SyncField<NineSliceSizing>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val).Slot).GetComponent<Image>((Predicate<Image>)null, false)).NineSliceSizing).Value = (NineSliceSizing)0;
		((Component)val).Slot.Name = name;
	}

	public static void CreateHeader(UIBuilder ui, string text)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Image val = ui.Image();
		((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)val).Sprite).Target = ui.Style.ButtonSprite;
		((SyncField<NineSliceSizing>)(object)((ImageBase)val).NineSliceSizing).Value = (NineSliceSizing)0;
		LocaleString val2 = LocaleString.op_Implicit(text);
		Text val3 = ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		((Component)val3).Slot.Parent = ((Component)val).Slot;
		((SyncField<colorX>)(object)val3.Color).Value = new colorX(0f, 0f, 0f, 1f, (ColorProfile)1);
	}

	public static void AddFeature(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot, string builderType)
	{
		Slot target = ComponentView.Target;
		switch (builderType)
		{
		case "Layout Element":
			((ContainerWorker<Component>)(object)target).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null);
			break;
		case "Sprite Provider":
			((ContainerWorker<Component>)(object)target).AttachComponent<SpriteProvider>(true, (Action<SpriteProvider>)null);
			break;
		case "Gradient Image":
			((ContainerWorker<Component>)(object)target).AttachComponent<GradientImage>(true, (Action<GradientImage>)null);
			break;
		}
		PanelRoot.Destroy();
	}

	public static void CreateBuildButton(UIBuilder ui, Slot UIVerticalLayout, SyncRef<Slot> ComponentView, Slot PanelRoot, string builderType)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		Button val = ui.Button();
		UIBuilder obj = ui;
		LocaleString val2 = LocaleString.op_Implicit("Build!!");
		((Component)obj.Text(ref val2, true, (Alignment?)null, true, (string)null)).Slot.Parent = ((Component)val).Slot;
		val.LocalPressed += (ButtonEventHandler)delegate
		{
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0757: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Expected O, but got Unknown
			//IL_0769: Unknown result type (might be due to invalid IL or missing references)
			//IL_0864: Unknown result type (might be due to invalid IL or missing references)
			//IL_086a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
			Slot val3 = ComponentView.Target.AddSlot(builderType, true);
			if (builderType == "Vertical Layout" || builderType == "Horizontal Layout")
			{
				Component val4 = ((ContainerWorker<Component>)(object)val3).AttachComponent((builderType == "Horizontal Layout") ? typeof(HorizontalLayout) : typeof(VerticalLayout), true, (Action<Component>)null);
				DirectionalLayout val5 = (DirectionalLayout)val4;
				val5.SetPadding(((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Padding").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value);
				((SyncField<float>)(object)val5.Spacing).Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Spacing").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				((SyncField<bool>)(object)val5.ForceExpandWidth).Value = Sync<bool>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Force Expand Width")).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)null, false).Value);
				((SyncField<bool>)(object)val5.ForceExpandHeight).Value = Sync<bool>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Force Expand Height")).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)null, false).Value);
				((SyncField<LayoutVerticalAlignment>)(object)val5.VerticalAlign).Value = Sync<LayoutVerticalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Vertical Alignment")).GetComponent<ValueField<LayoutVerticalAlignment>>((Predicate<ValueField<LayoutVerticalAlignment>>)null, false).Value);
				((SyncField<LayoutHorizontalAlignment>)(object)val5.HorizontalAlign).Value = Sync<LayoutHorizontalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Horizontal Alignment")).GetComponent<ValueField<LayoutHorizontalAlignment>>((Predicate<ValueField<LayoutHorizontalAlignment>>)null, false).Value);
				ComponentView.Target = ((Component)val5).Slot;
			}
			else if (builderType == "Overlapping Layout")
			{
				OverlappingLayout val6 = ((ContainerWorker<Component>)(object)val3).AttachComponent<OverlappingLayout>(true, (Action<OverlappingLayout>)null);
				float value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Padding").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				((SyncField<float>)(object)val6.PaddingBottom).Value = value;
				((SyncField<float>)(object)val6.PaddingTop).Value = value;
				((SyncField<float>)(object)val6.PaddingLeft).Value = value;
				((SyncField<float>)(object)val6.PaddingRight).Value = value;
				((SyncField<bool>)(object)val6.ForceExpandWidth).Value = Sync<bool>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Force Expand Width")).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)null, false).Value);
				((SyncField<bool>)(object)val6.ForceExpandHeight).Value = Sync<bool>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Force Expand Height")).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)null, false).Value);
				((SyncField<LayoutVerticalAlignment>)(object)val6.VerticalAlign).Value = Sync<LayoutVerticalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Vertical Alignment")).GetComponent<ValueField<LayoutVerticalAlignment>>((Predicate<ValueField<LayoutVerticalAlignment>>)null, false).Value);
				((SyncField<LayoutHorizontalAlignment>)(object)val6.HorizontalAlign).Value = Sync<LayoutHorizontalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Horizontal Alignment")).GetComponent<ValueField<LayoutHorizontalAlignment>>((Predicate<ValueField<LayoutHorizontalAlignment>>)null, false).Value);
				ComponentView.Target = ((Component)val6).Slot;
			}
			else if (builderType == "Scroll Rect")
			{
				((ContainerWorker<Component>)(object)val3).AttachComponent<Mask>(true, (Action<Mask>)null);
				Slot val7 = val3.AddSlot("Content", true);
				((ContainerWorker<Component>)(object)val7).AttachComponent<ScrollRect>(true, (Action<ScrollRect>)null);
				((ContainerWorker<Component>)(object)val7).AttachComponent<ContentSizeFitter>(true, (Action<ContentSizeFitter>)null);
				VerticalLayout val8 = ((ContainerWorker<Component>)(object)val7).AttachComponent<VerticalLayout>(true, (Action<VerticalLayout>)null);
				((DirectionalLayout)val8).SetPadding(((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Padding").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value);
				((SyncField<float>)(object)((DirectionalLayout)val8).Spacing).Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Spacing").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				((SyncField<bool>)(object)((DirectionalLayout)val8).ForceExpandWidth).Value = true;
				((SyncField<bool>)(object)((DirectionalLayout)val8).ForceExpandHeight).Value = false;
				((SyncField<LayoutHorizontalAlignment>)(object)((DirectionalLayout)val8).HorizontalAlign).Value = (LayoutHorizontalAlignment)1;
				((SyncField<LayoutVerticalAlignment>)(object)((DirectionalLayout)val8).VerticalAlign).Value = (LayoutVerticalAlignment)0;
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)val7).GetComponent<ContentSizeFitter>((Predicate<ContentSizeFitter>)null, false).VerticalFit).Value = Sync<SizeFit>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Vertical Fit")).GetComponent<ValueField<SizeFit>>((Predicate<ValueField<SizeFit>>)null, false).Value);
				((SyncField<SizeFit>)(object)((ContainerWorker<Component>)(object)val7).GetComponent<ContentSizeFitter>((Predicate<ContentSizeFitter>)null, false).HorizontalFit).Value = Sync<SizeFit>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Horizontal Fit")).GetComponent<ValueField<SizeFit>>((Predicate<ValueField<SizeFit>>)null, false).Value);
				((SyncField<LayoutHorizontalAlignment>)(object)((ContainerWorker<Component>)(object)val7).GetComponent<ScrollRect>((Predicate<ScrollRect>)null, false).HorizontalAlign).Value = Sync<LayoutHorizontalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Horizontal Alignment")).GetComponent<ValueField<LayoutHorizontalAlignment>>((Predicate<ValueField<LayoutHorizontalAlignment>>)null, false).Value);
				((SyncField<LayoutVerticalAlignment>)(object)((ContainerWorker<Component>)(object)val7).GetComponent<ScrollRect>((Predicate<ScrollRect>)null, false).VerticalAlign).Value = Sync<LayoutVerticalAlignment>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Vertical Alignment")).GetComponent<ValueField<LayoutVerticalAlignment>>((Predicate<ValueField<LayoutVerticalAlignment>>)null, false).Value);
				ComponentView.Target = val7;
			}
			else if (builderType == "Button")
			{
				LenowoTweeks.buttonMinHeightDefault.Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Min Height").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				Image val9 = ((ContainerWorker<Component>)(object)val3).AttachComponent<Image>(true, (Action<Image>)null);
				((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)val9).Sprite).Target = ui.Style.ButtonSprite;
				((SyncField<NineSliceSizing>)(object)((ImageBase)val9).NineSliceSizing).Value = (NineSliceSizing)0;
				((SyncField<colorX>)(object)val9.Tint).Value = ui.Style.ButtonColor;
				((ContainerWorker<Component>)(object)val3).AttachComponent<Button>(true, (Action<Button>)null);
				Slot val10 = val3.AddSlot("Text", true);
				((SyncField<string>)(object)((ContainerWorker<Component>)(object)val10).AttachComponent<Text>(true, (Action<Text>)null).Content).Value = Sync<string>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Text")).GetComponent<ValueField<string>>((Predicate<ValueField<string>>)null, false).Value);
				Text component = ((ContainerWorker<Component>)(object)val10).GetComponent<Text>((Predicate<Text>)null, false);
				((SyncField<float>)(object)component.Size).Value = 50f;
				((SyncField<TextHorizontalAlignment>)(object)component.HorizontalAlign).Value = (TextHorizontalAlignment)1;
				((SyncField<TextVerticalAlignment>)(object)component.VerticalAlign).Value = (TextVerticalAlignment)1;
				((SyncField<colorX>)(object)component.Color).Value = new colorX(1f, 1f, 1f, 1f, (ColorProfile)1);
				LayoutElement val11 = ((ContainerWorker<Component>)(object)val3).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null);
				((SyncField<float>)(object)val11.MinHeight).Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Min Height").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				((SyncField<float>)(object)val11.MinWidth).Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Min Width").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
			}
			else if (builderType == "String Field" || builderType == "Bool Field" || builderType == "Float Field" || builderType == "Slider" || builderType == "Reference Field")
			{
				LenowoTweeks.buttonMinHeightDefault.Value = ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Min Height").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value;
				UIBuilder val12 = new UIBuilder(val3, (Slot)null);
				SetUIColor(ui, LenowoTweeks.secondaryUIColor.Value);
				if (builderType == "Float Field")
				{
					FloatTextEditorParser val13 = val12.FloatField(float.MinValue, float.MaxValue, 2, (string)null, true);
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)val13).Slot.Parent).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
				}
				else if (builderType == "Bool Field")
				{
					ValueField<bool> val14 = ((ContainerWorker<Component>)(object)val3).AttachComponent<ValueField<bool>>(true, (Action<ValueField<bool>>)null);
					val12.NestInto(((Component)val14).Slot);
					BooleanMemberEditor val15 = UIBuilderEditors.BooleanMemberEditor(val12, (IField)(object)val14.Value, (string)null);
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)val15).Slot).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
					val12.NestOut();
				}
				else if (builderType == "String Field")
				{
					TextField val16 = val12.TextField("Text Field", false, (string)null, true, default(LocaleString));
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)val16).Slot.Parent).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
				}
				else if (builderType == "Slider")
				{
					Image val17 = ((ContainerWorker<Component>)(object)val3).AttachComponent<Image>(true, (Action<Image>)null);
					((SyncField<colorX>)(object)val17.Tint).Value = new colorX(0f, 0f, 0f, 1f, (ColorProfile)1);
					((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)val17).Sprite).Target = ui.Style.ButtonSprite;
					((SyncField<NineSliceSizing>)(object)((ImageBase)val17).NineSliceSizing).Value = (NineSliceSizing)0;
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)val3).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
					ValueField<float> val18 = ((ContainerWorker<Component>)(object)val3).AttachComponent<ValueField<float>>(true, (Action<ValueField<float>>)null);
					UIBuilderEditors.SliderMemberEditor(val12, ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Min").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value, ((SyncField<float>)(object)((TextEditorParser<float>)(object)((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Max").FindChild("Button")).GetComponent<FloatTextEditorParser>((Predicate<FloatTextEditorParser>)null, false)).ParsedValue).Value, (IField)(object)val18.Value, (string)null, (string)null);
					((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val18).Slot.FindChild("Horizontal Layout").FindChild("Button")).GetComponent<Image>((Predicate<Image>)null, false)).Sprite).Target = ui.Style.ButtonSprite;
					((SyncField<NineSliceSizing>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val18).Slot.FindChild("Horizontal Layout").FindChild("Button")).GetComponent<Image>((Predicate<Image>)null, false)).NineSliceSizing).Value = (NineSliceSizing)0;
				}
				else if (builderType == "Reference Field")
				{
					ReferenceField<Slot> val19 = ((ContainerWorker<Component>)(object)val3).AttachComponent<ReferenceField<Slot>>(true, (Action<ReferenceField<Slot>>)null);
					RefEditor val20 = UIBuilderEditors.RefMemberEditor(val12, (ISyncRef)(object)val19.Reference);
					((SyncField<float>)(object)((ContainerWorker<Component>)(object)((Component)val20).Slot).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
				}
			}
			else if (builderType == "Text")
			{
				Text val21 = ((ContainerWorker<Component>)(object)val3).AttachComponent<Text>(true, (Action<Text>)null);
				((SyncField<string>)(object)val21.Content).Value = Sync<string>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Text")).GetComponent<ValueField<string>>((Predicate<ValueField<string>>)null, false).Value);
				((SyncField<float>)(object)((ContainerWorker<Component>)(object)val3).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).MinHeight).Value = LenowoTweeks.buttonMinHeightDefault.Value;
			}
			else if (builderType == "Mask")
			{
				Image val22 = ((ContainerWorker<Component>)(object)val3).AttachComponent<Image>(true, (Action<Image>)null);
				Mask val23 = ((ContainerWorker<Component>)(object)val3).AttachComponent<Mask>(true, (Action<Mask>)null);
				((SyncField<bool>)(object)val23.ShowMaskGraphic).Value = Sync<bool>.op_Implicit(((ContainerWorker<Component>)(object)UIVerticalLayout.FindChild("Show Mash Graphic")).GetComponent<ValueField<bool>>((Predicate<ValueField<bool>>)null, false).Value);
			}
			PanelRoot.Destroy();
		};
	}
}

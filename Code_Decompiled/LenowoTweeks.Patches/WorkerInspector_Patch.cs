using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class WorkerInspector_Patch
{
	[HarmonyPrefix]
	[HarmonyPatch(typeof(WorkerInspector), "BuildUIForComponent")]
	public static bool BuildUIForComponent(WorkerInspector __instance, SyncRef<Worker> ____targetWorker, Worker worker, bool allowRemove = true, bool allowDuplicate = true, bool allowContainer = false, Predicate<ISyncMember> memberFilter = null)
	{
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		if (((Worker)__instance).LocalUser.IsHost && !Helpers.ModShouldRun(ObjectRootExtensions.GetObjectRoot(((Component)__instance).Slot.Parent, false)))
		{
			return true;
		}
		try
		{
			if (__instance == null || ((Worker)__instance).IsRemoved || ((Component)__instance).Slot == null || ((Worker)((Component)__instance).Slot).IsRemoved || worker == null || worker.IsRemoved)
			{
				return false;
			}
			UIBuilder ui = new UIBuilder(((Component)__instance).Slot, (Slot)null);
			if (ui == null)
			{
				return false;
			}
			RadiantUI_Constants.SetupEditorStyle(ui, false);
			ui.Style.RequireLockInToPress = true;
			Slot slot = ((Component)ui.VerticalLayout(6f, 0f, (Alignment?)null, (bool?)null, (bool?)null)).Slot;
			if (!(worker is Slot))
			{
				ui.Style.MinHeight = 32f;
				ui.HorizontalLayout(4f, 0f, (Alignment?)null);
				ui.Style.MinHeight = 24f;
				ui.Style.FlexibleWidth = 1000f;
				UIBuilder obj = ui;
				LocaleString val = LocaleString.op_Implicit(GetComponentHeaderName(worker));
				colorX? val2 = RadiantUI_Constants.BUTTON_COLOR;
				Button val3 = obj.Button(ref val, ref val2);
				((SyncField<colorX>)(object)val3.Label.Color).Value = RadiantUI_Constants.LABEL_COLOR;
				if (allowRemove || allowDuplicate || allowContainer)
				{
					ui.Style.FlexibleWidth = 0f;
					ui.Style.MinWidth = 40f;
					if (allowContainer && WorldElementExtensions.FindNearestParent<Slot>((IWorldElement)(object)worker, (Predicate<Slot>)null) != null)
					{
						UIBuilder obj2 = ui;
						Uri rootUp = Inspector.RootUp;
						val2 = Sub.PURPLE;
						ButtonRefRelay<Worker> val4 = ((ContainerWorker<Component>)(object)((Component)obj2.Button(rootUp, ref val2)).Slot).AttachComponent<ButtonRefRelay<Worker>>(true, (Action<ButtonRefRelay<Worker>>)null);
						val4.Argument.Target = worker;
						val4.ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler<Worker>>("OnOpenContainerPressed");
					}
					if (!LenowoTweeks.nohelp.Value)
					{
						Type type = ((object)worker).GetType();
						UIBuilder obj3 = ui;
						Uri help = Inspector.Help;
						val2 = Sub.CYAN;
						Hyperlink.AttachForWikiPage(((Component)obj3.Button(help, ref val2)).Slot, type);
					}
					if (allowDuplicate)
					{
						UIBuilder obj4 = ui;
						Uri duplicate = Inspector.Duplicate;
						val2 = Sub.GREEN;
						ButtonRefRelay<Worker> val5 = ((ContainerWorker<Component>)(object)((Component)obj4.Button(duplicate, ref val2)).Slot).AttachComponent<ButtonRefRelay<Worker>>(true, (Action<ButtonRefRelay<Worker>>)null);
						val5.Argument.Target = worker;
						val5.ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler<Worker>>("OnDuplicateComponentPressed");
					}
					if (allowRemove)
					{
						UIBuilder obj5 = ui;
						Uri destroy = Inspector.Destroy;
						val2 = Sub.RED;
						ButtonRefRelay<Worker> val6 = ((ContainerWorker<Component>)(object)((Component)obj5.Button(destroy, ref val2)).Slot).AttachComponent<ButtonRefRelay<Worker>>(true, (Action<ButtonRefRelay<Worker>>)null);
						val6.Argument.Target = worker;
						val6.ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler<Worker>>("OnRemoveComponentPressed");
					}
				}
				((SyncRef<IWorldElement>)(object)((ContainerWorker<Component>)(object)((Component)val3).Slot).AttachComponent<ReferenceProxySource>(true, (Action<ReferenceProxySource>)null).Reference).Target = (IWorldElement)(object)worker;
				ui.NestOut();
				if (____targetWorker.Target == null && LenowoTweeks.collapseComponents.Value)
				{
					Slot content = ((Component)ui.VerticalLayout(8f, 0f, (Alignment?)null, (bool?)null, (bool?)null)).Slot;
					content.Name = "Me when the best collapsable component system :3";
					((ComponentBase<Component>)(object)((ContainerWorker<Component>)(object)content).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false)).Destroy();
					ButtonToggle val7 = ((ContainerWorker<Component>)(object)((Component)val3).Slot).AttachComponent<ButtonToggle>(true, (Action<ButtonToggle>)null);
					val7.TargetValue.Target = (IField<bool>)(object)content.ActiveSelf_Field;
					content.ActiveSelf = false;
					((SyncField<bool>)(object)content.ActiveSelf_Field).OnValueChange += delegate(SyncField<bool> v)
					{
						//IL_0158: Unknown result type (might be due to invalid IL or missing references)
						//IL_015d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0222: Unknown result type (might be due to invalid IL or missing references)
						//IL_024d: Unknown result type (might be due to invalid IL or missing references)
						try
						{
							if (__instance != null && !((Worker)__instance).IsRemoved && ((Component)__instance).Slot != null && !((Worker)((Component)__instance).Slot).IsRemoved && worker != null && !worker.IsRemoved)
							{
								if (content.ChildrenCount == 0 && SyncField<bool>.op_Implicit(v))
								{
									ui.NestInto(content);
									InspectorHeaderAttribute customAttribute = ((MemberInfo)((object)worker).GetType()).GetCustomAttribute<InspectorHeaderAttribute>();
									if (customAttribute != null)
									{
										AddHeaderText(ui, customAttribute);
									}
									Worker obj8 = worker;
									ICustomInspector val10 = (ICustomInspector)(object)((obj8 is ICustomInspector) ? obj8 : null);
									if (val10 != null)
									{
										try
										{
											ui.Style.MinHeight = 24f;
											val10.BuildInspectorUI(ui);
										}
										catch (Exception ex2)
										{
											LocaleString val11 = LocaleString.op_Implicit("EXCEPTION BUILDING UI. See log");
											ui.Text(ref val11, true, (Alignment?)null, true, (string)null);
											UniLog.Error(ex2.ToString(), false);
										}
									}
									else
									{
										WorkerInspector.BuildInspectorUI(worker, ui, memberFilter);
									}
									ui.Style.MinHeight = 8f;
									ui.Panel();
									ui.NestOut();
								}
								Helpers.SetConfigVariable<colorX>(((Worker)__instance).LocalUser, "CollapsedColor", LenowoTweeks.collapsedComponentColor.Value);
								Helpers.SetConfigVariable<colorX>(((Worker)__instance).LocalUser, "ExpandedColor", LenowoTweeks.expandedComponentColor.Value);
							}
						}
						catch (Exception value2)
						{
							UniLog.Error($"LenowoTweeks // You broke it - Failed on ExpandComponent:\n{value2}", true);
						}
					};
					ui.NestOut();
					Helpers.SetConfigVariable<colorX>(((Worker)__instance).LocalUser, "CollapsedColor", LenowoTweeks.collapsedComponentColor.Value);
					Helpers.SetConfigVariable<colorX>(((Worker)__instance).LocalUser, "ExpandedColor", LenowoTweeks.expandedComponentColor.Value);
					BooleanValueDriver<colorX> val8 = ((ContainerWorker<Component>)(object)((Component)val3).Slot).AttachComponent<BooleanValueDriver<colorX>>(true, (Action<BooleanValueDriver<colorX>>)null);
					((SyncRef<IField<colorX>>)(object)((SyncElementList<ColorDriver>)(object)((InteractionElement)val3).ColorDrivers)[1].ColorDrive).Target = null;
					((SyncRef<IField<colorX>>)(object)val8.TargetField).Target = (IField<colorX>)(object)val3.Label.Color;
					((SyncField<colorX>)(object)val8.TrueValue).Value = RadiantUI_Constants.LABEL_COLOR;
					Helpers.DriveFromVariable<colorX>(((Worker)__instance).LocalUser, "ExpandedColor", (IField<colorX>)(object)val8.TrueValue);
					Helpers.DriveFromVariable<colorX>(((Worker)__instance).LocalUser, "CollapsedColor", (IField<colorX>)(object)val8.FalseValue);
					ValueCopyExtensions.DriveFrom<bool>((IField<bool>)(object)val8.State, (IField<bool>)(object)content.ActiveSelf_Field, false, false, true);
				}
			}
			Worker obj6 = worker;
			ICustomInspector val9 = (ICustomInspector)(object)((obj6 is ICustomInspector) ? obj6 : null);
			if (val9 != null)
			{
				try
				{
					if (worker is Slot || ____targetWorker.Target != null || !LenowoTweeks.collapseComponents.Value)
					{
						ui.Style.MinHeight = 24f;
						val9.BuildInspectorUI(ui);
					}
				}
				catch (Exception ex)
				{
					UIBuilder obj7 = ui;
					LocaleString val = LocaleString.op_Implicit("EXCEPTION BUILDING UI. See log");
					obj7.Text(ref val, true, (Alignment?)null, true, (string)null);
					UniLog.Error(ex.ToString(), false);
				}
			}
			else if (worker is Slot || ____targetWorker.Target != null || !LenowoTweeks.collapseComponents.Value)
			{
				WorkerInspector.BuildInspectorUI(worker, ui, memberFilter);
			}
			if (worker is Slot || !LenowoTweeks.collapseComponents.Value)
			{
				ui.Style.MinHeight = 8f;
				ui.Panel();
			}
			ui.NestOut();
		}
		catch (Exception value)
		{
			UniLog.Error($"LenowoTweeks // You broke it - Failed on BuildUIForComponent:\n{value}", true);
		}
		return false;
	}

	public static void AddHeaderText(UIBuilder ui, InspectorHeaderAttribute header)
	{
		ui.PushStyle();
		ui.Style.MinHeight = header.MinHeight;
		ui.Text(ref header.LocaleKey, true, (Alignment?)(Alignment)0, true, (string)null);
		ui.PopStyle();
	}

	private static string GetComponentHeaderName(Worker worker)
	{
		Type type = ((object)worker).GetType();
		string result = "<b>" + ReflectionExtensions.GetNiceName(type, "<", ">", "+") + "</b>";
		if (!LenowoTweeks.modifiedComponentHeaders.Value)
		{
			return result;
		}
		IDynamicVariable val = (IDynamicVariable)(object)((worker is IDynamicVariable) ? worker : null);
		if (val != null)
		{
			string text = LenowoTweeks.dynvarComponentHeaderName.Value ?? "";
			string[] array = text.Split(';');
			string text2 = ((array.Length >= 1) ? array[0] : "");
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "Variable";
			}
			string text3 = ((array.Length >= 2) ? array[1] : "");
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "Field";
			}
			string text4 = ((array.Length >= 3) ? array[2] : "");
			if (string.IsNullOrEmpty(text4))
			{
				text4 = "Reference";
			}
			result = $"<b>{((type == typeof(DynamicTypeVariable) || type == typeof(DynamicTypeField)) ? "Type" : ReflectionExtensions.GetNiceName(type.GenericTypeArguments.First(), "<", ">", "+"))} Variable: {(string.IsNullOrWhiteSpace(val.VariableName) ? "<i>unset</i>" : val.VariableName)}</b>";
			string niceName = ReflectionExtensions.GetNiceName(type, "<", ">", "+");
			if (niceName.Contains("Variable") && !niceName.Contains("Variable<") && !niceName.Contains("Type"))
			{
				string input = niceName.Substring(niceName.IndexOf("Variable"), MathX.Clamp(niceName.IndexOf('<') - niceName.IndexOf("Variable"), 0, 1000));
				result = result.Replace("Variable", Regex.Replace(input, "(?<!^)([A-Z])", " $1"));
			}
			else if (type == typeof(DynamicTypeVariable) || type == typeof(DynamicTypeField))
			{
				result = result.Replace("Variable", (type == typeof(DynamicTypeField)) ? text3 : "Variable");
			}
			else if (type.GetGenericTypeDefinition() == typeof(DynamicField<>) || type.GetGenericTypeDefinition() == typeof(DynamicReference<>))
			{
				result = result.Replace("Variable", (type.GetGenericTypeDefinition() == typeof(DynamicField<>)) ? text3 : text4);
			}
			result = result.Replace("Variable", text2);
		}
		DynamicVariableSpace val2 = (DynamicVariableSpace)(object)((worker is DynamicVariableSpace) ? worker : null);
		if (val2 != null)
		{
			result = $"<b>{ReflectionExtensions.GetNiceName(((object)val2).GetType(), "<", ">", "+")}: {(string.IsNullOrWhiteSpace(Sync<string>.op_Implicit(val2.SpaceName)) ? "<i>unset</i>" : Sync<string>.op_Implicit(val2.SpaceName))}</b>";
		}
		return result;
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(Slot), "BuildInspectorUI")]
	public static bool SlotCustomInspectorPatch(Slot __instance, UIBuilder ui)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		WorkerInspector.BuildInspectorUI((Worker)(object)__instance, ui, (Predicate<ISyncMember>)null);
		ui.Panel();
		List<RectTransform> list = ui.SplitHorizontally(new float[3] { 1f, 1f, 1f });
		ui.NestOut();
		UIBuilder val = new UIBuilder(list[0]);
		LocaleString val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Axis.X", "<color=#f00>{0}", true, (Dictionary<string, object>)null);
		val.Text(ref val2, true, (Alignment?)null, true, (string)null);
		UIBuilder val3 = new UIBuilder(list[1]);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Axis.Y", "<color=#0f0>{0}", true, (Dictionary<string, object>)null);
		val3.Text(ref val2, true, (Alignment?)null, true, (string)null);
		UIBuilder val4 = new UIBuilder(list[2]);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Axis.Z", "<color=#00f>{0}", true, (Dictionary<string, object>)null);
		val4.Text(ref val2, true, (Alignment?)null, true, (string)null);
		ui.HorizontalLayout(4f, 0f, (Alignment?)null);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Reset.Label", "<b>{0}</b>", true, (Dictionary<string, object>)null);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Reset.Position", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetPosition"));
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Reset.Rotation", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetRotation"));
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.Reset.Scale", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetScale"));
		if (LenowoTweeks.slotInspectorResetAll.Value)
		{
			val2 = LocaleHelper.AsLocaleKey("All", (string)null, true, (Dictionary<string, object>)null);
			Button val5 = ui.Button(ref val2);
			((ContainerWorker<Component>)(object)((Component)val5).Slot).AttachComponent<ButtonRelay>(true, (Action<ButtonRelay>)null).ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetPosition");
			((ContainerWorker<Component>)(object)((Component)val5).Slot).AttachComponent<ButtonRelay>(true, (Action<ButtonRelay>)null).ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetRotation");
			((ContainerWorker<Component>)(object)((Component)val5).Slot).AttachComponent<ButtonRelay>(true, (Action<ButtonRelay>)null).ButtonPressed.Target = ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ResetScale");
		}
		ui.NestOut();
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.CreatePivotAtCenter", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("OnCreatePivotAtCenter"));
		ui.HorizontalLayout(4f, 0f, (Alignment?)null);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.JumpTo", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("JumpTo"));
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.BringTo", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("BringTo"));
		ui.NestOut();
		ui.HorizontalLayout(4f, 0f, (Alignment?)null);
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.ParentUnder.Label", "<b>{0}</b>", true, (Dictionary<string, object>)null);
		ui.Text(ref val2, true, (Alignment?)null, true, (string)null);
		ui.PushStyle();
		ui.Style.MinWidth = 160f;
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.ParentUnder.LocalUserSpace", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ParentUnderLocalUserSpace"));
		val2 = LocaleHelper.AsLocaleKey("Inspector.Slot.ParentUnder.WorldRoot", (string)null, true, (Dictionary<string, object>)null);
		ui.Button(ref val2, ((object)__instance).GetMethodDelegate<ButtonEventHandler>("ParentUnderWorldRoot"));
		ui.PopStyle();
		ui.NestOut();
		return false;
	}
}

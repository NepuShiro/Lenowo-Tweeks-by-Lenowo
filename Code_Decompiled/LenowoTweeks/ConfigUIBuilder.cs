using System;
using System.Collections.Generic;
using System.Reflection;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using ResoniteModLoader;

namespace LenowoTweeks;

public class ConfigUIBuilder
{
	public ModConfiguration? ThisConfig;

	public int currentConfigIndex = 0;

	public const float ITEM_HEIGHT = 24f;

	public ConfigUIBuilder()
	{
	}

	public ConfigUIBuilder(ModConfiguration? config)
		: this()
	{
		ThisConfig = config;
	}

	public void BuildConfigUI(UIBuilder ui, Dictionary<string, Dictionary<string, List<ModConfigKey>>> configKeys)
	{
		if (ThisConfig == null)
		{
			return;
		}
		currentConfigIndex = 0;
		foreach (KeyValuePair<string, Dictionary<string, List<ModConfigKey>>> configKey in configKeys)
		{
			BuildSection(ui, configKey.Key, configKey.Value);
		}
	}

	public void BuildSection(UIBuilder ui, string name, Dictionary<string, List<ModConfigKey>> SubGroups)
	{
		VerticalLayout sectionLayout = null;
		BuildTitle(ui, name, 48f, delegate
		{
			VerticalLayout obj = sectionLayout;
			bool activeSelf = obj != null && !((Component)obj).Slot.ActiveSelf;
			VerticalLayout obj2 = sectionLayout;
			if (obj2 != null)
			{
				((Component)obj2).Slot.ActiveSelf = activeSelf;
			}
		});
		ui.Style.MinHeight = -1f;
		sectionLayout = ui.VerticalLayout(4f, 0f, (Alignment?)(Alignment)0, (bool?)true, (bool?)false);
		foreach (KeyValuePair<string, List<ModConfigKey>> SubGroup in SubGroups)
		{
			VerticalLayout groupLayout = null;
			if (SubGroup.Key != "Base")
			{
				BuildTitle(ui, SubGroup.Key, 32f, delegate
				{
					VerticalLayout obj = groupLayout;
					bool activeSelf = obj != null && !((Component)obj).Slot.ActiveSelf;
					VerticalLayout obj2 = groupLayout;
					if (obj2 != null)
					{
						((Component)obj2).Slot.ActiveSelf = activeSelf;
					}
				});
			}
			ui.Style.MinHeight = -1f;
			groupLayout = ui.VerticalLayout(4f, 0f, (Alignment?)(Alignment)0, (bool?)true, (bool?)false);
			foreach (ModConfigKey item in SubGroup.Value)
			{
				item.BuildField(this, ui);
				currentConfigIndex++;
			}
			ui.NestOut();
		}
		ui.NestOut();
	}

	public void BuildTitle(UIBuilder ui, string title, float size, Action onClicked)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		RadiantUI_Constants.SetupDefaultStyle(ui, false);
		ui.Style.MinHeight = size;
		ui.Style.TextAutoSizeMax = size;
		Alignment textAlignment = ui.Style.TextAlignment;
		ui.Style.TextAlignment = (Alignment)0;
		ui.Spacer(size);
		Slot val = ui.Empty("Title bar");
		ui.NestInto(val);
		LocaleString val2 = default(LocaleString);
		((LocaleString)(ref val2))._002Ector(title, "{0}", true, true, (Dictionary<string, object>)null);
		Button val3 = ui.Button(ref val2);
		val3.LocalPressed += (ButtonEventHandler)delegate
		{
			onClicked();
		};
		ui.NestOut();
		ui.Style.TextAlignment = textAlignment;
	}

	public void BuildGenericField<T>(UIBuilder ui, ModConfigKey<T> key)
	{
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		if (ThisConfig == null)
		{
			return;
		}
		bool flag = typeof(T) == typeof(Type);
		if (!flag && !DynamicValueVariable<T>.IsValidGenericType)
		{
			return;
		}
		string text = "com.Lenowo.LenowoTweeks." + key.ConfigKeyName;
		string configDescription = key.ConfigDescription;
		RadiantUI_Constants.SetupEditorStyle(ui, false);
		ui.Style.MinHeight = 24f;
		Slot root = ui.Empty(text);
		ui.NestInto(root);
		FieldInfo fieldInfo = null;
		SyncField<T> syncField;
		if (!flag)
		{
			DynamicValueVariable<T> val = ((ContainerWorker<Component>)(object)root).AttachComponent<DynamicValueVariable<T>>(true, (Action<DynamicValueVariable<T>>)null);
			((SyncField<string>)(object)((DynamicVariableBase<T>)(object)val).VariableName).Value = "Config/" + text;
			syncField = (SyncField<T>)(object)val.Value;
			if ((object)fieldInfo == null)
			{
				fieldInfo = ((Worker)val).GetSyncMemberFieldInfo(4);
			}
		}
		else
		{
			DynamicReferenceVariable<SyncType> val2 = ((ContainerWorker<Component>)(object)root).AttachComponent<DynamicReferenceVariable<SyncType>>(true, (Action<DynamicReferenceVariable<SyncType>>)null);
			((SyncField<string>)(object)((DynamicVariableBase<SyncType>)(object)val2).VariableName).Value = "Config/" + text;
			TypeField val3 = ((ContainerWorker<Component>)(object)root).AttachComponent<TypeField>(true, (Action<TypeField>)null);
			val2.Reference.TrySet((IWorldElement)(object)val3.Type);
			syncField = val3.Type as SyncField<T>;
			if ((object)fieldInfo == null)
			{
				fieldInfo = ((Worker)val3).GetSyncMemberFieldInfo(3);
			}
		}
		T value = key.Value;
		syncField.Value = value;
		syncField.OnValueChange += delegate(SyncField<T> syncF)
		{
			HandleConfigFieldChange(syncF, ThisConfig, key);
		};
		syncField.LocalFilter = (T value2, IField<T> field) => ValidateConfigField(value2, key);
		RadiantUI_Constants.SetupDefaultStyle(ui, false);
		ui.Style.TextAutoSizeMax = 24f;
		UIBuilder obj = ui;
		colorX val4 = ColorHSL.op_Implicit(new ColorHSL((float)(currentConfigIndex + 1) / 10f % 1f, 0.8f, 0.1f, 0.5f));
		obj.Image(ref val4, false);
		UIBuilder obj2 = ui;
		LocaleString val5 = LocaleString.op_Implicit(key.ConfigName);
		obj2.HorizontalElementWithLabel<Component>(ref val5, 0.55f, (Func<Component>)delegate
		{
			Slot val6 = ui.Root.Parent[0][0];
			((SyncField<bool>)(object)((DirectionalLayout)ui.HorizontalLayout(4f, 0f, (Alignment?)(Alignment)3)).ForceExpandHeight).Value = false;
			ui.Style.FlexibleWidth = 10f;
			SyncMemberEditorBuilder.Build((ISyncMember)(object)syncField, (string)null, fieldInfo, ui, 0f);
			ui.Style.FlexibleWidth = -1f;
			Slot obj4 = ui.Root[0];
			object obj5;
			if (obj4 == null)
			{
				obj5 = null;
			}
			else
			{
				InspectorMemberActions componentInChildren = obj4.GetComponentInChildren<InspectorMemberActions>((Predicate<InspectorMemberActions>)null, false, false);
				obj5 = ((componentInChildren != null) ? ((Component)componentInChildren).Slot : null);
			}
			Slot val7 = (Slot)obj5;
			if (val7 != null && typeof(T) == typeof(dummy))
			{
				val7.Destroy();
			}
			if (val7 != null && val6 != null && typeof(T) != typeof(dummy))
			{
				DynamicValueVariableDriver<bool> val8 = ((ContainerWorker<Component>)(object)val7).AttachComponent<DynamicValueVariableDriver<bool>>(true, (Action<DynamicValueVariableDriver<bool>>)null);
				((SyncRef<IField<bool>>)(object)val8.Target).TrySet((IWorldElement)(object)val7.ActiveSelf_Field);
				((SyncField<string>)(object)((DynamicVariableBase<bool>)(object)val8).VariableName).Value = "vr_active";
				val7.Parent = val6.Parent;
				val7.OrderOffset = -1L;
				LayoutElement val9 = ((ContainerWorker<Component>)(object)val7).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null);
				((SyncField<float>)(object)val9.PreferredHeight).Value = 24f;
				((SyncField<float>)(object)val9.MinHeight).Value = 24f;
				((SyncField<float>)(object)val9.MinWidth).Value = 24f;
				((ContainerWorker<Component>)(object)val6).CopyComponent<LayoutElement>(val9);
				HorizontalLayout val10 = ((ContainerWorker<Component>)(object)val6.Parent).AttachComponent<HorizontalLayout>(true, (Action<HorizontalLayout>)null);
				((SyncField<float>)(object)((DirectionalLayout)val10).Spacing).Value = 4f;
				((SyncField<LayoutHorizontalAlignment>)(object)((DirectionalLayout)val10).HorizontalAlign).Value = (LayoutHorizontalAlignment)0;
				((DirectionalLayout)val10).ForceExpand = false;
				((ContainerWorker<Component>)(object)val6).AttachComponent<Button>(true, (Action<Button>)null);
				((ContainerWorker<Component>)(object)val6).AttachComponent<FieldDriveReceiver<T>>(true, (Action<FieldDriveReceiver<T>>)null).TryAssignField((IField)(object)syncField);
				((ContainerWorker<Component>)(object)val6).AttachComponent<ValueReceiver<T>>(true, (Action<ValueReceiver<T>>)null).TryAssignField((IField)(object)syncField);
			}
			LayoutElement val11 = ((ContainerWorker<Component>)(object)ui.Root[0])?.GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false);
			if (val11 != null)
			{
				float num = 1f;
				Sync<float> minHeight = val11.MinHeight;
				((SyncField<float>)(object)minHeight).Value = ((SyncField<float>)(object)minHeight).Value * num;
				((SyncField<float>)(object)((ContainerWorker<Component>)(object)root).GetComponent<LayoutElement>((Predicate<LayoutElement>)null, false).MinHeight).Value = ((SyncField<float>)(object)val11.MinHeight).Value;
				List<LayoutElement> componentsInChildren = ((Component)val11).Slot.GetComponentsInChildren<LayoutElement>((Predicate<LayoutElement>)((LayoutElement element) => ((SyncField<float>)(object)element.MinHeight).Value == 24f), false, false, (Predicate<Slot>)null);
				foreach (LayoutElement item in componentsInChildren)
				{
					((SyncField<float>)(object)item.MinHeight).Value = 24f;
				}
			}
			ui.NestOut();
			return (Component)null;
		}, 0.01f);
		ui.NestOut();
		ui.NestInto(ui.Empty("Description"));
		ui.Style.TextAlignment = (Alignment)3;
		ui.Style.TextAutoSizeMax = 16f;
		UIBuilder obj3 = ui;
		val5 = LocaleString.op_Implicit(key.ConfigDescription);
		obj3.Text(ref val5, true, (Alignment?)null, true, (string)null);
		ui.Style.MinHeight = -1f;
		ui.NestOut();
	}

	private T ValidateConfigField<T>(T value, ModConfigKey<T> configKey)
	{
		bool flag = false;
		try
		{
			flag = configKey.TypedConfigKey.Validate(value);
		}
		catch
		{
		}
		if (!flag)
		{
			return configKey.Value;
		}
		return value;
	}

	private void HandleConfigFieldChange<T>(SyncField<T> syncField, ModConfiguration modConfiguration, ModConfigKey<T> configKey)
	{
		if (modConfiguration.TryGetValue(configKey.TypedConfigKey, out T value) && (object.Equals(value, syncField.Value) || !object.Equals(syncField.Value, syncField.Value)))
		{
			configKey.Value = value;
			return;
		}
		try
		{
			if (!configKey.TypedConfigKey.Validate(syncField.Value))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		configKey.Value = syncField.Value;
		modConfiguration.Save(saveDefaultValues: true);
	}
}

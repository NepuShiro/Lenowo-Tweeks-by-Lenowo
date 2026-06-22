using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Elements.Assets;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.FrooxEngine.ProtoFlux.CoreNodes;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes;
using FrooxEngine.UIX;
using HarmonyLib;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFlux_EditNames
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void AddEditableNamesPatch(ProtoFluxNodeVisual __instance)
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.protofluxEditableNames.Value)
		{
			return;
		}
		ProtoFluxNode target = ((SyncRef<ProtoFluxNode>)(object)__instance.Node).Target;
		Type type = ((object)target).GetType();
		Type type2 = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
		Type type3 = (type.IsGenericType ? type.GenericTypeArguments.Last() : type);
		int num = 14;
		List<Type> list = new List<Type>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Type> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
		span[num2] = typeof(ValueObjectInput<>);
		num2++;
		span[num2] = typeof(ValueInput<>);
		num2++;
		span[num2] = typeof(RefObjectInput<>);
		num2++;
		span[num2] = typeof(AssetInput<>);
		num2++;
		span[num2] = typeof(ObjectDisplay<>);
		num2++;
		span[num2] = typeof(ValueDisplay<>);
		num2++;
		span[num2] = typeof(ImpulseDisplay);
		num2++;
		span[num2] = typeof(CallInput);
		num2++;
		span[num2] = typeof(AsyncCallInput);
		num2++;
		span[num2] = typeof(ValueSource<>);
		num2++;
		span[num2] = typeof(ElementSource<>);
		num2++;
		span[num2] = typeof(ReferenceSource<>);
		num2++;
		span[num2] = typeof(ValueFieldDrive<>);
		num2++;
		span[num2] = typeof(ReferenceDrive<>);
		List<Type> list2 = list;
		if (list2.Contains(type2))
		{
			Slot val = ((IEnumerable<Slot>)(object)((Component)__instance).Slot.Children).ToList()[1];
			Slot val2 = val.FindChild("Text");
			UIBuilder val3 = new UIBuilder(val, (Slot)null);
			HorizontalLayout val4 = ((ContainerWorker<Component>)(object)val).AttachComponent<HorizontalLayout>(true, (Action<HorizontalLayout>)null);
			((SyncField<float>)(object)((DirectionalLayout)val4).Spacing).Value = 6f;
			((SyncField<float>)(object)((DirectionalLayout)val4).PaddingLeft).Value = 3f;
			((SyncField<float>)(object)((DirectionalLayout)val4).PaddingRight).Value = 3f;
			((SyncField<LayoutHorizontalAlignment>)(object)((DirectionalLayout)val4).HorizontalAlign).Value = (LayoutHorizontalAlignment)0;
			((SyncField<bool>)(object)((DirectionalLayout)val4).ForceExpandWidth).Value = false;
			Uri uri = new Uri("resdb:///8e79ad496c6cb57feb25c60ae879116b3e4da6609793b52d853bb6f1ffe3ad09.png");
			colorX? val5 = colorX.White;
			colorX white = colorX.White;
			Button val6 = val3.Button(uri, ref val5, ref white);
			LayoutElement val7 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null);
			((SyncField<float>)(object)val7.MinWidth).Value = 16f;
			((SyncField<float>)(object)val7.MinHeight).Value = 16f;
			TextField val8 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<TextField>(true, (Action<TextField>)null);
			TextEditor component = ((ContainerWorker<Component>)(object)((Component)val6).Slot).GetComponent<TextEditor>((Predicate<TextEditor>)null, false);
			Text component2 = ((ContainerWorker<Component>)(object)val2).GetComponent<Text>((Predicate<Text>)null, false);
			component.Text.Target = (IText)(object)component2;
			if (string.IsNullOrEmpty(((Component)target).Slot.Tag))
			{
				((Component)target).Slot.Tag = ((SyncField<string>)(object)component2.Content).Value;
			}
			ValueCopy<string> val9 = ((ContainerWorker<Component>)(object)val2).AttachComponent<ValueCopy<string>>(true, (Action<ValueCopy<string>>)null);
			((SyncRef<IField<string>>)(object)val9.Source).Target = (IField<string>)(object)((Component)target).Slot.Tag_Field;
			((SyncRef<IField<string>>)(object)val9.Target).Target = (IField<string>)(object)component2.Content;
			((SyncField<bool>)(object)val9.WriteBack).Value = true;
			Slot val10 = ((IEnumerable<Slot>)(object)((Component)val6).Slot.Children).First();
			SpriteProvider val11 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<SpriteProvider>(true, (Action<SpriteProvider>)null);
			StaticTexture2D val12 = ((ContainerWorker<Component>)(object)((Component)val6).Slot).AttachComponent<StaticTexture2D>(true, (Action<StaticTexture2D>)null);
			((SyncField<Uri>)(object)((StaticAssetProvider<Texture2D, BitmapMetadata, Texture2DVariantDescriptor>)(object)val12).URL).Value = ((SyncField<Uri>)(object)((StaticAssetProvider<Texture2D, BitmapMetadata, Texture2DVariantDescriptor>)(object)((ContainerWorker<Component>)(object)val10).GetComponent<StaticTexture2D>((Predicate<StaticTexture2D>)null, false)).URL).Value;
			((SyncRef<IAssetProvider<ITexture2D>>)(object)val11.Texture).Target = (IAssetProvider<ITexture2D>)(object)val12;
			((SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)((ContainerWorker<Component>)(object)((Component)val6).Slot).GetComponent<Image>((Predicate<Image>)null, false)).Sprite).Target = (IAssetProvider<Sprite>)(object)val11;
			val10.Destroy();
			((Component)val6).Slot.OrderOffset = -1L;
			((SyncField<float>)(object)((ContainerWorker<Component>)(object)val2).AttachComponent<LayoutElement>(true, (Action<LayoutElement>)null).FlexibleWidth).Value = 1f;
			if (type2 == typeof(CallInput) || type2 == typeof(AsyncCallInput))
			{
				Slot val13 = ((IEnumerable<Slot>)(object)((IEnumerable<Slot>)(object)((Component)__instance).Slot.FindChild("Overlapping Layout").FindChild("Panel").Children).First().Children).First();
				Sync<string> format = ((ContainerWorker<Component>)(object)val13).GetComponent<LocaleStringDriver>((Predicate<LocaleStringDriver>)null, false).Format;
				ValueCopy<string> val14 = ((ContainerWorker<Component>)(object)val13).AttachComponent<ValueCopy<string>>(true, (Action<ValueCopy<string>>)null);
				((SyncRef<IField<string>>)(object)val14.Source).Target = (IField<string>)(object)((Component)target).Slot.Tag_Field;
				((SyncRef<IField<string>>)(object)val14.Target).Target = (IField<string>)(object)format;
			}
		}
	}
}

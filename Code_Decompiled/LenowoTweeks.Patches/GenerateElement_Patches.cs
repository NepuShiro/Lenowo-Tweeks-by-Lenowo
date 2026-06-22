using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Assets;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes.Casts;
using FrooxEngine.UIX;
using HarmonyLib;
using Renderite.Shared;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class GenerateElement_Patches
{
	public static SpriteProvider CreateOrGetSpriteProvider(Slot root, Uri atlasUri, bool alt)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Slot connectorManager = Helpers.GetConnectorManager(((Worker)root).LocalUser);
		Slot val = connectorManager.FindChildOrAdd(alt ? "Alt Sprite" : "Normal Sprite", true);
		SpriteProvider componentOrAttach = ((ContainerWorker<Component>)(object)val).GetComponentOrAttach<SpriteProvider>((Predicate<SpriteProvider>)null);
		StaticTexture2D val2 = SlotAssets.AttachTexture(val, atlasUri, true, false, false, false, (TextureWrapMode)1, (int?)null);
		((SyncField<TextureFilterMode?>)(object)((StaticTextureProvider<Texture2D, Bitmap2D, BitmapMetadata, Texture2DVariantDescriptor>)(object)val2).FilterMode).Value = LenowoTweeks.wireTextureFilterMode.Value;
		((SyncField<bool>)(object)val2.MipMaps).Value = false;
		((SyncRef<IAssetProvider<ITexture2D>>)(object)componentOrAttach.Texture).Target = (IAssetProvider<ITexture2D>)(object)val2;
		return componentOrAttach;
	}

	public static void ApplyConnector(Image image, Type elementType = null, bool output = false, IWorldElement targetConnection = null)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		Slot modSharedSlot = Helpers.GetModSharedSlot(((Worker)image).LocalUser);
		if (!LenowoTweeks.useCustomProtofluxConnections.Value || modSharedSlot == null)
		{
			return;
		}
		Slot connectorManager = Helpers.GetConnectorManager(((Worker)image).LocalUser);
		UI_UnlitMaterial componentOrAttach = ((ContainerWorker<Component>)(object)connectorManager).GetComponentOrAttach<UI_UnlitMaterial>((Predicate<UI_UnlitMaterial>)null);
		((SyncField<BlendMode>)(object)componentOrAttach.BlendMode).Value = (BlendMode)2;
		((SyncField<Sidedness>)(object)componentOrAttach.Sidedness).Value = (Sidedness)3;
		((SyncField<ZWrite>)(object)componentOrAttach.ZWrite).Value = (ZWrite)2;
		((SyncField<int>)(object)((UI_StencilMaterial)componentOrAttach).RenderQueue).Value = 3000;
		Helpers.SetConfigVariable<colorX>(((Worker)image).LocalUser, "Protoflux.ConnectorColor", LenowoTweeks.connectorTextureColor.Value, connectorManager);
		Helpers.DriveFromVariable<colorX>(((Worker)image).LocalUser, "Protoflux.ConnectorColor", (IField<colorX>)(object)componentOrAttach.Tint, connectorManager);
		((SyncRef<IAssetProvider<Material>>)(object)((ImageBase)image).Material).Target = (IAssetProvider<Material>)(object)componentOrAttach;
		int num = 4;
		if (elementType != null)
		{
			num = (typeof(IVector).IsAssignableFrom(elementType) ? (ReflectionExtensions.GetVectorDimensions(elementType) - 1) : 0);
		}
		float4 value = default(float4);
		((float4)(ref value))._002Ector(0.5f, 0f, 0f, 0f);
		bool flag = (LenowoTweeks.wireConnectorFlipUV.Value ? (!output) : output);
		Rect value2 = default(Rect);
		((Rect)(ref value2))._002Ector(flag ? 0.5f : 0f, 0.2f * (float)(4 - num), 1f, 0.2f);
		Slot val = connectorManager.FindChildOrAdd($"Connector_{num}_{output}_True", true);
		Slot val2 = connectorManager.FindChildOrAdd($"Connector_{num}_{output}_False", true);
		SpriteProvider componentOrAttach2 = ((ContainerWorker<Component>)(object)val).GetComponentOrAttach<SpriteProvider>((Predicate<SpriteProvider>)null);
		SpriteProvider componentOrAttach3 = ((ContainerWorker<Component>)(object)val2).GetComponentOrAttach<SpriteProvider>((Predicate<SpriteProvider>)null);
		Uri value3 = LenowoTweeks.customProtofluxConnectorUIX.Value;
		Uri atlasUri = LenowoTweeks.customProtofluxEmptyConnectorUIX.Value ?? LenowoTweeks.customProtofluxConnectorUIX.Value;
		SpriteProvider val3 = CreateOrGetSpriteProvider(((Component)image).Slot, value3, alt: false);
		SpriteProvider val4 = CreateOrGetSpriteProvider(((Component)image).Slot, atlasUri, alt: true);
		BooleanReferenceDriver<IAssetProvider<Sprite>> val5 = ((ContainerWorker<Component>)(object)((Component)image).Slot).AttachComponent<BooleanReferenceDriver<IAssetProvider<Sprite>>>(true, (Action<BooleanReferenceDriver<IAssetProvider<Sprite>>>)null);
		if (val3 != null)
		{
			((SyncRef<IAssetProvider<ITexture2D>>)(object)componentOrAttach2.Texture).Target = ((SyncRef<IAssetProvider<ITexture2D>>)(object)val3.Texture).Target;
			((SyncField<Rect>)(object)componentOrAttach2.Rect).Value = value2;
			((SyncField<float4>)(object)componentOrAttach2.Borders).Value = value;
			val5.TrueTarget.Target = (IAssetProvider<Sprite>)(object)componentOrAttach2;
		}
		if (val4 != null)
		{
			((SyncRef<IAssetProvider<ITexture2D>>)(object)componentOrAttach3.Texture).Target = ((SyncRef<IAssetProvider<ITexture2D>>)(object)val4.Texture).Target;
			((SyncField<Rect>)(object)componentOrAttach3.Rect).Value = value2;
			((SyncField<float4>)(object)componentOrAttach3.Borders).Value = value;
			val5.FalseTarget.Target = (IAssetProvider<Sprite>)(object)componentOrAttach3;
		}
		((SyncRef<SyncRef<IAssetProvider<Sprite>>>)(object)val5.TargetReference).Target = (SyncRef<IAssetProvider<Sprite>>)(object)((ImageBase)image).Sprite;
		IField<bool> timeSineDriverSource = Helpers.GetTimeSineDriverSource(((Worker)image).LocalUser);
		ISyncRef val6 = (ISyncRef)(object)((targetConnection is ISyncRef) ? targetConnection : null);
		if (val6 != null)
		{
			ProtoFluxInputProxy component = ((ContainerWorker<Component>)(object)((Component)image).Slot).GetComponent<ProtoFluxInputProxy>((Predicate<ProtoFluxInputProxy>)null, false);
			ReferenceEqualityDriver<ProtoFluxWireManager> val7 = ((ContainerWorker<Component>)(object)((Component)image).Slot).AttachComponent<ReferenceEqualityDriver<ProtoFluxWireManager>>(true, (Action<ReferenceEqualityDriver<ProtoFluxWireManager>>)null);
			((SyncField<bool>)(object)val7.Invert).Value = true;
			if (component != null)
			{
				((SyncRef<SyncRef<ProtoFluxWireManager>>)(object)val7.TargetReference).Target = (SyncRef<ProtoFluxWireManager>)(object)((ProtoFluxWireProxy<INodeOutput>)(object)component).Wire;
			}
			ProtoFluxImpulseProxy component2 = ((ContainerWorker<Component>)(object)((Component)image).Slot).GetComponent<ProtoFluxImpulseProxy>((Predicate<ProtoFluxImpulseProxy>)null, false);
			if (component2 != null)
			{
				((SyncRef<SyncRef<ProtoFluxWireManager>>)(object)val7.TargetReference).Target = (SyncRef<ProtoFluxWireManager>)(object)((ProtoFluxWireProxy<INodeOperation>)(object)component2).Wire;
			}
			((SyncRef<IField<bool>>)(object)val7.Target).Target = (IField<bool>)(object)val5.State;
			Helpers.DriveFromTSD(((Worker)image).LocalUser, (IField<bool>)(object)((ComponentBase<Component>)(object)val7).EnabledField);
			return;
		}
		INodeOutput val8 = (INodeOutput)(object)((targetConnection is INodeOutput) ? targetConnection : null);
		if (val8 != null)
		{
			ReferenceList<ProtoFluxNode> componentOrAttach4 = ((ContainerWorker<Component>)(object)((Component)image).Slot).GetComponentOrAttach<ReferenceList<ProtoFluxNode>>((Predicate<ReferenceList<ProtoFluxNode>>)null);
			if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach4.References).Count == 0)
			{
				componentOrAttach4.References.Add((ProtoFluxNode)null);
			}
			ReferenceEqualityDriver<ProtoFluxNode> val9 = SetupEqualityDriver<ProtoFluxNode>(((Component)image).Slot, (IField<bool>)(object)val5.State, ((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach4.References).GetElement(0));
			Helpers.DriveFromTSD(((Worker)image).LocalUser, (IField<bool>)(object)((ComponentBase<Component>)(object)val9).EnabledField);
			return;
		}
		INodeOperation val10 = (INodeOperation)(object)((targetConnection is INodeOperation) ? targetConnection : null);
		if (val10 != null)
		{
			ReferenceList<ProtoFluxNode> componentOrAttach5 = ((ContainerWorker<Component>)(object)((Component)image).Slot).GetComponentOrAttach<ReferenceList<ProtoFluxNode>>((Predicate<ReferenceList<ProtoFluxNode>>)null);
			if (((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach5.References).Count == 0)
			{
				componentOrAttach5.References.Add((ProtoFluxNode)null);
			}
			ReferenceEqualityDriver<ProtoFluxNode> val11 = SetupEqualityDriver<ProtoFluxNode>(((Component)image).Slot, (IField<bool>)(object)val5.State, ((SyncElementList<SyncRef<ProtoFluxNode>>)(object)componentOrAttach5.References).GetElement(0));
			Helpers.DriveFromTSD(((Worker)image).LocalUser, (IField<bool>)(object)((ComponentBase<Component>)(object)val11).EnabledField);
		}
	}

	private static ReferenceEqualityDriver<T> SetupEqualityDriver<T>(Slot s, IField<bool> bv, SyncRef<T> field) where T : class, IWorldElement
	{
		ReferenceEqualityDriver<T> val = ((ContainerWorker<Component>)(object)s).AttachComponent<ReferenceEqualityDriver<T>>(true, (Action<ReferenceEqualityDriver<T>>)null);
		((SyncRef<IField<bool>>)(object)val.Target).Target = bv;
		((SyncRef<SyncRef<T>>)(object)val.TargetReference).Target = field;
		((SyncField<bool>)(object)val.Invert).Value = true;
		Helpers.DriveFromVariable<bool>(((Worker)s).LocalUser, "TSD.Source", (IField<bool>)(object)((ComponentBase<Component>)(object)val).EnabledField);
		return val;
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateInputElement")]
	public static void TweakInputElementVisual(ProtoFluxNodeVisual __instance, Type elementType, ISyncRef input, Slot __result)
	{
		ApplyConnector(__result.GetComponentInChildren<Image>((Predicate<Image>)null, false, false), elementType, output: false, (IWorldElement)(object)input);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateOutputElement")]
	public static void TweakOutputElementVisual(ProtoFluxNodeVisual __instance, Type elementType, INodeOutput output, Slot __result)
	{
		ApplyConnector(__result.GetComponentInChildren<Image>((Predicate<Image>)null, false, false), elementType, output: true, (IWorldElement)(object)output);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateImpulseElement")]
	public static void TweakImpulseElementVisual(ProtoFluxNodeVisual __instance, ISyncRef input, Slot __result)
	{
		ApplyConnector(__result.GetComponentInChildren<Image>((Predicate<Image>)null, false, false), null, output: true, (IWorldElement)(object)input);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateOperationElement")]
	public static void TweakOperationElementVisual(ProtoFluxNodeVisual __instance, INodeOperation operation, Slot __result)
	{
		ApplyConnector(__result.GetComponentInChildren<Image>((Predicate<Image>)null, false, false), null, output: false, (IWorldElement)(object)operation);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void TweakNodeVisual(ProtoFluxNodeVisual __instance)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		if (!LenowoTweeks.disableRelayBackground.Value)
		{
			return;
		}
		ProtoFluxNode target = ((SyncRef<ProtoFluxNode>)(object)__instance.Node).Target;
		if (target == null)
		{
			return;
		}
		Type type = ((object)target).GetType();
		if (type == typeof(ContinuationRelay) || type == typeof(CallRelay) || type == typeof(AsyncCallRelay))
		{
			((IEnumerable<Slot>)(object)((Component)__instance).Slot.Children).First().ActiveSelf = false;
			return;
		}
		Type baseType = type.BaseType;
		if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ValueCast<, >))
		{
			((IEnumerable<Slot>)(object)((Component)__instance).Slot.Children).First().ActiveSelf = false;
		}
		if (type.IsGenericType)
		{
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!(genericTypeDefinition != typeof(ValueRelay<>)) || !(genericTypeDefinition != typeof(ObjectRelay<>)) || !(genericTypeDefinition != typeof(ValueToObjectCast<>)) || !(genericTypeDefinition != typeof(ObjectCast<, >)) || !(genericTypeDefinition != typeof(NullableToObjectCast<>)) || !(genericTypeDefinition != typeof(ValueCast<, >)))
			{
				((IEnumerable<Slot>)(object)((Component)__instance).Slot.Children).First().ActiveSelf = false;
			}
		}
	}
}

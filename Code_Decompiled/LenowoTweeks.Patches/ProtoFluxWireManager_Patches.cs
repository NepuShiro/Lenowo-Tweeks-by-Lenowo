using System;
using Elements.Assets;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using Renderite.Shared;

namespace LenowoTweeks.Patches;

[HarmonyPatch]
public class ProtoFluxWireManager_Patches
{
	private static UI_UnlitMaterial WireMaterial(Slot assetsSlot)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		bool flag = default(bool);
		UI_UnlitMaterial componentOrAttach = ((ContainerWorker<Component>)(object)assetsSlot).GetComponentOrAttach<UI_UnlitMaterial>(ref flag, (Predicate<UI_UnlitMaterial>)null);
		StaticTexture2D componentOrAttach2 = ((ContainerWorker<Component>)(object)((Component)componentOrAttach).Slot).GetComponentOrAttach<StaticTexture2D>((Predicate<StaticTexture2D>)null);
		((SyncField<Uri>)(object)((StaticAssetProvider<Texture2D, BitmapMetadata, Texture2DVariantDescriptor>)(object)componentOrAttach2).URL).Value = LenowoTweeks.customProtofluxWireUIX.Value;
		((SyncField<TextureWrapMode>)(object)componentOrAttach2.WrapModeU).Value = LenowoTweeks.wireImageWrapMode.Value;
		((SyncField<TextureWrapMode>)(object)componentOrAttach2.WrapModeV).Value = (TextureWrapMode)1;
		((SyncField<TextureFilterMode?>)(object)((StaticTextureProvider<Texture2D, Bitmap2D, BitmapMetadata, Texture2DVariantDescriptor>)(object)componentOrAttach2).FilterMode).Value = LenowoTweeks.wireTextureFilterMode.Value;
		((SyncField<bool>)(object)componentOrAttach2.MipMaps).Value = false;
		((SyncRef<IAssetProvider<ITexture2D>>)(object)componentOrAttach.Texture).Target = (IAssetProvider<ITexture2D>)(object)componentOrAttach2;
		Helpers.SetConfigVariable<colorX>(((Worker)assetsSlot).LocalUser, "Protoflux.WireColor", LenowoTweeks.wireTextureColor.Value, assetsSlot);
		if (flag)
		{
			Helpers.DriveFromVariable<colorX>(((Worker)assetsSlot).LocalUser, "Protoflux.WireColor", (IField<colorX>)(object)componentOrAttach.Tint, assetsSlot);
		}
		((SyncField<Sidedness>)(object)componentOrAttach.Sidedness).Value = (Sidedness)3;
		((SyncField<BlendMode>)(object)componentOrAttach.BlendMode).Value = (BlendMode)2;
		((SyncField<ZWrite>)(object)componentOrAttach.ZWrite).Value = (ZWrite)2;
		((SyncField<int>)(object)((UI_StencilMaterial)componentOrAttach).RenderQueue).Value = 2000;
		return componentOrAttach;
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxWireManager), "OnAttach")]
	public static void ModifyConnectorWireMaterial(ProtoFluxWireManager __instance)
	{
		if (Helpers.ModShouldRun(((Component)__instance).Slot) && LenowoTweeks.useCustomProtofluxConnections.Value)
		{
			Slot wireManager = Helpers.GetWireManager(((Worker)__instance).LocalUser);
			UI_UnlitMaterial target = WireMaterial(wireManager);
			SyncRef<MeshRenderer> val = (SyncRef<MeshRenderer>)(object)((Worker)__instance).GetSyncMember("_renderer");
			((SyncRef<IAssetProvider<Material>>)(object)val.Target.Material).Target = (IAssetProvider<Material>)(object)target;
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxWireManager), "OnChanges")]
	public static void OnChanges(ProtoFluxWireManager __instance, SyncRef<MeshRenderer> ____renderer, SyncRef<StripeWireMesh> ____wireMesh)
	{
		if (!((Worker)__instance).IsRemoved && !((SyncElement)____renderer).IsRemoved && !((Worker)____renderer.Target).IsRemoved && !((SyncElement)____wireMesh).IsRemoved && !((Worker)____wireMesh.Target).IsRemoved && !((Worker)((Component)__instance).Slot).IsRemoved && LenowoTweeks.useCustomProtofluxConnections.Value && Helpers.ModShouldRun(((Component)__instance).Slot))
		{
			Slot wireManager = Helpers.GetWireManager(((Worker)__instance).LocalUser);
			((SyncRef<IAssetProvider<Material>>)(object)____renderer.Target.Material).Target = (IAssetProvider<Material>)(object)WireMaterial(wireManager);
		}
	}
}

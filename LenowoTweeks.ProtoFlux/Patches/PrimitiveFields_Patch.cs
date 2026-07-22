using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;

using HarmonyLib;

namespace LenowoTweeks.ProtoFlux.Patches;

[HarmonyPatch]
public class PrimitiveFields_Patch
{
	[HarmonyPostfix]
	[HarmonyPatch(typeof(PrimitiveMemberEditor), "BuildUI")]
	public static void Postfix(PrimitiveMemberEditor __instance, SyncRef<TextEditor> ____textEditor, RelayRef<IField> ____target)
	{
		Slot textEditorSlot = ____textEditor.Target.Slot;
		if (__instance.World.IsUserspace())
		{
			var parentDynSpace = __instance.Slot.GetComponentInParents<DynamicVariableSpace>();
			if (parentDynSpace != null && parentDynSpace.SpaceName.Value == "Config") return;
		}

		if (!LenowoTweeks_ProtoFlux.expandedProtofluxStringInputs.Value) return;

		if (__instance.Slot.GetComponentInParents<ProtoFluxNodeVisual>() == null) return;


		if (____target.Target.ValueType == typeof(string) || ____target.Target.ValueType == typeof(Uri))
		{
			Text text = ____textEditor.Target.Text.Target as Text;
			text.Size.Value = 16f;
			text.HorizontalAutoSize.Value = false;
			text.VerticalAutoSize.Value = false;
			____textEditor.Target.Slot.GetComponent<LayoutElement>().MinWidth.Value = 96f;
			OverlappingLayout overlap = textEditorSlot.GetComponentOrAttach<OverlappingLayout>();
			overlap.PaddingBottom.Value = 2f;
			overlap.PaddingTop.Value = 2f;
			overlap.PaddingLeft.Value = 6f;
			overlap.PaddingRight.Value = 6f;
			if (true)
			{
				textEditorSlot.GetComponentOrAttach<ContentSizeFitter>().VerticalFit.Value = SizeFit.PreferredSize;
				textEditorSlot.Parent.GetComponentOrAttach<ContentSizeFitter>().VerticalFit.Value = SizeFit.PreferredSize;
				textEditorSlot.Parent.Parent.GetComponentOrAttach<OverlappingLayout>();
				textEditorSlot.Parent.Parent.GetComponentOrAttach<ContentSizeFitter>().VerticalFit.Value = SizeFit.PreferredSize;
				textEditorSlot.Parent.Parent.Parent.GetComponentOrAttach<ContentSizeFitter>().VerticalFit.Value = SizeFit.MinSize;
				textEditorSlot.GetComponentInParents<Canvas>().Slot.GetComponentOrAttach<ContentSizeFitter>().VerticalFit.Value = SizeFit.MinSize;
				textEditorSlot.GetComponentInParents<Canvas>().Size.Value.SetY(0f);
			}
			if (true)
			{
				textEditorSlot.GetComponentOrAttach<ContentSizeFitter>().HorizontalFit.Value = SizeFit.PreferredSize;
				textEditorSlot.Parent.Parent.GetComponentOrAttach<OverlappingLayout>();
				textEditorSlot.Parent.Parent.GetComponentOrAttach<ContentSizeFitter>().HorizontalFit.Value = SizeFit.PreferredSize;
				textEditorSlot.Parent.Parent.Parent.GetComponentOrAttach<ContentSizeFitter>().HorizontalFit.Value = SizeFit.MinSize;
				textEditorSlot.GetComponentInParents<Canvas>().Slot.GetComponentOrAttach<ContentSizeFitter>().HorizontalFit.Value = SizeFit.MinSize;
				textEditorSlot.GetComponentInParents<Canvas>().Size.Value.SetX(0f);
			}
		}
	}
}

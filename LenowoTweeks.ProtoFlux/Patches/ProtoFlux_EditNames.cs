using Elements.Core;

using FrooxEngine;
using FrooxEngine.FrooxEngine.ProtoFlux.CoreNodes;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes.Casts;
using FrooxEngine.UIX;

using HarmonyLib;


namespace LenowoTweeks.ProtoFlux.Patches;

[HarmonyPatch]
public class ProtoFlux_EditNames
{
	public static Dictionary<ProtoFluxNodeGroup, KeyValuePair<int, List<string>>> GroupTagsToIgnore = [];

	[HarmonyPostfix]
	[HarmonyPatch(typeof(ProtoFluxNodeVisual), "BuildUI")]
	public static void AddEditableNamesPatch(ProtoFluxNodeVisual __instance)
	{
		if (!LenowoTweeks_ProtoFlux.protofluxEditableNames.Value) return;
		ProtoFluxNode node = __instance.Node.Target;

		Type nodeType = node.GetType();
		Type baseType = nodeType.IsGenericType ? nodeType.GetGenericTypeDefinition() : nodeType;

		if (node.Slot.Tag == node.LocalUserSpace.Tag) node.Slot.Tag = null;

		if (LenowoTweeks_ProtoFlux.allProtofluxEditableNames.Value)
		{
			List<Type> InvalidForRenaming = [
				// relays
				typeof(ValueRelay<>), typeof(ObjectRelay<>),
				// casts (except value->value casts)
				typeof(ObjectCast<,>), typeof(ValueToObjectCast<>), typeof(NullableToObjectCast<>)
			];

			if (InvalidForRenaming.Contains(baseType)) return;
			// value->value casts
			var splitType = baseType.Name.Split("_");
			if (splitType[0] == "Cast" && splitType[2] == "To") return;
		}
		else
		{
			List<Type> ValidForRenaming = [
				// inputs
				typeof(ValueObjectInput<>), typeof(ValueInput<>), typeof(RefObjectInput<>), typeof(AssetInput<>),
				// displays
				typeof(ObjectDisplay<>), typeof(ValueDisplay<>), typeof(ImpulseDisplay),
				// calls
				typeof(CallInput), typeof(AsyncCallInput),
				
				// sources
				typeof(ValueSource<>), typeof(ObjectValueSource<>), typeof(ElementSource<>), typeof(ReferenceSource<>),
				// drives
				typeof(ValueFieldDrive<>), typeof(ObjectFieldDrive<>), typeof(ReferenceDrive<>)
			];

			if (!ValidForRenaming.Contains(baseType)) return;
		}

		Slot TitleHolder = __instance.Slot.Children.ToList()[1];
		Slot originalText = TitleHolder.FindChild("Text");
		Slot overlapping = __instance.Slot.FindChild("Overlapping Layout");
		Slot overview = overlapping?.FindChild("Overview");
		Slot overviewText = (overview != null && overview.ChildrenCount >= 1) ? overview.Children.First() : null;
		originalText ??= overviewText;
		if (originalText == null)
		{
			originalText = __instance.Slot.Children.First();
			originalText.AttachComponent<Text>().Size.Value = 0;
		}
		UIBuilder ui = new(TitleHolder);
		var horizontal = TitleHolder.AttachComponent<HorizontalLayout>();
		horizontal.Spacing.Value = 6;
		horizontal.PaddingLeft.Value = 3;
		horizontal.PaddingRight.Value = 3;
		horizontal.HorizontalAlign.Value = LayoutHorizontalAlignment.Left;
		horizontal.ForceExpandWidth.Value = false;
		Button editButton = ui.Button(new Uri("resdb:///8e79ad496c6cb57feb25c60ae879116b3e4da6609793b52d853bb6f1ffe3ad09.png"), buttonTint: colorX.White, spriteTint: colorX.White);
		var layout = editButton.Slot.AttachComponent<LayoutElement>();
		layout.MinWidth.Value = 16;
		layout.MinHeight.Value = 16;
		TextField field = editButton.Slot.AttachComponent<TextField>();
		TextEditor editor = editButton.Slot.GetComponent<TextEditor>();
		Text originalTextComponent = originalText.GetComponent<Text>();
		editor.Text.Target = originalTextComponent;

		IField<string> NameField = node.Slot.Tag_Field;

		if (!string.IsNullOrEmpty(node.Slot.Tag))
		{
			// edge case to detect: if a bunch of nodes also have the EXACT tag
			// (like if the world has workspaces that have a tag)

			List<string> tagsToIgnore = [];
			if (node.Group != null)
			{
				if (GroupTagsToIgnore.ContainsKey(node.Group))
				{
					var kv = GroupTagsToIgnore[node.Group];
					if (node.Engine.UpdateTick - kv.Key > 5) CalculateGroup(node, tagsToIgnore);
					else tagsToIgnore = kv.Value;
				}
				else
				{
					CalculateGroup(node, tagsToIgnore);
				}
				string thisNodeTag = node.Slot.Tag;

				if (tagsToIgnore.Contains(thisNodeTag))
				{
					NameField.Value = originalTextComponent.Content.Value;
				}
			}
		}

		if (string.IsNullOrEmpty(NameField.Value)) NameField.Value = originalTextComponent.Content.Value;
		ValueCopy<string> vc = originalText.AttachComponent<ValueCopy<string>>();
		vc.Source.Target = NameField;
		vc.Target.Target = originalTextComponent.Content;
		vc.WriteBack.Value = true;
		Slot imageSlot = editButton.Slot.Children.First();
		var provider = editButton.Slot.AttachComponent<SpriteProvider>();
		var texture = editButton.Slot.AttachComponent<StaticTexture2D>();
		texture.URL.Value = imageSlot.GetComponent<StaticTexture2D>().URL.Value;
		provider.Texture.Target = texture;
		editButton.Slot.GetComponent<Image>().Sprite.Target = provider;
		imageSlot.Destroy();
		editButton.Slot.OrderOffset = -1;
		originalText.AttachComponent<LayoutElement>().FlexibleWidth.Value = 1;

		if (overviewText != null)
		{
			ValueCopy<string> vc2 = overviewText.AttachComponent<ValueCopy<string>>();
			vc2.Source.Target = NameField;
			vc2.Target.Target = overviewText.GetComponent<Text>().Content;
			vc2.WriteBack.Value = true;
		}

		if (baseType == typeof(CallInput) || baseType == typeof(AsyncCallInput))
		{
			// call inputs get the button text also replaced
			var textSlot = __instance.Slot.FindChild("Overlapping Layout").FindChild("Panel").Children.First().Children.First();
			var field2 = textSlot.GetComponent<LocaleStringDriver>().Format;
			ValueCopy<string> vc3 = textSlot.AttachComponent<ValueCopy<string>>();
			vc3.Source.Target = NameField;
			vc3.Target.Target = field2;
		}
	}

	public static void CalculateGroup(ProtoFluxNode node, List<string> tagsToIgnore)
	{
		if (node.Group == null) return;
		Dictionary<string, int> allNodeTags = [];
		foreach (var groupNode in node.Group.Nodes)
		{
			Slot groupSlot = groupNode.Slot;
			string tag = groupSlot.Tag;
			if (tag == null) continue;
			if (allNodeTags.ContainsKey(tag))
			{
				allNodeTags[tag]++;
			}
			else allNodeTags.Add(tag, 1);
		}
		// 5 is probably a reasonable threshold. usually you wont have this many of the exact same name.
		// if anything, there would only ever be 1 of any given tag.
		List<int> allCounts = [.. allNodeTags.Values.Where(v => v >= 5)];
		allCounts.Sort();
		for (int i = 0; i < MathX.Min(5, allCounts.Count); i++)
		{
			foreach (var (x, _) in allNodeTags.Where(v => v.Value == allCounts[i]))
			{
				tagsToIgnore.Add(x);
			}
		}
		if (GroupTagsToIgnore.ContainsKey(node.Group)) GroupTagsToIgnore[node.Group] = new(node.Engine.UpdateTick, tagsToIgnore);
		else GroupTagsToIgnore.Add(node.Group, new(node.Engine.UpdateTick, tagsToIgnore));
	}
}

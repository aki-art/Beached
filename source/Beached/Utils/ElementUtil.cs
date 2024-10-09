using Beached.Content;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Beached.Utils
{
	public class ElementUtil
	{
		public static readonly Dictionary<SimHashes, string> SimHashNameLookup = [];
		public static readonly Dictionary<string, object> ReverseSimHashNameLookup = [];
		public static readonly List<ElementInfo> elements = [];

		public static SimHashes RegisterSimHash(string name)
		{
			var simHash = (SimHashes)Hash.SDBMLower(name);
			SimHashNameLookup.Add(simHash, name);
			ReverseSimHashNameLookup.Add(name, simHash);

			return simHash;
		}

		public static Substance CreateSubstance(SimHashes id, bool specular, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor, string normal)
		{
			var animFile = Assets.Anims.Find(a => a.name == anim);

			if (animFile == null)
			{
				animFile = Assets.Anims.Find(a => a.name == "glass_kanim");
			}

			var newMaterial = new Material(material);

			if (state == Element.State.Solid)
			{
				SetTexture(newMaterial, id.ToString().ToLowerInvariant(), "_MainTex");

				if (specular)
				{
					SetTexture(newMaterial, id.ToString().ToLowerInvariant() + "_spec", "_ShineMask");

					if (specularColor.HasValue)
					{
						newMaterial.SetColor("_ShineColour", specularColor.Value);
					}
				}

				if (!normal.IsNullOrWhiteSpace())
				{
					SetTexture(newMaterial, normal, "_NormalNoise");
				}
			}

			var substance = ModUtil.CreateSubstance(id.ToString(), state, animFile, newMaterial, color, uiColor, conduitColor);

			return substance;
		}

		// TODO: load from an assetbundle later
		private static void SetTexture(Material material, string texture, string property)
		{
			var path = Path.Combine(Mod.folder, "textures", texture + ".png");

			if (ModAssets.TryLoadTexture(path, out var tex))
			{
				material.SetTexture(property, tex);
			}
		}

		public static ElementsAudio.ElementAudioConfig GetCrystalAudioConfig(SimHashes id)
		{
			var crushedIce = ElementsAudio.Instance.GetConfigForElement(SimHashes.CrushedIce);
			Log.Debug($"crystal audio index: {(int)Elements.crystalAmbiance}");
			return new ElementsAudio.ElementAudioConfig()
			{
				elementID = id,
				ambienceType = AmbienceType.None,
				solidAmbienceType = Elements.crystalAmbiance,
				miningSound = "PhosphateNodule", // kind of gritty glassy
				miningBreakSound = crushedIce.miningBreakSound,
				oreBumpSound = crushedIce.oreBumpSound,
				floorEventAudioCategory = "tileglass", // proper glassy sound
				creatureChewSound = crushedIce.creatureChewSound
			};
		}

		public static ElementsAudio.ElementAudioConfig CopyElementAudioConfig(ElementsAudio.ElementAudioConfig reference, SimHashes id)
		{
			return new ElementsAudio.ElementAudioConfig()
			{
				elementID = reference.elementID,
				ambienceType = reference.ambienceType,
				solidAmbienceType = reference.solidAmbienceType,
				miningSound = reference.miningSound,
				miningBreakSound = reference.miningBreakSound,
				oreBumpSound = reference.oreBumpSound,
				floorEventAudioCategory = reference.floorEventAudioCategory,
				creatureChewSound = reference.creatureChewSound,
			};
		}

		public static ElementsAudio.ElementAudioConfig CopyElementAudioConfig(SimHashes referenceId, SimHashes id)
		{
			var reference = ElementsAudio.Instance.GetConfigForElement(referenceId);

			return new ElementsAudio.ElementAudioConfig()
			{
				elementID = reference.elementID,
				ambienceType = reference.ambienceType,
				solidAmbienceType = reference.solidAmbienceType,
				miningSound = reference.miningSound,
				miningBreakSound = reference.miningBreakSound,
				oreBumpSound = reference.oreBumpSound,
				floorEventAudioCategory = reference.floorEventAudioCategory,
				creatureChewSound = reference.creatureChewSound,
			};
		}
	}
}

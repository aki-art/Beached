using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	// Cloning a symbol in a kanim
	// Todo: make the texture empty
	public class KGlobalAnimParserPatch
	{
		private static readonly Dictionary<KAnimHashedString, CloneSymbolInfo> cloneSymbols = new()
		{
			{
				new KAnimHashedString("pincher_kanim"),
				new CloneSymbolInfo()
				{
					targetSymbol = new KAnimHashedString("body"),
					newSymbolName = "beached_limpetgrowth"
				}
			}
		};

		public struct CloneSymbolInfo
		{
			public KAnimHashedString targetSymbol;
			public string newSymbolName;
		}

		private static readonly HashSet<KAnimHashedString> dupeAnims = [];

		// Copy the symbol itself in the build file
		[HarmonyPatch(typeof(KGlobalAnimParser), nameof(KGlobalAnimParser.ParseBuildData))]
		public class KGlobalAnimParser_ParseBuildData_Patch
		{
			public static void Postfix(KBatchGroupData data, KAnimHashedString fileNameHash, FastReader reader, int __result)
			{
				var build = data.builds[__result];

				if (cloneSymbols.TryGetValue(fileNameHash, out CloneSymbolInfo cloneSymbol))
				{
					var symbolIndex = build.symbols.Length;

					foreach (var symbol in build.symbols)
					{
						if (symbol.hash == cloneSymbol.targetSymbol)
						{
							CloneSymbol(data, build, symbolIndex++, symbol, cloneSymbol.newSymbolName);
							return;
						}
					}
				}
			}

			private static KAnim.Build.Symbol CloneSymbol(KBatchGroupData data, KAnim.Build build, int index, KAnim.Build.Symbol srcSymbol, string newName)
			{
				var symbol = new KAnim.Build.Symbol
				{
					build = srcSymbol.build,
					hash = new KAnimHashedString(newName),
					colourChannel = srcSymbol.colourChannel,
					flags = srcSymbol.flags,
					firstFrameIdx = data.symbolFrameInstances.Count,
					numFrames = srcSymbol.numFrames,
					symbolIndexInSourceBuild = index,
					path = new KAnimHashedString(newName), // TODO: keep folder // ver 10 anims have folders, in folder/name format
					numLookupFrames = srcSymbol.numLookupFrames
				};

				for (int i = srcSymbol.firstFrameIdx; i < srcSymbol.numFrames; i++)
				{
					var srcFrameInstance = data.symbolFrameInstances[i];

					var symbolFrameInstance = new KAnim.Build.SymbolFrameInstance
					{
						sourceFrameNum = srcFrameInstance.sourceFrameNum, //  ???
						duration = srcFrameInstance.duration,
						buildImageIdx = srcFrameInstance.buildImageIdx,
						symbolIdx = data.GetSymbolCount(),
						bboxMin = new Vector2(srcFrameInstance.bboxMin.x, srcFrameInstance.bboxMin.y),
						bboxMax = new Vector2(srcFrameInstance.bboxMax.x, srcFrameInstance.bboxMax.y),
						uvMin = new Vector2(srcFrameInstance.uvMin.x, srcFrameInstance.uvMin.y),
						uvMax = new Vector2(srcFrameInstance.uvMax.x, srcFrameInstance.uvMax.y)
						//uvMin = Vector2.zero,
						//uvMax = new Vector2(0.000001f, 0.000001f)
					};

					data.symbolFrameInstances.Add(symbolFrameInstance);
				}

				data.AddBuildSymbol(symbol);

				build.symbols = build.symbols.AddToArray(symbol);
				HashCache.Get().Add(symbol.hash.hash, newName);

				return symbol;
			}
		}

		// copy every instance of animation of the cloned symbol

		[HarmonyPatch(typeof(KGlobalAnimParser), nameof(KGlobalAnimParser.ParseAnimData))]
		public class KGlobalAnimParser_ParseAnimData_Patch
		{
			public static void Postfix(KBatchGroupData data, HashedString fileNameHash, FastReader reader, KAnimFileData animFile)
			{
				// TODO: check by batchtag to apply to grouped animations
				if (cloneSymbols.TryGetValue(fileNameHash, out CloneSymbolInfo cloneSymbol))
				{
					CopyAnimation(data, animFile, cloneSymbol);
					return;
				}
			}

			private static void CopyAnimation(KBatchGroupData data, KAnimFileData animFile, CloneSymbolInfo cloneSymbol)
			{
				var frameElementPadding = 0;

				for (var i = animFile.firstAnimIndex; i < animFile.animCount; i++)
				{
					var anim = data.GetAnim(i);
					for (var frameIndex = anim.firstFrameIdx; frameIndex < anim.numFrames + anim.firstFrameIdx; frameIndex++)
					{
						var frame = data.animFrames[frameIndex];
						// struct shenanigans
						data.animFrames[frameIndex] = new KAnim.Anim.Frame()
						{
							firstElementIdx = frameElementPadding + frame.firstElementIdx,
							numElements = frame.numElements,
							hasHead = frame.hasHead
						};

						frame = data.animFrames[frameIndex];

						for (int elementIdx = frame.firstElementIdx; elementIdx < frame.firstElementIdx + frame.numElements; elementIdx++)
						{
							var element = data.frameElements[elementIdx];

							if (element.symbol == cloneSymbol.targetSymbol)
							{
								var frameElement = new KAnim.Anim.FrameElement
								{
									symbol = cloneSymbol.newSymbolName,
									frame = element.frame,
									multAlpha = element.multAlpha
								};

								frameElement.transform.m00 = element.transform.m00;
								frameElement.transform.m01 = element.transform.m01;
								frameElement.transform.m02 = element.transform.m02;
								frameElement.transform.m10 = element.transform.m10;
								frameElement.transform.m11 = element.transform.m11;
								frameElement.transform.m12 = element.transform.m12;

								data.frameElements.Insert(elementIdx, frameElement); // elementIdx + 1 to put it behind
								elementIdx++; // skip checking the next element otherwise it's an infinite loop, or just rechecking self
								frame.numElements++;
								animFile.elementCount++;
								frameElementPadding++;
							}
						}

					}
				}
			}

			private static void LogAnimData(KBatchGroupData data, KAnimFileData animFile)
			{
				Log.Debug("------------------------------------------------------------------------------------------------");
				for (var i = animFile.firstElementIndex; i < animFile.animCount; i++)
				{
					var anim = data.GetAnim(i);
					Log.Debug("Anim: " + anim.name);
					for (var frameIndex = anim.firstFrameIdx; frameIndex < anim.numFrames + anim.firstFrameIdx; frameIndex++)
					{
						var frame = data.animFrames[frameIndex];
						Log.Debug("\tFrame: " + frame.firstElementIdx);
						for (int elementIdx = frame.firstElementIdx; elementIdx < frame.firstElementIdx + frame.numElements; elementIdx++)
						{
							var element = data.frameElements[elementIdx];
							Log.Debug("\t\tElement: " + HashCache.Get().Get(element.symbol));
						}
					}
				}
			}
		}
	}
}

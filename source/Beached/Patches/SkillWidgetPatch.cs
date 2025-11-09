using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Beached.Patches
{
	public class SkillWidgetPatch
	{
		private const int POINT_PER_LINE = 6;
		private const float Y_STEP = 92.3f;

		[HarmonyPatch(typeof(SkillWidget), nameof(SkillWidget.RefreshLines))]
		public class SkillWidget_RefreshLines_Patch
		{
			public class OffsetInfo(string skillId, string requisiteId, Direction left, Direction right)
			{
				public string skillId = skillId;
				public string requisiteId = requisiteId;
				public Direction left = left;
				public Direction right = right;
			}

			private static List<OffsetInfo> offsetPairs =
				[
				new (BSkills.ARCHEOLOGY2_ID, "Mining1", Direction.Down, Direction.None),
				new (BSkills.CRYSTALLOGRAPHY_ID, BSkills.ARCHEOLOGY_ID, Direction.Down, Direction.None),
				new ("Hauling2", "Hauling1", Direction.Down, Direction.Down),
				new ("RocketPiloting2", "Astronomy", Direction.Down, Direction.None),
				new ("ThermalSuits", "RocketPiloting1", Direction.Down, Direction.None),

			];

			// TODO: TRANSPILE
			public static bool Prefix(SkillWidget __instance)
			{
				__instance.prerequisiteSkillWidgets.Clear();
				var targetPositions = new List<Vector2>();
				var dim = false;

				if (__instance.skillID == BSkills.GEOCHEMISTRY_ID)
					dim = true;

				var counter = 0;
				var geoChemistryLineIndex = -1;

				var sortedOffsets = new Dictionary<int, OffsetInfo>();

				foreach (var priorSkill in Db.Get().Skills.Get(__instance.skillID).priorSkills)
				{
					targetPositions.Add(__instance.skillsScreen.GetSkillWidgetLineTargetPosition(priorSkill));

					var offset = offsetPairs.FindIndex(o => o.skillId == __instance.skillID && o.requisiteId == priorSkill);
					if (offset != -1)
					{
						sortedOffsets[counter] = offsetPairs[offset];
					}

					if (__instance.skillID == BSkills.GEOCHEMISTRY_ID && priorSkill == "Researching2")
						geoChemistryLineIndex = counter;

					__instance.prerequisiteSkillWidgets.Add(__instance.skillsScreen.GetSkillWidget(priorSkill));
					counter++;
				}
				if (__instance.lines != null)
				{
					for (var index = __instance.lines.Length - 1; index >= 0; --index)
						Object.Destroy(__instance.lines[index].gameObject);
				}

				__instance.linePoints.Clear();

				for (var index = 0; index < targetPositions.Count; ++index)
				{
					var offsetInfo = sortedOffsets.TryGetValue(index, out var offset) ? offset : null;


					var rightSideYOffset = 0.0f;
					var leftSideYOffset = 0.0f;

					if (offsetInfo != null)
					{
						Log.Debug($"setting offset for {offsetInfo.requisiteId} {offsetInfo.left} -> {offsetInfo.skillId} {offsetInfo.right}");
						rightSideYOffset = offsetInfo.right switch
						{
							Direction.Down => -10.0f,
							Direction.Up => 10.0f,
							_ => 0.0f,
						};

						leftSideYOffset = offsetInfo.left switch
						{
							Direction.Down => -10.0f,
							Direction.Up => 10.0f,
							_ => 0.0f,
						};
					}

					var leftX = __instance.lines_left.GetPosition().x - targetPositions[index].x - 12.0f;
					var leftY = -(__instance.lines_left.GetPosition().y - targetPositions[index].y - leftSideYOffset);
					var rightY = rightSideYOffset;

					__instance.linePoints.Add(new Vector2(0.0f, rightY));
					__instance.linePoints.Add(new Vector2(-leftX, rightY));


					// TODO: refactor this mess
					if (index == geoChemistryLineIndex)
					{
						// TODO: store this array from actually crossing lines
						bool[] testData = [true, false, false, true, true, true, true, false, false];

						/*
						- 0 y1: -702.36 y2: -632.36
						- 1 y1: -616.36 y2: -538.36
						- 2 y1: -546.36 y2: -476.36
						- 3 y1: -460.36 y2: -398.36
						- 4 y1: -382.36 y2: -320.36
						- 5 y1: -304.36 y2: -242.36
						- 6 y1: -226.36 y2: -164.36
						- 7 y1: -148.36 y2: -70.35999
						- 8 y1: -78.35999 y2: -0.3599854
						 * */

						var layoutRowheight = 78.0f;
						var margin = 6.0f;
						var skip = margin * 2.0f;
						var segmentLength = layoutRowheight - skip;
						var totalLength = Mathf.Abs(rightY - leftY);

						var segmentCount = (totalLength / layoutRowheight) - 1;
						var previousWasShort = false;

						for (var i = 0; i < segmentCount; i++)
						{
							float y1, y2;

							if (testData.Length > i && testData[i])
							{
								y1 = leftY + ((i * segmentLength) + (i * skip));

								if (i != 0)
									y1 += margin;

								if (i == segmentCount - 1)
									y2 = y1 + layoutRowheight;
								else
									y2 = y1 + segmentLength;

								if (i == 0)
									y2 += margin;

								previousWasShort = true;
							}
							else
							{
								if (previousWasShort)
								{
									y1 = leftY + ((i * segmentLength) + (i * skip));
									if (i != 0)
										y1 += margin;
								}
								else
								{
									y1 = leftY + (i * layoutRowheight);
								}
								y2 = y1 + layoutRowheight;

								var isNextShort = testData.Length > i + 1 && testData[i + 1];
								if (isNextShort)
									y2 -= margin;


								previousWasShort = false;
							}

							Log.Debug($"{i} y1: {y1} y2: {y2}");
							__instance.linePoints.Add(new Vector2(-leftX, y1));
							__instance.linePoints.Add(new Vector2(-leftX, y2)); // top
						}
					}
					else
					{
						__instance.linePoints.Add(new Vector2(-leftX, rightY));
						__instance.linePoints.Add(new Vector2(-leftX, leftY));
					}

					__instance.linePoints.Add(new Vector2(-leftX, leftY));
					__instance.linePoints.Add(new Vector2((float)-(__instance.lines_left.GetPosition().x - (double)targetPositions[index].x), leftY));

					if (__instance.skillID == BSkills.GEOCHEMISTRY_ID)
					{
						//var texture = Assets.GetSprite("beached_stripe").texture;
						//texture.wrapMode = TextureWrapMode.Repeat;
						//__instance.lines[index1].material.SetTexture("_MainTex", texture);
						//__instance.lines[index1].materialForRendering.SetTexture("_MainTex", texture);

					}
				}

				__instance.lines = new UILineRenderer[__instance.linePoints.Count / 2];

				var index1 = 0;

				for (var index2 = 0; index2 < __instance.linePoints.Count; index2 += 2)
				{
					var go = new GameObject("Line");
					go.AddComponent<RectTransform>();
					go.transform.SetParent(__instance.lines_left.transform);
					go.transform.SetLocalPosition(Vector3.zero);
					go.rectTransform().sizeDelta = Vector2.zero;
					__instance.lines[index1] = go.AddComponent<UILineRenderer>();
					var color = new Color(0.6509804f, 0.6509804f, 0.6509804f, dim ? 0.66f : 1f);
					__instance.lines[index1].color = color;
					__instance.lines[index1].Points =
					[
						__instance.linePoints[index2],
						__instance.linePoints[index2 + 1]
					];


					++index1;
				}

				return false;
			}

			/*			public static void Postfix(SkillWidget __instance)
						{
							var skill = Db.Get().Skills.Get(__instance.skillID);

							for (var i = 0; i < skill.priorSkills.Count; i++)
							{
								var priorSkill = skill.priorSkills[i];
								if (priorSkill == BSkills.ARCHEOLOGY_ID)
								{
									var startIndex = POINT_PER_LINE * i;
									Log.Debug($"prior skill archeology on {skill.Name} {startIndex}");
									for (var offset = startIndex; offset < POINT_PER_LINE; offset++)
									{
										var position = __instance.linePoints[offset];
										position.x += 4.0f;
										__instance.linePoints[offset] = position;
									}
								}
							}
						}*/


			/*	public static void Postfix(UILineRenderer[] ___lines)
				{
					foreach (var line in ___lines)
					{
						line.BezierMode = UILineRenderer.BezierType.Basic;
						line.BezierSegmentsPerCurve = 16;
						line.LineJoins = UILineRenderer.JoinType.Bevel;
						line.color = Color.red;
						line.Rebuild(UnityEngine.UI.CanvasUpdate.Layout);
						line.SetAllDirty();
					}
				}*/
		}
	}
}

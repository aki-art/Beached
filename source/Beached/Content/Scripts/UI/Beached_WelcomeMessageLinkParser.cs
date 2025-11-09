using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Beached.Content.Scripts.UI
{
	public class Beached_WelcomeMessageLinkParser : KMonoBehaviour, IPointerClickHandler
	{
		private static readonly Dictionary<string, string> links = new()
		{
			{ "BEACHED_GITHUBTRACKERURL", "https://github.com/aki-art/Beached/issues/new" },
			{ "BEACHED_DISCORDURL", "https://discord.gg/XE8J6N2Fjm" }
		};

		private LocText text;
		private bool hovered;

		private void Update()
		{
			var hoveredElement = GetHovering();
			var shouldCursorBeHand = !hoveredElement.IsNullOrWhiteSpace() && links.ContainsKey(hoveredElement);
			if (hovered != shouldCursorBeHand)
			{
				var cursor = !hovered ? Assets.GetTexture("cursor_hand") : Assets.GetTexture("cursor_arrow");
				SetCursor(cursor, Vector2.zero, CursorMode.Auto);
				hovered = shouldCursorBeHand;
			}
		}

		protected void SetCursor(Texture2D newCursor, Vector2 offset, CursorMode mode)
		{
			if (newCursor != InterfaceTool.activeCursor && newCursor != null)
			{
				InterfaceTool.activeCursor = newCursor;

				try
				{
					Cursor.SetCursor(newCursor, offset, mode);

					if (PlayerController.Instance.vim != null)
						PlayerController.Instance.vim.SetCursor(newCursor);
				}
				catch (Exception ex)
				{
					Log.Warning("SetCursor Failed" + ex.StackTrace + string.Format("SetCursor Failed new_cursor={0} offset={1} mode={2}", newCursor, offset, mode));
				}
			}
		}

		public override void OnPrefabInit()
		{
			if (text == null)
				text = GetComponent<LocText>();
		}

		private string GetHovering()
		{
			var mousePos = KInputManager.GetMousePos();
			var linkIndex = TMP_TextUtilities.FindIntersectingLink(text, mousePos, null);

			return linkIndex != -1 ? text.textInfo.linkInfo[linkIndex].GetLinkID() : null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			var mousePos = KInputManager.GetMousePos();
			var linkIndex = TMP_TextUtilities.FindIntersectingLink(text, mousePos, null);

			if (linkIndex == -1)
				return;

			var linkId = text.textInfo.linkInfo[linkIndex].GetLinkID();

			if (!linkId.IsNullOrWhiteSpace() && links.TryGetValue(linkId, out var link))
				Application.OpenURL(link);
		}
	}
}

﻿using Beached.Content.ModDb;
using ImGuiNET;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
	// goes onto beds
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_PlushiePlaceable : KMonoBehaviour, IImguiDebug, ISim4000ms
	{
		[MyCmpReq] private Assignable assignable;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private KSelectable kSelectable;

		[Serialize] public string plushieId;
		[Serialize] public float lastPlushiePlaced;
		[Serialize] public bool worn;

		[SerializeField] public float plushieWornThreshold;
		[SerializeField] public float plushieLifeSpan;

		private int debugCurrentlySelectedPlushie;
		private KBatchedAnimController plushie;
		private KAnimLink link;
		private Vector3 offset;
		// plushed up statusitem

		public Plushie GetPlushie() => BDb.plushies.Get(plushieId);

		public bool HasPlushie() => !plushieId.IsNullOrWhiteSpace();

		public void OnImguiDraw()
		{
			if (ImGui.Combo("Set Plushie", ref debugCurrentlySelectedPlushie, BDb.plushies.ids, BDb.plushies.ids.Length))
				StorePlushie(BDb.plushies.ids[debugCurrentlySelectedPlushie]);

			ImGui.Text($"Age: {plushieLifeSpan - (GameClock.Instance.GetTime() - lastPlushiePlaced)} s");
			if (ImGui.DragFloat("X", ref offset.x) || ImGui.DragFloat("Y", ref offset.y))
				plushie.transform.position = (transform.position + offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) };
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (plushieId != null)
				StorePlushie(plushieId);

			ModCmps.plushiePlaceables.Add(this);
		}

		public void StorePlushie(string plushieId)
		{
			RemovePlushie();

			var plushieResource = BDb.plushies.TryGet(plushieId);

			if (plushieResource == null)
			{
				Log.Warning($"Plushie {plushieId} does not exist.");
				return;
			}

			CreatePlushieGameObject(plushieResource);
			this.plushieId = plushieId;
			lastPlushiePlaced = GameClock.Instance.GetTime();

			kSelectable.AddStatusItem(BStatusItems.plushed);

			if (worn)
				GetWorn();
		}

		private void CreatePlushieGameObject(Plushie plushieResource)
		{
			var gameObject = new GameObject("beached plushie visualizer");
			gameObject.SetActive(false);

			var pos = (transform.position + plushieResource.offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) };
			gameObject.transform.SetPosition(pos);
			plushie = gameObject.AddComponent<KBatchedAnimController>();
			plushie.AnimFiles =
			[
				Assets.GetAnim( plushieResource.animFile)
			];

			plushie.initialAnim = "object";
			gameObject.transform.parent = transform;
			plushie.gameObject.SetActive(true);

			link = new KAnimLink(kbac, plushie);
		}

		private void RemovePlushie()
		{
			if (link != null)
			{
				link.Unregister();
				link = null;
			}

			if (plushie != null)
				Util.KDestroyGameObject(plushie);

			plushieId = null;
			worn = false;

			if (kSelectable.HasStatusItem(BStatusItems.plushed))
				kSelectable.AddStatusItem(BStatusItems.plushed);
		}

		public override void OnCleanUp()
		{
			if (isLoadingScene || Game.IsQuitting() || App.IsExiting)
				return;

			RemovePlushie();
			ModCmps.plushiePlaceables.Remove(this);
			base.OnCleanUp();
		}

		public static string GetStatusItemTooltip(string str, object data)
		{
			if (data is Beached_PlushiePlaceable bed)
			{
				if (bed.plushieId == null)
					return str;

				var plush = BDb.plushies.TryGet(bed.plushieId);

				if (plush == null)
					return str;

				str = string.Format(str, plush.Name);
				var effect = Db.Get().effects.Get(plush.effect);
				str += Effect.CreateTooltip(effect, true);
			}

			return str;
		}

		public void Sim4000ms(float dt)
		{
			if (plushieId == null)
				return;

			var age = GameClock.Instance.GetTime() - lastPlushiePlaced;

			if (!worn)
			{
				if (age > plushieWornThreshold)
					GetWorn();
			}
			else
			{
				if (age > plushieLifeSpan)
					RemovePlushie();
			}
		}

		private void GetWorn()
		{
			worn = true;

			if (plushie == null)
				return;

			plushie.Play("worn", KAnim.PlayMode.Paused);
		}
	}
}


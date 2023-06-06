using Beached.Content.ModDb;
using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
	// goes onto beds
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_PlushiePlaceable : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private Assignable assignable;
		[MyCmpReq] private KBatchedAnimController kbac;
		[Serialize] private string plushieId;
		private int debugCurrentlySelectedPlushie;
		private KBatchedAnimController plushie;
		private KAnimLink link;
		private Vector3 offset;

		public void OnImguiDraw()
		{
			if (ImGui.Combo("Set Plushie", ref debugCurrentlySelectedPlushie, BDb.plushies.ids, BDb.plushies.ids.Length))
				StorePlushie(BDb.plushies.ids[debugCurrentlySelectedPlushie]);

			if (ImGui.DragFloat("X", ref offset.x) || ImGui.DragFloat("Y", ref offset.y))
				plushie.transform.position = (transform.position + offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) };
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (plushieId != null)
				StorePlushie(plushieId);
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
		}

		private void CreatePlushieGameObject(Plushie plushieResource)
		{
			var gameObject = new GameObject("beached plushie visualizer");
			gameObject.SetActive(false);

			var pos = (transform.position + plushieResource.offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) };
			gameObject.transform.SetPosition(pos);
			plushie = gameObject.AddComponent<KBatchedAnimController>();
			plushie.AnimFiles = new[]
			{
				Assets.GetAnim( plushieResource.animFile)
			};

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

			if (plushieId != null)
				Util.KDestroyGameObject(plushie);

			plushieId = null;
		}

		public override void OnCleanUp()
		{
			RemovePlushie();
			base.OnCleanUp();
		}
	}
}


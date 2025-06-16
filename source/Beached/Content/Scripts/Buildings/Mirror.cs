using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class Mirror : KMonoBehaviour, ISim33ms, IRenderEveryTick
	{
		private CavityInfo currentCavity;
		private List<KAnimSynchronizer> synchronizers;
		private List<KAnimControllerBase> reflections;
		private HandleVector<int>.Handle pickupablesChangedEntry;
		private bool pickupablesDirty;
		private Extents pickupableExtents;
		[MyCmpReq] private KBoxCollider2D collider;
		[SerializeField] public Vector3 reflectionOffset;

		public override void OnPrefabInit()
		{
			gameObject.AddTag(BTags.FastTrack_registerRoom);

			Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
			synchronizers = [];
			reflections = [];
		}

		public override void OnSpawn()
		{
			OnUpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));

			Grid.PosToXY(transform.position + reflectionOffset, out var x, out var y);
			pickupableExtents = new Extents(x - 1, y, 4, 3);

			pickupablesChangedEntry = GameScenePartitioner.Instance.Add("Beached_Mirror.PickupablesChanged", gameObject, pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);
			pickupablesDirty = true;
		}

		public override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
		}

		private void OnPickupablesChanged(object obj)
		{
			if (obj is Pickupable pickupable)
			{
				pickupablesDirty = true;
			}
		}

		private void OnUpdateRoom(object obj)
		{
			if (obj is Room room)
			{
				UpdateRoom(room?.cavity);
			}
		}

		public void UpdateRoom(CavityInfo cavity)
		{
			if (Game.IsQuitting())
				return;

			if (cavity == currentCavity)
				return;

			currentCavity?.RemoveMirror(this);

			cavity?.AddMirror(this);

			currentCavity = cavity;
		}

		public void Sim33ms(float dt)
		{
			if (!pickupablesDirty)
				return;

			for (var i = synchronizers.Count - 1; i >= 0; i--)
			{
				var sync = synchronizers[i];

				if (sync == null || sync.masterController == null || !pickupableExtents.Contains(sync.masterController.PositionIncludingOffset))//collider.Intersects(sync.masterController.PositionIncludingOffset))
				{
					var reflection = reflections[i];
					sync.Remove(reflections[i]);

					Destroy(reflection.gameObject);
					synchronizers.RemoveAt(i);
					reflections.RemoveAt(i);
				}
			}

			if (synchronizers.Count >= Mod.settings.Graphics.MaxEntitiesVisibleInMirrors)
				return;

			var gathered_entries = ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				pickupableExtents.x,
				pickupableExtents.y,
				pickupableExtents.width,
				pickupableExtents.height,
				GameScenePartitioner.Instance.pickupablesLayer,
				gathered_entries);

			var maxNewAdditions = Mathf.Clamp(Mod.settings.Graphics.MaxEntitiesVisibleInMirrors - synchronizers.Count, 0, gathered_entries.Count);

			for (var i = 0; i < maxNewAdditions; i++)
			{
				var pickup = gathered_entries[i].obj as Pickupable;

				if (pickup == null)
					continue;

				if (pickup.TryGetComponent(out OccupyArea _) && pickup.TryGetComponent(out KBatchedAnimController originalKbac))
				{
					var reflection = FXHelpers.CreateEffectOverride(
						originalKbac.animFiles.Select(file => file.name).ToArray(),
						transform.position + reflectionOffset,
						transform,
						false,
						Grid.SceneLayer.Creatures);

					var scale = reflection.transform.localScale;
					scale = new Vector3(scale.x * -1f, scale.y, 1f);

					// sync symbols

					reflection.HighlightColour = Color.blue;
					reflection.Offset = originalKbac.Offset;
					reflection.SetSceneLayer(Grid.SceneLayer.Building);

					reflection.transform.localScale = new Vector3(
						reflection.transform.localScale.x * -1f,
						reflection.transform.localScale.y,
						Grid.GetLayerZ(Grid.SceneLayer.Building));

					var position = transform.position;
					reflection.GetBatchInstanceData().SetClipRadius(position.x + 0.5f, position.y + 1f, 1.2f * 1.2f, true);

					var sync = originalKbac.GetSynchronizer();
					synchronizers.Add(sync);
					reflections.Add(reflection);

					sync.Sync();

					if (synchronizers.Count != reflections.Count)
						ReSyncReflections();

					originalKbac.GetSynchronizer().Add(reflection);
				}
			}
		}

		private void ReSyncReflections()
		{
			Log.Warning("Something wrnt wrong, reflections are desynced D:");
		}


		public void RenderEveryTick(float dt)
		{
			if (reflections == null) return;

			for (var i = 0; i < reflections.Count; i++)
			{
				if (synchronizers.Count <= i)
					continue;

				var reflection = reflections[i];
				var master = synchronizers[i];

				if (reflection == null || master == null)
					continue;

				var originalPos = master.masterController.PositionIncludingOffset;

				var centerX = transform.position.x + 0.5f;
				var xDiff = originalPos.x - centerX;

				var position = new Vector3(
					originalPos.x - (2f * xDiff),
					originalPos.y,
					reflection.transform.position.z);

				reflection.transform.position = position;
			}
		}
	}
}

using Beached.Content.Defs.Entities;
using Beached.Content.Scripts.Entities;
using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class CrystalGrower : SingleEntityReceptacle
	{
		[MyCmpAdd] public CopyBuildingSettings copyBuildingSettings;

		[Serialize] public Ref<KPrefabID> plantRef;

		public Vector3 occupyingObjectVisualOffset = Vector3.zero;
		public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;
		public EntityPreview preview;

		public Tag tagOnPlanted = Tag.Invalid;

		public KPrefabID plant
		{
			get => plantRef.Get();
			set => plantRef.Set(value);
		}


		public bool ValidPlant
		{
			get
			{
				return preview == null || preview.Valid;
			}
		}


		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			choreType = Db.Get().ChoreTypes.FarmFetch;
			statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
			statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
			statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
			plantRef = new Ref<KPrefabID>();

			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);

			storage.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);

			if (TryGetComponent(out DropAllWorkable dropAllWorkable))
				dropAllWorkable.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);

			if (TryGetComponent(out Toggleable toggleable))
				toggleable.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		}

		private void OnCopySettings(object obj)
		{
			// TODO
		}

		public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
		{
			SetPreview(entityTag);

			if (ValidPlant)
				base.CreateOrder(entityTag, additionalFilterTag);
			else
				SetPreview(Tag.Invalid);
		}

		public void SyncPriority(PrioritySetting priority)
		{
			Prioritizable prioritizable = GetComponent<Prioritizable>();
			if (!Equals(prioritizable.GetMasterPriority(), priority))
			{
				prioritizable.SetMasterPriority(priority);
			}

			if (occupyingObject != null)
			{
				Prioritizable component2 = occupyingObject.GetComponent<Prioritizable>();
				if (component2 != null && !Equals(component2.GetMasterPriority(), priority))
				{
					component2.SetMasterPriority(prioritizable.GetMasterPriority());
				}
			}
		}

		public override void OnSpawn()
		{
			if (plant != null)
			{
				RegisterWithPlant(plant.gameObject);
			}

			base.OnSpawn();
			autoReplaceEntity = false;

			ModCmps.crystalGrowers.Add(gameObject.GetMyWorldId(), this);

			GetComponent<Prioritizable>().onPriorityChanged += SyncPriority;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			if (preview != null)
				Util.KDestroyGameObject(preview.gameObject);

			if ((bool)occupyingObject)
				occupyingObject.Trigger(-216549700);

			ModCmps.crystalGrowers.Remove(gameObject.GetMyWorldId(), this);
		}


		public override GameObject SpawnOccupyingObject(GameObject depositedEntity)
		{
			// TODO
			var position = Grid.CellToPosCBC(Grid.PosToCell(this), plantLayer);
			var crystalGo = GameUtil.KInstantiate(Assets.GetPrefab(CrystalConfig.ZEOLITE), position, plantLayer);

			crystalGo.SetActive(true);
			destroyEntityOnDeposit = true;

			return crystalGo;
		}

		public override void ConfigureOccupyingObject(GameObject newPlant)
		{
			KPrefabID component = newPlant.GetComponent<KPrefabID>();
			plantRef.Set(component);
			RegisterWithPlant(newPlant);

			// configure crystal stuff here


			autoReplaceEntity = false;
			Prioritizable component3 = GetComponent<Prioritizable>();
			if (component3 != null)
			{
				Prioritizable component4 = newPlant.GetComponent<Prioritizable>();
				if (component4 != null)
				{
					component4.SetMasterPriority(component3.GetMasterPriority());
					component4.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(component4.onPriorityChanged, new Action<PrioritySetting>(SyncPriority));
				}
			}
		}

		public void RegisterWithPlant(GameObject plant)
		{
			base.occupyingObject = plant;
			ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
			if ((bool)component)
			{
				if (tagOnPlanted != Tag.Invalid)
				{
					component.AddTag(tagOnPlanted);
				}

				component.SetReceptacle();
			}

			plant.Trigger(1309017699, storage);
		}

		// Todo harvested
		public override void SubscribeToOccupant()
		{
			base.SubscribeToOccupant();
			if (base.occupyingObject != null)
			{
				Subscribe(base.occupyingObject, -216549700, OnOccupantDugUp);
			}
		}

		public override void UnsubscribeFromOccupant()
		{
			base.UnsubscribeFromOccupant();
			if (base.occupyingObject != null)
			{
				Unsubscribe(base.occupyingObject, -216549700, OnOccupantDugUp);
			}
		}

		public void OnOccupantDugUp(object data)
		{
			autoReplaceEntity = false;
			requestedEntityTag = Tag.Invalid;
			requestedEntityAdditionalFilterTag = Tag.Invalid;
		}


		/*		public override void OrderRemoveOccupant()
				{
					if (!(base.Occupant == null))
					{
						Uprootable component = base.Occupant.GetComponent<Uprootable>();
						if (!(component == null))
						{
							component.MarkForUproot();
						}
					}
				}*/

		public override void SetPreview(Tag entityTag, bool solid = false)
		{
			base.SetPreview(entityTag, solid);

			CrystalCluster crystalCluster = null;

			if (entityTag.IsValid)
			{
				var prefab = Assets.GetPrefab(entityTag);

				if (prefab == null)
				{
					Log.Warning($"Cluster tried previewing a tag with no asset! {entityTag}");
					return;
				}

				crystalCluster = prefab.GetComponent<CrystalCluster>();
			}

			if (preview != null)
			{
				var kPrefabId = preview.GetComponent<KPrefabID>();
				if (crystalCluster != null && kPrefabId != null && kPrefabId.PrefabTag == crystalCluster.previewID)
					return;

				preview.gameObject.Unsubscribe((int)GameHashes.OccupantValidChanged, OnValidChanged);

				Util.KDestroyGameObject(preview.gameObject);
			}

			if (crystalCluster == null)
				return;


			var gameObject = GameUtil.KInstantiate(Assets.GetPrefab(crystalCluster.previewID), Grid.SceneLayer.Front);
			preview = gameObject.GetComponent<EntityPreview>();
			gameObject.transform.SetPosition(Vector3.zero);
			gameObject.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			gameObject.transform.SetLocalPosition(Vector3.zero);

			if (rotatable != null)
			{
				if (crystalCluster.direction == ReceptacleDirection.Top)
				{
					gameObject.transform.SetLocalPosition(occupyingObjectRelativePosition);
				}
				else if (crystalCluster.direction == ReceptacleDirection.Side)
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(occupyingObjectRelativePosition, Orientation.R90));
				}
				else
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(occupyingObjectRelativePosition, Orientation.R180));
				}
			}
			else
			{
				gameObject.transform.SetLocalPosition(occupyingObjectRelativePosition);
			}

			KBatchedAnimController kbac = gameObject.GetComponent<KBatchedAnimController>();
			OffsetAnim(kbac, occupyingObjectVisualOffset);

			gameObject.SetActive(value: true);
			gameObject.Subscribe((int)GameHashes.OccupantValidChanged, OnValidChanged);


			if (solid)
				preview.SetSolid();

			preview.UpdateValidity();
		}


		public void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
		{
			if (rotatable != null)
				offset = rotatable.GetRotatedOffset(offset);

			kanim.Offset = offset;
		}

		public void OnValidChanged(object obj)
		{
			Trigger((int)GameHashes.OccupantValidChanged, obj);

			if (!preview.Valid && GetActiveRequest != null)
				CancelActiveRequest();
		}
	}
}

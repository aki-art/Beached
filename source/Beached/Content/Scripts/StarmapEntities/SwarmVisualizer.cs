using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Beached.Content.Scripts.StarmapEntities
{
	internal class SwarmVisualizer : KMonoBehaviour
	{
		[MyCmpReq]
		ClusterGridEntity clusterGridEntity;
		[Serialize]
		int clusterRadius = -1;


		ClusterMapVisualizer KbacContainer;
		List<GameObject> Visualizers = new(64);

		public override void OnSpawn()
		{
			base.OnSpawn();
		}
		public override void OnCleanUp()
		{
			ClearVisualizers();
			base.OnCleanUp();
		}
		void ClearVisualizers()
		{
			for (int i = Visualizers.Count - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(Visualizers[i]);
			}
			Visualizers.Clear();
			UnityEngine.Object.Destroy(KbacContainer);
			KbacContainer = null;
		}

		private bool VisualizersInitialized = false;
		public void SpawnVisualizers()
		{
			if (VisualizersInitialized)
				return;

			if (clusterRadius < 0)
				clusterRadius = ClusterGrid.Instance.numRings-1; //-2 for inner ring, +-0 for outer ring

			var clustermap = ClusterMapScreen.Instance;
			KbacContainer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(clustermap.staticVisPrefab, clustermap.POIVisContainer.transform);

			AxialI originPoint = clusterGridEntity.Location; //origin point of the circle
			var radiusHexes = clusterRadius;  //radius of the circle in hexes
			float segmentLength = 6.4f; //the actual length of one segment, used in calculating the segment count, determined by anim
			float segmentScale = 6f; //anim scale of one segment
			float offGridThreshold = 4; //distance to hex grid where it stops spawning anim slices


			AxialI offsetPoint = originPoint;
			offsetPoint.R += radiusHexes;

			var origin = AxialUtil.AxialToWorld(originPoint.R, originPoint.Q);
			var offset = AxialUtil.AxialToWorld(offsetPoint.R, offsetPoint.Q);

			var radius = Vector3.Distance(origin, offset) * 1.5f;

			//account for position offsets so its in the middle of the hex
			origin.x -= 0.33f;
			origin.y -= 0.33f;

			Debug.Log("Origin: " + origin);
			Debug.Log("Segment Length: " + segmentLength);

			var offsetPos = new Vector3(0, radius);

			var circumference = 2 * Mathf.PI * radius;
			float numberOfSlices = Mathf.RoundToInt(circumference / segmentLength);
			float anglePerIteration = 360f / numberOfSlices;

			Debug.Log("Number of Segments: " + numberOfSlices);

			//cache all world positions of all hexes to check later if the position is near the hex grid (somewhat inefficient, but it runs only once on load)
			var AllGridHexPos = ClusterGrid.Instance.cellContents.Keys.Select(hexCell => AxialUtil.AxialToWorld(hexCell.R, hexCell.Q)).ToArray();

			KbacContainer.gameObject.SetActive(true);
			var containerRectTransform = KbacContainer.rectTransform();
			for (int i = 0; i < numberOfSlices; i++)
			{
				var angle = i * anglePerIteration;
				var angleRadian = angle * Mathf.PI / 180f;

				var rotatedX = origin.x + Mathf.Cos(angleRadian) * offsetPos.x - Mathf.Sin(angleRadian) * offsetPos.y;
				var rotatedY = origin.y + Mathf.Cos(angleRadian) * offsetPos.y + Mathf.Sin(angleRadian) * offsetPos.x;


				bool outsideOfHexGrid = true;
				var slicePos = new Vector3(rotatedX, rotatedY);

				//not beautiful or efficient but it get the job done
				if (AllGridHexPos.Any(pos => Vector3.Distance(pos, slicePos) < offGridThreshold)) 
					outsideOfHexGrid = false;

				if (outsideOfHexGrid) continue;

				Debug.Log("current angle: " + angle + ", Pos:  " + rotatedX + "," + rotatedY);

				if (KbacContainer.animContainer == null)
				{
					GameObject vis = new GameObject("AnimContainer", typeof(RectTransform));
					RectTransform visRectTransform = vis.GetComponent<RectTransform>();
					visRectTransform.SetParent(containerRectTransform, worldPositionStays: false);
					visRectTransform.SetLocalPosition(new Vector3(0f, 0f, 0f));
					visRectTransform.sizeDelta = containerRectTransform.sizeDelta;
					visRectTransform.localScale = Vector3.one;
					KbacContainer.animContainer = visRectTransform;
				}
				//vis.gameObject.SetActive(true);
				var kbac = UnityEngine.Object.Instantiate<KBatchedAnimController>(KbacContainer.animControllerPrefab, KbacContainer.animContainer);
				kbac.AnimFiles = [Assets.GetAnim("meteor_swarm_kanim")];
				kbac.gameObject.SetActive(true);
				kbac.Play("swarming", KAnim.PlayMode.Loop);
				kbac.rectTransform().SetLocalPosition(slicePos);
				kbac.Rotation = angle;
				kbac.animScale *= segmentScale;
				Visualizers.Add(kbac.gameObject);
			}
			VisualizersInitialized = true;
		}
	}
}

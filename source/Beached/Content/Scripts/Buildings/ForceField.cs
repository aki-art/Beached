using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class ForceField : StateMachineComponent<ForceField.StatesInstance>, IImguiDebug
	{
		[SerializeField] public Vector2 offset;
		[SerializeField] public float distanceFromWorldTop;
		[SerializeField] public float radiusMultiplier;
		[SerializeField] public float cometExplosionMargin;
		private float radius;
		private Vector3 center;

		public LineRenderer debugVisualizer;
		private bool showDebug;
		private Transform visualizer;
		private Material material;

		public override void OnSpawn()
		{
			ModCmps.forceFields.Add(this);

			var myWorld = gameObject.GetMyWorld();
			if (myWorld != null)
			{
				radius = myWorld.Width * radiusMultiplier;
				center = new Vector2(myWorld.Width / 2f, myWorld.Height - distanceFromWorldTop - radius) + myWorld.PosMin();
			}

			Beached_Mod.Instance.forceFields[myWorld.id] = this;

			var visualizer = Instantiate(ModAssets.Prefabs.forceFieldDome, transform);

			material = visualizer.GetComponent<MeshRenderer>().materials[0];

			var x = myWorld.Width / 2.0f + myWorld.PosMin().x;
			var y = myWorld.Height - distanceFromWorldTop + myWorld.PosMin().y + 1.0f;

			visualizer.transform.position = new Vector3(x, y, 45);
			visualizer.transform.SetParent(transform, true);
			var scale = myWorld.Width * radiusMultiplier;
			visualizer.transform.localScale = Vector3.one * scale;
			visualizer.SetActive(true);

			this.visualizer = visualizer.transform;

			Subscribe(ModHashes.debugDataChange, OnDebugChange);

			ToggleDebugLines(showDebug);

			smi.StartSM();
		}

		private void OnDebugChange(object obj)
		{
			ToggleDebugLines(Mod.drawDebugGuides);
		}

		public bool IsIntersecting(GameObject obj)
		{
			return Vector3.Distance(obj.transform.position, center) + cometExplosionMargin < radius;
		}

		private void ToggleDebugLines(bool visible)
		{
			if (visible)
			{
				if (debugVisualizer == null)
				{
					debugVisualizer = ModDebug.AddSimpleLineRenderer(transform, Color.magenta, Color.magenta, 0.1f);
					debugVisualizer.loop = true;
					debugVisualizer.positionCount = 360;
					debugVisualizer.useWorldSpace = false;

					debugVisualizer.transform.SetParent(transform, true);
					debugVisualizer.transform.position = center;
				}

				UpdateDebugVisualizer();
			}

			if (debugVisualizer != null)
				debugVisualizer.gameObject.SetActive(visible);
		}

		private void UpdateDebugVisualizer()
		{
			var segments = 360;
			var points = new Vector3[debugVisualizer.positionCount];

			for (int i = 0; i < debugVisualizer.positionCount; i++)
			{
				var rad = Mathf.Deg2Rad * (i * 360f / segments);
				points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
			}

			debugVisualizer.SetPositions(points);
		}

		public override void OnCleanUp()
		{
			ModCmps.forceFields.Remove(this);
			Beached_Mod.Instance.forceFields.Remove(this.GetMyWorldId());

			if (debugVisualizer != null)
				Util.KDestroyGameObject(debugVisualizer.gameObject);

			base.OnCleanUp();
		}

		public void OnCometCollided(Comet comet)
		{
			comet.Explode();
		}

		private float rimStrength;
		private float alpha;
		public void OnImguiDraw()
		{
			if (ImGui.Checkbox("Show Range", ref showDebug))
				ToggleDebugLines(showDebug);

			if (ImGui.DragFloat("Rim Strength", ref rimStrength))
				material.SetFloat("_RimStrength", rimStrength);

			if (ImGui.DragFloat("Alpha Mult", ref alpha))
				material.SetFloat("_Alpha", alpha);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, ForceField, object>.GameInstance
		{
			public StatesInstance(ForceField master) : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, ForceField>
		{
			public State off;
			public State on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off
					.PlayAnim("off", KAnim.PlayMode.Loop);
			}
		}
	}
}

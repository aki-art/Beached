using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class ForceField : StateMachineComponent<ForceField.StatesInstance>, IImguiDebug, IRenderEveryTick
	{
		[SerializeField] public Vector2 offset;
		[SerializeField] public float distanceFromWorldTop;
		[SerializeField] public float radiusMultiplier;
		private float radius;
		private Vector3 center;

		public ForceFieldVisualizer visualizer;
		public LineRenderer debugVisualizer;
		private bool showDebug;
		private float rotationSpeed;
		private Transform testField;
		private Material material;

		public override void OnSpawn()
		{
			/*			visualizer = MiscUtil.Spawn(ForceFieldConfig.ID, transform.position + offset).AddOrGet<ForceFieldVisualizer>();
						visualizer.transform.SetParent(transform);
						visualizer.CreateMesh();*/

			ModCmps.forceFields.Add(this);

			var myWorld = gameObject.GetMyWorld();
			if (myWorld != null)
			{
				radius = myWorld.Width * radiusMultiplier;
				center = new Vector2(myWorld.Width / 2f, myWorld.Height - distanceFromWorldTop) + myWorld.PosMin();
			}

			var test = Instantiate(ModAssets.Prefabs.forceFieldDome, transform);

			material = test.GetComponent<MeshRenderer>().materials[0];

			var x = myWorld.Width / 2.0f + myWorld.PosMin().x;
			var y = myWorld.Height - distanceFromWorldTop + myWorld.PosMin().y + 1.0f;

			test.transform.position = new Vector3(x, y, 45);
			test.transform.SetParent(transform, true);
			var scale = myWorld.Width * radiusMultiplier;
			test.transform.localScale = Vector3.one * scale;
			test.SetActive(true);

			testField = test.transform;

			Subscribe(ModHashes.debugDataChange, OnDebugChange);

			showDebug = true;
			ToggleDebugLines(showDebug);

			smi.StartSM();
		}

		private void OnDebugChange(object obj)
		{
			ToggleDebugLines(Mod.drawDebugGuides);
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
					debugVisualizer.useWorldSpace = true;

					debugVisualizer.transform.position = center;
					debugVisualizer.transform.SetParent(transform, true);
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

			if (visualizer != null)
				Util.KDestroyGameObject(visualizer.gameObject);

			if (debugVisualizer != null)
				Util.KDestroyGameObject(debugVisualizer.gameObject);

			base.OnCleanUp();
		}

		public bool IsIntersecting(Vector3 position)
		{
			return (double)(position - center).sqrMagnitude <= radius * radius;
		}

		public void OnCometCollided(Comet comet)
		{
			comet.Explode();
		}

		private float rimStrength;
		private float alpha;
		public void OnImguiDraw()
		{
			if (visualizer != null)
				visualizer.OnDebugSelected();

			if (ImGui.Checkbox("Show Range", ref showDebug))
				ToggleDebugLines(showDebug);

			ImGui.DragFloat("Rotation Speed", ref rotationSpeed);
			if (ImGui.DragFloat("Top Offset", ref distanceFromWorldTop))
			{
				var myWorld = this.GetMyWorld();
				var y = myWorld.Height - distanceFromWorldTop + myWorld.PosMin().y + 1.0f;
			}

			if (ImGui.DragFloat("Rim Strength", ref rimStrength))
				material.SetFloat("_RimStrength", rimStrength);

			if (ImGui.DragFloat("Alpha Mult", ref alpha))
				material.SetFloat("_Alpha", alpha);
		}

		public void RenderEveryTick(float dt)
		{
			testField.RotateAround(Vector3.up, rotationSpeed * dt);
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

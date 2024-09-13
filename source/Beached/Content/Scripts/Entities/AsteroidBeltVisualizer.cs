using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class AsteroidBeltVisualizer : KMonoBehaviour
	{
		public int radius;
		public int count = 300;

		public const int MAX_LIFETIME = 100000;

		private ParticleSystem visualizer;

		public static AsteroidBeltVisualizer TestingInstance;

		public override void OnSpawn()
		{
			TestingInstance = this;

			if (ModAssets.Prefabs.asteroidBelt != null)
			{
				visualizer = Instantiate(ModAssets.Prefabs.asteroidBelt).GetComponent<ParticleSystem>();

				var main = visualizer.main;
				main.maxParticles = count; // there is also a max 500 subemitter not affected by this
				main.playOnAwake = true;
				main.startLifetime = MAX_LIFETIME;

				visualizer.transform.position = transform.position;
				visualizer.gameObject.SetLayerRecursively(transform.gameObject.layer);
				visualizer.transform.SetParent(transform, true);


				visualizer.gameObject.SetActive(true);
				visualizer.transform.SetLocalPosition(Vector3.zero);


				visualizer.Emit(count);

			}
			else
				Log.Warning("Could not initialize Asteroid Belt visualizer.");
		}

		/*		public void FixedUpdate()
				{
					//if (StarmapScreen.Instance.IsActive())
					//{
					if (visualizer == null)
					{
						Log.Warning("visualizer is null :(");
						return;
					}
					visualizer.Simulate(Time.fixedDeltaTime, true, true);
					//}
				}*/

		private static float scale;
		private static float z;
		private static int renderQeueue = 3500;

		public void OnImguiDebug()
		{
			ImGui.Text("particle count: " + visualizer.particleCount);
			ImGui.Text("subemitter particle count: " + visualizer.subEmitters.GetSubEmitterSystem(0).particleCount);
			ImGui.Text($"position: {transform.position}");

			if (ImGui.DragFloat("AsteroidBelt Scale", ref scale))
			{
				visualizer.transform.localScale = Vector3.one * scale;
			}
			if (ImGui.DragFloat("AsteroidBelt Z", ref z))
			{
				visualizer.transform.position =
					new Vector3(visualizer.transform.position.x,
					visualizer.transform.position.y,
					z);
			}
			if (ImGui.DragInt("AsteoidBelt Renderqueue", ref renderQeueue))
				visualizer.GetComponent<ParticleSystemRenderer>().material.renderQueue = renderQeueue;

			if (ImGui.Button("Play"))
				visualizer.Play();

			ImGui.SameLine();

			if (ImGui.Button("Stop"))
				visualizer.Stop();

			ImGui.SameLine();

			if (ImGui.Button("Emit"))
				visualizer.Emit(count);
		}

		public override void OnCleanUp()
		{
			if (visualizer != null)
				Destroy(visualizer.gameObject);
		}
	}
}

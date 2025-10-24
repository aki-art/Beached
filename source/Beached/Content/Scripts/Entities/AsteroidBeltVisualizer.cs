/*using Coffee.UIExtensions;
using ImGuiNET;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class AsteroidBeltVisualizer : KMonoBehaviour
	{
		public int radius;
		public int count = 300;

		public const int MAX_LIFETIME = 100000;

		private ParticleSystem visualizer;
		private UIParticle uiParticles;

		public static AsteroidBeltVisualizer TestingInstance;

		public override void OnSpawn()
		{
			TestingInstance = this;

			if (ModAssets.Prefabs.asteroidBelt != null)
			{
				try
				{

					var go = Instantiate(ModAssets.Prefabs.asteroidBelt);

					visualizer = go.GetComponent<HierarchyReferences>().GetReference<ParticleSystem>("Particles");
					go.name = "Beached_AsteroidBelt_UIParticles";

					visualizer.transform.parent = go.transform;

					visualizer.GetComponent<ParticleSystemRenderer>().sortingFudge = -100;

					var sub = visualizer.subEmitters;
					sub.RemoveSubEmitter(0);

					visualizer.gameObject.SetActive(true);
					uiParticles = go.AddComponent<UIParticle>();
					uiParticles.raycastTarget = false;

					//uiParticles.SetParticleSystemInstance(visualizer.gameObject);

					//uiParticles.material = new Material(Shader.Find("UI/Default"));

					uiParticles.transform.position = transform.position;
					uiParticles.gameObject.SetLayerRecursively(transform.gameObject.layer);
					uiParticles.transform.SetParent(transform, true);
					uiParticles.transform.SetLocalPosition(new Vector3(0, 0, -0.1f));

					uiParticles.scale = 100;
					uiParticles.positionMode = UIParticle.PositionMode.Relative;
					uiParticles.scale3D = Vector3.one;
					uiParticles.useCustomView = true;
					uiParticles.customViewSize = 10;
					uiParticles.maskable = false;



					uiParticles.gameObject.SetActive(true);

					visualizer.Play();
					visualizer.Emit(count);

					var bounds = visualizer.GetComponent<ParticleSystemRenderer>().bounds;
					var line = Utils.ModDebug.AddSimpleLineRenderer(visualizer.transform, Color.magenta, Color.magenta);
					line.material = new Material(Shader.Find("UI/Default"));
					ModDebug.Square(line, bounds.center, bounds.size.x / 2f);

					uiParticles.enabled = false;
					uiParticles.enabled = true;

					Log.Debug("particles: " + uiParticles.particles.Count);
					foreach (var particle in uiParticles.particles)
						Log.Debug($"registered particle {particle.name}");

					Log.Debug(uiParticles.IsActive());

				}
				catch (Exception e)
				{
					Log.Warning($"Error in instantiating Asteroid Belt prefab: " + e.Message);
				}
			}
			else
				Log.Warning("Could not initialize Asteroid Belt visualizer.");
		}

		*//*		public void FixedUpdate()
				{
					//if (StarmapScreen.Instance.IsActive())
					//{
					if (visualizer == null)
					{
						Log.Warning("visualizer is null :(");
						return;
					}
					visualizer.Simulate(Time.fixedDeltaTime, true, false);
					//}
				}
		*//*
		private static float scale;
		private static float particleScale;
		private static float x, y, z;
		private static int renderQeueue = 3500;
		private static float displaySize = 1;

		public void OnImguiDebug()
		{
			ImGui.Text("particle count: " + visualizer.particleCount);
			ImGui.Text("bounds: " + visualizer.GetComponent<ParticleSystemRenderer>().bounds.ToString());
			//ImGui.Text("subemitter particle count: " + visualizer.subEmitters.GetSubEmitterSystem(0).particleCount);
			ImGui.Text($"position: {transform.position}");

			if (x == 0 && y == 0 && z == 0)
			{
				x = uiParticles.transform.position.x;
				y = uiParticles.transform.position.y;
				z = uiParticles.transform.position.z;
			}

			if (ImGui.DragFloat("UIParticle View Size", ref displaySize))
			{
				uiParticles.useCustomView = true;
				uiParticles.customViewSize = displaySize;
				uiParticles.SetAllDirty();
			}
			if (ImGui.DragFloat("UIParticle Scale", ref scale))
			{
				visualizer.transform.localScale = Vector3.one * scale;
			}
			if (ImGui.DragFloat("AsteroidBelt Scale", ref particleScale))
			{
				uiParticles.scale = particleScale;
				uiParticles.SetAllDirty();
			}
			if (ImGui.DragFloat("X###AsteroidBeltX", ref x)
				|| ImGui.DragFloat("Y###AsteroidBeltY", ref y)
				|| ImGui.DragFloat("Z###AsteroidBeltZ", ref z))
			{
				uiParticles.transform.position = new Vector3(x, y, z);
			}
			if (ImGui.DragInt("AsteoidBelt Renderqueue", ref renderQeueue))
				visualizer.GetComponent<ParticleSystemRenderer>().material.renderQueue = renderQeueue;

			if (ImGui.Button("Play"))
				visualizer.Play();

			if (ImGui.Button("Reset"))
			{
				visualizer.Clear(true);
			}

			ImGui.SameLine();

			if (ImGui.Button("Stop"))
				visualizer.Stop();

			ImGui.SameLine();

			if (ImGui.Button("Emit"))
			{
				visualizer.Emit(count);
				//var uiParticles = visualizer.transform.parent.GetComponent<UIParticle>();
				//uiParticles.StartEmission();
				//uiParticles.Play();
			}
		}

		public override void OnCleanUp()
		{
			if (visualizer != null)
				Destroy(visualizer.gameObject);
		}
	}
}
*/
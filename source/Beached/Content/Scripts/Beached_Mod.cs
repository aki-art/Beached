using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_Mod : KMonoBehaviour, IRender200ms
	{
		public static Beached_Mod Instance;

		public AxialI beltCenter;

		// a bunch of singleton components i don't want to spam on SaveGame
		// these do not serialize, put serialialized data on this component instead
		public IridescenceEffect iridescenceEffect;
		public ElementInteractions elementInteractions;
		public Tutorials tutorials;
		public Treasury treasury;
		public WishingStarEvent wishingStarEvent;

		public Camera waterCamera;
		private RenderTexture waterTarget;
		private Texture2D debugWaterTex;

		public bool isDampWorld;
		private bool shaderPropertyUpdated;

		public static float tempDayTimeProgress;

		[Serialize] public int tallestBambooGrown;
		[Serialize] public bool rareJewelleryObjectiveComplete;

		private Gradient overlayGradient;

		public Dictionary<int, ForceField> forceFields = [];


		public override void OnPrefabInit()
		{
			Instance = this;

			var childGo = new GameObject("BeachedStuff");
			DontDestroyOnLoad(childGo);
			childGo.transform.SetParent(transform);
			childGo.SetActive(true);

			iridescenceEffect = childGo.AddOrGet<IridescenceEffect>();
			elementInteractions = childGo.AddOrGet<ElementInteractions>();
			tutorials = childGo.AddOrGet<Tutorials>();
			wishingStarEvent = childGo.AddOrGet<WishingStarEvent>();

			treasury = childGo.AddOrGet<Treasury>();
			treasury.Configure();

			childGo.AddOrGet<KelpSubmersionMonitorUpdater>();
			childGo.AddOrGet<TileUpdater>();

			// todo: maybe a simple lerp is good enough here
			var dayTime = Util.ColorFromHex("C8AB9B");
			var nightTime = Util.ColorFromHex("A5CAFF");

			overlayGradient = new Gradient
			{
				colorKeys =
				[
					new GradientColorKey(dayTime, 0f),
					new GradientColorKey(nightTime, 1f),
				],
				alphaKeys =
				[
					new GradientAlphaKey(1f, 0f)
				]
			};
		}

		public void InitWaterCamera(Camera reference)
		{
			int mWidth = Screen.width;
			int mHeight = Screen.height;

			waterTarget = new RenderTexture(mWidth, mHeight, 24);
			debugWaterTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			debugWaterTex.SetPixel(0, 0, new Color(0, 0, 0, 0));
			debugWaterTex.Resize(mWidth, mHeight);
			debugWaterTex.Apply();

			waterCamera = CameraController.Instance.CopyCamera(reference, "Beached_WaterCamera");
			waterCamera.transform.parent = reference.transform.parent;
			waterCamera.cullingMask = LayerMask.GetMask("Water"); /// <see cref="Patches.WaterCubesPatch.WaterCubes_Init_Patch"/>  //, "Pickupables"); 
			waterCamera.targetTexture = waterTarget;

			ModAssets.Materials.liquidRefractionMat.SetTexture("_RenderedLiquid", waterTarget);
		}

		public void SetCullingMask(string cullingMask)
		{
			waterCamera.cullingMask = LayerMask.GetMask(cullingMask);
		}

		public override void OnCleanUp()
		{
			Instance = null;

			if (iridescenceEffect != null)
				Util.KDestroyGameObject(iridescenceEffect.gameObject);
		}

		public void Render200ms(float dt)
		{
			if (Game.Instance == null)
				return;

			if (Game.Instance.IsPaused && shaderPropertyUpdated)
				return;

			Shader.SetGlobalColor("_Beached_TimeOfDayColor", overlayGradient.Evaluate(TimeOfDay.Instance.scale));
			shaderPropertyUpdated = true;
		}

		public void OnStarmapScreenLoadPlanets()
		{

		}

		public void SetBeltOrigin(AxialI swarmOriginLocation)
		{
			beltCenter = swarmOriginLocation;

		}
	}
}

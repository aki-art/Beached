using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.PostFX;
using KSerialization;
using UnityEngine;
using static STRINGS.DUPLICANTS.ATTRIBUTES;

namespace Beached.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Beached_Mod : KMonoBehaviour
    {
        public static Beached_Mod Instance;

        // a bunch of singleton components i don't want to spam on SaveGame
        // these do not serialize, put serialialized data here
        public IridescenceEffect iridescenceEffect;
        public ElementInteractions elementInteractions;
        public Tutorials tutorials;
        public Treasury treasury;
        public WishingStarEvent wishingStarEvent;

        public Camera waterCamera;
        private RenderTexture waterTarget;
        private Texture2D debugWaterTex;
        private CameraRenderTexture render;

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
            waterCamera.cullingMask = LayerMask.GetMask("Water"); /// <see cref="Patches.WaterCubesPatch.WaterCubes_Init_Patch"/>
            waterCamera.targetTexture = waterTarget;

            //ModAssets.Materials.liquidRefractionMat.SetTexture("_RenderedLiquid", waterTarget);
            //waterCamera.depthTextureMode |= DepthTextureMode.Depth;

            //waterCamera.gameObject.AddComponent<WaterPostFx>();

            //reference.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_Water";
            //waterCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = reference;
        }

        public void SetCullingMask(string cullingMask)
        {
            waterCamera.cullingMask = LayerMask.GetMask(cullingMask);
        }

        
        public void RenderDebugWater()
        {
            RenderTexture.active = waterTarget;

            GL.Clear(true, true, Color.clear);

            waterCamera.Render();

            var rect = new Rect(0, 0, debugWaterTex.width, debugWaterTex.height);
            debugWaterTex.ReadPixels(rect, 0, 0);
            debugWaterTex.Apply();

            ModAssets.SaveImage(debugWaterTex, "watertest_");

            RenderTexture.active = null;
        }

        public override void OnCleanUp()
        {
            Instance = null;

            if (iridescenceEffect != null)
            {
                Util.KDestroyGameObject(iridescenceEffect.gameObject);
            }
        }
    }
}

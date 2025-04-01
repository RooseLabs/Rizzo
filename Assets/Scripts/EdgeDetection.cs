// Sourced from https://ameye.dev/notes/edge-detection-outlines/
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class EdgeDetection : ScriptableRendererFeature
{
    private class EdgeDetectionPass : ScriptableRenderPass
    {
        private Material _material;

        private static readonly int OutlineThicknessProperty = Shader.PropertyToID("_OutlineThickness");
        private static readonly int OutlineColorProperty = Shader.PropertyToID("_OutlineColor");
        private static readonly int DepthThresholdProperty = Shader.PropertyToID("_DepthThreshold");
        private static readonly int NormalThresholdProperty = Shader.PropertyToID("_NormalThreshold");
        private static readonly int LuminanceThresholdProperty = Shader.PropertyToID("_LuminanceThreshold");
        
        public EdgeDetectionPass()
        {
            profilingSampler = new ProfilingSampler(nameof(EdgeDetectionPass));
        }

        public void Setup(ref EdgeDetectionSettings settings, ref Material edgeDetectionMaterial)
        {
            _material = edgeDetectionMaterial;
            renderPassEvent = settings.renderPassEvent;

            if (settings.scaleWithResolution)
            {
                float scaleFactor = Screen.height / (float)settings.resolution;
                float adjustedOutlineThickness = settings.outlineThickness * scaleFactor;
                _material.SetFloat(OutlineThicknessProperty, adjustedOutlineThickness);
            }
            else
            {
                _material.SetFloat(OutlineThicknessProperty, settings.outlineThickness);
            }
            _material.SetColor(OutlineColorProperty, settings.outlineColor);
            _material.SetFloat(DepthThresholdProperty, settings.depthThreshold);
            _material.SetFloat(NormalThresholdProperty, settings.normalThreshold);
            _material.SetFloat(LuminanceThresholdProperty, settings.luminanceThreshold);
        }

        private class PassData { }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();

            using var builder = renderGraph.AddRasterRenderPass<PassData>("Edge Detection", out _);

            builder.SetRenderAttachment(resourceData.activeColorTexture, 0);
            builder.UseAllGlobalTextures(true);
            builder.AllowPassCulling(false);
            builder.SetRenderFunc((PassData _, RasterGraphContext context) =>
            {
                Blitter.BlitTexture(context.cmd, Vector2.one, _material, 0);
            });
        }
    }

    [Serializable]
    public class EdgeDetectionSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        [Range(0, 15)] public int outlineThickness = 3;
        public Color outlineColor = Color.black;
        public float depthThreshold = 1 / 200.0f;
        public float normalThreshold = 1 / 4.0f;
        public float luminanceThreshold = 1 / 0.5f;
        public bool scaleWithResolution = true;
        public Resolution resolution = Resolution._1440p;

        public enum Resolution
        {
            _720p = 720,
            _1080p = 1080,
            _1440p = 1440,
            _2160p = 2160
        }
    }

    [SerializeField] private EdgeDetectionSettings settings;
    private Material _edgeDetectionMaterial;
    private EdgeDetectionPass _edgeDetectionPass;

    /// <summary>
    /// Called
    /// - When the Scriptable Renderer Feature loads the first time.
    /// - When you enable or disable the Scriptable Renderer Feature.
    /// - When you change a property in the Inspector window of the Renderer Feature.
    /// </summary>
    public override void Create()
    {
        _edgeDetectionPass ??= new EdgeDetectionPass();
    }

    /// <summary>
    /// Called
    /// - Every frame, once for each camera.
    /// </summary>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Don't render for some views.
        if (renderingData.cameraData.cameraType == CameraType.Preview
            || renderingData.cameraData.cameraType == CameraType.Reflection
            || UniversalRenderer.IsOffscreenDepthTexture(ref renderingData.cameraData))
            return;
        
        if (_edgeDetectionMaterial == null)
        {
            _edgeDetectionMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Edge Detection"));
            if (_edgeDetectionMaterial == null)
            {
                Debug.LogWarning("Not all required materials could be created. Edge Detection will not render.");
                return;
            }
        }

        _edgeDetectionPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal | ScriptableRenderPassInput.Color);
        _edgeDetectionPass.requiresIntermediateTexture = true;
        _edgeDetectionPass.Setup(ref settings, ref _edgeDetectionMaterial);

        renderer.EnqueuePass(_edgeDetectionPass);
    }

    /// <summary>
    /// Clean up resources allocated to the Scriptable Renderer Feature such as materials.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        _edgeDetectionPass = null;
        CoreUtils.Destroy(_edgeDetectionMaterial);
    }
}
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public struct SingleBlitPostProcessRenderPassConfig
{
    public Shader renderingShader;
    public string passName;
}

public class SingleBlitPostProcessRenderPass<TVolumeComponent> : ScriptableRenderPass where TVolumeComponent : VolumeComponent
{
    protected Material renderingMaterial;
    private RTHandle temporalBlitHandle;
    private string passName;
    
    public SingleBlitPostProcessRenderPass(SingleBlitPostProcessRenderPassConfig config)
    {
        this.passName = config.passName;
        if(config.renderingShader != null)
            renderingMaterial = new Material(config.renderingShader);
    }

    protected virtual int FindPass()
    {
        return renderingMaterial.FindPass("Universal Forward");
    }

    protected virtual void UpdateMaterialValues(TVolumeComponent volumeComponent)
    {
        throw new NotImplementedException($"Update Material Values should be implemented for a Scriptable Render pass suited for {typeof(TVolumeComponent)}");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (!(renderingData.cameraData.cameraType.HasFlag(CameraType.Game) ||
              renderingData.cameraData.cameraType.HasFlag(CameraType.SceneView))) return;
        if (renderingMaterial == null) return;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        RenderingUtils.ReAllocateIfNeeded(ref temporalBlitHandle, descriptor, name:$"{passName}_TemporalBlitTarget");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!(renderingData.cameraData.cameraType.HasFlag(CameraType.Game) ||
              renderingData.cameraData.cameraType.HasFlag(CameraType.SceneView))) return;
        //Validate material
        if (renderingMaterial == null)
        {
            //Debug.LogError("Material is null, can't render");
            return;
        }

        TVolumeComponent volumeComponent = VolumeManager.instance.stack.GetComponent<TVolumeComponent>();
        if (volumeComponent is IPostProcessComponent postProcessComponent)
        {
            if (!postProcessComponent.IsActive())
            {
                //Debug.Log($"Post process ({typeof(TVolumeComponent)}) is not active");
                return;
            }
        }
        CommandBuffer cmd = CommandBufferPool.Get(passName);
        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
        UpdateMaterialValues(volumeComponent);
        cmd.Blit(cameraTargetHandle, temporalBlitHandle);
        cmd.Blit(temporalBlitHandle, cameraTargetHandle, renderingMaterial, FindPass());
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

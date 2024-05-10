using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class MultipleBlitPostProcessRenderPass<TVolumeComponent> : ScriptableRenderPass where TVolumeComponent : VolumeComponent
{
    protected Material renderingMaterial;
    private RTHandle temporalBlitHandle;
    private RTHandle temporalBlitHandle1;
    private string passName;
    
    public MultipleBlitPostProcessRenderPass(SingleBlitPostProcessRenderPassConfig config)
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

    protected virtual int GetIterationCount(TVolumeComponent volumeComponent)
    {
        throw new NotImplementedException($"Get Iteration Count should be implemented for a Scriptable Render pass suited for {typeof(TVolumeComponent)}");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (!(renderingData.cameraData.cameraType.HasFlag(CameraType.Game) ||
              renderingData.cameraData.cameraType.HasFlag(CameraType.SceneView))) return;
        if (renderingMaterial == null) return;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        RenderingUtils.ReAllocateIfNeeded(ref temporalBlitHandle, descriptor, name:$"{passName}_TemporalBlitTarget");
        RenderingUtils.ReAllocateIfNeeded(ref temporalBlitHandle1, descriptor, name:$"{passName}_TemporalBlitTarget_1");
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
        if (volumeComponent == null) return;
        if (!volumeComponent.active) return;
        if (volumeComponent is IPostProcessComponent postProcessComponent)
        {
            if (!postProcessComponent.IsActive())
            {
                //Debug.Log($"Post process ({typeof(TVolumeComponent)}) is not active");
                return;
            }
        }
        Debug.Log(volumeComponent.active);
        CommandBuffer cmd = CommandBufferPool.Get(passName);
        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
        UpdateMaterialValues(volumeComponent);
        
        RTHandle src = cameraTargetHandle;
        RTHandle dest = temporalBlitHandle;
        cmd.Blit(src, dest);
        src = temporalBlitHandle;
        dest = temporalBlitHandle1;
        for (int i = 0; i < GetIterationCount(volumeComponent); i++)
        {
            cmd.Blit(src,dest,renderingMaterial, FindPass());
            (src, dest) = (dest, src);
        }

        dest = cameraTargetHandle;
        
        cmd.Blit(src,dest);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

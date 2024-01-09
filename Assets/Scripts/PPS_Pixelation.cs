using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
 
[Serializable]
[PostProcess(typeof(PPS_Pixelation_Renderer), PostProcessEvent.AfterStack, "Mobile/Bloom")]
public sealed class PPS_Pixelation : PostProcessEffectSettings
{
    [Range(1f, 64f), Tooltip("Pixelation")]
    public IntParameter Pixelation = new IntParameter { value = 1 };
}
 
public sealed class PPS_Pixelation_Renderer : PostProcessEffectRenderer<PPS_Pixelation>
{
    public override void Render(PostProcessRenderContext context)
    {
      //  var sheet = context.propertySheets.Get(Shader.Find("Hidden/PPSM_Bloom"));
        int width = context.screenWidth / settings.Pixelation;
        int height = context.screenHeight / settings.Pixelation;
        RenderTextureFormat format = context.sourceFormat;

        RenderTexture r = RenderTexture.GetTemporary(
            width, height, 0, format
        );

        //Change the render texture's filter mode to point to make the screen pixelated
        r.filterMode = FilterMode.Point;

        //context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        context.command.BlitFullscreenTriangle(context.source,r, true);
        context.command.BlitFullscreenTriangle(r, context.destination);
        RenderTexture.ReleaseTemporary(r);
    }
}
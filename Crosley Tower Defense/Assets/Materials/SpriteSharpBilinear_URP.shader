Shader "Universal Render Pipeline/2D/Custom/SpriteSharpBilinear"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        _Sharpness ("Sharpness", Range(0.1, 8)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }

        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize; // .xy = 1/width,1/height  .zw = width,height

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _RendererColor;
                float _Sharpness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color * _RendererColor;

                return OUT;
            }

            // Bilinear-samples _MainTex, but compresses the blend region to a
            // thin band centered on each texel boundary (sized to ~1 screen
            // pixel via fwidth). Texel interiors stay flat/crisp; only the
            // seam blends, continuously, as uv changes sub-pixel amounts.
            float4 SampleSharp(float2 uv)
            {
                float2 texSize = _MainTex_TexelSize.zw;
                float2 texel = uv * texSize;

                // Nearest texel center at or below this position, and the
                // fractional position between it and the next center up.
                // f == 0.5 lands exactly on the true texel boundary.
                float2 centerCoord = floor(texel - 0.5) + 0.5;
                float2 f = texel - centerCoord;

                // Width of one screen pixel, projected into texel space.
                float2 screenDelta = fwidth(texel);
                float2 halfWidth = clamp((screenDelta * 0.5) / max(_Sharpness, 0.0001), 1e-5, 0.5);

                // 0 well inside the lower texel, 1 well inside the upper
                // texel, linear ramp only within the boundary band.
                float2 t = saturate((f - (0.5 - halfWidth)) / (2.0 * halfWidth));

                float2 sharpUV = (centerCoord + t) / texSize;
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sharpUV);
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 c = SampleSharp(IN.uv) * IN.color;
                c.rgb *= c.a; // premultiplied alpha, matches Blend One OneMinusSrcAlpha
                return c;
            }
            ENDHLSL
        }
    }
}

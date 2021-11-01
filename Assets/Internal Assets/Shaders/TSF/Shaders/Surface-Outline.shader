Shader "MyShader/Surface Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(0, 10)) = 0.03
        _Pixel("Pixel Perfect On", Range(0, 1)) = 0
        _TextureEmission("Texture Emission", Range(0, 10)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _EmissionColor;
        half _TextureEmission;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = _EmissionColor.rgb * _EmissionColor.a;
            if (_TextureEmission > 0) o.Emission *= c.rgb * _TextureEmission;
        }
        ENDCG

        Pass{
            Cull Front
            CGPROGRAM

            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram
        
            half _OutlineWidth;
            half _Pixel;
        
            float4 VertexProgram(float4 position : POSITION, float3 normal : NORMAL) : SV_POSITION {
                if (_Pixel == 0) {
                    position.xyz += normal * _OutlineWidth * 0.02;
                    return UnityObjectToClipPos(position);
                }

                float4 clipPosition = UnityObjectToClipPos(position);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, normal));

                float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _OutlineWidth * clipPosition.w * 2;
                clipPosition.xy += offset;

                return clipPosition;

            }
        
            half4 _OutlineColor;

            half4 FragmentProgram() : SV_TARGET {
                return _OutlineColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
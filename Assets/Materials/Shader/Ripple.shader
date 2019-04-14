Shader "Custom/Ripple"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmmisionTex("Emmision Texture",2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_RippleOrigin("Ripple Origin",Vector) = (0,0,0,0)
		_RippleDistance("Ripple Distance",Float) = 10
		_RippleWidth("Ripple Width",Float) = 0.1
		_RippleBrightness("Ripple Brightness",Float) = 1
		_RippleSharpness("Ripple Sharpness",Float) = 2
		_RippleFrequency("Ripple Frequency",Float) = 1
		_PointsCount("Points Count",Range(0,10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
		//#pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0



        sampler2D _MainTex;
		sampler2D _EmmisionTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_EmmisionTex;
			float3 worldPos;
		};

		/*
		struct vertexInput {
			float4 vertex : POSITION;
			float4 texcoord0 : TEXCOORD0;
		};

		float4 vert(Input v) : POSITION{
			return mul(UNITY_MATRIX_MVP, v.vertex);

		}
		*/


        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _RippleOrigin;
		float _RippleDistance;
		float _RippleWidth;
		float _RippleSharpness;
		float _RippleBrightness;
		float _RippleFrequency;
		fixed4 _Points[10];
		half _PointsCount;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

			 half distance = length(IN.worldPos.xyz - _RippleOrigin.xyz) - _RippleDistance;
			 half ringStrength = pow(1 - (abs(distance) / 1000), _RippleSharpness);

            // Albedo comes from a texture tinted by color


			 float2 uv_MainAlterered = IN.uv_MainTex;

			fixed4 c = tex2D(_MainTex, uv_MainAlterered) * _Color;
			fixed4 e = tex2D(_EmmisionTex, uv_MainAlterered);

            o.Albedo += clamp(c.rgb * ringStrength	,0,1);
			o.Emission += clamp(o.Albedo * (e.rgb) * _RippleBrightness *(0.5f+(abs(distance) / 1000)),0,1);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			//o.normal = e.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

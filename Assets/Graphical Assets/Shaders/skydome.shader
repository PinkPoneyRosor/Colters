Shader "Environment/Skydome" {
	Properties {
		DirectionalityFactor("DirectionalityFactor",float) = 0.50468
		SunColorIntensity("SunColorIntensity",float) = 1.468
		curveTexture ("curveTexture", 2D) = "white" {}
		LightDir("LightDir",Vector) = (-0.657,-0.024,0.7758)
		vBetaRayleigh("vBetaRayleigh",Vector) = (0.0008,0.0014,0.0029)
		BetaRayTheta("BetaRayTheta",Vector) = (0.0001,0.0002,0.005)
		vBetaMie("vBetaMie",Vector) = (0.0012,0.0016,0.0023)
		BetaMieTheta("BetaMieTheta",Vector) = (0.0009,0.0012,0.0017)
		g_vSunColor("g_vSunColor",Vector) = (0.6878,0.5951,0.4217)
		hueShift("hueShift",float) =0
		satM("saturationMultiplier",float) =1
		briM("brightnessMultiplier",float) =1
	}

	SubShader {
	Pass {
		Cull Front
		Fog {Mode Off}
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"
		float3 vBetaRayleigh;
		float3 BetaRayTheta;
		float3 vBetaMie;
		float3 BetaMieTheta;
		float3 LightDir;
		float3 g_vSunColor;
		float DirectionalityFactor;
		float SunColorIntensity;
		float satM;
		float briM;
		float hueShift;

		sampler2D curveTexture;

		struct vertex_output {
			float4 position 		: POSITION;
			float4 color 			: COLOR;
			float orgposz 			: TEXCOORD0;
		};

		float3 BetaR(float Theta){
			return BetaRayTheta*(3.0f+0.5f*Theta*Theta);
		}

		float3 BetaM(float Theta){
			float g =DirectionalityFactor;
			return(BetaMieTheta*pow(1.0f-g,2.0f))/(pow(1+g*g-2*g*Theta,1.5f));
		}

		float3 Lin(float Theta,float SR,float SM){
			return ((BetaR(Theta)+BetaM(Theta))*(1.0f-exp(-(vBetaRayleigh*SR+vBetaMie*SM))))/(vBetaRayleigh + vBetaMie );
		}

		float3 RGBtoHSV(in float3 RGB)
		{
			float3 HSV = float3(0,0,0);
	
			HSV.z = max(RGB.r, max(RGB.g, RGB.b));
			float M = min(RGB.r, min(RGB.g, RGB.b));
			float C = HSV.z - M;
		
			if (C != 0)
			{
				HSV.y = C / HSV.z;
				float3 Delta = (HSV.z - RGB) / C;
				Delta.rgb -= Delta.brg;
				Delta.rg += float2(2,4);
				if (RGB.r >= HSV.z)
					HSV.x = Delta.b;
				else if (RGB.g >= HSV.z)
					HSV.x = Delta.r;
				else
					HSV.x = Delta.g;
				HSV.x = frac(HSV.x / 6);
			}
			return HSV;
		}

		float3 Hue(float H)
		{
			float R = abs(H * 6 - 3) - 1;
			float G = 2 - abs(H * 6 - 2);
			float B = 2 - abs(H * 6 - 4);
			return saturate(float3(R,G,B));
		}

		float3 HSVtoRGB(float3 HSV)
		{
			return ((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z;
		}

		float3 Saturate(float3 rgb, float h, float s, float b,float st, float bt) {
			float3 hsv = RGBtoHSV(rgb);
			hsv.x = fmod(hsv.x+h,360);
			hsv.y = clamp(hsv.y*s +st,0,1);
			hsv.z = clamp(hsv.z*b +bt,0,1);
			return (HSVtoRGB(hsv));
			return rgb;
		}
		
		float3 Curve(float3 rgb) {
			#if !defined(SHADER_API_OPENGL)
			rgb.r = tex2D(curveTexture, float2(rgb.r,0)).r;
			rgb.g = tex2D(curveTexture, float2(rgb.g,0)).g;
			rgb.b = tex2D(curveTexture, float2(rgb.b,0)).b;
			#endif 
			return rgb;
		}
		
		vertex_output vert(appdata_base Input) {
			vertex_output OUT;
			
			float3 vPosWorld = mul(UNITY_MATRIX_MV,Input.vertex);
			
			float3 ray =  ObjSpaceViewDir(Input.vertex);
			float far = length(ray) ;
			ray = normalize(ray);
			float Theta = dot(ray, LightDir);
			float SR =(1.05f-pow(ray.y,0.3f)) * 2000;
			float SM=far*0.05f;
			float3 L=Lin(Theta, SR, SM );
 
			float3 normVect=normalize(Input.vertex)/100;
			OUT.orgposz=abs(Input.vertex.y);
			OUT.position=mul(UNITY_MATRIX_MVP,Input.vertex);
			OUT.color.rgb=Saturate(Curve(L*g_vSunColor*SunColorIntensity),hueShift,satM,briM,0,0);
			OUT.color.a=1.0f;
			return OUT;
		}
		
		float4 frag (vertex_output IN): COLOR {
			return IN.color;
		}
		
		ENDCG
		}
	}
	FallBack "VertexLit"
}
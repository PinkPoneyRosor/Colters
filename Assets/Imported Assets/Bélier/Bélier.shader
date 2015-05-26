// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:False,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31841,y:32814|normal-793-RGB,emission-1181-OUT,custl-712-OUT;n:type:ShaderForge.SFN_Multiply,id:712,x:32244,y:33165|A-1104-OUT,B-718-RGB,C-719-OUT;n:type:ShaderForge.SFN_LightColor,id:718,x:32490,y:33349;n:type:ShaderForge.SFN_LightAttenuation,id:719,x:32490,y:33492;n:type:ShaderForge.SFN_LightVector,id:730,x:33289,y:32853;n:type:ShaderForge.SFN_NormalVector,id:731,x:33289,y:32993,pt:True;n:type:ShaderForge.SFN_Dot,id:733,x:33066,y:32898,dt:1|A-730-OUT,B-731-OUT;n:type:ShaderForge.SFN_Tex2d,id:751,x:33066,y:32707,ptlb:Diffuse,ptin:_Diffuse,tex:87860a9905cab644ea1167ce6a84761c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:752,x:32845,y:32777,cmnt:Diffuse Contrib|A-751-RGB,B-733-OUT;n:type:ShaderForge.SFN_Tex2d,id:793,x:32247,y:32443,ptlb:Normal,ptin:_Normal,tex:dbde42370f4de0243a0ee4a5d604208a,ntxv:3,isnm:False;n:type:ShaderForge.SFN_HalfVector,id:1097,x:33289,y:33163;n:type:ShaderForge.SFN_Dot,id:1098,x:33066,y:33135,dt:1|A-731-OUT,B-1097-OUT;n:type:ShaderForge.SFN_Power,id:1100,x:32883,y:33135|VAL-1098-OUT,EXP-1163-OUT;n:type:ShaderForge.SFN_Add,id:1104,x:32543,y:32964|A-752-OUT,B-1137-OUT;n:type:ShaderForge.SFN_Slider,id:1111,x:33425,y:33428,ptlb:Glossiness,ptin:_Glossiness,min:1,cur:1,max:11;n:type:ShaderForge.SFN_Multiply,id:1137,x:32666,y:33135,cmnt:Specular Contrib|A-1100-OUT,B-1144-OUT;n:type:ShaderForge.SFN_Slider,id:1138,x:33099,y:33774,ptlb:Specularity,ptin:_Specularity,min:0,cur:0,max:4;n:type:ShaderForge.SFN_Multiply,id:1144,x:32975,y:33598|A-1145-RGB,B-1138-OUT;n:type:ShaderForge.SFN_Tex2d,id:1145,x:33218,y:33581,ptlb:node_1145,ptin:_node_1145,tex:8617885c59c7baa4480db105400e88a7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Exp,id:1163,x:33077,y:33367,et:1|IN-1111-OUT;n:type:ShaderForge.SFN_AmbientLight,id:1174,x:33083,y:32471;n:type:ShaderForge.SFN_Multiply,id:1181,x:32712,y:32585|A-1174-RGB,B-751-RGB;proporder:751-793-1111-1138-1145;pass:END;sub:END;*/

Shader "Custom/Bélier" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _Glossiness ("Glossiness", Range(1, 11)) = 1
        _Specularity ("Specularity", Range(0, 4)) = 0
        _node_1145 ("node_1145", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glossiness;
            uniform float _Specularity;
            uniform sampler2D _node_1145; uniform float4 _node_1145_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_1238 = i.uv0;
                float3 normalLocal = tex2D(_Normal,TRANSFORM_TEX(node_1238.rg, _Normal)).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
////// Emissive:
                float4 node_751 = tex2D(_Diffuse,TRANSFORM_TEX(node_1238.rg, _Diffuse));
                float3 emissive = (UNITY_LIGHTMODEL_AMBIENT.rgb*node_751.rgb);
                float3 node_731 = normalDirection;
                float3 finalColor = emissive + (((node_751.rgb*max(0,dot(lightDirection,node_731)))+(pow(max(0,dot(node_731,halfDirection)),exp2(_Glossiness))*(tex2D(_node_1145,TRANSFORM_TEX(node_1238.rg, _node_1145)).rgb*_Specularity)))*_LightColor0.rgb*attenuation);
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _Glossiness;
            uniform float _Specularity;
            uniform sampler2D _node_1145; uniform float4 _node_1145_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_1239 = i.uv0;
                float3 normalLocal = tex2D(_Normal,TRANSFORM_TEX(node_1239.rg, _Normal)).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float4 node_751 = tex2D(_Diffuse,TRANSFORM_TEX(node_1239.rg, _Diffuse));
                float3 node_731 = normalDirection;
                float3 finalColor = (((node_751.rgb*max(0,dot(lightDirection,node_731)))+(pow(max(0,dot(node_731,halfDirection)),exp2(_Glossiness))*(tex2D(_node_1145,TRANSFORM_TEX(node_1239.rg, _node_1145)).rgb*_Specularity)))*_LightColor0.rgb*attenuation);
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9938,x:32057,y:32594|diff-244-OUT,normal-169-OUT;n:type:ShaderForge.SFN_Lerp,id:244,x:32365,y:32473|A-2637-OUT,B-1741-RGB,T-846-RGB;n:type:ShaderForge.SFN_Tex2d,id:5835,x:32803,y:31744,ptlb:TA_diffuse,ptin:_TA_diffuse,ntxv:0,isnm:False|UVIN-7488-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1541,x:32803,y:31918,ptlb:TB_diffuse,ptin:_TB_diffuse,ntxv:0,isnm:False|UVIN-4975-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2081,x:32803,y:32095,ptlb:MASK2,ptin:_MASK2,ntxv:0,isnm:False|UVIN-2262-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2262,x:33043,y:32095,uv:1;n:type:ShaderForge.SFN_TexCoord,id:7488,x:33043,y:31744,uv:0;n:type:ShaderForge.SFN_TexCoord,id:4975,x:33043,y:31918,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1556,x:32803,y:32820,ptlb:TB_normal,ptin:_TB_normal,ntxv:3,isnm:True|UVIN-9290-UVOUT;n:type:ShaderForge.SFN_Lerp,id:4051,x:32576,y:32649|A-636-RGB,B-1556-RGB,T-2081-RGB;n:type:ShaderForge.SFN_Tex2d,id:636,x:32803,y:32642,ptlb:TA_normal,ptin:_TA_normal,ntxv:3,isnm:True|UVIN-7325-UVOUT;n:type:ShaderForge.SFN_Lerp,id:2637,x:32586,y:31928|A-5835-RGB,B-1541-RGB,T-2081-RGB;n:type:ShaderForge.SFN_Tex2d,id:1741,x:32803,y:32279,ptlb:TC_diffuse,ptin:_TC_diffuse,ntxv:0,isnm:False|UVIN-4845-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:846,x:32803,y:32457,ptlb:MASK1,ptin:_MASK1,ntxv:0,isnm:False|UVIN-2288-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2288,x:33043,y:32457,uv:1;n:type:ShaderForge.SFN_TexCoord,id:4845,x:33043,y:32279,uv:0;n:type:ShaderForge.SFN_TexCoord,id:7325,x:33043,y:32642,uv:0;n:type:ShaderForge.SFN_TexCoord,id:9290,x:33043,y:32820,uv:0;n:type:ShaderForge.SFN_TexCoord,id:7211,x:33043,y:33010,uv:0;n:type:ShaderForge.SFN_Lerp,id:169,x:32344,y:32671|A-4051-OUT,B-2233-RGB,T-846-RGB;n:type:ShaderForge.SFN_Tex2d,id:2233,x:32803,y:33010,ptlb:TC_normal,ptin:_TC_normal,ntxv:3,isnm:True|UVIN-7211-UVOUT;proporder:5835-636-1541-1556-1741-2233-846-2081;pass:END;sub:END;*/

Shader "Custom/Lerp" {
    Properties {
        _TA_diffuse ("TA_diffuse", 2D) = "white" {}
        _TA_normal ("TA_normal", 2D) = "bump" {}
        _TB_diffuse ("TB_diffuse", 2D) = "white" {}
        _TB_normal ("TB_normal", 2D) = "bump" {}
        _TC_diffuse ("TC_diffuse", 2D) = "white" {}
        _TC_normal ("TC_normal", 2D) = "bump" {}
        _MASK1 ("MASK1", 2D) = "white" {}
        _MASK2 ("MASK2", 2D) = "white" {}
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
            uniform sampler2D _TA_diffuse; uniform float4 _TA_diffuse_ST;
            uniform sampler2D _TB_diffuse; uniform float4 _TB_diffuse_ST;
            uniform sampler2D _MASK2; uniform float4 _MASK2_ST;
            uniform sampler2D _TB_normal; uniform float4 _TB_normal_ST;
            uniform sampler2D _TA_normal; uniform float4 _TA_normal_ST;
            uniform sampler2D _TC_diffuse; uniform float4 _TC_diffuse_ST;
            uniform sampler2D _MASK1; uniform float4 _MASK1_ST;
            uniform sampler2D _TC_normal; uniform float4 _TC_normal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
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
                float2 node_7325 = i.uv0;
                float2 node_9290 = i.uv0;
                float2 node_2262 = i.uv1;
                float4 node_2081 = tex2D(_MASK2,TRANSFORM_TEX(node_2262.rg, _MASK2));
                float2 node_7211 = i.uv0;
                float2 node_2288 = i.uv1;
                float4 node_846 = tex2D(_MASK1,TRANSFORM_TEX(node_2288.rg, _MASK1));
                float3 normalLocal = lerp(lerp(UnpackNormal(tex2D(_TA_normal,TRANSFORM_TEX(node_7325.rg, _TA_normal))).rgb,UnpackNormal(tex2D(_TB_normal,TRANSFORM_TEX(node_9290.rg, _TB_normal))).rgb,node_2081.rgb),UnpackNormal(tex2D(_TC_normal,TRANSFORM_TEX(node_7211.rg, _TC_normal))).rgb,node_846.rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz*2;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_7488 = i.uv0;
                float2 node_4975 = i.uv0;
                float2 node_4845 = i.uv0;
                finalColor += diffuseLight * lerp(lerp(tex2D(_TA_diffuse,TRANSFORM_TEX(node_7488.rg, _TA_diffuse)).rgb,tex2D(_TB_diffuse,TRANSFORM_TEX(node_4975.rg, _TB_diffuse)).rgb,node_2081.rgb),tex2D(_TC_diffuse,TRANSFORM_TEX(node_4845.rg, _TC_diffuse)).rgb,node_846.rgb);
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
            uniform sampler2D _TA_diffuse; uniform float4 _TA_diffuse_ST;
            uniform sampler2D _TB_diffuse; uniform float4 _TB_diffuse_ST;
            uniform sampler2D _MASK2; uniform float4 _MASK2_ST;
            uniform sampler2D _TB_normal; uniform float4 _TB_normal_ST;
            uniform sampler2D _TA_normal; uniform float4 _TA_normal_ST;
            uniform sampler2D _TC_diffuse; uniform float4 _TC_diffuse_ST;
            uniform sampler2D _MASK1; uniform float4 _MASK1_ST;
            uniform sampler2D _TC_normal; uniform float4 _TC_normal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
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
                float2 node_7325 = i.uv0;
                float2 node_9290 = i.uv0;
                float2 node_2262 = i.uv1;
                float4 node_2081 = tex2D(_MASK2,TRANSFORM_TEX(node_2262.rg, _MASK2));
                float2 node_7211 = i.uv0;
                float2 node_2288 = i.uv1;
                float4 node_846 = tex2D(_MASK1,TRANSFORM_TEX(node_2288.rg, _MASK1));
                float3 normalLocal = lerp(lerp(UnpackNormal(tex2D(_TA_normal,TRANSFORM_TEX(node_7325.rg, _TA_normal))).rgb,UnpackNormal(tex2D(_TB_normal,TRANSFORM_TEX(node_9290.rg, _TB_normal))).rgb,node_2081.rgb),UnpackNormal(tex2D(_TC_normal,TRANSFORM_TEX(node_7211.rg, _TC_normal))).rgb,node_846.rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_7488 = i.uv0;
                float2 node_4975 = i.uv0;
                float2 node_4845 = i.uv0;
                finalColor += diffuseLight * lerp(lerp(tex2D(_TA_diffuse,TRANSFORM_TEX(node_7488.rg, _TA_diffuse)).rgb,tex2D(_TB_diffuse,TRANSFORM_TEX(node_4975.rg, _TB_diffuse)).rgb,node_2081.rgb),tex2D(_TC_diffuse,TRANSFORM_TEX(node_4845.rg, _TC_diffuse)).rgb,node_846.rgb);
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

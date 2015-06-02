// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9938,x:33052,y:32620,varname:node_9938,prsc:2|diff-244-OUT,normal-4051-OUT;n:type:ShaderForge.SFN_Lerp,id:244,x:32848,y:32593,varname:node_244,prsc:2|A-5835-RGB,B-1541-RGB,T-2081-RGB;n:type:ShaderForge.SFN_Tex2d,id:5835,x:32686,y:32439,ptovrint:False,ptlb:TA_diffuse,ptin:_TA_diffuse,varname:node_5835,prsc:2,tex:4af080d9167ea884dacdb01f64507f81,ntxv:0,isnm:False|UVIN-7488-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1541,x:32561,y:32606,ptovrint:False,ptlb:TB_texture,ptin:_TB_texture,varname:node_1541,prsc:2,tex:4847d6fafc52ec642b03652c575fbae0,ntxv:0,isnm:False|UVIN-4975-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2081,x:32681,y:32772,ptovrint:False,ptlb:MASK,ptin:_MASK,varname:node_2081,prsc:2,tex:afd74c66579372c488129ec35bf1d754,ntxv:0,isnm:False|UVIN-2262-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2262,x:32525,y:32772,varname:node_2262,prsc:2,uv:1;n:type:ShaderForge.SFN_TexCoord,id:7488,x:32517,y:32439,varname:node_7488,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:4975,x:32394,y:32606,varname:node_4975,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1556,x:32479,y:33138,ptovrint:False,ptlb:TB_normal,ptin:_TB_normal,varname:node_1556,prsc:2,ntxv:3,isnm:True|UVIN-7441-UVOUT;n:type:ShaderForge.SFN_Lerp,id:4051,x:32882,y:32922,varname:node_4051,prsc:2|A-636-RGB,B-1556-RGB,T-5392-RGB;n:type:ShaderForge.SFN_Tex2d,id:636,x:32567,y:32958,ptovrint:False,ptlb:TA_normal,ptin:_TA_normal,varname:node_636,prsc:2,ntxv:3,isnm:True|UVIN-4625-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:5392,x:32814,y:33175,ptovrint:False,ptlb:MASK0,ptin:_MASK0,varname:node_5392,prsc:2,ntxv:3,isnm:False|UVIN-2459-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:4625,x:32400,y:32958,varname:node_4625,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:7441,x:32318,y:33138,varname:node_7441,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:2459,x:32649,y:33175,varname:node_2459,prsc:2,uv:1;proporder:5835-636-1541-1556-2081-5392;pass:END;sub:END;*/

Shader "Custom/Lerp" {
    Properties {
        _TA_diffuse ("TA_diffuse", 2D) = "white" {}
        _TA_normal ("TA_normal", 2D) = "bump" {}
        _TB_texture ("TB_texture", 2D) = "white" {}
        _TB_normal ("TB_normal", 2D) = "bump" {}
        _MASK ("MASK", 2D) = "white" {}
        _MASK0 ("MASK0", 2D) = "bump" {}
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
            uniform sampler2D _TB_texture; uniform float4 _TB_texture_ST;
            uniform sampler2D _MASK; uniform float4 _MASK_ST;
            uniform sampler2D _TB_normal; uniform float4 _TB_normal_ST;
            uniform sampler2D _TA_normal; uniform float4 _TA_normal_ST;
            uniform sampler2D _MASK0; uniform float4 _MASK0_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _TA_normal_var = UnpackNormal(tex2D(_TA_normal,TRANSFORM_TEX(i.uv0, _TA_normal)));
                float3 _TB_normal_var = UnpackNormal(tex2D(_TB_normal,TRANSFORM_TEX(i.uv0, _TB_normal)));
                float4 _MASK0_var = tex2D(_MASK0,TRANSFORM_TEX(i.uv1, _MASK0));
                float3 normalLocal = lerp(_TA_normal_var.rgb,_TB_normal_var.rgb,_MASK0_var.rgb);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb*2; // Ambient Light
                float4 _TA_diffuse_var = tex2D(_TA_diffuse,TRANSFORM_TEX(i.uv0, _TA_diffuse));
                float4 _TB_texture_var = tex2D(_TB_texture,TRANSFORM_TEX(i.uv0, _TB_texture));
                float4 _MASK_var = tex2D(_MASK,TRANSFORM_TEX(i.uv1, _MASK));
                float3 diffuse = (directDiffuse + indirectDiffuse) * lerp(_TA_diffuse_var.rgb,_TB_texture_var.rgb,_MASK_var.rgb);
/// Final Color:
                float3 finalColor = diffuse;
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
            uniform sampler2D _TB_texture; uniform float4 _TB_texture_ST;
            uniform sampler2D _MASK; uniform float4 _MASK_ST;
            uniform sampler2D _TB_normal; uniform float4 _TB_normal_ST;
            uniform sampler2D _TA_normal; uniform float4 _TA_normal_ST;
            uniform sampler2D _MASK0; uniform float4 _MASK0_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _TA_normal_var = UnpackNormal(tex2D(_TA_normal,TRANSFORM_TEX(i.uv0, _TA_normal)));
                float3 _TB_normal_var = UnpackNormal(tex2D(_TB_normal,TRANSFORM_TEX(i.uv0, _TB_normal)));
                float4 _MASK0_var = tex2D(_MASK0,TRANSFORM_TEX(i.uv1, _MASK0));
                float3 normalLocal = lerp(_TA_normal_var.rgb,_TB_normal_var.rgb,_MASK0_var.rgb);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _TA_diffuse_var = tex2D(_TA_diffuse,TRANSFORM_TEX(i.uv0, _TA_diffuse));
                float4 _TB_texture_var = tex2D(_TB_texture,TRANSFORM_TEX(i.uv0, _TB_texture));
                float4 _MASK_var = tex2D(_MASK,TRANSFORM_TEX(i.uv1, _MASK));
                float3 diffuse = directDiffuse * lerp(_TA_diffuse_var.rgb,_TB_texture_var.rgb,_MASK_var.rgb);
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

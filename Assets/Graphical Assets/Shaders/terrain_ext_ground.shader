// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32639,y:32704|diff-4-OUT,normal-3-OUT;n:type:ShaderForge.SFN_Lerp,id:2,x:33132,y:32290|A-5-RGB,B-6-RGB,T-11-RGB;n:type:ShaderForge.SFN_Lerp,id:3,x:32922,y:33239|A-34-OUT,B-53-RGB,T-65-RGB;n:type:ShaderForge.SFN_Lerp,id:4,x:32899,y:32607|A-2-OUT,B-64-RGB,T-65-RGB;n:type:ShaderForge.SFN_Tex2d,id:5,x:33349,y:32092,ptlb:texture1,ptin:_texture1,tex:8efddc9e7f432ad40bb93d9515485c96,ntxv:0,isnm:False|UVIN-28-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6,x:33349,y:32273,ptlb:texture3,ptin:_texture3,tex:af16cce485204fa4599917464e36639a,ntxv:0,isnm:False|UVIN-29-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:11,x:33711,y:32935,ptlb:mask2,ptin:_mask2,tex:45b11b95f87e8074db8a2833c6d71b2a,ntxv:0,isnm:False|UVIN-22-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:22,x:33881,y:32935,uv:1;n:type:ShaderForge.SFN_TexCoord,id:28,x:33503,y:32092,uv:0;n:type:ShaderForge.SFN_TexCoord,id:29,x:33503,y:32273,uv:0;n:type:ShaderForge.SFN_Lerp,id:34,x:33128,y:33100|A-36-RGB,B-38-RGB,T-11-RGB;n:type:ShaderForge.SFN_Tex2d,id:36,x:33341,y:33023,ptlb:normal1,ptin:_normal1,tex:8efddc9e7f432ad40bb93d9515485c96,ntxv:3,isnm:True|UVIN-44-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:38,x:33341,y:33204,ptlb:normal3,ptin:_normal3,tex:af16cce485204fa4599917464e36639a,ntxv:3,isnm:True|UVIN-46-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:44,x:33495,y:33023,uv:0;n:type:ShaderForge.SFN_TexCoord,id:46,x:33495,y:33204,uv:0;n:type:ShaderForge.SFN_Tex2d,id:53,x:33341,y:33591,ptlb:normal2,ptin:_normal2,tex:8efddc9e7f432ad40bb93d9515485c96,ntxv:3,isnm:True|UVIN-61-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:61,x:33495,y:33591,uv:0;n:type:ShaderForge.SFN_Tex2d,id:64,x:33341,y:32643,ptlb:texture2,ptin:_texture2,tex:c0937e55cd394194e9cf6de353fc1142,ntxv:0,isnm:False|UVIN-72-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:65,x:33271,y:32813,ptlb:mask1,ptin:_mask1,tex:b271b752f88ea45478b526f9a8b763b0,ntxv:0,isnm:False|UVIN-73-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:72,x:33503,y:32643,uv:0;n:type:ShaderForge.SFN_TexCoord,id:73,x:33503,y:32813,uv:1;proporder:5-6-11-36-38-53-64-65;pass:END;sub:END;*/

Shader "Shader Forge/terrain_ext_ground" {
    Properties {
        _texture1 ("texture1", 2D) = "white" {}
        _texture3 ("texture3", 2D) = "white" {}
        _mask2 ("mask2", 2D) = "white" {}
        _normal1 ("normal1", 2D) = "bump" {}
        _normal3 ("normal3", 2D) = "bump" {}
        _normal2 ("normal2", 2D) = "bump" {}
        _texture2 ("texture2", 2D) = "white" {}
        _mask1 ("mask1", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
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
            uniform sampler2D _texture1; uniform float4 _texture1_ST;
            uniform sampler2D _texture3; uniform float4 _texture3_ST;
            uniform sampler2D _mask2; uniform float4 _mask2_ST;
            uniform sampler2D _normal1; uniform float4 _normal1_ST;
            uniform sampler2D _normal3; uniform float4 _normal3_ST;
            uniform sampler2D _normal2; uniform float4 _normal2_ST;
            uniform sampler2D _texture2; uniform float4 _texture2_ST;
            uniform sampler2D _mask1; uniform float4 _mask1_ST;
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
                float2 node_44 = i.uv0;
                float2 node_46 = i.uv0;
                float2 node_22 = i.uv1;
                float4 node_11 = tex2D(_mask2,TRANSFORM_TEX(node_22.rg, _mask2));
                float2 node_61 = i.uv0;
                float2 node_73 = i.uv1;
                float4 node_65 = tex2D(_mask1,TRANSFORM_TEX(node_73.rg, _mask1));
                float3 normalLocal = lerp(lerp(UnpackNormal(tex2D(_normal1,TRANSFORM_TEX(node_44.rg, _normal1))).rgb,UnpackNormal(tex2D(_normal3,TRANSFORM_TEX(node_46.rg, _normal3))).rgb,node_11.rgb),UnpackNormal(tex2D(_normal2,TRANSFORM_TEX(node_61.rg, _normal2))).rgb,node_65.rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_28 = i.uv0;
                float2 node_29 = i.uv0;
                float2 node_72 = i.uv0;
                finalColor += diffuseLight * lerp(lerp(tex2D(_texture1,TRANSFORM_TEX(node_28.rg, _texture1)).rgb,tex2D(_texture3,TRANSFORM_TEX(node_29.rg, _texture3)).rgb,node_11.rgb),tex2D(_texture2,TRANSFORM_TEX(node_72.rg, _texture2)).rgb,node_65.rgb);
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
            uniform sampler2D _texture1; uniform float4 _texture1_ST;
            uniform sampler2D _texture3; uniform float4 _texture3_ST;
            uniform sampler2D _mask2; uniform float4 _mask2_ST;
            uniform sampler2D _normal1; uniform float4 _normal1_ST;
            uniform sampler2D _normal3; uniform float4 _normal3_ST;
            uniform sampler2D _normal2; uniform float4 _normal2_ST;
            uniform sampler2D _texture2; uniform float4 _texture2_ST;
            uniform sampler2D _mask1; uniform float4 _mask1_ST;
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
                float2 node_44 = i.uv0;
                float2 node_46 = i.uv0;
                float2 node_22 = i.uv1;
                float4 node_11 = tex2D(_mask2,TRANSFORM_TEX(node_22.rg, _mask2));
                float2 node_61 = i.uv0;
                float2 node_73 = i.uv1;
                float4 node_65 = tex2D(_mask1,TRANSFORM_TEX(node_73.rg, _mask1));
                float3 normalLocal = lerp(lerp(UnpackNormal(tex2D(_normal1,TRANSFORM_TEX(node_44.rg, _normal1))).rgb,UnpackNormal(tex2D(_normal3,TRANSFORM_TEX(node_46.rg, _normal3))).rgb,node_11.rgb),UnpackNormal(tex2D(_normal2,TRANSFORM_TEX(node_61.rg, _normal2))).rgb,node_65.rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_28 = i.uv0;
                float2 node_29 = i.uv0;
                float2 node_72 = i.uv0;
                finalColor += diffuseLight * lerp(lerp(tex2D(_texture1,TRANSFORM_TEX(node_28.rg, _texture1)).rgb,tex2D(_texture3,TRANSFORM_TEX(node_29.rg, _texture3)).rgb,node_11.rgb),tex2D(_texture2,TRANSFORM_TEX(node_72.rg, _texture2)).rgb,node_65.rgb);
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

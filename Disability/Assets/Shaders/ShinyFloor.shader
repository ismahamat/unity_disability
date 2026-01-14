Shader "Custom/AdvancedShinyFloor"
{
    Properties
    {
        [Header(Base Textures)]
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1, 1, 1, 1)
        
        [Header(Normal and Height)]
        [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0, 2)) = 1.0
        _HeightMap ("Height Map", 2D) = "grey" {}
        _Parallax ("Parallax Height", Range(0.005, 0.08)) = 0.02
        
        [Header(Surface Properties)]
        _Metallic ("Metallic", Range(0, 1)) = 0.15
        _Smoothness ("Smoothness", Range(0, 1)) = 0.65
        
        [Header(Reflection)]
        _ReflectionStrength ("Reflection Strength", Range(0, 2)) = 0.5
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 4
        
        [Header(Wet Floor Effect)]
        _WetMask ("Wet Mask", 2D) = "black" {}
        _WetSmoothness ("Wet Smoothness", Range(0, 1)) = 0.85
        _WetDarkening ("Wet Darkening", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows vertex:vert
        #pragma target 4.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _HeightMap;
        sampler2D _WetMask;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_WetMask;
            float3 viewDir;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        float4 _Color;
        float _BumpScale;
        float _Parallax;
        float _Metallic;
        float _Smoothness;
        float _ReflectionStrength;
        float _FresnelPower;
        float _WetSmoothness;
        float _WetDarkening;

        // Fonction Parallax Occlusion Mapping
        float2 ParallaxMapping(float2 uv, float3 viewDir)
        {
            // Nombre de couches pour l'effet parallax
            const float minLayers = 10;
            const float maxLayers = 32;
            float numLayers = lerp(maxLayers, minLayers, abs(dot(float3(0, 0, 1), viewDir)));
            
            float layerDepth = 1.0 / numLayers;
            float currentLayerDepth = 0.0;
            float2 deltaUV = viewDir.xy * _Parallax / numLayers;
            
            float2 currentUV = uv;
            float currentDepthMapValue = tex2D(_HeightMap, currentUV).r;
            
            // Steep Parallax Mapping
            [loop]
            for(int i = 0; i < numLayers; i++)
            {
                if(currentLayerDepth >= currentDepthMapValue)
                    break;
                    
                currentUV -= deltaUV;
                currentDepthMapValue = tex2D(_HeightMap, currentUV).r;
                currentLayerDepth += layerDepth;
            }
            
            // Parallax Occlusion Mapping (interpolation)
            float2 prevUV = currentUV + deltaUV;
            float afterDepth = currentDepthMapValue - currentLayerDepth;
            float beforeDepth = tex2D(_HeightMap, prevUV).r - currentLayerDepth + layerDepth;
            float weight = afterDepth / (afterDepth - beforeDepth);
            
            return prevUV * weight + currentUV * (1.0 - weight);
        }

        // Fonction Triplanar Mapping pour les murs/angles
        float4 TriplanarMapping(sampler2D tex, float3 worldPos, float3 normal, float scale)
        {
            float3 blendWeights = abs(normal);
            blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
            
            float4 xProj = tex2D(tex, worldPos.yz * scale);
            float4 yProj = tex2D(tex, worldPos.xz * scale);
            float4 zProj = tex2D(tex, worldPos.xy * scale);
            
            return xProj * blendWeights.x + yProj * blendWeights.y + zProj * blendWeights.z;
        }

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
        }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // === PARALLAX OCCLUSION MAPPING ===
            float3 viewDir = normalize(IN.viewDir);
            float2 parallaxUV = ParallaxMapping(IN.uv_MainTex, viewDir);
            
            // === TEXTURES DE BASE ===
            fixed4 albedo = tex2D(_MainTex, parallaxUV) * _Color;
            fixed3 normal = UnpackScaleNormal(tex2D(_BumpMap, parallaxUV), _BumpScale);
            
            // === WET FLOOR ===
            float wetMask = tex2D(_WetMask, IN.uv_WetMask).r;
            
            // Assombrir les zones mouillées
            albedo.rgb = lerp(albedo.rgb, albedo.rgb * (1 - _WetDarkening), wetMask);
            
            // Smoothness augmentée sur zones mouillées
            float smoothness = lerp(_Smoothness, _WetSmoothness, wetMask);
            
            // === FRESNEL EFFECT ===
            float3 worldNormal = WorldNormalVector(IN, normal);
            float fresnel = 1.0 - saturate(dot(normalize(IN.viewDir), worldNormal));
            fresnel = pow(fresnel, _FresnelPower);
            
            // === REFLECTION BOOST ===
            float reflectionFactor = smoothness * _ReflectionStrength * fresnel;
            
            // === SPECULAR CUSTOM ===
            float3 specular = lerp(float3(_Metallic, _Metallic, _Metallic), 
                                   float3(0.5, 0.5, 0.5), 
                                   wetMask);
            
            // === OUTPUTS ===
            o.Albedo = albedo.rgb;
            o.Normal = normal;
            o.Specular = specular * 0.5; // Réduit pour moins de reflets
            o.Smoothness = smoothness * 0.7; // Limite la brillance
            o.Emission = 0; // Pas d'émission
            o.Alpha = albedo.a;
            
            // Boost réflectivité réduit sur zones mouillées
            o.Smoothness = lerp(o.Smoothness, 0.8, wetMask * 0.3);
        }
        ENDCG
    }
    
    FallBack "Standard"
}
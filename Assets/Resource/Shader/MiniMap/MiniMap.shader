Shader "CustomShader/MiniMapWithOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutBlack("CutBlack", Range(0, 0.05)) = 0.01
        _PlayerPositions("Player Positions", Vector) = (0,0,0,0)
        _PlayerTeam("PlayerTeam", Float) = 1
        _PlayerSize("PlayerSize",Range(0.001,0.01))=0.01
        _MaxPlayer("MaxPlayer",Float)=6

        _PixelationAmount("Pixelation Amount", Float) = 100
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1) 
        _OutlineThickness("Outline Thickness", Range(0.2, 0.5)) = 0.002 
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        // ピクセル晒、��甑��、嘘尚��す
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _CutBlack;
            float _PixelationAmount;
            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ピクセル晒
                float2 pixelatedUV = floor(i.uv * _PixelationAmount) / _PixelationAmount;
                fixed4 col = tex2D(_MainTex, pixelatedUV);

                // �\い嘘尚を��す
                float isBlack = step(_CutBlack, col.r) + step(_CutBlack, col.g) + step(_CutBlack, col.b);
                col.a *= isBlack < 3 ? 0 : 1;

                // �x�碧�
                float edge = 0.0;
                float2 gradient = fwidth(i.uv);
                float3 n1 = tex2D(_MainTex, i.uv + gradient).rgb;
                float3 n2 = tex2D(_MainTex, i.uv - gradient).rgb;

                // 弼の餓�eで�xを函る
                edge = length(n1 - n2);

                // �xを�_協したら、アウトラインピクセル�_協
                if (edge > _OutlineThickness)
                {
                    return _OutlineColor; // アウトラインの弼を�O協する
                }

                return col;
            }
            ENDCG
        }

        // プレ�`ヤ�`宙鮫
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragPlayerPass

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float3 _PlayerPositions[6];
            float _PlayerTeam[6];
            float _PlayerSize;
            float _MaxPlayer;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 fragPlayerPass(v2f i) : SV_Target
            {
                fixed4 playerColor = fixed4(0, 0, 0, 0); 


                for (int idx = 0; idx < _MaxPlayer; idx++)
                {
                    float3 playerPos = _PlayerPositions[idx];

                    
                    float2 playerUV = playerPos.xz;
                    float distance = length(playerUV - i.uv);

                    
                    if (distance < _PlayerSize)
                    {
                        if (_PlayerTeam[idx] == 1) 
                            playerColor = fixed4(0, 1, 0, 1);
                        else 
                            playerColor = fixed4(1, 0, 0, 1);
                    }
                }

                return playerColor;
            }
            ENDCG
        }
    }
}

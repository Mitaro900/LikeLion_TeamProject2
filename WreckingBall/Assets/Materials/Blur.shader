Shader "Custom/Blur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
        _BlurSize ("Blur Size", Range(0.0,0.1)) = 0.05
        _BlurDirection ("Blur Direction", Vector) = (0,1,0,0) // (x,y) : (1,0)=가로, (0,1)=세로
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "CanUseSpriteAtlas"= "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // 사용자 설정 값
            fixed4 _Color;
            float _BlurSize;
            float2 _BlurDirection;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.color = IN.color * _Color;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                OUT.uv = IN.texcoord;
                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;

            // (선택) 알파 분리 텍스처 처리
            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 c = tex2D(_MainTex, uv);
                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    c.a = tex2D(_AlphaTex, uv).r;
                #endif
                return c;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 9-tap 가우시안 계수
                const fixed w0 = 0.05;
                const fixed w1 = 0.09;
                const fixed w2 = 0.12;
                const fixed w3 = 0.15;
                const fixed w4 = 0.16;

                fixed4 sum = fixed4(0, 0, 0, 0);
                float2 dir = normalize(_BlurDirection);

                // -4 ~ +4 offset, 계수는 대칭
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * -4)) * w0;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * -3)) * w1;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * -2)) * w2;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * -1)) * w3;
                sum += SampleSpriteTexture(IN.uv) * w4;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * 1)) * w3;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * 2)) * w2;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * 3)) * w1;
                sum += SampleSpriteTexture(IN.uv + dir * (_BlurSize * 4)) * w0;

                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    sum.a = tex2D(_AlphaTex, IN.uv).r;
                #endif

                // 원래 정점 컬러·틴트 컬러 곱해주기
                sum *= IN.color;
                return sum;
            }
            ENDCG
        }
    }
}
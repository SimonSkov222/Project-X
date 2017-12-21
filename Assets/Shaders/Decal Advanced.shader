// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Decal Advanced"{
	Properties {
		_Color1 ("Color Main", Color) = (1,0,0,1)
		_Color2 ("Color Decal 01", Color) = (1,0,0,1)
		_Color3 ("Color Decal 02", Color) = (1,0,0,1)
		_MainTex ("Main", 2D) = "white" { }
		_DecalTex01 ("Decal 01", 2D) = "white" { }
		_DecalTex02 ("Decal 02", 2D) = "white" { }
	}
	SubShader {
 
		Pass {
			CGPROGRAM //Shader Start, Vertex Shader named vert, Fragment shader named frag
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				//Link properties to the shader
				float4 _Color1;
				float4 _Color2;
				float4 _Color3;
				sampler2D _MainTex;
				sampler2D _DecalTex01;
				sampler2D _DecalTex02;
				sampler2D _DecalTex03;
				sampler2D _DecalTex04;
				sampler2D _DecalTex05;
				sampler2D _DecalTex06;
				sampler2D _DecalTex07;
				sampler2D _DecalTex08;
      
				struct v2f 
				{
					float4  pos : SV_POSITION;
					float2  uv : TEXCOORD0;
					float2  uv1 : TEXCOORD1;
					float2  uv2 : TEXCOORD2;
				};
      
				float4 _MainTex_ST; //?? skal være der
				float4 _DecalTex01_ST; //?? skal være der
				float4 _DecalTex02_ST; //?? skal være der

      
				v2f vert (appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex); //Transform the vertex position
					o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); //Prepare the vertex uv
					o.uv1 = TRANSFORM_TEX (v.texcoord, _DecalTex01); //Prepare the vertex uv
					o.uv2 = TRANSFORM_TEX (v.texcoord, _DecalTex02); //Prepare the vertex uv
					return o;
				}
      
				half4 frag (v2f i) : COLOR
				{
					float4 texcol = tex2D (_MainTex, i.uv); //base texture
					float4 deccol = tex2D (_DecalTex01, i.uv1); //decal texture
					float4 deccol2 = tex2D (_DecalTex02, i.uv2); //decal texture
					float4 temp = _Color2* _Color2.a * deccol;
					float4 temp2 = _Color1 * _Color1.a * texcol;
					float4 temp3 = _Color1 * _Color1.a * deccol2;

					if(true)
					{
						temp2 = lerp(temp2, temp3, temp3.a);
					}
					
					return lerp(temp2, temp, temp.a);
				}
      
			ENDCG //Shader End
		}
	}
}
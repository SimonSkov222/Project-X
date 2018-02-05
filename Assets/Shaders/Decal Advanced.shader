
////////////////////////////////////////////////////////////////////////////////
//                      Beskrivelse
//
//		Denne shader gør at vi kan kombinere flere billeder i et materiale
//		Hvis kan også flytte hvor de forskellige billeder skal starte fra
//
//		#NOTE
//		Vi er ikke eksperter i shader og hvordan de bliver kodet. 
//		Vi fandt nogle andre shader på nettet og prøvet os frem til vi 
//		fik dette resultat.
//
////////////////////////////////////////////////////////////////////////////////
Shader "Custom/Decal Advanced"{
	Properties {

		//Farve overlay
		_ColorMain ("Color Main", Color) = (1,1,1,0.5)
		_Color01 ("Color Decal 01", Color) = (1,1,1,0)
		_Color02("Color Decal 02", Color) = (1,1,1,0)
		_Color03("Color Decal 03", Color) = (1,1,1,0)
		_Color04("Color Decal 04", Color) = (1,1,1,0)
		_Color05("Color Decal 05", Color) = (1,1,1,0)
		_Color06("Color Decal 06", Color) = (1,1,1,0)
		_Color07("Color Decal 07", Color) = (1,1,1,0)
		_Color08("Color Decal 08", Color) = (1,1,1,0)
		_Color09("Color Decal 09", Color) = (1,1,1,0)
		_Color10("Color Decal 10", Color) = (1,1,1,0)

		//Billedet
		_MainTex ("Main", 2D) = "white" { }
		_DecalTex01 ("Decal 01", 2D) = "white" { }
		_DecalTex02("Decal 02", 2D) = "white" { }
		_DecalTex03("Decal 03", 2D) = "white" { }
		_DecalTex04("Decal 04", 2D) = "white" { }
		_DecalTex05("Decal 05", 2D) = "white" { }
		_DecalTex06("Decal 06", 2D) = "white" { }
		_DecalTex07("Decal 07", 2D) = "white" { }
		_DecalTex08("Decal 08", 2D) = "white" { }
		_DecalTex09("Decal 09", 2D) = "white" { }
		_DecalTex10 ("Decal 10", 2D) = "white" { }
	}
	SubShader {
 
		Pass {
			CGPROGRAM //Shader Start, Vertex Shader named vert, Fragment shader named frag
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				//Link properties to the shader
				float4 _ColorMain;
				float4 _Color01;
				float4 _Color02;
				float4 _Color03;
				float4 _Color04;
				float4 _Color05;
				float4 _Color06;
				float4 _Color07;
				float4 _Color08;
				float4 _Color09;
				float4 _Color10;
				sampler2D _MainTex;
				sampler2D _DecalTex01;
				sampler2D _DecalTex02;
				sampler2D _DecalTex03;
				sampler2D _DecalTex04;
				sampler2D _DecalTex05;
				sampler2D _DecalTex06;
				sampler2D _DecalTex07;
				sampler2D _DecalTex08;
				sampler2D _DecalTex09;
				sampler2D _DecalTex10;
      
				///
				/// Noget med hvor billederne skal side
				///
				struct v2f 
				{
					float4  pos : SV_POSITION;
					float2  uv : TEXCOORD0;
					float2  uv1 : TEXCOORD1;
					float2  uv2 : TEXCOORD2;
					float2  uv3 : TEXCOORD3;
					float2  uv4 : TEXCOORD4;
					float2  uv5 : TEXCOORD5;
					float2  uv6 : TEXCOORD6;
					float2  uv7 : TEXCOORD7;
					float2  uv8 : TEXCOORD8;
					float2  uv9 : TEXCOORD9;
					float2  uv10 : TEXCOORD10;
				};

				///
				/// Noget med hvor billederne skal side
				/// Hvis disse ikke er der bliver billederne ikke flyttet
				///
				float4 _MainTex_ST; //?? skal være der
				float4 _DecalTex01_ST; //?? skal være der
				float4 _DecalTex02_ST; //?? skal være der
				float4 _DecalTex03_ST; //?? skal være der
				float4 _DecalTex04_ST; //?? skal være der
				float4 _DecalTex05_ST; //?? skal være der
				float4 _DecalTex06_ST; //?? skal være der
				float4 _DecalTex07_ST; //?? skal være der
				float4 _DecalTex08_ST; //?? skal være der
				float4 _DecalTex09_ST; //?? skal være der
				float4 _DecalTex10_ST; //?? skal være der

				///
				/// Giver billederne deres plads/Definere det
				///
				v2f vert (appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex); //Transform the vertex position
					o.uv = TRANSFORM_TEX (v.texcoord, _MainTex); //Prepare the vertex uv
					o.uv1 = TRANSFORM_TEX (v.texcoord, _DecalTex01); //Prepare the vertex uv
					o.uv2 = TRANSFORM_TEX(v.texcoord, _DecalTex02); //Prepare the vertex uv
					o.uv3 = TRANSFORM_TEX(v.texcoord, _DecalTex03); //Prepare the vertex uv
					o.uv4 = TRANSFORM_TEX(v.texcoord, _DecalTex04); //Prepare the vertex uv
					o.uv5 = TRANSFORM_TEX(v.texcoord, _DecalTex05); //Prepare the vertex uv
					o.uv6 = TRANSFORM_TEX(v.texcoord, _DecalTex06); //Prepare the vertex uv
					o.uv7 = TRANSFORM_TEX(v.texcoord, _DecalTex07); //Prepare the vertex uv
					o.uv8 = TRANSFORM_TEX(v.texcoord, _DecalTex08); //Prepare the vertex uv
					o.uv9 = TRANSFORM_TEX(v.texcoord, _DecalTex09); //Prepare the vertex uv
					o.uv10 = TRANSFORM_TEX(v.texcoord, _DecalTex10); //Prepare the vertex uv
					return o;
				}
				

				///
				/// Her kombinere vi billederne og farverne
				///
				half4 frag (v2f i) : COLOR
				{
					//Hent billederne
					float4 texcol = tex2D (_MainTex, i.uv); //base texture
					float4 deccol01 = tex2D (_DecalTex01, i.uv1); //decal texture
					float4 deccol02 = tex2D(_DecalTex02, i.uv2); //decal texture
					float4 deccol03 = tex2D(_DecalTex03, i.uv3); //decal texture
					float4 deccol04 = tex2D(_DecalTex04, i.uv4); //decal texture
					float4 deccol05 = tex2D(_DecalTex05, i.uv5); //decal texture
					float4 deccol06 = tex2D(_DecalTex06, i.uv6); //decal texture
					float4 deccol07 = tex2D(_DecalTex07, i.uv7); //decal texture
					float4 deccol08 = tex2D(_DecalTex08, i.uv8); //decal texture
					float4 deccol09 = tex2D(_DecalTex09, i.uv9); //decal texture
					float4 deccol10 = tex2D (_DecalTex10, i.uv10); //decal texture

					//Giver billederne farve
					float4 temp00 = _ColorMain * _ColorMain.a * texcol;
					float4 temp01 = _Color01 * _Color01.a * deccol01;
					float4 temp02 = _Color02 * _Color02.a * deccol02;
					float4 temp03 = _Color03 * _Color03.a * deccol03;
					float4 temp04 = _Color04 * _Color04.a * deccol04;
					float4 temp05 = _Color05 * _Color05.a * deccol05;
					float4 temp06 = _Color06 * _Color06.a * deccol06;
					float4 temp07 = _Color07 * _Color07.a * deccol07;
					float4 temp08 = _Color08 * _Color08.a * deccol08;
					float4 temp09 = _Color09 * _Color09.a * deccol09;
					float4 temp10 = _Color10 * _Color10.a * deccol10;

					//Kombinere billederne
					temp01 = lerp(temp01, temp02, temp02.a);
					temp01 = lerp(temp01, temp03, temp03.a);
					temp01 = lerp(temp01, temp04, temp04.a);
					temp01 = lerp(temp01, temp05, temp05.a);
					temp01 = lerp(temp01, temp06, temp06.a);
					temp01 = lerp(temp01, temp07, temp07.a);
					temp01 = lerp(temp01, temp08, temp08.a);
					temp01 = lerp(temp01, temp09, temp09.a);
					temp01 = lerp(temp01, temp10, temp10.a);

					return lerp(temp00, temp01, temp01.a);
				}
      
			ENDCG //Shader End
		}
	}
}
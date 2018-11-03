Shader "Custom/Untextured Vertex Color Surface Shader" {

	Properties{
	}

	SubShader{

		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

			#pragma surface surf Lambert

			struct Input {
				half4 color : COLOR;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				o.Albedo = IN.color.rgb;
				o.Alpha = IN.color.a;
			}

		ENDCG

	}

		FallBack "Standard"

}

/*******************************************************************************
CutOutWorldUVAmplify Shader
 
Author:
      Paulus Ery Wasito Adhi <paupawsan@gmail.com>

Copyright (c) 2018

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*******************************************************************************/

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sakaki Entertainment/CutOutWorldUVAmplify"
{
	Properties
	{
		_MaskCenterPosition("Mask Center Position", Vector) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureTilingSize("Texture Tiling Size", Float) = 0.01
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_MaskRadius("Mask Radius", Float) = 1
		_Exposure("Exposure", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float _TextureTilingSize;
		uniform float _Exposure;
		uniform float _Opacity;
		uniform float3 _MaskCenterPosition;
		uniform float _MaskRadius;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 tex2DNode2 = tex2D( _TextureSample0, ( (ase_worldPos).xz * _TextureTilingSize ).xy );
			o.Albedo = ( tex2DNode2 * _Exposure ).rgb;
			o.Alpha = ( tex2DNode2.a * _Opacity );
			clip( (( distance( (ase_worldPos).xz , (_MaskCenterPosition).xz ) < _MaskRadius ) ? 1.0 :  0.0 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15600
49;205;1344;625;2443.692;694.1187;3.038979;True;True
Node;AmplifyShaderEditor.CommentaryNode;21;-1333.057,114.9328;Float;False;942.2561;857.3163;Clip by distance and radius;8;15;16;18;8;17;11;13;10;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1492.948,-520.7936;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;20;-947.1278,-663.5952;Float;False;506.7529;394.4092;WorldUV Mapper;3;6;7;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;8;-1283.057,405.5517;Float;False;Property;_MaskCenterPosition;Mask Center Position;0;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;6;-897.1278,-613.5952;Float;True;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-882.5637,-383.686;Float;False;Property;_TextureTilingSize;Texture Tiling Size;3;0;Create;True;0;0;False;0;0.01;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;10;-1257.701,164.9328;Float;True;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;11;-1040.377,438.4301;Float;True;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-606.3749,-514.9186;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-632.6646,857.7491;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-801.794,817.0723;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-913.1195,759.2686;Float;False;Property;_MaskRadius;Mask Radius;5;0;Create;True;0;0;False;0;1;4.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-82.83089,-9.261539;Float;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-233.9167,-253.8116;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;13;-884.9094,339.0177;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;67.19339,-326.7937;Float;False;Property;_Exposure;Exposure;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;276.9282,21.89869;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;18;-652.7996,332.4893;Float;True;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;254.845,-214.6603;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;515.2968,166.7932;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Sakaki Entertainment/CutOffWorldUVAmplify;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;0
WireConnection;10;0;5;0
WireConnection;11;0;8;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;2;1;9;0
WireConnection;13;0;10;0
WireConnection;13;1;11;0
WireConnection;3;0;2;4
WireConnection;3;1;4;0
WireConnection;18;0;13;0
WireConnection;18;1;15;0
WireConnection;18;2;17;0
WireConnection;18;3;16;0
WireConnection;22;0;2;0
WireConnection;22;1;23;0
WireConnection;0;0;22;0
WireConnection;0;9;3;0
WireConnection;0;10;18;0
ASEEND*/
//CHKSM=F75D5AD0342064251CACBE88929F508E59EEB09A
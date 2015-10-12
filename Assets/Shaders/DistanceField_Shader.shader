Shader "Custom/DistanceField_Shader" {
	Properties {
	//CameraTarget
    _CameraTarget 	("Camera Target", Vector) = (0,0,0,0)
    _CameraUp		("Camera Up", Vector) = (0,0,0,0)
    //Objects
    _ObjectPosition ("Object Position", Vector) = (0,0,0,0)
    _DisplaceStrength ("Displacement Strength", float) = 0.0
    //Sphere
    _SphereRadius ("Sphere Radius", Float) = 0.2
    //Torus
    _TorusInnerRadius ("Torus Inner Radius", Float) = 0.1
    _TorusOuterRadius ("Torus Outer Radius", Float) = 0.2
    
    //Color
    _BackgroundColor ("Background Color", Color) = (1.0,1.0,1.0,1.0)

	}	
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass
			{
                GLSLPROGRAM   
                #include "UnityCG.glslinc"             
                                       
                #ifdef VERTEX  
                void main()
                {          
                    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                }
                #endif  
                #ifdef FRAGMENT
                
                //Camera
                uniform vec3 _CameraTarget;
                uniform vec3 _CameraPosition;
                uniform vec3 _CameraUp;
               	
               	//Object
               	uniform float _DisplaceStrength;
               	uniform vec3 _ObjectPosition;
               	uniform mat4 _ObjectRotation;
               	//Sphere
               	uniform float _SphereRadius;
               	//Torus
               	uniform float _TorusInnerRadius;
               	uniform float _TorusOuterRadius;
               	
               	
               	//Colors
               	uniform vec4 _BackgroundColor;
               	
               	
				////////////////////////////////////////
				//
				// Distance Functions
	
				//Sphere
				//Smallest distance from pos to ball (ballPos, ballRadius)
				float dBall(vec3 pos, vec3 ballPos, float ballRadius)
				{   
   					float d = length(pos - ballPos) - ballRadius;
    				return d;
				}
				
				//Torus - Signed
				//Smallest distance form pos to torus with specified radii
				float sdTorus( vec3 pos, vec3 torusPos, float innerRadius,float outerRadius)
				{
					vec3 adjustedPosition=pos-torusPos;
 					vec2 q = vec2(length(adjustedPosition.xz)-outerRadius,adjustedPosition.y);
  					return length(q)-innerRadius;
				}
				
				////////////////////////////////////////
				//
				// Helper Functions
				
				//Displace by sin values, used for cool distortion efect
				float displacement(vec3 pos)
				{
					vec3 displaceValue;
					float displaceStrength=_DisplaceStrength;
					
					displaceValue.x=sin(pos.x*displaceStrength);
					displaceValue.y=sin(pos.y*displaceStrength);
					displaceValue.z=sin(pos.z*displaceStrength);
					
					return displaceValue.x*displaceValue.y*displaceValue.z;
				}
				
				//Repeat the object using modulus
				vec3 opRep( vec3 p, vec3 c )
				{
    				vec3 q = mod(p,c)-0.5*c;
    				return q;
				}
				
//				vec3 opTx( vec3 p, mat4 m )
//				{
//				    vec3 q = inverse(m)*p;
//				    return q;
//				}
				
				// Distance to closest scene hit
				float map(vec3 pos)
				{
					mat4 rotation;
					rotation[0].xyzw=vec4(3.0, 1.0, 2.0,3.0);
					rotation[1].xyzw=vec4(3.0, 1.0, 2.0,3.0);
					rotation[2].xyzw=vec4(3.0, 1.0, 2.0,3.0);
					rotation[3].xyzw=vec4(3.0, 1.0, 2.0,3.0);
					
//					vec3 rotatedPositon = opTx(pos,rotation);
					vec3 repeatedPosition=opRep(pos,vec3(1.0,1.0,1.0));
					
					//Sphere
					vec3 spherePosition=_ObjectPosition;
					float sphereDistance = dBall(repeatedPosition, _ObjectPosition, _SphereRadius);
					
					//Torus
					float torusDistance = sdTorus(repeatedPosition,_ObjectPosition, _TorusInnerRadius,_TorusOuterRadius);
					
				    return min(sphereDistance,torusDistance)+displacement(pos);
				}
				

				// Uses map function (smallest distance to scene) for
				// approximating normal at pos
				vec3 approxNormal(vec3 pos)
				{
				    float epsilon = 0.0001;
					vec2 t = vec2(0, epsilon);
				    vec3 n = vec3(
				    	map(pos + t.yxx) - map(pos - t.yxx),
	           	  		map(pos + t.xyx) - map(pos - t.xyx),
	              		map(pos + t.xxy) - map(pos - t.xxy));
    				return normalize(n);
				}

				vec3 getColor(vec3 rayPos, vec3 rayDir)
				{
    				vec3 color = _BackgroundColor.xyz;
    				
    				for (int i = 0; i < 128; ++i)
    				{
    					float d = map(rayPos);
        				rayPos += d * rayDir;
        				if (d < 0.001)
        				{
        					color =  abs(approxNormal(rayPos));
        				    break;
        				}
    				}
    				
    				return color;
				}

				void main(void)
				{
					vec2 uv = gl_FragCoord.xy / _ScreenParams.xy;
    				float aspect = _ScreenParams.x / _ScreenParams.y;
    
    				// Make uv go [-0.5, 0.5] and scale uv.x according to aspect ratio
    				uv -= .5;
    				uv.x = aspect * uv.x;
    
    				// Initialize camera stuff
    				vec3 camPos = _CameraPosition;
    				vec3 camTarget = _CameraTarget;
    				vec3 camUp = _CameraUp;//vec3(0.3, 0.9, -.1);
    				vec3 camDir = normalize(camTarget - camPos);
    				vec3 camRight = normalize(cross(camUp, camDir));
    				camUp = normalize(cross(camDir, camRight));
    
    				vec3 rayPos = camPos;
    				vec3 rayDir = normalize(camDir + uv.x * camRight + uv.y * camUp);
    
    				// Raymarch scene to get pixel color
    				vec3 color = getColor(rayPos, rayDir);
    
    				// Set pixel color
					gl_FragColor = vec4(color, 1.0);
				}
                #endif                          
                ENDGLSL
    		}
		} 
	FallBack "Diffuse"
}

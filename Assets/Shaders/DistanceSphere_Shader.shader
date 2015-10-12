
// Original Source: http://www.iquilezles.org/apps/shadertoy/
// Thanks to Inigo Quilez
// Modified by Ippokratis Bournellis http://ippomed.com
 
Shader "Custom/DistanceSphere"
{    
    Properties
    {
    _LightX ("Rim Intensity", Float) = 1.0
    _LightY ("Rim Intensity", Float) = 1.0
   
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" }
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
                 
                uniform float _LightX;
               	uniform float _LightY;
 
 				//Calculate object distance
                float inObj(in vec3 p)
                {
                	//Sphere
//                   return  length(p)-3.0;
                   //Cube
//                    return length(max(abs(p)-1.0,0.0));
                    //Torus
                    vec2 q = vec2(length(p.xz)-2.0,p.y);
 					 return length(q)-0.2;
                }
               
                void main(void)
                {
                  //Center the uv
                  vec2 vPos=-1.0+2.0*gl_FragCoord.xy/_ScreenParams.xy;
                  //Camera properties
                  vec3 vuv=vec3(0.0,1.0,0.0);//camera up
                  vec3 vrp = vec3(0.0,0.0,0.0);//camera rotation
                  vec3 prp= vec3(0.0,4.0,3.0);//camera position
                  //Camera setup, voodoo atm
                  vec3 vpn=normalize(vrp-prp);
                  vec3 u=normalize(cross(vuv,vpn));
                  vec3 v=cross(vpn,u);
                  vec3 vcv=(prp+vpn);
                  vec3 scrCoord=vcv+vPos.x*u*_ScreenParams.x/_ScreenParams.y+vPos.y*v;
                  vec3 scp=normalize(scrCoord-prp);
               
                  //Raymarching, voodoo atm
                  vec2 e = vec2(_LightX,_LightY);
                  const float maxd=5.0;
                  float s=0.1;
                  vec3 c,p,n;
               
                  //speed optimization -advance ray (simple raytracing) until plane y=3.0
                  //voodoo atm
                  float f=-(prp.y-3.0)/scp.y;
                 
                  if (f>0.0) p=prp+scp*f;
                  else f=maxd;
               
                  for(int i=0;i<128;i++)
                  {
                    if (abs(s)<.01||f>maxd) break;
                    f+=s;
                    p=prp+scp*f;
                   
                    s=inObj(p);
                  }
                 
                  if (f<maxd)
                  {
                    if(p.y<-3.0)//do not draw below the plane level
                    {
                        return;
                    }
                    else
                    {
                      //Color
                      c = vec3(1.0,0.0,0.0); //sphere color      
                      n=normalize(
                        vec3(s-inObj(p-e.xyy),//This produces
                             s-inObj(p-e.yxy),//a lit sphere
                             s-inObj(p-e.yyx))//
                                             );//
                    }
                    float b=dot(n,normalize(prp-p));
                   
                    gl_FragColor=vec4((b*c+pow(b,32.0))*(1.0-f*.0004),1.0);//Voodoo lighting stuff
                  }
                  else gl_FragColor=vec4(0,0.2,0,1);//Color of the horizon
                }
                #endif                          
                ENDGLSL        
            }
     }
    FallBack "Diffuse"
}
 
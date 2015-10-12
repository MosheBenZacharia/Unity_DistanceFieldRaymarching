/**
This work is licensed under a Creative Commons Attribution 3.0 Unported License.
http://creativecommons.org/licenses/by/3.0/deed.en_GB

You are free:

to copy, distribute, display, and perform the work
to make derivative works
to make commercial use of the work
*/

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/FractalEffect")]
public class FractalEffect : ImageEffectBase {

	[Range(-1.0f,1.0f)]
	public float lightX,lightY;

	[Header("Camera Target")]
	public Vector3 cameraTarget;

	[Header("Objects")]
	public Vector3 objectPosition;
	[Range(-15.0f,15.0f)]
	public float displacementStrength;

	[Header("Sphere")]
	[Range(0.0f,1.0f)]
	public float sphereRadius=.2f;
	
	[Header("Torus")]
	[Range(0.0f,1.0f)]
	public float torusInnerRadius=.1f;
	[Range(0.0f,1.0f)]
	public float torusOuterRadius=.2f;
	
	[Header("Colors")]
	public Color backgroundColor=new Color(0.1607843137f,0.1607843137f,0.1607843137f);

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("time", Time.time);
		material.SetFloat("_LightX",lightX);
		material.SetFloat("_LightY",lightY);

		//DistanceField

		//Camera
		material.SetVector("_CameraTarget",cameraTarget);
		//Objects
		material.SetVector("_ObjectPosition",objectPosition);
		material.SetFloat("_DisplaceStrength",displacementStrength);
		//Sphere
		material.SetFloat("_SphereRadius",sphereRadius);
		//Torus
		material.SetFloat("_TorusInnerRadius",torusInnerRadius);
		material.SetFloat("_TorusOuterRadius",torusOuterRadius);

		//Colors
		material.SetColor("_BackgroundColor",backgroundColor);

		Graphics.Blit (source, destination, material);
	}
}
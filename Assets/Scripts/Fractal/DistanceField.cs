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
public class DistanceField : ImageEffectBase {

//	[Header("Camera Target")]
//	public Vector3 cameraTarget;

	[Header("Objects")]
	public Vector3 objectPosition;
	public Vector3 objectRotation;
	[Range(0.0f,1.0f)]
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

		//DistanceField
		Transform mainCameraTransform=GetComponent<Transform>();

		Quaternion objectRotationQuaternion = Quaternion.Euler(objectRotation);
		Matrix4x4 objectRotationMatrix = Matrix4x4.TRS(Vector3.zero, objectRotationQuaternion, Vector3.one);
		
		//Camera
		material.SetVector("_CameraTarget",mainCameraTransform.forward+mainCameraTransform.position);
		material.SetVector("_CameraUp",mainCameraTransform.up);
		material.SetVector("_CameraPosition",mainCameraTransform.position);
		//Objects
		material.SetVector("_ObjectPosition",objectPosition);
		material.SetMatrix("_ObjectRotation",objectRotationMatrix);
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
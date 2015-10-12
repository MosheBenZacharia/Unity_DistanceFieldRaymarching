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
[AddComponentMenu("Image Effects/Mandelbulb")]
public class Mandelbulb : ImageEffectBase {

    [Range(3f,5f)]
    public float power;
    
    // Called by camera to apply image effect
    void OnRenderImage (RenderTexture source, RenderTexture destination) {

        material.SetFloat("time", Time.time);
        material.SetFloat("_Power",power);

        material.SetVector("screen", new Vector2(Screen.width, Screen.height));
        
        Graphics.Blit (source, destination, material);
    }
}

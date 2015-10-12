#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Tracker))]
class TrackerEditor : Editor {
	
	Transform[] children ;
	void OnEnable(){
		Transform myTransform = ((Tracker) target).transform;
		children = new Transform[myTransform.childCount];
		for (int i=0; i<children.Length; ++i) {
			children [i] = myTransform.GetChild (i);
		}
	}
	static readonly float disableColorValue=.85f;
	Color disabledColor = new Color(disableColorValue,disableColorValue,disableColorValue);
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(10);
		GUILayout.BeginHorizontal ();

		for(int i=0;i<children.Length;++i){
			GUIStyle currentStyle = EditorStyles.miniButtonMid;
			if(i==0)
				currentStyle=EditorStyles.miniButtonLeft;
			if(i==children.Length-1)
				currentStyle=EditorStyles.miniButtonRight;
			if(!children[i].gameObject.activeSelf){
				GUI.backgroundColor=disabledColor;
					GUI.color=disabledColor;
			}
			if(GUILayout.Button(children[i].gameObject.name,currentStyle)){
				SetCam(i);
			}
			GUI.backgroundColor=Color.white;
			GUI.color=Color.white;
		}
//		if (GUILayout.Button ("Main", EditorStyles.miniButtonLeft)) {
//			SetCam(0);
//		}
//		if(GUILayout.Button("OVR",EditorStyles.miniButtonMid)){
//			SetCam(1);
//		}
//		if (GUILayout.Button ("Dive", EditorStyles.miniButtonRight)) {
//			SetCam(2);
//		}
		GUILayout.EndHorizontal ();
		GUILayout.Space(10);
	}


	// Use this for initialization
	void SetCam (int index)
	{
		for (int i=0; i<children.Length; ++i) {
			children[i].gameObject.SetActive(index==i);
		}
	}
}
#endif

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public static class Utility_GroupObjects  {

	[MenuItem("GameObject/Group Objects", false, 0)]
	static void Init () {
		// create temporary camera for rendering
		Transform activeTransform = Selection.activeTransform;
		if(activeTransform!=null){
			GameObject newObject = new GameObject();
			newObject.transform.parent=activeTransform.parent;
			newObject.transform.SetSiblingIndex(activeTransform.GetSiblingIndex());
			newObject.name=activeTransform.gameObject.name+"_Group";
			if(activeTransform.GetComponent<RectTransform>()){
				newObject.AddComponent<RectTransform>();
			}
			newObject.transform.localScale=activeTransform.localScale;
			newObject.transform.rotation=activeTransform.rotation;
			newObject.transform.position=activeTransform.position;
			activeTransform.parent=newObject.transform;
		
			Selection.activeTransform=newObject.transform;
		}
	}
	
	
}
#endif
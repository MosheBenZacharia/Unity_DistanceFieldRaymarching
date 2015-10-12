using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static Vector2 GetRandomVector2(float range){
		return new Vector2 (Random.Range (-range, range), Random.Range (-range, range));
	}
	public static Vector3 GetRandomVector3(float range){
		return new Vector3 (Random.Range (-range, range), Random.Range (-range, range), Random.Range (-range, range));
	}
	public static Vector3 Interp(Vector3[] pts, float t){
		int numSections = pts.Length - 3;
		int currPt = Mathf.Min(Mathf.FloorToInt(t * (float) numSections), numSections - 1);
		float u = t * (float) numSections - (float) currPt;
		
		Vector3 a = pts[currPt];
		Vector3 b = pts[currPt + 1];
		Vector3 c = pts[currPt + 2];
		Vector3 d = pts[currPt + 3];
		
		return .5f * (
			(-a + 3f * b - 3f * c + d) * (u * u * u)
			+ (2f * a - 5f * b + 4f * c - d) * (u * u)
			+ (-a + c) * u
			+ 2f * b
			);
	}	
	public static Vector3[] PathControlPointGenerator(Vector3[] path){
		Vector3[] suppliedPath;
		Vector3[] vector3s;
		
		//create and store path points:
		suppliedPath = path;
		
		//populate calculate path;
		int offset = 2;
		vector3s = new Vector3[suppliedPath.Length+offset];
		System.Array.Copy(suppliedPath,0,vector3s,1,suppliedPath.Length);
		
		//populate start and end control points:
		//vector3s[0] = vector3s[1] - vector3s[2];
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length-1] = vector3s[vector3s.Length-2] + (vector3s[vector3s.Length-2] - vector3s[vector3s.Length-3]);
		
		//is this a closed, continuous loop? yes? well then so let's make a continuous Catmull-Rom spline!
		if(vector3s[1] == vector3s[vector3s.Length-2]){
			Vector3[] tmpLoopSpline = new Vector3[vector3s.Length];
			System.Array.Copy(vector3s,tmpLoopSpline,vector3s.Length);
			tmpLoopSpline[0]=tmpLoopSpline[tmpLoopSpline.Length-3];
			tmpLoopSpline[tmpLoopSpline.Length-1]=tmpLoopSpline[2];
			vector3s=new Vector3[tmpLoopSpline.Length];
			System.Array.Copy(tmpLoopSpline,vector3s,tmpLoopSpline.Length);
		}	
		
		return(vector3s);
	}
	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
	public static AnimationCurve GetCurve_Linear(float duration){
		return AnimationCurve.Linear(Time.time,0,Time.time+duration,1f);
	}
	public static AnimationCurve GetCurve_Ease(float duration){
		return AnimationCurve.EaseInOut(Time.time,0,Time.time+duration,1f);
	}
	
	
	public static string Truncate(string source, int length){
		if(source == null) return null;
		
		if (source.Length > length){
			int index = source.Substring(0, length).LastIndexOf(' ');
			if(index==-1){
				index=length-1;
			}
			source = source.Insert(index, "\n");
			
			if (source.Substring(index).Length > length) {
				source = source.Substring(0, index + length) + "...";
			}
		}
		return source;
	}
	
	//Utility function: print Dictionary of List of Dictionary
	public static void printDictList (Dictionary<string,  List<Dictionary<string, string>>> d)
	{
		foreach (KeyValuePair<string, List<Dictionary<string, string>>> kvp in d) {
			Debug.Log (kvp.Key + "------------------------------");
			printList (kvp.Value);
		}
	}
	
	//Utility function: print List of Dictionary
	public static void printList (List<Dictionary<string, string>> list)
	{
		foreach (Dictionary<string, string> i in list) {
			foreach (string k in i.Keys) {
				Debug.Log (k + " " + i [k]);
			}
		}
	}

}

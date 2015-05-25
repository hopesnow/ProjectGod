using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AC_Create : MonoBehaviour {

	const string duplicatePostfix = "_copy";
	
	[MenuItem("Assets/Transfer Clip Curves to Copy")]
	static void CopyCurvesToDuplicate()
	{
		List<AnimationClip> clipList = new List<AnimationClip>();
		clipList.Clear();
		foreach( Object obj in Selection.objects){
			AnimationClip clip = obj as AnimationClip;
			if ( clip != null ) {
				clipList.Add(clip);
			}
		}
		
		if ( clipList.Count == 0 ) {
			return;
		}
		
		foreach( AnimationClip clip in clipList ) {
			AnimationClip copyClip = createFile(clip);
			clip.name = clip.name.Replace("rig","");
			clip.name = clip.name.Replace("|","");
			//Debug.Log("clip.name"+clip.name);
			duplicate( clip, copyClip );
			Debug.Log("Copying curves into " + copyClip.name + " is done");
		}
	}
	static AnimationClip createFile(AnimationClip importedClip)
	{
		string importedPath = AssetDatabase.GetAssetPath( importedClip );
		//Debug.Log("相対パスは" + importedClip);
		string copyPath = importedPath.Substring(0, importedPath.LastIndexOf("/"));
		copyPath += "/" + importedClip.name + duplicatePostfix + ".anim";
		copyPath = copyPath.Replace("|","");
		//Debug.Log ("copyPathは" + copyPath);
		AnimationClip src = AssetDatabase.LoadAssetAtPath(importedPath, typeof(AnimationClip)) as AnimationClip;
		AnimationClip newClip = new AnimationClip();
		newClip.name = src.name + duplicatePostfix;
		//Debug.Log ("newClip.nameは" + newClip.name);
		AssetDatabase.CreateAsset(newClip, copyPath);
		AssetDatabase.Refresh();
		return newClip;
	}
	static AnimationClip duplicate(AnimationClip src, AnimationClip dest)
	{
		AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(src, true);
		for (int i = 0; i < curveDatas.Length; i++)
		{
			AnimationUtility.SetEditorCurve(
				dest,
				curveDatas[i].path,
				curveDatas[i].type,
				curveDatas[i].propertyName,
				curveDatas[i].curve
				);
		}
		return dest;
	}
}

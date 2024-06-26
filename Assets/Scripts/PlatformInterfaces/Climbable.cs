using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Climbable : Walkable
{
    //stores info about "grabbable ledges"
    public List<float> climbLevels = new List<float>();
    public bool onlyUp = false;
    public BoxCollider bc;

    void OnDrawGizmosSelected()
    {
        if (bc == null) { bc = GetComponent<BoxCollider>(); }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Climbable))]
public class DrawWireCube : Editor
{
    // draws info in editor and allows for interaction (rn weirds out when rotating, otherwise works)
    void OnSceneGUI()
    {
        Handles.color = Color.yellow;
        Climbable myObj = (Climbable)target;
        for (int i = 0; i < myObj.climbLevels.Count; i++)
        {
            var entry = myObj.climbLevels[i];
            Handles.DrawWireCube(myObj.transform.position + myObj.transform.up * entry, new Vector3(myObj.bc.transform.localScale.x, 0, myObj.transform.localScale.z));
            EditorGUI.BeginChangeCheck();
            float new_amount = (float)Handles.ScaleValueHandle(entry, myObj.transform.position + myObj.transform.up * entry, Quaternion.LookRotation(myObj.transform.up, Vector3.up), 1, Handles.ConeHandleCap, 1);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myObj, "Change Climbable Height");
                myObj.climbLevels[i] = new_amount;
            }
        }

    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class FloorBuilder : MonoBehaviour
{
    [SerializeField]
    int xAmount;
    [SerializeField]
    int yAmount;
    [SerializeField]
    GameObject fieldPrefab;


    public void DoCopy()
    {
        Debug.Log("Hello");
        int xHalf = xAmount / 2;
        int yHalf = yAmount / 2;
        for (var x = -xHalf; x <= xHalf; x++)
        {
            for (var y = -yHalf; y <= yHalf; y++)
            {
                var currGo = GameObject.Instantiate(fieldPrefab);
                currGo.transform.position = transform.position + new Vector3(x, 0, y);
            }
        }
    }
}

[CustomEditor(typeof(FloorBuilder))]
[CanEditMultipleObjects]
public class FloorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var t = (target as FloorBuilder);

        if (GUILayout.Button("Copy"))
        {
            t.DoCopy();
        }
    }
}
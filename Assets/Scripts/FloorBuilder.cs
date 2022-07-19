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
    [SerializeField]
    float Scale;
    
    [SerializeField]
    Material MaterialOne;
    [SerializeField]
    Material MaterialTwo;

    public void DoCopy()
    {
        Debug.Log("Hello");
        int xHalf = xAmount / 2;
        int yHalf = yAmount / 2;
        int idx = 0;
        for (var y = -yHalf; y <= yHalf; y++)
        {
                for (var x = -xHalf; x <= xHalf; x++)
                {
                    idx++;
                    var currGo = Instantiate(fieldPrefab);
                    currGo.transform.parent = transform;
                    currGo.transform.position = transform.position + new Vector3(x * Scale, 0, y * Scale);
                    currGo.GetComponent<Renderer>().material = idx % 2 == 0 ? MaterialOne : MaterialTwo;
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
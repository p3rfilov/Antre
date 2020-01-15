using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ObjectPlacer))]
public class ObjectSpacerEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        ObjectPlacer objectPlacer = (ObjectPlacer)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Place / Update"))
        {
            objectPlacer.PlaceObjects();
        }
    }
}

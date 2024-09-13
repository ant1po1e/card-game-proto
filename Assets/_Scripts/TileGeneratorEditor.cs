using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

// Custom Editor Button
[CustomEditor(typeof(TileGenerator))]
public class TileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();  // Draws the default inspector

        TileGenerator generator = (TileGenerator)target;

        // Create a button to trigger tilemap generation
        if (GUILayout.Button("Generate Tilemap"))
        {
            generator.GenerateTilemap();
        }
    }
}
#endif

using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Arena))]
public class ArenaEditor : Editor
{
    SerializedProperty BuildIndex;

    void OnEnable()
    {
        BuildIndex = serializedObject.FindProperty("BuildIndex");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(BuildIndex.intValue)));
    }
}

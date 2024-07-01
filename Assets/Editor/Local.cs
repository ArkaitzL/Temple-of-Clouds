using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Templo))]
public class MiScriptEditor : Editor
{

    string[] checkpointVars = { "id", "marcadorCheckpoint", "apagado", "encendido" };
    string[] powerupsVars = { "habilidad", "marcadorPowerup" };
    string[] infoVars = { "informacion" };

    public override void OnInspectorGUI()
    {
        Templo miScript = (Templo)target;

        // Mostrar los toggles para checkpoint, info y powerup
        miScript.checkpoint = EditorGUILayout.Toggle("Checkpoint", miScript.checkpoint);
        miScript.info = EditorGUILayout.Toggle("Info", miScript.info);
        miScript.powerup = EditorGUILayout.Toggle("Powerup", miScript.powerup);

        // Llamar a OnValidate manualmente para aplicar la lógica
        miScript.OnValidate();

        // Mostrar/ocultar la variable habilidad basado en los bools
        if (miScript.powerup)
        {
            foreach (var var in powerupsVars)
            {
                SerializedProperty prop = serializedObject.FindProperty(var);
                EditorGUILayout.PropertyField(prop);
            }
        }

        if (miScript.info || miScript.powerup)
        {
            foreach (var var in infoVars)
            {
                SerializedProperty prop = serializedObject.FindProperty(var);
                EditorGUILayout.PropertyField(prop);
            }
        }

        if (miScript.checkpoint)
        {
            foreach (var var in checkpointVars)
            {
                SerializedProperty prop = serializedObject.FindProperty(var);
                EditorGUILayout.PropertyField(prop);
            }
        }

        // Aplicar cualquier cambio realizado en el inspector
        serializedObject.ApplyModifiedProperties();
    }
}
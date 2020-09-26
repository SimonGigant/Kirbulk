using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[System.Serializable]
public class PortraitData
{
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public string name;

    public PortraitData(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}

[CreateAssetMenu(fileName = "New CharacterText", menuName = "Dialogue/CharacterText", order = 51)]
[System.Serializable]
public class CharacterText : ScriptableObject
{
    [HideInInspector] public List<PortraitData> portraits;
    [HideInInspector] public string characterName;
    [HideInInspector] public Color color;

    public CharacterText()
    {
        portraits = new List<PortraitData>();
    }

    public List<string> EnumEmotions()
    {
        List<string> emotions = new List<string>();
        foreach(PortraitData p in portraits)
        {
            emotions.Add(p.name);
        }
        return emotions;
    }

    public void CreateNewPortraitData()
    {
        portraits.Add(new PortraitData("new emotion", null));
    }

    public void RemovePortraitData(PortraitData p)
    {
        portraits.Remove(p);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(CharacterText))]
class CharacterTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterText characterText = (CharacterText)target;
        if (characterText == null) return;
        Undo.RecordObject(characterText, "Change CharacterText");

        characterText.characterName = EditorGUILayout.TextField("Name:", characterText.characterName);
        characterText.color = EditorGUILayout.ColorField(characterText.color);
        GUILayout.Space(50f);

        foreach(PortraitData portrait in characterText.portraits)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            portrait.name = EditorGUILayout.TextField("", portrait.name);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("-", GUILayout.Width(30f)))
            {
                characterText.RemovePortraitData(portrait);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            portrait.sprite = (Sprite)EditorGUILayout.ObjectField("", portrait.sprite, typeof(Sprite), allowSceneObjects: false);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(30f)))
        {
            characterText.CreateNewPortraitData();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(characterText);
        }
    }
}
#endif
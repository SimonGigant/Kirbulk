using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New DialogueText", menuName = "Dialogue/DialogueText", order = 51)]
[System.Serializable]
public class DialogueText : ScriptableObject
{
    [HideInInspector] public List<DialogueData> dialogues;

    public void Remove(DialogueData d)
    {
        dialogues.Remove(d);
    }

    public void Add()
    {
        dialogues.Add(new DialogueData());
    }

    public void Switch(DialogueData d, bool up)
    {
        int i = dialogues.IndexOf(d);
        i += up ? -1 : 1;
        if (i < 0 || i >= dialogues.Count)
            return;
        dialogues.Remove(d);
        dialogues.Insert(i, d);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueText))]
class DialogueTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueText dialogueText = (DialogueText)target;
        if (dialogueText == null) return;
        if(dialogueText.dialogues == null)
        {
            dialogueText.dialogues = new List<DialogueData>();
        }
        Undo.RecordObject(dialogueText, "Change DialogueText");

        foreach(DialogueData dialogueData in dialogueText.dialogues)
        {
            GUILayout.Space(50f);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            dialogueData.overloadName = GUILayout.Toggle(dialogueData.overloadName, "Overload name");
            if (dialogueData.overloadName)
            {
                dialogueData.overloadedName = GUILayout.TextField(dialogueData.overloadedName);
            }

            dialogueData.mirror = GUILayout.Toggle(dialogueData.mirror, "Mirror");
            dialogueData.leftSide = GUILayout.Toggle(dialogueData.leftSide, "Left side");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            dialogueData.character = (CharacterText)EditorGUILayout.ObjectField("", dialogueData.character, typeof(CharacterText), allowSceneObjects: false);
            if (dialogueData.character != null && dialogueData.character.portraits.Count > 0)
            {
                List<string> names = dialogueData.character.EnumEmotions();
                int[] ids = new int[names.Count];
                for (int i = 0; i < names.Count; ++i)
                    ids[i] = i;
                if (dialogueData.emotionID >= names.Count)
                    dialogueData.emotionID = 0;
                if (names.Count != 0)
                    dialogueData.emotionID = EditorGUILayout.IntPopup(dialogueData.emotionID, names.ToArray(), ids);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                if(dialogueData.portrait != null && dialogueData.portrait.texture != null)
                    GUILayout.Label(dialogueData.portrait.texture as Texture, GUILayout.Width(100), GUILayout.Height(100) );
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            dialogueData.text = GUILayout.TextArea(dialogueData.text, GUILayout.Height(3f * EditorGUIUtility.singleLineHeight));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (dialogueText.dialogues.IndexOf(dialogueData) > 0 && GUILayout.Button("↑", GUILayout.Width(30f)))
            {
                dialogueText.Switch(dialogueData, true);
            }
            if (dialogueText.dialogues.IndexOf(dialogueData) < dialogueText.dialogues.Count -1 && GUILayout.Button("↓", GUILayout.Width(30f)))
            {
                dialogueText.Switch(dialogueData, false);
            }
            if (GUILayout.Button("-", GUILayout.Width(30f)))
            {
                dialogueText.Remove(dialogueData);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        GUILayout.Space(40f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(30f)))
        {
            dialogueText.Add();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogueText);
        }
    }
}
#endif
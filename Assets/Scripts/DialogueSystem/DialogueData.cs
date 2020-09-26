using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextSize { Small, Normal, Big }
public enum TextEffect { None, Wave, Spooky }

public enum TextSpeed { Slow, Normal, Fast, Faster}

[System.Serializable]
public class DialogueData
{
    public DialogueData(DialogueData d)
    {
        character = d.character;
        emotionID = d.emotionID;
        text = d.text;
        overloadedName = d.overloadedName;
        overloadName = d.overloadName;
        leftSide = d.leftSide;
        mirror = d.mirror;
    }

    public DialogueData()
    {
        character = null;
        emotionID = 0;
        text = "";
        leftSide = false;
        mirror = false;
    }

    public Sprite portrait { get { return character.portraits[emotionID].sprite; } }
    public string name { get { return overloadName ? overloadedName : character.characterName; } }

    public bool overloadName;
    public string overloadedName;
    public CharacterText character;
    public int emotionID;
    public bool mirror;
    public bool leftSide;
    [TextArea(2, 4)] public string text;
    [HideInInspector] public List<bool> emphasis;
    [HideInInspector] public List<TextSize> size;
    [HideInInspector] public List<TextEffect> effect;
    [HideInInspector] public List<TextSpeed> speed;
}

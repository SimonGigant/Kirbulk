using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefabDialogueSystem;
    public DialogueText dialogueSequence;

    public void StartDialogue()
    {
        GameObject dialogue = Instantiate(prefabDialogueSystem);
        DialogueManager dManager = dialogue.GetComponentInChildren<DialogueManager>();
        dManager.thisDialogue = dialogueSequence;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefabDialogueSystem;
    public DialogueText dialogueSequence;
    public string scene;

    public void StartDialogue()
    {
        GameObject dialogue = Instantiate(prefabDialogueSystem);
        DialogueManager dManager = dialogue.GetComponentInChildren<DialogueManager>();
        dManager.thisDialogue = dialogueSequence;
        dManager.SceneAfterDialog = scene;
    }

    public void StartDialogueAfterDelay(float delay)
    {
        StartCoroutine(Delay(delay));
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartDialogue();
    }
}

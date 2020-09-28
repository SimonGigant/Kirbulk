using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefabDialogueSystem;
    public DialogueText dialogueSequence;
    public string scene;

    public Animator JojoAnim;

    private void Start()
    {
        if(JojoAnim != null)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        GameObject dialogue = Instantiate(prefabDialogueSystem);
        DialogueManager dManager = dialogue.GetComponentInChildren<DialogueManager>();
        dManager.thisDialogue = dialogueSequence;
        dManager.SceneAfterDialog = scene;
        if (JojoAnim != null)
        {
            dManager.inJojo = true;
            dManager.jojoAnim = JojoAnim;
        }
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

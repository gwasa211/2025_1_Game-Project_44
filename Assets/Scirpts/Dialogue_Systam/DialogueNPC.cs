using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueDataOS myDialogue;

    private DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null )
        {
            Debug.LogError("���̾�α� �Ŵ����� �����ϴ�");
        }
    }

    private void OnMouseDown()
    {
        if (dialogueManager == null) return;
        if (dialogueManager.IsDialogueActive()) return;
        if (myDialogue == null) return;

        dialogueManager.StatDialogue(myDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

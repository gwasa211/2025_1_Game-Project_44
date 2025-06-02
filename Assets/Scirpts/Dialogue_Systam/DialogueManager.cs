using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - Instector에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImge;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaultCharacterImge;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingClick = true;

    private DialogueDataOS currentDialogue;
    private int currentDLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;


    void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);

        }

        isTyping = false;

    }

    private void CompleteTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;

        if(currentDialogue != null && currentDLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentDLineIndex];
        }

        
    }
    void ShowCurrentLine()
    {
        if(currentDialogue != null && currentDLineIndex < currentDialogue.dialogueLines.Count)
        {
            if(typingCoroutine != null)
            {
                StopCoroutine (typingCoroutine);

            }


            string currentText = currentDialogue.dialogueLines[currentDLineIndex];
            typingCoroutine = StartCoroutine(TypeText(currentText));
        }    
    }

    public void ShowNextLine()
    {
        currentDLineIndex++;

        if(currentDLineIndex >= currentDialogue.dialogueLines.Count)
        {

        }
        else
        {
            ShowCurrentLine();  
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentDLineIndex = 0;
    }

    public void HandleNextInput()
    {
        if (isTyping && skipTypingClick)
        {
            CompleteTyping();
        }
        else if(!isTyping)
        {
            ShowNextLine();
        }

        

    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StatDialogue(DialogueDataOS dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;

        currentDialogue = dialogue;
        currentDLineIndex = 0;
        isDialogueActive = true;

        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if(characterImge != null )
        {
            if(dialogue.characterImage != null)
            {
                characterImge.sprite = dialogue.characterImage;
            }

            else
            {
                characterImge.sprite = defaultCharacterImge;
            }
        }
        
    }
}

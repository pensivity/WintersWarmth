using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ConversationUIManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string text;
        public Sprite leftCharacterSprite;
        public Sprite rightCharacterSprite;
        public bool isLeftCharacterSpeaking;
        public bool flipLeftCharacter;
    }

    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private List<DialogueLine> conversation = new List<DialogueLine>();
    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;

    public void SetConversation(List<DialogueLine> newConversation)
    {
        conversation = newConversation;
    }

    public void StartConversation()
    {
        gameObject.SetActive(true);
        currentLineIndex = 0;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex < conversation.Count)
        {
            DialogueLine line = conversation[currentLineIndex];
            UpdateCharacterSprites(line);
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(line.text));
            currentLineIndex++;
        }
        else
        {
            EndConversation();
        }
    }

    private void UpdateCharacterSprites(DialogueLine line)
    {
        leftCharacterImage.sprite = line.leftCharacterSprite;
        rightCharacterImage.sprite = line.rightCharacterSprite;

        leftCharacterImage.transform.localScale = new Vector3(
            line.flipLeftCharacter ? -1 : 1,
            1,
            1
        );

        leftCharacterImage.color = line.isLeftCharacterSpeaking ? Color.white : Color.gray;
        rightCharacterImage.color = line.isLeftCharacterSpeaking ? Color.gray : Color.white;
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void EndConversation()
    {
        gameObject.SetActive(false);
        ConversationSystemManager.Instance.EndConversation();
    }

    public void AdvanceConversation()
    {
        if (typingCoroutine != null && dialogueText.text.Length < conversation[currentLineIndex - 1].text.Length)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = conversation[currentLineIndex - 1].text;
        }
        else
        {
            DisplayNextLine();
        }
    }

    public List<DialogueLine> GetCurrentConversation()
    {
        return conversation;
    }

}
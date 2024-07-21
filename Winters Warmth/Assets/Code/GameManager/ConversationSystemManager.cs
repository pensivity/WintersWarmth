using UnityEngine;
using System.Collections.Generic;
using System;

public class ConversationSystemManager : MonoBehaviour
{
    public static ConversationSystemManager Instance { get; private set; }

    public GameObject conversationUIPrefab;
    private ConversationUIManager activeConversationUI;

    [System.Serializable]
    public class Conversation
    {
        public string conversationID;
        public List<ConversationUIManager.DialogueLine> lines;
    }

    public List<Conversation> conversations = new List<Conversation>();

    // Add an event for conversation end
    public event Action<string> OnConversationEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartConversation(string conversationID)
    {
        Conversation convo = conversations.Find(c => c.conversationID == conversationID);
        if (convo == null)
        {
            Debug.LogError($"Conversation with ID {conversationID} not found!");
            return;
        }

        if (activeConversationUI == null)
        {
            GameObject conversationUIObject = Instantiate(conversationUIPrefab, transform);
            activeConversationUI = conversationUIObject.GetComponent<ConversationUIManager>();
        }

        activeConversationUI.SetConversation(convo.lines);
        activeConversationUI.StartConversation();
    }

    public void AdvanceConversation()
    {
        if (activeConversationUI != null)
        {
            activeConversationUI.AdvanceConversation();
        }
    }

    public void EndConversation()
    {
        if (activeConversationUI != null)
        {
            string endedConversationID = conversations.Find(c => c.lines == activeConversationUI.GetCurrentConversation())?.conversationID;
            Destroy(activeConversationUI.gameObject);
            activeConversationUI = null;

            // Trigger the event when the conversation ends
            if (!string.IsNullOrEmpty(endedConversationID))
            {
                OnConversationEnded?.Invoke(endedConversationID);
            }
        }
    }
}
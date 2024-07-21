using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private string initialConversationID = "Introduction";

    private void Start()
    {
        StartDialogue(initialConversationID);
    }

    public void StartDialogue(string conversationID)
    {
        ConversationSystemManager.Instance.StartConversation(conversationID);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConversationSystemManager.Instance.AdvanceConversation();
        }
    }

    public void TriggerEventConversation(string eventType)
    {
        switch (eventType)
        {
            case "MeetNPC":
                StartDialogue("SubwayConversation");
                break;
            case "FindItem":
                StartDialogue("ItemDiscovery");
                break;
            case "EndGame":
                StartDialogue("GameConclusion");
                break;
            default:
                Debug.LogWarning($"No conversation defined for event: {eventType}");
                break;
        }
    }
}
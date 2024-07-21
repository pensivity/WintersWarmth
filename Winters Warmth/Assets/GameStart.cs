using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        ConversationSystemManager.Instance.StartConversation("Intro");
        AkSoundEngine.PostEvent("play_music", gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConversationSystemManager.Instance.AdvanceConversation();
        }
    }
}

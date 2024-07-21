using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool hasWon;
    [SerializeField] public bool hasLost;

    private void Awake()
    {
        Time.timeScale = 1;
        hasWon = false;
        hasLost = false;
    }

    void Update()
    {
        if (hasLost)
        {
            Time.timeScale = 0;
            Debug.Log("You lost!");
        }
    }

}
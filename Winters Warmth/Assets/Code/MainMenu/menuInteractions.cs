using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInteractions : MonoBehaviour
{
    [Header("Menu elements")]
    [SerializeField] public GameObject startMenu;
    [SerializeField] public GameObject instructions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startMenu.SetActive(true);
        instructions.SetActive(false);
    }

    public void HowToPlay()
    {
        startMenu.SetActive(false);
        instructions.SetActive(true);
    }

    public void ReturnToStart()
    {
        startMenu.SetActive(true);
        instructions.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("HomeBase");
    }
}

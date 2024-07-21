using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menu elements")]
    [SerializeField] public GameObject HUD;
    [SerializeField] public GameObject pauseMenu;

    [SerializeField] private TMP_Text carrying;
    [SerializeField] private TMP_Text maxCapacity;


    private PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        // Set up game menus
        HUD.SetActive(true);
        pauseMenu.SetActive(false);

        carrying.SetText("{0}", player.carryCapacity);
        maxCapacity.SetText("{0}", player.maxCapacity);
    }

    
    public void RefreshHUD()
    {
        carrying.SetText("{0}", player.carryCapacity);
        maxCapacity.SetText("{0}", player.maxCapacity);
    }
}

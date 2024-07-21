using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menu elements")]
    [SerializeField] public GameObject HUD;
    [SerializeField] public GameObject pauseMenu;

    [SerializeField] public GameObject freezing;

    [SerializeField] public Slider playerHealth;
    [SerializeField] public Slider houseHealth;

    [SerializeField] private TMP_Text carrying;
    [SerializeField] private TMP_Text maxCapacity;


    private PlayerController player;
    private ResourceManager house;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        house = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();

        // Set up game menus
        HUD.SetActive(true);
        pauseMenu.SetActive(false);
        freezing.SetActive(false);

        //Set up HUD
        playerHealth.maxValue = player.maxPlayerWarmth;
        playerHealth.value = player.playerWarmth;
        houseHealth.maxValue = house.houseWarmthMax;
        houseHealth.value = house.houseWarmth;
        carrying.SetText("{0}", player.carryCapacity);
        maxCapacity.SetText("{0}", player.maxCapacity);
    }


    private void Update()
    {
        RefreshHUD();
    }


    public void RefreshHUD()
    {
        playerHealth.value = player.playerWarmth;
        houseHealth.value = house.houseWarmth;

        carrying.SetText("{0}", player.carryCapacity);
        maxCapacity.SetText("{0}", player.maxCapacity);

        // Activate freezing effect
        if (player.playerWarmth < 40)
        {
            freezing.SetActive(true);
        } else
        {
            freezing.SetActive(false);
        }
    }
}

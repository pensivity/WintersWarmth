using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public int carryCapacity;
    [SerializeField] public int maxCapacity;
    [SerializeField] public int maxPlayerWarmth;
    [SerializeField] public int playerWarmth;
    [SerializeField] public int coolFactor;

    [SerializeField] private GameManager gm;

    [SerializeField] public bool isOutside;
    private bool isCooling;
    private bool isWarming;


    private void Awake()
    {
        // Set up initial player parameters
        speed = 5f;
        carryCapacity = 0;
        maxCapacity = 5;
        maxPlayerWarmth = 100;
        playerWarmth = maxPlayerWarmth;
        coolFactor = 5;

        // Find Game Manager
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Set up whether the player is inside and warming, or outside and cooling
        isOutside = false;

        isCooling = false;
        isWarming = false;
    }

    private void FixedUpdate()
    {
        if (isOutside)
        {
            StartCoroutine(DecreasePlayerWarmth());
        }
        else
        {
            StartCoroutine(IncreasePlayerWarmth());
        }

        if (playerWarmth < 0)
        {
            gm.hasLost = true;
        }
    }

    IEnumerator DecreasePlayerWarmth()
    {
        if (!isCooling)
        {
            isCooling = true;
            playerWarmth -= coolFactor;
            // Add sound effects here!
            yield return new WaitForSeconds(2);
            isCooling = false;
            yield return null;
        }
    }


    IEnumerator IncreasePlayerWarmth()
    {
        if (!isWarming && playerWarmth < maxPlayerWarmth)
        {
            isWarming = true;
            playerWarmth += 10;
            // Add sound effects here!
            yield return new WaitForSeconds(2);
            isWarming = false;
            yield return null;
        }
    }


    public void CoolingDown(Component sender, object data)
    {
        isOutside = true;
    }

    public void WarmingUp(Component sender, object data)
    {
        isOutside = false;
    }
}

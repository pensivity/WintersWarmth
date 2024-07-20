using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public int carryCapacity;
    [SerializeField] public int maxCapacity;
    [SerializeField] public int playerWarmth;
    [SerializeField] public bool isOutside;

    private bool isCooling;


    private void Awake()
    {
        speed = 5f;
        carryCapacity = 0;
        maxCapacity = 5;
        playerWarmth = 100;
        isOutside = false;

        isCooling = false;
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

    }

    IEnumerator DecreasePlayerWarmth()
    {
        if (!isCooling)
        {
            isCooling = true;
            playerWarmth--;
            // Add sound effects here!
            yield return new WaitForSeconds(2);
            isCooling = false;
            yield return null;
        }
    }


    IEnumerator IncreasePlayerWarmth()
    {
        if (!isCooling)
        {
            isCooling = true;
            playerWarmth++;
            // Add sound effects here!
            yield return new WaitForSeconds(2);
            isCooling = false;
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

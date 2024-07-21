using System.Collections;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] public int houseWarmth;

    private bool isCooling;
    private PlayerController player;
    private GameManager gm;
    private int addedWarmth;

    private void Awake()
    {
        houseWarmth = 100;
        isCooling = false;
        player = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        addedWarmth = 20;
    }


    // FixedUpdate is called once per set time
    void FixedUpdate()
    {
        StartCoroutine(DecreaseWarmth());

        if (houseWarmth < 0)
        {
            gm.hasLost = true;
        }
    }

    IEnumerator DecreaseWarmth ()
    {
        if (!isCooling)
        {
            isCooling = true;
            houseWarmth--;
            // Add sound effects here!
            yield return new WaitForSeconds(3);
            isCooling = false;
            yield return null;
        }
    }

    public void AddFuel(Component sender, object data)
    {
        houseWarmth += player.carryCapacity * addedWarmth;
        player.carryCapacity = 0;
    }
}

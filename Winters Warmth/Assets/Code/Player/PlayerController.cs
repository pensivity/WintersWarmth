using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public int carryCapacity;
    [SerializeField] public int maxCapacity;
    [SerializeField] public int playerWarmth;


    private void Awake()
    {
        speed = 5f;
        carryCapacity = 0;
        maxCapacity = 5;
        playerWarmth = 100;
    }



}

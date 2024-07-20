using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;


    private void Awake()
    {
        speed = 5f;
    }


}

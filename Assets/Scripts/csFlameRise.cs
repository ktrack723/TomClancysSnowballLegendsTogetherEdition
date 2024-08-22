using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class csFlameRise : MonoBehaviour
{
    [Header("Assigned")]

    [SerializeField] private GameObject flame_Drop;

    [SerializeField] private Vector3 fixedVelocity;



    void Start()
    {
        
    }



    void Update()
    {
        transform.Translate(fixedVelocity * Time.deltaTime);

        if (transform.position.y > 12)
        {
            Instantiate(flame_Drop);

            Destroy(gameObject);
        }
    }
}

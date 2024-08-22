using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public Collider collider;
    public Rigidbody rigidbody;

    public bool IsCaught = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

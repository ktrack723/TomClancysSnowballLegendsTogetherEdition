using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public Collider collider;
    public Rigidbody rigidbody;

    public bool IsCaught = false;
    private bool hasTriggeredSpawn = false;

    public GameObject originalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class csFlameDrop : MonoBehaviour
{
    [Header("Fetch on start")]

    [SerializeField] private GameObject player;

    [SerializeField] private csURUKManager URUKManager;

    [Header("Assigned")]

    [SerializeField] private GameObject shadow;

    [SerializeField] private AudioClip boomClip;

    [Header("Parameters")]

    [SerializeField] private float dropSpeed;

    [SerializeField] private float smoothTime;

    [Header("Debug")]

    [SerializeField] private GameObject instancedShadow;

    [SerializeField] private Vector3 currentVelocity;

    [SerializeField] private float startHeight;



    void Start()
    {
        URUKManager = GameObject.FindGameObjectWithTag("URUKManager").GetComponent<csURUKManager>();

        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = new Vector3(player.transform.position.x, 12, player.transform.position.z);

        instancedShadow = Instantiate(shadow, player.transform.position, Quaternion.identity);

        instancedShadow.transform.localScale = Vector3.zero;

        startHeight = transform.position.y;

        dropSpeed *= URUKManager.speedMultiplier;
    }



    void Update()
    {
        // Track
        Vector3 trackedPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref currentVelocity, smoothTime);
        transform.position = new Vector3(trackedPosition.x, transform.position.y, trackedPosition.z);
        instancedShadow.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        smoothTime = 0.25f + 5f / transform.position.y;

        // Drop
        transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);
        instancedShadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, transform.position.y / startHeight);

        if (transform.position.y < 2f)
        {
            Destroy(gameObject);
        }
    }



    private void OnDestroy()
    {
        if (URUKManager != null)
        {
            URUKManager.GetComponent<AudioSource>().PlayOneShot(boomClip, 1.275f);
        }

        Destroy(instancedShadow);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House") == true)
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Floor") == true)
        {
            URUKManager.floors.Remove(other.gameObject);

            Destroy(other.gameObject);

            Destroy(gameObject);
        }

        if (other.CompareTag("Player") == true)
        {
            Destroy(GameObject.FindGameObjectWithTag("BGM"));

            csBGMRandomizer.instanceExists = false;

            SceneManager.LoadScene("Death");

            //Destroy(gameObject);
        }
    }
}
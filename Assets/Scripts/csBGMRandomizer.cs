using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class csBGMRandomizer : MonoBehaviour
{
    [Header("Fetch on start")]

    [SerializeField] private AudioSource audioSource;

    [Header("Parameters")]

    [SerializeField] private List<AudioClip> BGMs;

    public static bool instanceExists;



    private void Awake()
    {
        if (instanceExists == true)
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        instanceExists = true;
    }



    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = BGMs[Random.Range(0, BGMs.Count)];

        audioSource.Play();
    }



    void Update()
    {
        
    }
}

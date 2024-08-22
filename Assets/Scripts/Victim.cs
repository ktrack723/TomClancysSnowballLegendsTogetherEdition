using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public Collider collider;
    public Rigidbody rigidbody;
    public Runaway runaway;

    public bool IsCaught = false;
    private bool hasTriggeredSpawn = false;

    public GameObject originalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        runaway = GetComponent<Runaway>();
        //Kill();

        AnimalManager.Instance.VictimList.Add(this);
    }

    public void Kill()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(SkinnedMeshRenderer skinned in skinnedMeshRenderer)
        {
            // Check if the material has a BaseMap property
            if (skinned.material.HasProperty("_BaseMap"))
            {
                // Change the color of the BaseMap to black
                skinned.material.SetColor("_BaseColor", Color.black);
            }
            else
            {
                Debug.LogError("Material does not have a BaseMap property.");
            }
        }

        var anim = GetComponent<Animator>();
        if (anim)
        {
            anim.enabled = false;
        }

        AnimalManager.Instance.VictimList.Remove(this);

        if (AnimalManager.Instance.VictimList.Count == 0)
        {
            GameObject.Find("TimerText").GetComponent<TimerUI>().EndGame();
        }
    }
}

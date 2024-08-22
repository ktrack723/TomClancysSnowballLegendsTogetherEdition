using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

    public List<Victim> VictimList = new List<Victim>();

    private void Awake()
    {
        Instance = this;
    }
}

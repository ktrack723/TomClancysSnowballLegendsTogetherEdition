using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;



public class csDeath : MonoBehaviour
{
    [Header("Assigned")]

    [SerializeField] private TransitionSettings transitionSettings;



    void Start()
    {
        TransitionManager.Instance().Transition("Heaven", transitionSettings, 3);
    }



    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using EasyTransition;



public class csURUKManager : MonoBehaviour
{
    [Header("Fetch on start")]

    [SerializeField] private AudioSource audioSource;

    [Header("Assigned")]

    public float speedMultiplier;

    [SerializeField] private string stageNumber;

    [SerializeField] private string nextStage;

    [SerializeField] private SpriteRenderer URUK_renderer;

    [SerializeField] private Sprite URUK_Dumb;
    [SerializeField] private Sprite URUK_Eat_01;
    [SerializeField] private Sprite URUK_Eat_02;
    [SerializeField] private Sprite URUK_Charge;
    [SerializeField] private Sprite URUK_Fire_01;
    [SerializeField] private Sprite URUK_Fire_02;
    [SerializeField] private Sprite URUK_Happy;
    [SerializeField] private Sprite URUK_Sad;

    [SerializeField] private GameObject anchor_Flame;

    [SerializeField] private GameObject flame_Rise;

    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip chargeClip;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip happyClip;
    [SerializeField] private AudioClip sadClip;

    [SerializeField] private TransitionSettings transitionSettings;

    [Header("Debug")]

    public List<GameObject> floors;

    [SerializeField] private int flameCount;

    [SerializeField] private bool isFiring;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        floors = GameObject.FindGameObjectsWithTag("Floor").ToList();

        StartCoroutine(stageNumber);
    }



    private void Update()
    {
        if (isFiring == true)
        {
            if (URUK_renderer.sprite == URUK_Fire_02)
            {
                URUK_renderer.sprite = URUK_Fire_01;
            }
            else
            {
                URUK_renderer.sprite = URUK_Fire_02;
            }
        }
    }



    private IEnumerator Stage_01()
    {
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5);

        DetermineEnding();
    }



    private IEnumerator Stage_02()
    {
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(ShootFlame(4));

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5);

        DetermineEnding();
    }



    private IEnumerator Stage_03()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(5));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(2));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator Stage_04()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(2));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(2));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator Stage_05()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(1));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(4));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator Stage_06()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(4));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(2));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator Stage_07()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(6));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(4));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(3));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator Stage_08()
    {
        yield return new WaitForSeconds(3 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(5));

        yield return new WaitForSeconds(1 / speedMultiplier);

        yield return StartCoroutine(ShootFlame(6));

        yield return new WaitForSeconds(4.25f);

        DetermineURUKFeeling();

        yield return new WaitForSeconds(5 / speedMultiplier);

        DetermineEnding();
    }



    private IEnumerator ShootFlame(int arg)
    {
        flameCount = arg;

        URUK_renderer.sprite = URUK_Eat_01;

        audioSource.PlayOneShot(eatClip);

        for (int i = 0; i < flameCount - 1; i++)
        {
            yield return new WaitForSeconds(1.0f / speedMultiplier);

            audioSource.PlayOneShot(eatClip, 1.75f);

            if (URUK_renderer.sprite == URUK_Eat_02)
            {
                URUK_renderer.sprite = URUK_Eat_01;
            }
            else
            {
                URUK_renderer.sprite = URUK_Eat_02;
            }
        }

        yield return new WaitForSeconds(1.0f / speedMultiplier);

        audioSource.pitch = speedMultiplier;

        audioSource.PlayOneShot(chargeClip);

        URUK_renderer.sprite = URUK_Charge;

        yield return new WaitForSeconds(2.5f / speedMultiplier);

        audioSource.pitch = 1.0f;

        isFiring = true;

        for (int i = 0; i < flameCount; i++)
        {
            audioSource.PlayOneShot(fireClip, 0.8f);

            Instantiate(flame_Rise, anchor_Flame.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(2.0f / speedMultiplier);
        }

        isFiring = false;

        URUK_renderer.sprite = URUK_Dumb;
    }



    public void DetermineURUKFeeling()
    {
        if (floors.Count == 1)
        {
            audioSource.PlayOneShot(happyClip, 1.55f);

            URUK_renderer.sprite = URUK_Happy;
        }
        else
        {
            audioSource.PlayOneShot(sadClip, 1.65f);

            URUK_renderer.sprite = URUK_Sad;
        }
    }



    private void DetermineEnding()
    {
        if (URUK_renderer.sprite == URUK_Happy)
        {
            if (nextStage != string.Empty)
            {
                TransitionManager.Instance().Transition(nextStage, transitionSettings, 0);
            }
            else
            {
                GoodEnding();
            }
        }
        else
        {
            BadEnding();
        }
    }



    private void GoodEnding()
    {
        Debug.Log("GoodEnding!");

        TransitionManager.Instance().Transition("Credits", transitionSettings, 0);
    }



    private void BadEnding()
    {
        Debug.Log("BadEnding!");

        TransitionManager.Instance().Transition(stageNumber, transitionSettings, 0);
    }
}

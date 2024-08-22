using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EasyTransition;

public class TimerUI : MonoBehaviour
{
    public TextMeshPro TMPro;
    public float RemainTimeSeconds;

    public bool timerStop = false;
    public TextMeshPro snowManText;

    public Camera mainCam;
    public Camera endingCam;

    public TransitionSettings transition;

    // Start is called before the first frame update
    void Start()
    {
        snowManText = GameObject.Find("SnowmanText").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerStop)
        {
            return;
        }

        RemainTimeSeconds -= Time.fixedDeltaTime;

        if (RemainTimeSeconds >= 60)
        {
            TMPro.text = $"{(int)(RemainTimeSeconds / 60)}:{(int)RemainTimeSeconds % 60}";
        }
        else if (RemainTimeSeconds >= 10)
        {
            TMPro.text = $"{(int)RemainTimeSeconds}";
        }
        else if (RemainTimeSeconds >= 0)
        {
            TMPro.text = $"{RemainTimeSeconds.ToString("F2")}";
        }
        else
        {
            if (snowManText.gameObject.activeSelf == false)
            {
                return;
            }
            TMPro.text = $"Time Over!";
            EndGame();
            snowManText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (endingCam.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                TransitionManager.Instance().Transition("Heaven", transition, 0);
            }
        }
    }

    public void EndGame()
    {
        TransitionManager.Instance().Transition(transition, 0);
        TransitionManager.Instance().onTransitionCutPointReached = () => {
            mainCam.enabled = false;
            endingCam.enabled = true;
        };
    }
}

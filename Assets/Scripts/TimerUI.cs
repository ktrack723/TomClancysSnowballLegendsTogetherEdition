using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshPro TMPro;
    public float RemainTimeSeconds;

    public bool timerStop = false;
    public TextMeshPro snowManText;

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


        var min = Mathf.FloorToInt(RemainTimeSeconds / 60f).ToString("00");
        var sec = Mathf.FloorToInt(RemainTimeSeconds % 60f).ToString("00");
        
        if (RemainTimeSeconds >= 10)
        {
            TMPro.text = $"{min}:{sec}";
        }
        else if (RemainTimeSeconds >= 0)
        {
            TMPro.text = $"{RemainTimeSeconds.ToString("F2")}";
        }
        else
        {
            TMPro.text = $"Time Over!";
            snowManText.gameObject.SetActive(false);
        }
    }
}

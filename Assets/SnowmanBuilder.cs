using AutoLetterbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBuilder : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerSpawnParent;

    public GameObject MainCamera;
    public GameObject BuilderCamera;

    public Transform SnowmanDropTransformLeft;
    public Transform SnowmanDropTransformRight;
    public GameObject SnowmanDropper;

    public float DropperMoveSpeed;

    private float OriginalDropperMoveSpeed;
    private int SnowBallIndex = 0;

    private bool InBuildMode = false;
    private bool IsDropped = false;

    private void Awake()
    {
        BackToGame();
    }

    public void BackToGame()
    {
        MainCamera.SetActive(true);
        BuilderCamera.SetActive(false);

        InBuildMode = false;

        var player = Instantiate(PlayerPrefab);
        player.transform.position = PlayerSpawnParent.transform.position;

        SmoothCameraFollow.Instance.target = player.transform;
    }

    public void EnterBuilderMode()
    {
        MainCamera.SetActive(false);
        BuilderCamera.SetActive(true);

        InBuildMode = true;
        IsDropped = false;

        BF_PlayerSnow.Instance.rB.isKinematic = true;
        BF_PlayerSnow.Instance.transform.SetParent(SnowmanDropper.transform, false);
        BF_PlayerSnow.Instance.transform.localPosition = Vector3.zero;

        BF_PlayerSnow.Instance.enabled = false;
        BF_PlayerMovement.Instance.enabled = false;

        OriginalDropperMoveSpeed = DropperMoveSpeed;

        if (SnowBallIndex == 0)
        {
            DropperMoveSpeed = 0;
        }
    }

    public void Update()
    {
        float t = (Mathf.Sin(Time.time * DropperMoveSpeed) + 1.0f) / 2.0f;
        SnowmanDropper.transform.position = Vector3.Lerp(SnowmanDropTransformLeft.position, SnowmanDropTransformRight.position, t);

        if (InBuildMode && !IsDropped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropSnowball();
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            EnterBuilderMode();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            BackToGame();
        }
    }

    private void DropSnowball()
    {
        IsDropped = true;

        BF_PlayerSnow.Instance.transform.SetParent(SnowmanDropper.transform.parent, true);
        BF_PlayerSnow.Instance.rB.isKinematic = false;

        if (SnowBallIndex == 0)
        {
            DropperMoveSpeed = OriginalDropperMoveSpeed;
        }

        SnowBallIndex++;
    }
}

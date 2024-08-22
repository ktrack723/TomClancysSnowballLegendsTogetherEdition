using AutoLetterbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanBuilder : MonoBehaviour
{
    public static SnowmanBuilder Instance;

    public GameObject PlayerPrefab;
    public Transform PlayerSpawnParent;

    public GameObject MainCamera;
    public GameObject BuilderCamera;

    public Transform SnowmanDropTransformLeft;
    public Transform SnowmanDropTransformRight;
    public GameObject SnowmanDropper;

    public SmoothCameraFollow cameraFollow;

    public float DropperMoveSpeed;

    private float OriginalDropperMoveSpeed;

    private bool InBuildMode = true;
    private bool IsDropped = false;

    private Vector3 inputDirection;

    private void Awake()
    {
        Instance = this;
        BackToGame();
    }

    public void BackToGame()
    {
        if (!InBuildMode)
        {
            return;
        }

        MainCamera.SetActive(true);
        BuilderCamera.SetActive(false);

        InBuildMode = false;

        var player = Instantiate(PlayerPrefab);
        player.transform.position = PlayerSpawnParent.transform.position;

        cameraFollow.target = player.transform;
    }

    public void EnterBuilderMode()
    {
        if (InBuildMode)
        {
            return;
        }

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

        if (BF_PlayerSnow.Instance.index == 0)
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

            if (Input.GetKey(KeyCode.A))
            {
                inputDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDirection += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                inputDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDirection += Vector3.back;
            }
            MoveBall();
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            EnterBuilderMode();
        }
    }

    private void MoveBall()
    {
        BF_PlayerSnow.Instance.transform.Rotate(new Vector3(inputDirection.z, 0, -inputDirection.x) * 1);
        inputDirection = Vector3.zero;
    }

    private void DropSnowball()
    {
        IsDropped = true;

        BF_PlayerSnow.Instance.transform.SetParent(SnowmanDropper.transform.parent, true);
        BF_PlayerSnow.Instance.rB.isKinematic = false;

        BF_PlayerSnow.Instance.ddb.enabled = true;

        if (BF_PlayerSnow.Instance.index == 0)
        {
            DropperMoveSpeed = OriginalDropperMoveSpeed;
        }
    }

    public void BackToGameAfterSeconds(float sleepTimes)
    {
        StartCoroutine(internalCoroutine(sleepTimes));
    }

    private IEnumerator internalCoroutine(float sleepTimes)
    {
        yield return new WaitForSeconds(sleepTimes);

        BackToGame();
    }
}

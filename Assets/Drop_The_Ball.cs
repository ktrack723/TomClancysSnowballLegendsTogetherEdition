using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_The_Ball : MonoBehaviour
{
    public static Drop_The_Ball Instance;
    public BF_PlayerSnow snow;

    public GameObject hitEffect;

    void Start()
    {
        Instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            // FIrst SnOWBalL is Safe
            if (snow.index == 0)
            {
                FixSnowball(collision.contacts[0].point);
                return;
            }

            enabled = false;
            SnowmanBuilder.Instance.BoomAllSnowball(snow);
            return;
        }

        var otherSnow = collision.gameObject.GetComponent<BF_PlayerSnow>();
        if (otherSnow)
        {
            var radius = otherSnow.sphereColider.radius * otherSnow.sphereColider.transform.localScale.x;
            var dropPos = new Vector2(transform.position.x, transform.position.z);
            var underPos = new Vector2(otherSnow.transform.position.x, otherSnow.transform.position.z);

            var distance = Vector2.Distance(dropPos, underPos);
            var isSafe = distance <= radius;

            Debug.Log($"´« ¶³¾îÁü, °á°ú={isSafe}, ¶³¾îÁø¾Ö={dropPos}, ¸ÂÀº¾Ö={underPos}, ¹ÝÁö¸§={radius}, °Å¸®={distance}");

            if (isSafe)
            {
                FixSnowball(collision.contacts[0].point);
            }
            else
            {
                enabled = false;
                SnowmanBuilder.Instance.BoomAllSnowball(snow);
                return;
            }
        }
    }

    private void FixSnowball(Vector3 collisionPoint)
    {
        snow.rB.isKinematic = true;
        snow.rB.useGravity = false;
        enabled = false;

        if (transform.position.y > SnowmanBuilder.Instance.HighestSnowmanHeight)
        {
            SnowmanBuilder.Instance.HighestSnowmanHeight = transform.position.y;
        }
        SnowmanBuilder.Instance.BackToGameAfterSeconds(3f);

        var effect = Instantiate(hitEffect, collisionPoint, Quaternion.identity);

        SnowmanBuilder.Instance.BuiltSnowballList.Add(snow);
    }
}

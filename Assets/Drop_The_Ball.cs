using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_The_Ball : MonoBehaviour
{
    public static Drop_The_Ball Instance;
    public BF_PlayerSnow snow;

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
                snow.rB.isKinematic = true;
                snow.rB.useGravity = false;
                enabled = false;

                SnowmanBuilder.Instance.BackToGameAfterSeconds(3f);
                return;
            }

            enabled = false;
            // /execute BOOM -CODE -hEre
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
                snow.rB.isKinematic = true;
                snow.rB.useGravity = false;
                enabled = false;

                SnowmanBuilder.Instance.BackToGameAfterSeconds(3f);
            }
            else
            {
                enabled = false;
                // GOM - MUN - GOM - MUN and Boom on collide with floor.
                return;
            }
        }
    }
}

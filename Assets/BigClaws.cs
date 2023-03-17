using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClaws : MonoBehaviour
{
    Transform player;
    PlayerAnimation playerAnimation;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject clawSprite;
    ParticleSystem particles;
    Vector3 startDirection;
    Vector3 startPosition;
    Vector3 endDirection;
    Vector3 endPosition;
    Vector3 height = new Vector3(0, 1, 0);
    Vector3 playerDirection;
    Vector3 swipeDirection;
    bool swiping = false;
    float swipeSpeed = 35;

    float distance;
    float currentDistance;
    float k = 2f;
    float a;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        player = transform.parent;
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        clawSprite.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (swiping)
        {

            currentDistance -= Time.fixedDeltaTime * swipeSpeed;
            transform.position = GetPositionFromParabola();
            if(currentDistance < Time.fixedDeltaTime * swipeSpeed)
            {
                swiping = false;
                StartCoroutine(EndDelay());
            }
        }
    }

    public void ClawSwipe(AttackProfiles attackProfile)
    {
        playerDirection = (attackPoint.position - player.position).normalized;
        startDirection = RotateVectorByAngle(playerDirection, attackProfile.clawAngle[playerAnimation.facingDirection]);
        startPosition = player.transform.position + startDirection * attackProfile.attackRange + height;

        endDirection = RotateVectorByAngle(playerDirection, -attackProfile.clawAngle[playerAnimation.facingDirection]);
        endPosition = player.transform.position + endDirection * attackProfile.attackRange + height;

        swipeDirection = endPosition - startPosition;

        transform.position = startPosition;
        transform.eulerAngles = new Vector3(25, 0, attackProfile.clawRotations[playerAnimation.facingDirection]);
        transform.localScale = attackProfile.clawScales[playerAnimation.facingDirection];

        DefineParabola();

        clawSprite.SetActive(true);
        StartCoroutine(StartDelay());
    }

    void DefineParabola()
    {
        // define a parabola with equation y = a(x-h)^2 + k
        // define a new coordinate system where the axis parallel to the line between the startPosition and endPosition is the x axis
        // define the midpoint between startPosition and endPosition as x = 0
        // define the y axis as the line perpedicular to the new x axis in the Unity xz plane
        // h = 0, at startPosition and endPosition y = 0, k is height of the arc
        // solve for a

        distance = Vector3.Distance(startPosition, endPosition);
        currentDistance = distance;
        a = -4 * k / Mathf.Pow(distance, 2);
    }

    Vector3 GetPositionFromParabola()
    {
        float xValue = currentDistance - distance / 2;
        float yValue = a * Mathf.Pow(xValue, 2) + k;
        return endPosition - swipeDirection.normalized * currentDistance + playerDirection.normalized * yValue;
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(.2f);
        swiping = true;
        particles.Play();
    }

    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.2f);
        clawSprite.SetActive(false);
        particles.Stop();
    }

    Vector3 RotateVectorByAngle(Vector3 startVector, float angle)
    {
        float newX = Mathf.Cos(Mathf.Deg2Rad * angle) * startVector.x - Mathf.Sin(Mathf.Deg2Rad * angle) * startVector.z;
        float newY = Mathf.Sin(Mathf.Deg2Rad * angle) * startVector.x + Mathf.Cos(Mathf.Deg2Rad * angle) * startVector.z;
        return new Vector3(newX, 0, newY).normalized;
    }
}

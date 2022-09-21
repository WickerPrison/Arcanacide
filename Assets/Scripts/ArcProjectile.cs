using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcProjectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject indicatorCirclePrefab;
    TouchingCollider touchingCircle;
    [SerializeField] float explosionRadius;
    public Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] int spellDamage;
    [SerializeField] int poiseDamage;
    PlayerScript player;
    Collider playerCollider;
    public Vector3 endPoint;
    Vector3 startPoint;
    [SerializeField] float arcHeight;
    float midpoint;
    float arcWidth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerCollider = player.gameObject.GetComponent<Collider>();

        
        

        startPoint = transform.position;
        direction = new Vector3(endPoint.x, 0, endPoint.z) - new Vector3(startPoint.x, 0, startPoint.z);
        float distance = Vector2.Distance(new Vector2(startPoint.x, startPoint.z), new Vector2(endPoint.x, endPoint.z));

        midpoint = distance / 2;
        arcWidth = arcHeight / Mathf.Pow(midpoint, 2);


        IndicatorCircle indicatorCircle = Instantiate(indicatorCirclePrefab).GetComponent<IndicatorCircle>();
        indicatorCircle.transform.position = new Vector3(endPoint.x, 0, endPoint.z);
        indicatorCircle.finalScale = explosionRadius;
        indicatorCircle.startScale = explosionRadius;
        indicatorCircle.deathTime = 1.5f;
        touchingCircle = indicatorCircle.gameObject.GetComponentInChildren<TouchingCollider>();
    }

    void FixedUpdate()
    {
        transform.position = transform.position + direction.normalized * Time.fixedDeltaTime * speed;
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(startPoint.x, startPoint.z));
        float currentHeight = -arcWidth * Mathf.Pow(distance - midpoint, 2) + arcHeight;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        if(transform.position.y <= 0)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        List<Collider> objects = touchingCircle.GetTouchingObjects();
        if (objects.Contains(playerCollider))
        {
            player.LoseHealth(spellDamage);
            player.LosePoise(poiseDamage);
        }

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = new Vector3(endPoint.x, .3f, endPoint.z);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRipple : MonoBehaviour
{
    [SerializeField] GameObject iceBoxPrefab;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    [System.NonSerialized] public float startRadius = 2;
    [System.NonSerialized] public int numberOfBoxes = 35;
    [System.NonSerialized] public float rippleSpeed = 5;
    [System.NonSerialized] public float lifeTime = 2;
    public int damage;
    public float poiseDamage;
    public Color boxColor;

    // Start is called before the first frame update
    void Start()
    {
        float rotateAngle = 360 / numberOfBoxes;
        for(int box = 0; box < numberOfBoxes; box++)
        {
            RippleBox iceBox = Instantiate(iceBoxPrefab).GetComponent<RippleBox>();
            iceBox.transform.position = transform.position + new Vector3(startRadius, 0, 0);
            iceBox.transform.RotateAround(transform.position, transform.up, rotateAngle * box);
            iceBox.rippleSpeed = rippleSpeed;
            iceBox.lifeTime = lifeTime;
            iceBox.direction = Vector3.Normalize(iceBox.transform.position - transform.position);
            if(boxColor != null)
            {
                ParticleSystem.MainModule particleSystem = iceBox.GetComponent<ParticleSystem>().main;
                particleSystem.startColor = boxColor;
            }
            WaveBox waveBox = iceBox.GetComponent<WaveBox>();
            waveBox.enemyOfOrigin = enemyOfOrigin;
            waveBox.damage = damage;
            waveBox.poiseDamage = poiseDamage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

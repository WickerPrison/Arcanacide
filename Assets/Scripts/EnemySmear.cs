using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmear : MonoBehaviour
{
    public ParticleSystem frontSmear;
    public ParticleSystem backSmear;
    [SerializeField] Transform attackPoint;
    EnemyController enemyController;
    Vector3 frontSmearScale;
    Vector3 frontSmearRotation;
    Vector3 frontSmearPosition;
    Vector3 backSmearScale;
    Vector3 backSmearRotation;
    Vector3 backSmearPosition;
    Vector3 away = new Vector3(100, 100, 100);

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        frontSmearScale = frontSmear.transform.localScale;
        frontSmearRotation = new Vector3(90, -20, 0);
        frontSmearPosition = new Vector3(-0.17f, -0.3f, 0.17f);
        backSmearScale = backSmear.transform.localScale;
        backSmearRotation = new Vector3(-90, 70, 0);
        backSmearPosition = new Vector3(0.17f, -0.3f, 0.17f);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyController.navAgent.enabled && !enemyController.directionLock)
        {
            SmearDirection();
        }
    }

    public void PlaySmear(int smearSpeed)
    {
        ParticleSystem.ShapeModule frontSmearShape = frontSmear.shape;
        ParticleSystem.ShapeModule backSmearShape = backSmear.shape;
        frontSmearShape.arcSpeed = smearSpeed;
        backSmearShape.arcSpeed = -smearSpeed;
        frontSmear.Play();
        backSmear.Play();
    }

    public void SmearDirection()
    {
        if (attackPoint.position.z < transform.position.z)
        {
            if (attackPoint.position.x > transform.position.x)
            {
                backSmear.transform.position = away;
                frontSmear.transform.localScale = frontSmearScale;
                frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, frontSmearRotation.y, frontSmearRotation.z);
                frontSmear.transform.localPosition = frontSmearPosition;
            }
            else
            {
                backSmear.transform.position = away;
                frontSmear.transform.localScale = new Vector3(-frontSmearScale.x, frontSmearScale.y, frontSmearScale.z);
                frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, -frontSmearRotation.y, frontSmearRotation.z);
                frontSmear.transform.localPosition = new Vector3(-frontSmearPosition.x, frontSmearPosition.y, frontSmearPosition.z);
            }
        }
        else
        {
            if (attackPoint.position.x < transform.position.x)
            {
                frontSmear.transform.position = away;
                backSmear.transform.localScale = backSmearScale;
                backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, -backSmearRotation.y, backSmearRotation.z);
                backSmear.transform.localPosition = new Vector3(-backSmearPosition.x, backSmearPosition.y, backSmearPosition.z);
            }
            else
            {
                frontSmear.transform.position = away;
                backSmear.transform.localScale = new Vector3(-backSmearScale.x, backSmearScale.y, backSmearScale.z);
                backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, backSmearRotation.y, backSmearRotation.z);
                backSmear.transform.localPosition = backSmearPosition;
            }
        }
    }
}

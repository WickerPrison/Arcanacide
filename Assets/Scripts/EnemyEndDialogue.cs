using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndDialogue : MonoBehaviour, IEndDialogue
{
    EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    public void EndDialogue()
    {
        enemyController.state = EnemyState.IDLE;
    }
}

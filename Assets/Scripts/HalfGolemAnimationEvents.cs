using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfGolemAnimationEvents : MonoBehaviour
{
    HalfGolemController halfGolemController;

    // Start is called before the first frame update
    void Start()
    {
        halfGolemController = GetComponentInParent<HalfGolemController>();
    }
    
    public void Stomp()
    {
        halfGolemController.Stomp();
    }
}

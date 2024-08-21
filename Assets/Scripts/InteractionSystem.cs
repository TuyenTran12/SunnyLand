using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public Transform detectionPoint;

    private const float detectionRadius = 0.2f;

    public LayerMask detectionLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectObject())
        {
            if(InteractInput())
            {

            }
        }
    }
    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}

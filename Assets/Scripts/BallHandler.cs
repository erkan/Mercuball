using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    [SerializeField] private Rigidbody2D currentBallRigidBody;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float delayDuration; 
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidBody == null) {return; }
        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
           // currentBallRigidBody.isKinematic = false;
            return;
        }
        isDragging=true;
        //currentBallRigidBody.isKinematic = true;
        Vector2 touchpos = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldpos = mainCamera.ScreenToWorldPoint(touchpos);
        currentBallRigidBody.position = worldpos;        
        //Debug.Log(worldpos);
        
    }
    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = true;
        currentBallRigidBody = null;
        Invoke(nameof(DetachBall),delayDuration);

    }
    private void DetachBall()
    {
        currentBallSpringJoint.enabled=false;
        currentBallSpringJoint = null;
    }
}

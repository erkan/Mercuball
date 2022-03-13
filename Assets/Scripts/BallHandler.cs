using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float delayDuration; 
     [SerializeField] private float respawnDelay; 
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
         SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidBody == null) {return; }  //frame tamamlanınca rigidbody null ise bişey yapma.
        if(!Touchscreen.current.primaryTouch.press.isPressed) //Ekrana dokunulmayan frame ler için yapılacaklar. Örn. Top bırakıldığında buraya girer.
        {
            if (isDragging) // İlk dokunma sonrası top bırakıldığı anda isPressed false isDragging true olacak ve LaunchBall çağırılacak.
            {
                LaunchBall(); 
            }
            isDragging = false;
           // currentBallRigidBody.isKinematic = false;
            return;
        }
        isDragging=true;
        currentBallRigidBody.isKinematic = true;
        Vector2 touchpos = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldpos = mainCamera.ScreenToWorldPoint(touchpos); //Touch position ekran pixelinden oyun dünyası noktalarına (x,y,z) çevrildi. 
        currentBallRigidBody.position = worldpos;        
        //Debug.Log(worldpos);
        
    }
    private void LaunchBall() //Top çekildikten sonra bırakıldığında yapılacaklar
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;
        Invoke(nameof(DetachBall),delayDuration); //delay kadar süre fark oluşturarak sprintjointin hareketten sonra disable edilmesi sağlandı.  

    }
    private void DetachBall() //Topun bağlı olduğu SprintJoint i çözerek topu özgür bırakma
    {
        currentBallSpringJoint.enabled=false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnNewBall),respawnDelay);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab,pivot.position,Quaternion.identity);
        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSpringJoint.connectedBody = pivot;
    }
}

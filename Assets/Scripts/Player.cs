using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5f;
    public Transform aimTarget;
    bool hitting;

    public float force = 10f;
    public Transform ball;

    Animator animator;
     Vector3 aimTargetInitialPosition;

     ShotManager shotManager;
     Shot currentShot;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();//Getting the animation instance
        aimTargetInitialPosition = aimTarget.position; // Getting the initial position of the aim target to allow us to spawn back
        shotManager = GetComponent<ShotManager>(); // Shot Manager to alternate between different shot types
        currentShot = shotManager.topSpin;    // The current shot 
    }

    // Update is called once per frame
    void Update()
    {
        //Getting the axis for the player movement
      float h = Input.GetAxis("Horizontal");  
      float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true;
            currentShot = shotManager.topSpin;
        }
        else if(Input.GetKeyUp(KeyCode.F)){
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true;
            currentShot = shotManager.flat;
        }
        else if(Input.GetKeyUp(KeyCode.E)){
            hitting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            hitting = true;
            currentShot = shotManager.flatServe;
            GetComponent<BoxCollider>().enabled = false;
        }
        else if(Input.GetKeyUp(KeyCode.R)){
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f,1,0);
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0,currentShot.upForce,0);

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            hitting = true;
            currentShot = shotManager.kickServe;
            GetComponent<BoxCollider>().enabled = false;
        }
        else if(Input.GetKeyUp(KeyCode.T)){
            hitting = false;
            GetComponent<BoxCollider>().enabled = true;
            ball.transform.position = transform.position + new Vector3(0.2f,1,0);
            Vector3 dir = aimTarget.position - transform.position;
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0,currentShot.upForce,0);

        }


        if (hitting)
        {
            aimTarget.Translate(new Vector3(0,0,h)*speed* 2 *Time.deltaTime);
        }


      if((h!=0 || v!= 0) && !hitting)
      {
            // The player movement 
          transform.Translate(new Vector3(-v,0,h) * speed * Time.deltaTime);
      }
    }

    void OnTriggerEnter(Collider other){
        //Checking if the player and the ball touches each other to play the ball
        if(other.CompareTag("Ball")){
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0,currentShot.upForce,0);
            
            Vector3 ballDir = ball.transform.position - transform.position;
            if(ballDir.z >= 0){
            animator.Play("forehand");
            }else{
                animator.Play("Backhand");
            }
            aimTarget.position = aimTargetInitialPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;

    public float speed = 15.0f;

    private bool isTraveling;

    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 1000;

    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;

    private Vector2 currentSwipe;

    private Color solvecColor;

    private AudioSource audioSource;
    public AudioClip rollingsoundEffect;
    public AudioClip hitwallsoundEffect;



    // Start is called before the first frame update
    private void Start()
    {
        solvecColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solvecColor;

        audioSource = GetComponent<AudioSource>();
       
        audioSource.clip = hitwallsoundEffect;


    }

    private void FixedUpdate()
    {
        if (isTraveling) 
        {
            rb.velocity = speed * travelDirection;

        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;

        while (i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if (ground && !ground.isColored)//if on the gound and the ground color is not changed
            { 
                ground.ChangeColor(solvecColor);
            
            
            }

            i++;
        
        }

        if (nextCollisionPosition != Vector3.zero) 
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1) 
            { 
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;

            
            }
        
        }
        if (isTraveling) 
        {
            return;
        
        }

        if (Input.GetMouseButton(0)) 
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero) 
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
                if (currentSwipe.sqrMagnitude < minSwipeRecognition) 
                {
                    //do nothing
                    return;

                
                }
                currentSwipe.Normalize();//get the direction not the distance
                //to go up/down
                if (currentSwipe.x>-0.5f && currentSwipe.x< 0.5f) 
                {
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                
                }
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);

                }

             }

            swipePosLastFrame = swipePosCurrentFrame;

        }

        if (Input.GetMouseButtonUp(0)) 
        { 
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;

        
        
        }
        
    }
    private void SetDestination(Vector3 direction)
    { 
        travelDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f)) 
        { 
            nextCollisionPosition = hit.point;
        
        }
        isTraveling = true;

        audioSource.clip = rollingsoundEffect;
        audioSource.Play();

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play the hit sound when colliding with a wall
        audioSource.clip = hitwallsoundEffect;
        audioSource.Play();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

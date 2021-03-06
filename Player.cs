﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] public float horspeed;
    [SerializeField] public float horaccel;
    [SerializeField] public float jumpspeed;
    [SerializeField] public float gravity;
    //IMPORTANT NOTE: Set the width to something small, like 0.2.
    //Even if your character's sprite is 1 unit wide. The character will
    //be embedded somewhat in walls, this is fine, and will look fine once we switch to 2D sprites.
    [SerializeField] public float width;
    [SerializeField] public float height;
    [SerializeField] public int raycasts;
    //Tap Grav = Bonus gravity when travelling upwards while not holding the Up key. This causes you to jump less high when you are not holding the Up key.
    [SerializeField] public float tapGrav;
    //Should be slightly higher than 45, like 47, just for the sake of rounding errors.
    [SerializeField] public float maxSlope;
    float magicNumber = 0.03f;
    bool isJumping;
    public Vector3 speed;
	
    //override any of these functions using
    //protected override void Example () {
    //Debug.log("yo :)");
    //}
    //this way you can make your own class that inherits from player
    //and we can add overridable functions that most players will do the same way
    //e.g taking damage
    
    // Use this for initialization
	protected virtual void Start () {
        speed = Vector3.zero;
	
    }
    
    
    // Update is called once per frame
    protected virtual void Update()
    {

        //handle horizontal movement (left+right keys)
        //does not handle dashing yet, that might be a seperate function
        HorMove();
        //handles gravity, duh.
        Gravity();
        //Floor Collide is a boolean because it returns whether or not you can jump. Should I leave it this way? Y/N.
        
        bool canjump = FloorCollide();
        if (Input.GetButton("Jump") && canjump)
        {
           //Again, a single line of code for now, but you can put animation-y stuff here.
            Jump();
        }
        //Handles collision with the wall.
        WallCollide();
        //Handles collision with the ceiling.
        CeilCollide();
        transform.localPosition += speed * Time.deltaTime;
        

    }

    protected virtual void HorMove() {
        if (Input.GetAxis("Horizontal") < 0)
        {
            //move left
            speed.x += -horaccel * Time.deltaTime;
            this.gameObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            //move right
            speed.x += horaccel * Time.deltaTime;
            this.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            //slow down if not holding left or right
            if (speed.x > 0) { speed.x = Mathf.Max(speed.x - horaccel * Time.deltaTime, 0); }
            else { speed.x = Mathf.Min(speed.x + horaccel * Time.deltaTime, 0); }
        }

        speed.x = Mathf.Clamp(speed.x, -horspeed, horspeed);
    }

    protected virtual void Gravity() {
        //fall at a linear rate (you should know this but w/e I'm commenting EVERYTHING MUAHAHAHA)
        speed.y -= gravity * Time.deltaTime;
        //Fall faster if you're travelling upwards and not holding the up key. Allows for variable jumping, a la Mario.
        if (!Input.GetButton("Jump") && speed.y > 0) {
            speed.y -= tapGrav * Time.deltaTime;
        }
    }

    protected virtual bool FloorCollide() {
        bool canjump = false;
        bool floor = false;
        float oldyspeed = speed.y;
        RaycastHit hit;
        

            for (int i = 0; i <= 2; i++)
            {
                float xoff = (width / 2) * (i - 1);
                
                if (Physics.Raycast(transform.position + new Vector3(xoff, magicNumber, 0f), Vector3.down, out hit, Mathf.Abs(speed.y * Time.deltaTime) + magicNumber) && speed.y <= 0)
                {
                    //snap to floor, and enable jumping.
                    transform.position += new Vector3(0f, -hit.distance + magicNumber, 0f);
                    speed.y = 0f;
                    canjump = true;
                    isJumping = false;
                    floor = true;
                }
            }

            if (floor) {
                RaycastHit h1, h2;
                if (Physics.Raycast(transform.position + new Vector3(-width / 2, height - magicNumber, 0f), Vector3.down, out h1) &&
                    Physics.Raycast(transform.position + new Vector3(width / 2, height - magicNumber, 0f), Vector3.down, out h2))
                {

                        float slope = Vector2.Angle( h2.point - h1.point,Vector2.right);
                       // Debug.Log(h1.point.ToString() + " " + h2.point.ToString());
                       // Debug.Log(slope);
                        if (slope > maxSlope)
                        {
                            canjump = false;
                            if (h1.distance < h2.distance) { transform.Translate(-oldyspeed * Mathf.Atan(slope * Mathf.Deg2Rad) * Time.deltaTime + magicNumber, 0f, 0f); speed.x = Mathf.Max(0f, speed.x); speed.y = oldyspeed; }
                            else { transform.Translate(oldyspeed * Mathf.Atan(slope * Mathf.Deg2Rad) * Time.deltaTime - magicNumber, 0f, 0f); speed.x = Mathf.Min(0f, speed.x); speed.y = oldyspeed; }

                        }
                        

                }
                 
            
            }

        return canjump;
    }

    protected virtual void CeilCollide() {

        for (int i = 0; i <= 2; i++)
        {
            float xoff = (width / 2) * (i - 1);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(xoff, height-magicNumber, 0f), Vector3.up, out hit, Mathf.Abs(speed.y * Time.deltaTime) + magicNumber) && speed.y >= 0)
            {
                //snap to floor, and enable jumping.
                transform.position += new Vector3(0f, hit.distance - magicNumber, 0f);
                speed.y = 0f;
            }
        }

    }

    protected virtual void Jump() {
        speed.y = jumpspeed;
    }

   /* protected virtual void WallCollide() {
        
    }*/

    protected virtual void WallCollide() {

        float check = speed.x*Time.deltaTime;
        float widthCheck = width / 2 * Mathf.Sign(check);
        RaycastHit hit;
        if(Physics.Raycast(transform.position+new Vector3(-widthCheck,magicNumber,0f),Vector3.right*Mathf.Sign(check),out hit, Mathf.Abs(check)+Mathf.Abs(widthCheck*2))){

            RaycastHit secondHit;
            if (Physics.Raycast(transform.position + new Vector3(-widthCheck, Mathf.Abs(check) + magicNumber, 0f), Vector3.right * Mathf.Sign(check), out secondHit))
            {

                Vector2 p1 = hit.point;
                Vector2 p2 = secondHit.point;
                float slope = Vector2.Angle(p1 - p2, Vector2.left * Mathf.Sign(check));
                float slopeRatio = Mathf.Tan(slope * Mathf.Deg2Rad);

                if (slope <= maxSlope && slope > 0)
                {
                    transform.position += new Vector3(0f, Mathf.Abs(check) * slopeRatio, 0f);
                }
                
                    
                
            }
            else {
                transform.position+=new Vector3(0f,Mathf.Abs(check),0f);
            }

        }
        for (int i = 1; i <= raycasts; i++)
        {
            float yoff = height / raycasts * i;

            if (Physics.Raycast(transform.position + new Vector3(0f, yoff, 0f), Vector3.right * Mathf.Sign(speed.x), out hit, Mathf.Abs(speed.x * Time.deltaTime) + width / 2))
            {
                transform.position += new Vector3((hit.distance - width / 2) * Mathf.Sign(speed.x), 0f, 0f);
                speed.x = 0f;
            }
        }

    
    }



}

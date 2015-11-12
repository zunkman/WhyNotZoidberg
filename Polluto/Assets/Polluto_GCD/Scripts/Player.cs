using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    /* All of these variables need to be public */
    [SerializeField]    public float horspeed;
    [SerializeField]    public float horaccel;
    [SerializeField]    public float jumpspeed;
    [SerializeField]    public float gravity;
    [SerializeField]    public int playerNumber; // this is needed for multiple players
    //IMPORTANT NOTE: Set the width to something small, like 0.2.
    //Even if your character's sprite is 1 unit wide. The character will
    //be embedded somewhat in walls, this is fine, and will look fine once we switch to 2D sprites.
    [SerializeField]    public float width;
    [SerializeField]    public float height;
    [SerializeField]    public int raycasts;
    //Tap Grav = Bonus gravity when travelling upwards while not holding the Up key. This causes you to jump less high when you are not holding the Up key.
    [SerializeField]    public float tapGrav;
    //Should be slightly higher than 45, like 47, just for the sake of rounding errors.
    [SerializeField]    public float maxSlope;

    [SerializeField]    public Vector2 inputDirection = new Vector2(0, 0);//Map horizontal/vertical axis to a direction here

    [SerializeField]    protected float magicNumber = 0.05f;
    [SerializeField]    protected bool isJumping;
    [SerializeField]    public Vector3 speed, velocity; //velocity is speed * deltatime
    [SerializeField]    Vector3 groundNormal = new Vector3(0, 1, 0);
    [SerializeField]    Vector3 speedAdjust = new Vector3(0, 0, 0);

    // Edit by Joe
    [SerializeField] public float health, baseHealth;

    [SerializeField] public PlayerUI UIScript;
    
    //override any of these functions using
    //protected override void Example () {
    ////Debug.Log("yo :)");
    //}
    //this way you can make your own class that inherits from player
    //and we can add overridable functions that most players will do the same way
    //e.g taking damage

    // Use this for initialization
    protected virtual void Start()
    {
        speed = Vector3.zero;
        UIScript = FindObjectOfType<PlayerUI>();//check if this is null when accessing, some scenes may not have it
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        // Edited by Joe
        healthCheck();
        //-- Edited by CT, gets a direction from the axis. Can use for weapon direction.--//
        if (playerNumber == 1)inputDirection.x = Input.GetAxis("Horizontal");
        if (playerNumber == 2)inputDirection.x = Input.GetAxis("Horizontal 2");
        if (playerNumber == 1)inputDirection.y = Input.GetAxis("Vertical");
        if (playerNumber == 2)inputDirection.y = Input.GetAxis("Vertical 2");
        if(inputDirection != Vector2.zero) inputDirection.Normalize();
        //--  There may be some bugs with joysticks due to input settings.  --//

        //handle horizontal movement (left+right keys)
        //does not handle dashing yet, that might be a seperate function
        HorMove();
        //handles gravity, duh.
        Gravity();
        //Floor Collide is a boolean because it returns whether or not you can jump. Should I leave it this way? Y/N.

        velocity = speed * Time.deltaTime;
        bool canjump = FloorCollide();

        // Edited by Joe. Checks if we're player 1. If so, gets input from normal axis. Otherwise, player two axis.
        if (playerNumber == 1)
        {
            if (Input.GetButton("Jump") && canjump)
            {
                //Again, a single line of code for now, but you can put animation-y stuff here.
                Jump();
            }
        }

        if (playerNumber == 2)
        {
            if (Input.GetButton("Jump 2") && canjump)
            {
                //Again, a single line of code for now, but you can put animation-y stuff here.
                Jump();
            }
        }

        //Handles collision with the wall.
        //WallCollide();
        //Handles collision with the ceiling.
        //CeilCollide();

        transform.position += velocity;
        //transform.position += speed * Time.deltaTime;// + speedAdjust; speed += speedAdjust;
    }

    protected virtual void HorMove()
    {

        // Edited by Joe. Checks if we're player 1. If so, gets input from normal axis. Otherwise, player two axis.
        if (playerNumber == 1)
        {

            if (Input.GetAxis("Horizontal") < 0)
            {
                //move left
                speed.x += -horaccel * Time.deltaTime;
                this.gameObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            } else if (Input.GetAxis("Horizontal") > 0)
            {
                //move right
                speed.x += horaccel * Time.deltaTime;
                this.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else
            {
                //slow down if not holding left or right
                if (speed.x > 0) { speed.x = Mathf.Max(speed.x - horaccel * Time.deltaTime, 0); }
                else { speed.x = Mathf.Min(speed.x + horaccel * Time.deltaTime, 0); }
            }
        }

        if (playerNumber == 2)
        {
            if (Input.GetAxis("Horizontal 2") < 0)
            {
                //move left
                speed.x += -horaccel * Time.deltaTime;
                this.gameObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            } else if (Input.GetAxis("Horizontal 2") > 0)
            {
                //move right
                speed.x += horaccel * Time.deltaTime;
                this.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            } else
            {
                //slow down if not holding left or right
                if (speed.x > 0) { speed.x = Mathf.Max(speed.x - horaccel * Time.deltaTime, 0); }
                else { speed.x = Mathf.Min(speed.x + horaccel * Time.deltaTime, 0); }
            }
        }

        speed.x = Mathf.Clamp(speed.x, -horspeed, horspeed);
    }

    protected virtual void Gravity()
    {
        //fall at a linear rate (you should know this but w/e I'm commenting EVERYTHING MUAHAHAHA)
        speed.y -= gravity * Time.deltaTime;

        // Edited by Joe. Checks if we're player 1. If so, gets input from normal axis. Otherwise, player two axis.
        if (playerNumber == 1)
        {
            //Fall faster if you're travelling upwards and not holding the up key. Allows for variable jumping, a la Mario.
            if (!Input.GetButton("Jump") && speed.y > 0)
            {
                speed.y -= tapGrav * Time.deltaTime;
            }
        }

        if (playerNumber == 2)
        {
            //Fall faster if you're travelling upwards and not holding the up key. Allows for variable jumping, a la Mario.
            if (!Input.GetButton("Jump 2") && speed.y > 0)
            {
                speed.y -= tapGrav * Time.deltaTime;
            }
        }
    }

    protected virtual bool FloorCollide()
    {
        bool canjump = false;
        //bool floor = false;
        
        //float oldyspeed = speed.y;
        RaycastHit hit;

        //--Raycast all sample code below.--//
        RaycastHit[] hits;
        Vector3 tempPos = transform.position;
        Vector3 nxOffset = Vector3.zero;
        //right, left, up, down
		Vector3[] dirChk = {new Vector3(1f, 0, 0), new Vector3(-1f, 0, 0), new Vector3(0, 1f, 0), new Vector3(0, -1f, 0)};
        Vector3 rayOffset = new Vector3(0, 0, 0);
        Vector3 hitNormal = new Vector3(0, 0, 0);
        float[] dirOffsets = { 0, 0, 0, 0 };
        //Ray ray;
        bool wasHit; int hitCount = 0;
        speedAdjust = new Vector3(0, 0, 0);
        for(int i=0;i<4;i++){
            tempPos = transform.position;
            if(i == 3) groundNormal = new Vector3(0, 1, 0);
            for(int rc = -1; rc <= 1;rc++) {
                nxOffset = Vector3.zero;
			    //ray = new Ray(tempPos, dirChk[i]);
                float rayDist = Mathf.Abs(dirChk[i].x) * ((width * 0.5f) + Mathf.Abs(velocity.x)) 
                                + Mathf.Abs(dirChk[i].y) * ((height * 0.5f) + Mathf.Abs(velocity.y));
			    hits = null;
                rayOffset = new Vector3(dirChk[i][1]* ((width * 0.45f) * rc), dirChk[i][0]* ((height * 0.25f) * rc), 0);//offset on other axis
			    hits = Physics.RaycastAll (tempPos + rayOffset, dirChk[i], rayDist);//ray, 1.0f);//

			    Debug.DrawRay (tempPos + rayOffset, dirChk[i]*rayDist, Color.green, 1.0f);
			    wasHit = false; int j = 0;
			    while (j < hits.Length) {
				    hit = hits[j];
				    j++;
					
                    if(hit.collider.isTrigger == false) {
					    if(hit.collider.gameObject.tag.Contains("Ground") || hit.collider.gameObject.tag.Contains("Wall")
                            || (hit.collider.gameObject.tag.Contains("Enemy") && i != 2))
                        {
						    hitCount++;
                            //only consider the nearest obstruction, or the one that will push the player back the most
                            if(Mathf.Abs(rayDist - hit.distance) > nxOffset.magnitude) { 
                                wasHit = true;
                                nxOffset = dirChk[i] * 1.0f * (hit.distance - rayDist);
                                hitNormal = hit.normal;
                            }
					    }

                        //Debug.Log ("RCA hit:[" + hit.collider.gameObject.name + ", " + hit.collider.gameObject.tag + "],pos:["+ hit.transform.position + "],player:" + transform.position
                        //  + ",dist:" + hit.distance + ",offset:" + nxOffset + ",dirchk:" + dirChk[i]);
				    }// else { Debug.Log ("RCA hit:[" + hit.collider.gameObject.name + ", " + hit.collider.gameObject.tag + "],pos:["+ hit.transform.position + "] T"); }
			    }
			    if(wasHit == true) {
                    //transform.position = tempPos + nxOffset;//new Vector3(0f,  rayDist - hit.distance, 0f);
                    {
                        //hitNormal, speedAdjust, nxOffset, speed
                        float adjustment = 0;
                        adjustment = nxOffset.x;// * Mathf.Abs(hitNormal.x) + nxOffset.y * (1 - Mathf.Abs(hitNormal.y));
                        if(Mathf.Abs(adjustment) > Mathf.Abs(speedAdjust.x))speedAdjust.x = adjustment;
                        if(Mathf.Abs(adjustment) > dirOffsets[i]) dirOffsets[i] = nxOffset.magnitude;
                        adjustment = nxOffset.y;// * Mathf.Abs(hitNormal.y) - nxOffset.x * (1 - Mathf.Abs(hitNormal.x));
                        if(Mathf.Abs(adjustment) > Mathf.Abs(speedAdjust.y)) speedAdjust.y = adjustment;
                        if(Mathf.Abs(adjustment) > dirOffsets[i]) dirOffsets[i] = nxOffset.magnitude;
                        //if(nxOffset.y != 0) Debug.Log ("hitNormal:" + hitNormal.x + "," + hitNormal.y + ", nxOffset:" + nxOffset.x + "," + nxOffset.y + "speed:"+ speed.y);
                        //if(nxOffset.y != 0) Debug.Log ("hitNormal:" + hitNormal + ", nxOffset:" + nxOffset);// + "speed:"+ speed + "speedA:"+ speedAdjust);
                    }
                    if(i == 0 && hitNormal.y < 0.5) speed.x = Mathf.Min(0, speed.x);//Mathf.Min(0f, speed.x);
                    if(i == 1 && hitNormal.y < 0.5) speed.x = Mathf.Max(0, speed.x);//Mathf.Max(0f, speed.x);
                    if(i == 2) speed.y = Mathf.Min(0, speed.y);//Mathf.Min(0f, speed.y);
                    if(i == 3) {
                        if(hitNormal.y < 1.0f) groundNormal = hitNormal;
                        speed.y = Mathf.Max(0, speed.y);//Mathf.Max(0f, speed.y);
                        canjump = true;
                        isJumping = false;
                        //floor = true;
                    }
			    }
            }
		}
        {
            speedAdjust = new Vector3(dirOffsets[1]-dirOffsets[0], dirOffsets[3]-dirOffsets[2], 0);
            //Debug.Log(dirOffsets[3] + " " + dirOffsets[2] + ".." + groundNormal);
            if(groundNormal.y >= 0.5 && groundNormal.y < 1.0f && velocity.x != 0.0f && speed.x != 0.0f) {
                float slope = Vector2.Angle(new Vector2(0.0f, 1.0f), new Vector2(groundNormal.x, groundNormal.y));
                {
                    //canjump = false;
                    //Debug.Log(slope + ", arc:" + new Vector3(Mathf.Atan((90 - slope) * Mathf.Deg2Rad)*Mathf.Sign(velocity.x), Mathf.Atan(slope * Mathf.Deg2Rad), 0.0f) + ", stuff:" + groundNormal);
                    float hillScale = velocity.magnitude - (velocity + speedAdjust).magnitude;//velocity.magnitude;//speedAdjust.magnitude
                    if(hillScale > 0) speedAdjust += new Vector3(Mathf.Atan((90 - slope) * Mathf.Deg2Rad)*Mathf.Sign(velocity.x), Mathf.Atan(slope * Mathf.Deg2Rad), 0.0f) * hillScale;
                }
            }
        }
        if(hitCount > 0 && speedAdjust.magnitude > 0.0f) {
            velocity += speedAdjust;
            //Debug.Log ("vel:" + velocity);
        }
        return canjump;
    }

    protected virtual void CeilCollide()
    {

        for (int i = 0; i <= 2; i++)
        {
            //check for ceil from various points //--modified to match changes to Player --> Origin is now at the center of the player--//
            float xoff = (width / 2) * (i - 1);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(xoff, 0f, 0f), Vector3.up, out hit, (height / 2f) + Mathf.Abs(speed.y * Time.deltaTime) + magicNumber) && speed.y >= 0 && hit.transform.gameObject.tag == "Ground")
            {
                //snap to floor, and enable jumping.
                transform.position += new Vector3(0f, hit.distance - (height / 2f), 0f);
                speed.y = 0f;
            }
        }

    }

    protected virtual void Jump()
    {
        speed.y = jumpspeed;
    }

    protected virtual void WallCollide()
    {

        float check = speed.x * Time.deltaTime;
        float widthCheck = width / 2 * Mathf.Sign(check);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(-widthCheck, 0f, 0f), Vector3.right * Mathf.Sign(check), out hit, Mathf.Abs(check) + Mathf.Abs(widthCheck * 2)) && hit.transform.gameObject.tag == "Ground")
        {
            //cast a second ray a bit above the first to check for a slope on the wall
            RaycastHit secondHit;
            if (Physics.Raycast(transform.position + new Vector3(-widthCheck, (height / 4), 0f), Vector3.right * Mathf.Sign(check), out secondHit) && secondHit.transform.gameObject.tag == "Ground")
            {

                Vector2 p1 = hit.point;
                Vector2 p2 = secondHit.point;
                float slope = Vector2.Angle(p1 - p2, Vector2.left * Mathf.Sign(check));
                float slopeRatio = Mathf.Tan(slope * Mathf.Deg2Rad);
                //raise the y position apparently
                if (slope <= maxSlope && slope > 0)
                {
                    transform.position += new Vector3(0f, Mathf.Abs(check) * slopeRatio, 0f);
                }
            } else
            {
                //slope check raycast failed, assume no slope?
                transform.position += new Vector3(0f, Mathf.Abs(check), 0f);
            }
        }
        for (int i = 1; i <= raycasts; i++)
        {
            float yoff = (height / raycasts * i) - (height / 2f);

            if (Physics.Raycast(transform.position + new Vector3(0f, yoff, 0f), Vector3.right * Mathf.Sign(speed.x), out hit, Mathf.Abs(speed.x * Time.deltaTime) + (width / 2f)) && hit.transform.gameObject.tag == "Ground")
            {
                transform.position += new Vector3((hit.distance - width / 2) * Mathf.Sign(speed.x), 0f, 0f);
                speed.x = 0f;
            }
        }


    }


    void healthCheck ()
    {
        GameObject handler = GameObject.FindGameObjectWithTag("gameHandler");
        if (health <= 0 && handler != null)
        {
            handler.GetComponentInChildren<GameHandler>().respawnPlayer(this.gameObject);
        }
    }
}
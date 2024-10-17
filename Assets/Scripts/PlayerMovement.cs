using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Looking at the enemy
    
    public GameObject[] enemylookedat;

    public float lookSpeed = 200f;
    public bool isAutoLook = false;

    //distance of player and enemy 
    public float enemyDistance;

    //MovingStuff
    public Joystick MovingJoystick;

    float moveSpeed = 4.5f;

    public Transform MovingArrow;

    Animator anim;

    //AimingStuff
    public Transform ShootingArrow;

    public Joystick ShootingJoystick;

    //ShootingStuff
    ShootingScript shootingscript;

    //Grenade Stuff
    public Vector3 GrenadeTargetPos;

    //This is set to true if the player is
    //holding a grenade and taps the grenade button
    public bool isThrowing = false;

    //If the player moves the shooting Joystick
    //after setting the isThrowing to true, the player has then
    //aimed the grenade
    bool aimedGrenade = false;

    //The line renderer component to show the trajectory
    [SerializeField]
    LineRenderer TrajectoryLine;

    //The timer we use to generate the TrajectoryLine
    [SerializeField]
    float TrajectoryTimer = 0;


    // Start is called before the first frame update
    void Start()
    {   
    
        if(PlayerPrefs.GetInt("auto")==1){
            isAutoLook = true;
        }
        else{
            isAutoLook = false;
        }
        
        TrajectoryLine = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        shootingscript = GetComponent<ShootingScript>();
    }

    // Update is called once per frame
    void Update()
    {
     
        enemylookedat = GameObject.FindGameObjectsWithTag("Enemy");


      

        //set the rotation of the moving arrow to match that of the movingJoystick
        MovingArrow.rotation = MovingArrow.rotation = Quaternion.Euler
        (0f, Mathf.Atan2(MovingJoystick.Horizontal, MovingJoystick.Vertical) * Mathf.Rad2Deg, 0f);

        //If the gamer moves the joystick
        if (Mathf.Abs(MovingJoystick.Horizontal) > .1f || Mathf.Abs(MovingJoystick.Vertical) > .1f)
        {
            //check to see if the movingArrow indicator is off
            //if so turn it on.
            if (!MovingArrow.gameObject.activeSelf)
                MovingArrow.gameObject.SetActive(true);

            //force the player to look in the direction of the movement joysticks axis'
            transform.LookAt(new Vector3(MovingJoystick.Horizontal + transform.position.x,
            0, MovingJoystick.Vertical + transform.position.z));

            //fixes a rotation issues
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            //Move the player in the direction of the movementJoystick
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

            //Set the animator parameter isRunning to true
            //We will use this to play the running animation
            anim.SetBool("isRunning", true);

        }
        else
        {
            //Set the animator parameter isRunning to false
            //We will use this to play the idle animation
            anim.SetBool("isRunning", false);

            //check to see if the movingArrow indicator is on
            //if so turn it off
            if (MovingArrow.gameObject.activeSelf)
                MovingArrow.gameObject.SetActive(false);
        }

        if (isAutoLook)
        {
            AutoShoot();
        }
        

    }

    private void LateUpdate()
    {
        Vector3 startPos;

        //If the gamer moves the Shootingjoystick
        if (Mathf.Abs(ShootingJoystick.Horizontal) > .1f || Mathf.Abs(ShootingJoystick.Vertical) > .1f)
        {
            //if we are not throwing then we can continue with the shooting stuff
            if (!isThrowing)
            {
                //use the reference to the shootingScript component attached to the 
                //player, to fire a bullet
                shootingscript.FireBullets();

                //check to see if the movingArrow indicator is off
                //if so turn it on
                if (!ShootingArrow.gameObject.activeSelf)
                    ShootingArrow.gameObject.SetActive(true);
            }
            else
            {
                trajector();

                aimedGrenade = true;

                //use the axis of the shootingJoystick to
                //move the target when aiming the grenade throw
                GrenadeTargetPos = new Vector3((ShootingJoystick.Horizontal * 5f) + transform.position.x, 0,
                (ShootingJoystick.Vertical * 5f) + transform.position.z);

            }

            //force the player to look in the direction of the shooting joysticks axis'
            transform.LookAt(new Vector3(ShootingJoystick.Horizontal + transform.position.x,
            0, ShootingJoystick.Vertical + transform.position.z));

            //fixes a rotation issues
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }

        //if we have moved the shootingJoystick while the player isThrowing
        else if (aimedGrenade)
        {
         //   TrajectoryLine.enabled = false;

            //reset the aimed grenade bool
            aimedGrenade = false;
            //turn the isThrowing bool to false
            isThrowing = false;

            //make an array of vector3 to hold the trajectory path
            //Then copy the path to the new array
            Vector3[] Path = new Vector3[TrajectoryLine.positionCount];
        //    TrajectoryLine.GetPositions(Path);

            //set the start of the trajectory line slightly above the player
            startPos = new Vector3(transform.localPosition.x,
            transform.localPosition.y + 1, transform.localPosition.z);

            //Use the shootingScript to throw the grenade 
            //Passing the start and target position of the trajectory
           // shootingscript.ThrowGrenade(startPos,GrenadeTargetPos);

        }
        else
        {
            //check to see if the movingArrow indicator is on
            //if so turn it off
            if (ShootingArrow.gameObject.activeSelf)
                ShootingArrow.gameObject.SetActive(false);
        }
        
    }


    void trajector()
    {
        //set the start of the trajectory line slightly above the player
        Vector3 startPos = new Vector3(transform.localPosition.x,
        transform.localPosition.y+1, transform.localPosition.z);

        //gather the distance between the player and the target
        float targetDistance = Vector3.Distance(startPos, GrenadeTargetPos);

        //If the target distance is move than half a unit
        //Creating an arc from position 0 to position 0 throws a line renderer error
        if(targetDistance >=.5f)
        {
            //Make sure the trajectory line is on
            TrajectoryLine.enabled = true;
            //Reset the timer every time we recreate the line
            TrajectoryTimer = 0;

            TrajectoryLine.positionCount = 15;
            //The first position in the trajectory is set to the start position
            TrajectoryLine.SetPosition(0, startPos);

            for (int i = 0; i < 15; i++)
            {
                //gradually increase the trajectoryTimer to simulate time 
                //change when generating the Trajectory Line

                TrajectoryTimer += .05f;
            //    TrajectoryLine.SetPosition(i, TrajectoryArcCalc.TrajectoryArc(startPos, GrenadeTargetPos, TrajectoryTimer));
                //save this and send it to the grenade as the travel path
               
            }
        }
        //if the target distance is less than half a unit
        //turn off the line renderer and return with no action
        else
        {
            TrajectoryLine.enabled = false;
            return;
        }
    }

public void change_mode(){
    if(isAutoLook)
    {
        isAutoLook=false;
        PlayerPrefs.SetInt("auto", 0);
        PlayerPrefs.Save();
    }
    else{isAutoLook=true;
      PlayerPrefs.SetInt("auto", 1);
        PlayerPrefs.Save();
    }
}
    public void AutoShoot()
    {

        //isAutoLook = !isAutoLook;
        if(isAutoLook)
        {

           
            //force the player to look in the direction of the enemy
            for(int i = 0; i < enemylookedat.Length; i++)
            {
                if(enemylookedat[i] != null)
                {
                    transform.LookAt(new Vector3(enemylookedat[i].transform.position.x,
                    0, enemylookedat[i].transform.position.z));

                    //fixes a rotation issues
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                    enemyDistance = Vector3.Distance(transform.position, enemylookedat[i].transform.position);
                    if (enemyDistance < 15f)
                    {
                        shootingscript.FireBullets();
                    }
                    
                }
            }
          
        }
    }

}

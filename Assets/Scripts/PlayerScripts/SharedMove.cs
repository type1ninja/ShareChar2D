using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SharedMove : MonoBehaviour {

    CharacterController charControl;

    //Constants
    //The cap for an individual player's contribution to max speed, in m/s
    static float PLAYER_SPEED_CAP = 10f;
    //The acceleration an individual player contributes in m/s^2
    static float PLAYER_ACCELERATION = 0.2f;
    //The multiplier on player acceleration when airborne
    static float AIR_ACCELERATION_MOD = 0.7f;
    //Acceleration due to gravity
    static float GRAVITY = -1f;
    //Speed gain 
    static float JUMP_SPEED = 15f;
    //How fast the player slows down when going faster than they should (idk the unit of measure)
    static float SLOW_RATE = 0.05f;
    //How fast the player can be going down before the slow down
    static float TERMINAL_VELOCITY = -70f;

    //Variables
    //Whether each player has jumped since last touching the ground
    bool hasJumped1 = false;
    bool hasJumped2 = false;
    bool hasJumped3 = false;
    bool hasJumped4 = false;
    //The sum of the player input values
    float playerInput;
    //Move values, reset every frame
    float horizMove;
    float verticalMove;
    //Reset every frame, depends on the net number of players pointing in each direction
    float speedCap;

    public void Start()
    {
        charControl = GetComponent<CharacterController>();
    }

    public void Update()
    {
        playerInput = Input.GetAxis("Horizontal1") + Input.GetAxis("Horizontal2") + Input.GetAxis("Horizontal3") + Input.GetAxis("Horizontal4");

        speedCap = PLAYER_SPEED_CAP * playerInput;

        horizMove = charControl.velocity.x;
        if (charControl.isGrounded)
        {
            horizMove += playerInput * PLAYER_ACCELERATION;
        } else
        {
            horizMove += playerInput * PLAYER_ACCELERATION * AIR_ACCELERATION_MOD;
        }

        if (Mathf.Abs(horizMove) > Mathf.Abs(speedCap) && speedCap != 0 && Mathf.Sign(speedCap) == Mathf.Sign(horizMove))
        {
            horizMove = Mathf.Lerp(horizMove, speedCap, SLOW_RATE);
        }

        if (playerInput == 0 && charControl.isGrounded)
        {
            horizMove = Mathf.Lerp(horizMove, 0, SLOW_RATE);
        }

        verticalMove = charControl.velocity.y + GRAVITY;
        verticalMove = CheckJumps(verticalMove);

        if (verticalMove < TERMINAL_VELOCITY)
        {
            verticalMove = Mathf.Lerp(verticalMove, TERMINAL_VELOCITY, SLOW_RATE);
        }

        charControl.Move(new Vector3(horizMove * Time.deltaTime, verticalMove * Time.deltaTime, 0));

        if (charControl.isGrounded)
        {
            hasJumped1 = false;
            hasJumped2 = false;
            hasJumped3 = false;
            hasJumped4 = false;
        }
    }

    //Called every frame to add jumpSpeeds as necessary
    private float CheckJumps(float vertical_move)
    {
        if (Input.GetButtonDown("Jump1") && !hasJumped1)
        {
            hasJumped1 = true;

            if (vertical_move < 0)
            {
                vertical_move = 0;
            }

            vertical_move += JUMP_SPEED;
        }

        if (Input.GetButtonDown("Jump2") && !hasJumped2)
        {
            hasJumped2 = true;
            if (vertical_move < 0)
            {
                vertical_move = 0;
            }
            vertical_move += JUMP_SPEED;
        }

        if (Input.GetButtonDown("Jump3") && !hasJumped3)
        {
            hasJumped3 = true;
            if (vertical_move < 0)
            {
                vertical_move = 0;
            }
            vertical_move += JUMP_SPEED;
        }

        if (Input.GetButtonDown("Jump4") && !hasJumped4)
        {
            hasJumped4 = true;
            if (vertical_move < 0)
            {
                vertical_move = 0;
            }
            vertical_move += JUMP_SPEED;
        }

        return vertical_move;
    }

    //Called every frame to figure out how many players are pushing in each direction
    private int NetMoveNormal()
    {
        int netMoveNormal = 0;
        if (Input.GetAxis("Horizontal1") > 0)
        {
            netMoveNormal += 1;
        } else if (Input.GetAxis("Horizontal1") < 0) 
        {
            netMoveNormal += -1;
        }
        if (Input.GetAxis("Horizontal2") > 0)
        {
            netMoveNormal += 1;
        }
        else if (Input.GetAxis("Horizontal2") < 0) 
        {
            netMoveNormal += -1;
        }
        if (Input.GetAxis("Horizontal3") > 0)
        {
            netMoveNormal += 1;
        }
        else if (Input.GetAxis("Horizontal3") < 0) 
        {
            netMoveNormal += -1;
        }
        if (Input.GetAxis("Horizontal4") > 0)
        {
            netMoveNormal += 1;
        }
        else if (Input.GetAxis("Horizontal4") < 0) 
        {
            netMoveNormal += -1;
        }

        return netMoveNormal;
    }
}
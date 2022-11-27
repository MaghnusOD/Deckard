using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If wanting to add player ragdoll death, need to make seperate body with ragdoll, destroy player body and instantiate ragdoll clone on death
/// 
/// with current controller using rigidbody, it is impossible to use a ragdoll on death otherwise
/// </summary>



public class rigidbodyMovement : MonoBehaviour
{
    private Vector3 player_movement;
    Vector3 move_vec;

    public Rigidbody player_body;
    public CapsuleCollider c_collider;

    public float speed;
    public float sprint;
    public float jump;

    bool is_crouching = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player_movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        MovePlayer();
    }

    private void MovePlayer()
    {    
        if (Input.GetKey(KeyCode.C))
        {
            PlayerCrouch();
        }
        else
        {
            PlayerStand();
        }

        //  player speeds up to sprint speed
        //  MUST BE PLACED AFTER CROUCH CHECK TO WORK
        if (Input.GetKey(KeyCode.LeftShift) && is_crouching == false)
        {
            move_vec = transform.TransformDirection(player_movement) * sprint;
        }



        player_body.velocity = new Vector3(move_vec.x, player_body.velocity.y, move_vec.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player_body.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }

    void PlayerCrouch()
    {
        is_crouching = true;

        //  linearally interpolates between current position and crouching position 
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GameObject.Find("player/camera/CameraPositions/crouch").transform.position, Time.deltaTime * 5f);// GameObject.Find("player/camera/CameraPositions/crouch").transform.position;

        //  changes height of character by half and moves center of character collider from 1 to 0.5
        c_collider.height = 1f;
        c_collider.center = new Vector3(0f, 0.5f, 0f);



        //  movement speed reduced to 75 percent of normal walking speed
        move_vec = transform.TransformDirection(player_movement) * speed / 2;


        //  linearally interpolates between current position and crouching position
        GameObject.Find("player/weapon").transform.position = Vector3.Lerp(GameObject.Find("player/weapon").transform.position, GameObject.Find("player/camera/CameraPositions/crouch").transform.position, Time.deltaTime * 5f);
    }

    void PlayerStand()
    {
        is_crouching = false;

        //  linearally interpolates between current position and standing position 
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GameObject.Find("player/camera/CameraPositions/stand").transform.position, Time.deltaTime * 4f);

        //  changes height of character to 2 and moves center of character collider from 0.5 to 1
        c_collider.height = 2f;
        c_collider.center = new Vector3(0f, 1f, 0f);

        //  movement speed at 100 percent
        move_vec = transform.TransformDirection(player_movement) * speed;

        //  linearally interpolates between current position and standing position
        GameObject.Find("player/weapon").transform.position = Vector3.Lerp(GameObject.Find("player/weapon").transform.position, GameObject.Find("player/camera/CameraPositions/stand").transform.position, Time.deltaTime * 5f);

    }
}
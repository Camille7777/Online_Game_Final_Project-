using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public class PlayerBehaviour : MonoBehaviourPunCallbacks
{
    private float health;
    public float startHealth = 3;
    public float currentscore;
    public bool reach_destination;
    public float destination_sequence;
    bool isfalling = false;
    public Rigidbody rb;
    float currentplayertimer = 0f;
    float timeout = 0f;
    public bool ChoosenTrap = false;

    public static PlayerBehaviour instance;

    public ExitGames.Client.Photon.Hashtable _customproperties= new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        if (instance != null)
        {
           // Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = startHealth;
        currentscore = 0;   // or System assingned score;
        destination_sequence = 0; // 0 represent havent reach, large than one means the sequence
        reach_destination = false;

        // initialise key for each players
        _customproperties.Add("Health",health);
        _customproperties.Add("Score", currentscore);
        _customproperties.Add("Des_sequence", destination_sequence);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customproperties);
        Debug.Log(" first nitialise score success");

    }

    // Update is called once per frame
    void Update()
    {

        currentplayertimer += 1 * Time.deltaTime;

        
    }

    private void FixedUpdate()
    {
         float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;

        RaycastHit ray;

         bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, out ray, DisstanceToTheGround + 0.1f);

         Debug.DrawRay(transform.position, Vector3.down * DisstanceToTheGround , Color.red);
         //Debug.Log(DisstanceToTheGround);


         if (IsGrounded)
         {
           //  Debug.Log("is gorunded");
             isfalling = false;
             // Anim.SetBool("in_the_air", false);
         }
         else
         {
           //  Debug.Log("is not gorunded");
             isfalling = true;
             // Anim.SetBool("in_the_air", true);
             //rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
         }


         //condition 1 , if is falling and distance to the ground >100, then means below have nothings die

         if (ray.distance > 20)
         {
             photonView.RPC("TakeDamage",
                             RpcTarget.AllBuffered,
                            1f);


         }
          

        //condition 2, get squeeze down by other player and is falling== true, then also die  
    }

    [PunRPC]
    public void TakeDamage(float _damage, Player target)
    {
        health = (float)target.CustomProperties["Health"];
        health -= _damage;
       



        if (health <= 0f)
        {
            //Die
            Die();
            health = 0;
        }

        else
        {

            // Respawn();
            _customproperties["Health"] = health;
           target.SetCustomProperties(_customproperties);
        }

        
    }

    public void Respawn()
    {
        Debug.Log("get hurt and wait for going b to starting point");
           
        transform.position = GameManager.instance.BattleStateStartingPoint.position;
    }

    [PunRPC]
    public void AddScore(float _score, Player Targetplayerscore )
    {
        currentscore = (float)Targetplayerscore.CustomProperties["Score"];
        currentscore += _score;
        _customproperties["Score"] = currentscore;
        Targetplayerscore.SetCustomProperties(_customproperties);
       
       // Debug.Log(currentscore);


    }

    [PunRPC]
    public void SetreachDestination(bool yes, float sequence_number, Player Targetplayerscore)
    {
        reach_destination = yes;
        _customproperties["Des_sequence"] = sequence_number;
        Targetplayerscore.SetCustomProperties(_customproperties);

    }

    void Die()
    {
        if (photonView.IsMine)
        {
            // PixelGunGameManager.instance.LeaveRoom();
            Debug.Log("you re dead");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("collided" + other.gameObject.tag +"and with name" +other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "BigFan":
                UpdateScore(other.gameObject, ScoreManager.instance.bigfanScore);
                //photonView.RPC("UpdateScore", RpcTarget.AllBuffered, other.gameObject, ScoreManager.instance.bigfanScore);//1
               
                break;
                

            case "Gun":
                UpdateScore(other.gameObject, ScoreManager.instance.gunScore);
                
                break;

            case "TrapForChoose":

                if(!ChoosenTrap)
                {
                    GameManager.instance.assignned_child_trap = (other.gameObject.GetPhotonView().ViewID % GameManager.instance.traps_available_for_player.Length) - 1;
                    //transfer the ownership to me first, because only owner of object can destroy
                    TransferOwnershipRequest(other.gameObject.GetPhotonView(), PhotonNetwork.LocalPlayer);

                    //if owenership is mine then destroy
                    if (other.gameObject.GetPhotonView().IsMine)
                    {
                        PhotonNetwork.Destroy(other.gameObject.GetPhotonView());
                       //  Debug.Log("transfer sucess,can destroy");
                    }
                    ChoosenTrap = true;
                }
               


                break;
        }
       
    }

    private void TransferOwnershipRequest(PhotonView targetView,Player requestingPlayer)
    {
        targetView.TransferOwnership(requestingPlayer);

    }

    private void OnCollisionExit(Collision collision)
    {
       if( collision.gameObject.tag=="Player")
        {
            
            

            if(isfalling&& rb.velocity.y < -1.5f)
            {

                Debug.Log("fall damage");
                if(photonView.IsMine)
                {
                    photonView.RPC("TakeDamage",
                                 RpcTarget.AllBuffered,
                                1f,PhotonNetwork.LocalPlayer);
                }
                
                //timer start 

                /*
                timeout = currentplayertimer + 2f;
                if (currentplayertimer >= timeout && rb.velocity.y < -5f)
                {

                    Debug.Log("fall damage");
                    photonView.RPC("TakeDamage",
                                     RpcTarget.AllBuffered,
                                    1f);
                }
                */


            }
        }

       
    }


    void UpdateScore(GameObject trap, float trapType_score)
    {
        if (trap.gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
           
            Debug.Log("my trap");

            

        }
        else if (trap.gameObject.GetComponentInParent<PhotonView>().Owner.ActorNumber!=photonView.Owner.ActorNumber)
        {
            Debug.Log("is not mine");
            
            
            if (trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"] != null)
            {
                currentscore = (float)trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"];
                AddScore(trapType_score, trap.gameObject.GetComponentInParent<PhotonView>().Owner);
                //  Debug.Log(" second set score success");
            }
            else
            {
                Debug.LogError("Score key not initialise yet");
            }


        }
    }

    public override void OnRoomUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnRoomUpdate(targetPlayer, changedProps);
        
      
        if(targetPlayer!=null )
        {
            Debug.Log(targetPlayer + "changed" + changedProps);
            
            ScoreManager.instance.displayRanking();

        }
       
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerBehaviour : MonoBehaviourPunCallbacks
{
    private float health;
    public float startHealth = 1;
    public float currentscore;
    public bool reach_destination;
    public float destination_sequence;
    


    public ExitGames.Client.Photon.Hashtable _customproperties= new ExitGames.Client.Photon.Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        currentscore = 0;   // or System assingned score;
        destination_sequence = 0; // 0 represent havent reach, large than one means the sequence
        reach_destination = false;

        // initialise key for each players
        _customproperties.Add("Score", currentscore);
        _customproperties.Add("Des_sequence", destination_sequence);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customproperties);
        Debug.Log(" first nitialise score success");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        Debug.Log(health);

       
        if (health <= 0f)
        {
            //Die
            Die();
        }
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

        Debug.Log("collided" + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "BigFan":
                UpdateScore(other.gameObject, ScoreManager.instance.bigfanScore);
                //photonView.RPC("UpdateScore", RpcTarget.AllBuffered, other.gameObject, ScoreManager.instance.bigfanScore);//1
               
                break;
                

            case "Gun":
                UpdateScore(other.gameObject, ScoreManager.instance.gunScore);
                
                break;
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


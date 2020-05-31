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


    public ExitGames.Client.Photon.Hashtable _customscore = new ExitGames.Client.Photon.Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        currentscore = 0;   // or System assingned score;
        destination_sequence = 0; // 0 represent havent reach, large than one means the sequence
        reach_destination = false;
        _customscore.Add("Score", currentscore);
        _customscore.Add("Des_sequence", destination_sequence);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customscore);
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
        _customscore["Score"] = currentscore;
        Targetplayerscore.SetCustomProperties(_customscore);
        //Targetplayerscore.CustomProperties = _customscore;
       // Debug.Log(currentscore);


    }

    [PunRPC]
    public void SetreachDestination(bool yes, float sequence_number, Player Targetplayerscore)
    {
        reach_destination = yes;
        _customscore["Des_sequence"] = sequence_number;
        Targetplayerscore.SetCustomProperties(_customscore);

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
       

        switch (other.gameObject.tag)
        {
            case "BigFan":
                UpdateScore(other.gameObject, ScoreManager.instance.bigfanScore);
                Debug.Log("collided");
                break;

            case "Gun":
                UpdateScore(other.gameObject, ScoreManager.instance.gunScore);
                Debug.Log("collided");
                break;
        }
    }

    void UpdateScore(GameObject trap, float trapType_score)
    {
        if (!trap.gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            Debug.Log("score updated");




           
            if(trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"] != null)
            {
                currentscore = (float)trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"];
               AddScore(trapType_score, trap.gameObject.GetComponentInParent<PhotonView>().Owner);
              //  Debug.Log(" second set score success");
            }
            else
            {
                Debug.LogError("Score key not initialise yet");
            }
           


           
           // Debug.Log(trap.gameObject.GetComponent<PhotonView>().Owner.CustomProperties["Score"]);

        }
        else
        {
            Debug.Log("my trap");

            //if (trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"] != null)
            //{
            //    currentscore = (float)trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"];
            //    AddScore(trapType_score, trap.gameObject.GetComponentInParent<PhotonView>().Owner);
            //    Debug.Log(" second set score success");
            //}
            //else
            //{
            //    Debug.LogError("Score key not initialise yet");
            //}





        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log("call back score done");
        ScoreManager.instance.displayRanking();
    }
}


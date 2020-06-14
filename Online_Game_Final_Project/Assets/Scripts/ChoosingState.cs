using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;


public class ChoosingState : MonoBehaviourPunCallbacks, Istate
{
    public Transform ChooseState_Traps_spawnLocation;
    public Transform ChooseState_Players_spawnLocation;
    public GameObject[] traps_available_for_player;
    public float ChooseStateTimeLimit ;
    public Action CallBack;
    

    private float xdistance = 5f;
    private float timer;
    private GameObject[] remaining_traps;

    public void onStateEnter()
    {
        timer = 0;
        PrepareBattleAssets(RandomPlayer());

        //if master then spawn
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for(int i=0;i<traps_available_for_player.Length;i++)
            {
                PhotonNetwork.InstantiateSceneObject(traps_available_for_player[i].name,
                   new Vector3(ChooseState_Traps_spawnLocation.position.x+(xdistance*i), ChooseState_Traps_spawnLocation.position.y,ChooseState_Traps_spawnLocation.position.z),
                    Quaternion.identity);
            }

           
        }

       

    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;

        if(timer>=ChooseStateTimeLimit-2)
        {
            remaining_traps = GameObject.FindGameObjectsWithTag("TrapForChoose");

            //if player did not trigger with any, assign him random ramaining trap
            if (!PlayerBehaviour.instance.ChoosenTrap)
            {
                //for each gameobject with nametag trapassets, randomly pick one view id, 
                //destroy and assign it to gamemanager.instance.assigned prefab
                Debug.Log("assign random trap");

                int random_index = UnityEngine.Random.Range(0, remaining_traps.Length);
                if (remaining_traps == null)
                {
                    random_index = 1;
                }
                


                GameManager.instance.assignned_child_trap = (remaining_traps[random_index].GetPhotonView().ViewID % GameManager.instance.traps_available_for_player.Length) - 1;
                PlayerBehaviour.instance.ChoosenTrap = true;
                Debug.Log("assign random trap"+ GameManager.instance.assignned_child_trap);
            }

        }


        if (timer > ChooseStateTimeLimit)
        {
           
            

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {

                //halt the move
                if (Player.GetComponent<PlayerMovementController>() != null)
                {
                    // Player.GetComponent<PlayerMovementController>().speed = 0;
                    Player.GetComponent<PlayerMovementController>().enabled = false;
                }


                //stop the camera
                Player.transform.GetChild(0).gameObject.SetActive(false);
            }

            //destroy all the prefab with tag.... //allow the delay of 1s
            if(PhotonNetwork.IsMasterClient)
            {
                foreach (GameObject remainingtraps in remaining_traps)
                {
                    PhotonNetwork.Destroy(remainingtraps.GetPhotonView());
                }

                   
            }
            CallBack();
        }

        else
        {
            //if player trigger with certain trap, record the view id down and spawn it later
           
            //make sure the player marked as choosen to avoid 2 choose
            
        //find this part in player behaviour.cs

           
       
        }

    }

    public void onFixedUpdate()
    {
        
    }
    
    public void onStateExit()
    {
       
    }

    private void PrepareBattleAssets(int whichprefab)
    {


        //if never spawn any player
        if (!GameManager.instance.player_spawned)
        {

           if (GameManager.instance.playerselection != null)
          {
              GameManager.instance.Playerslist.Add(PhotonNetwork.Instantiate(GameManager.instance.playerselection[1].name, ChooseState_Players_spawnLocation.position, Quaternion.identity));

            }
           // //player spawned
           GameManager.instance.player_spawned = true;
        }
        else
        { //else no need spawn alrd but need to regain control

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                //revoke the move

                // Player.GetComponent<PlayerMovementController>().speed=8;
                Player.GetComponent<PlayerMovementController>().enabled = true;

                //reset the reach destination
                Player.GetComponent<PlayerBehaviour>().reach_destination = false;
                //enable the camera
                Player.transform.GetChild(0).gameObject.SetActive(true);
                //mark player havent choose trap
                PlayerBehaviour.instance.ChoosenTrap = false;
            }


        }







    }

    public int RandomPlayer()
    {
        int Randomindex = UnityEngine.Random.Range(0, GameManager.instance.traps.Length);
        // Debug.Log(Randomindex);
        return Randomindex;
    }


}

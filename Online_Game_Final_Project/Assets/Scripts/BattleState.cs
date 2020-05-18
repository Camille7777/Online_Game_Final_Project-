using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class BattleState : MonoBehaviourPunCallbacks,Istate
{
    public Transform BattleStateStartingPoint;
    public Transform BattleStateDestination;
    public float BattleState_Timer;
    public float BattleState_TimeLimit;
    public GameObject RealplayerPrefab;
    
    public Action WinCallBack;
    public Action LostCallBack;
    private bool player_spawned = false;



    public void onStateEnter()
    {
        BattleState_Timer = 0;

      //prepare this state assets
        PrepareBattleAssets();

        Debug.Log(BattleStateDestination.position);

    }

    private void PrepareBattleAssets()
    {

        
        //if never spawn any player
        if (!player_spawned)
        {
            GameManager.instance.Playerslist.Add(PhotonNetwork.Instantiate(RealplayerPrefab.name, BattleStateStartingPoint.position, Quaternion.identity));
            // spawn the new proper players
            //for (int i = 0; i < 4; i++)
            //{
            //    Playerslist.Add(GameObject.Instantiate(RealplayerPrefab, BattleStateStartingPoint.position, Quaternion.identity) as GameObject);
            //    Playerslist[i].name = "player_" + i;
            //    // Playerslist[0].setActive(false); //This sets the first healthpack inactive. And works on either arrays or List.
            //}

            //player spawned
            player_spawned = true;
        }
        else
        { //else no need spawn alrd but need to regain control

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                //revoke the move

                Player.GetComponent<PlayerMovementController>().speed = 8;

                //enable the camera
                Player.transform.GetChild(0).gameObject.SetActive(true);
            }

           
        }

       





    }

    public void onStateUpdate()
    {
        BattleState_Timer += Time.deltaTime * 1;
        //if within timer
       
        // if time limit exceed
        if (BattleState_Timer>BattleState_TimeLimit)
        {




            foreach (GameObject Player in GameManager.instance.Playerslist)
            {

                //halt the move

                Player.GetComponent<PlayerMovementController>().speed = 0;

                //stop the camera
                Player.transform.GetChild(0).gameObject.SetActive(false);
            }


                //exit this state and prepare go to lose state




                LostCallBack();

        }
        else
        {
            //else 


            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                //if one of players reach destination
                if ((Player.transform.position - BattleStateDestination.position).magnitude < 2f)
                {

                    Debug.Log("reach destination");

                    //reset the timer to 0 so that system would not think is lose.
                    BattleState_Timer = 0;

                    //wait for others


                    // when everyone reached
                    

                    WinCallBack();
                }

                
            }

        }




    }

    public void onFixedUpdate()
    {

    }

    public void onStateExit()
    {
       // CallBack();

    }




}

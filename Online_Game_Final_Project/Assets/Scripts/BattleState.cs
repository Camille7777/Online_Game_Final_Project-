using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class BattleState : MonoBehaviourPunCallbacks,Istate
{
    public Transform BattleStateStartingPoint;
    public Transform BattleStateDestination;
    public float BattleState_Timer;
    public float BattleState_TimeLimit;
    public GameObject RealplayerPrefab;
    private const byte RECORD_SEQUENCE_EVENT=0;

    
    public Action WinCallBack;
    public Action LostCallBack;
    private bool player_spawned = false;

    public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();

    public void onStateEnter()
    {
        BattleState_Timer = 0;

      //prepare this state assets
        PrepareBattleAssets(RandomPlayer());

       // Debug.Log(BattleStateDestination.position);
       if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
        {
            _customroomproperties.Add("Global_destination_Var", 0);
            Debug.Log("room properties created");
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);

        }

    }

    private void PrepareBattleAssets(int whichprefab)
    {

        
        //if never spawn any player
        if (!player_spawned)
        {

            // spawn the new proper players
            //for (int i = 0; i < 4; i++)
            //{
            //    Playerslist.Add(GameObject.Instantiate(RealplayerPrefab, BattleStateStartingPoint.position, Quaternion.identity) as GameObject);

            GameManager.instance.Playerslist.Add(PhotonNetwork.Instantiate(GameManager.instance.playerselection[whichprefab].name, BattleStateStartingPoint.position, Quaternion.identity));
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

               // Player.GetComponent<PlayerMovementController>().speed=8;
                Player.GetComponent<PlayerMovementController>().enabled = true;

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
                if (Player.GetComponent<PlayerMovementController>()!=null)
                {
                    // Player.GetComponent<PlayerMovementController>().speed = 0;
                    Player.GetComponent<PlayerMovementController>().enabled = false;
                }
               

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
                    //record player reached destination
                    if(Player.GetComponent<PlayerBehaviour>().reach_destination==false && Player.GetComponent<PhotonView>().IsMine)
                    {  
                       
                        Player.GetComponent<PhotonView>().RPC("AddScore", RpcTarget.AllBuffered, ScoreManager.instance.destination_objective_point,Player.GetComponent<PhotonView>().Owner);



                        int current_sequence=(int) PhotonNetwork.CurrentRoom.CustomProperties["Global_destination_Var"];
                        current_sequence += 1;
                        _customroomproperties["Global_destination_Var"] = current_sequence;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
                        Debug.Log(current_sequence);

                       

                        Player.GetComponent<PhotonView>().RPC("SetreachDestination", RpcTarget.AllBuffered, true ,(float) current_sequence ,Player.GetComponent<PhotonView>().Owner);
                        
                        
                    }


                    //reset the timer to 0 so that system would not think is lose.
                   // BattleState_Timer = 0;

                    //wait for others


                    // when everyone reached
                    

                   // WinCallBack();
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

    public int RandomPlayer()
    {
        int Randomindex = UnityEngine.Random.Range(0, GameManager.instance.traps.Length);
        Debug.Log(Randomindex);
        return Randomindex;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private  void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
       if(obj.Code==RECORD_SEQUENCE_EVENT)
        {
            object[] data = (object[])obj.CustomData;
            float id = (float)data[0];
            
        }
    }
}
// pass the id to the rank
// object[] id_data = new object[] { id };
// PhotonNetwork.RaiseEvent(RECORD_SEQUENCE_EVENT, id_data, RaiseEventOptions.Default, SendOptions.SendReliable);
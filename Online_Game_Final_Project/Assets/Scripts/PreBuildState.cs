﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PreBuildState : MonoBehaviourPunCallbacks,Istate 
{
   public Transform spawnLocation;
    public GameObject playerTransparentPrefab;
    public GameObject[] traps;
    public Action CallBack;
    private GameObject Objplayer_1;
    private GameObject Objplayer_2;
    private GameObject Objplayer_3;
    private GameObject Objplayer_4;
    public GameObject[] Generatedtraps;
    public float buildStateTimeLimit ;
    private float timer;
   
    List<GameObject> objectPlayers = new List<GameObject>();
    List<GameObject> objectPlayers_trap_belongings = new List<GameObject>();
    private PhotonView pv;

    public void onStateEnter()
    {
        //loadIn all prefabs

        //for (int i = 0; i < 4; i++)
        // {
        //     traps[i] = Resources.Load("prefab_" + (i + 1)) as GameObject;
        //}
        // playerTransparentPrefab = Resources.Load("Invi_player") as GameObject;

        ////spwan four player prefabs to spawnlocation
         InstantiateAssets();
        //SpawnPlayer();
       
        
        //setting up players


        ////assign random player to random traps(prefabs);

        timer = 0;



    }
    
    private void InstantiateAssets()
    {
        if (playerTransparentPrefab != null)
        {


            //Objplayer_1 = PhotonNetwork.Instantiate(playerTransparentPrefab.name, spawnLocation.position, Quaternion.identity);
            //Objplayer_1.name = "Objplayer_1";
            //GameObject.Instantiate(traps[0], Objplayer_1.transform.position, Quaternion.identity, Objplayer_1.transform);

            objectPlayers.Add(PhotonNetwork.Instantiate(playerTransparentPrefab.name, spawnLocation.position, Quaternion.identity));
            objectPlayers_trap_belongings.Add(PhotonNetwork.Instantiate(traps[0].name, spawnLocation.position, Quaternion.identity));

            pv = objectPlayers_trap_belongings[objectPlayers_trap_belongings.Count - 1].GetComponent<PhotonView>();

            if(pv)
            {
                Debug.Log("getted");
            }
            else
            {
                Debug.Log(" no get");
            }
        


            if (pv.IsMine)
            {
                objectPlayers_trap_belongings[objectPlayers_trap_belongings.Count - 1].transform.SetParent(objectPlayers[objectPlayers.Count - 1].transform);
            }
           

            //for (int i=0;i<objectPlayers.Count;i++)
            //{
            //    objectPlayers[i].name= "Objplayer_"+i;
            //    PhotonView photonView = objectPlayers[i].GetComponent<PhotonView>();
            //    if (photonView.IsMine)
            //    {
            //        photonView.RPC("RPC_InstantiateCloth", RpcTarget.All, photonView.ViewID);
            //    }

              

            //}

           

        }

        //Objplayer_1 = GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        //Objplayer_1.name = "Objplayer_1";
        //GameObject trap_1=GameObject.Instantiate(traps[0], Objplayer_1.transform.position, Quaternion.identity,Objplayer_1.transform);
       
        //Objplayer_2 =GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        //Objplayer_2.name = "Objplayer_2";
        // GameObject trap_2 = GameObject.Instantiate(traps[1], Objplayer_2.transform.position, Quaternion.identity, Objplayer_2.transform);

        //Objplayer_3 = GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        //Objplayer_3.name = "Objplayer_3";
        //GameObject trap_3 = GameObject.Instantiate(traps[2], Objplayer_3.transform.position, Quaternion.identity, Objplayer_3.transform);

        //Objplayer_4 = GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        //Objplayer_4.name = "Objplayer_4";
        //GameObject trap_4 = GameObject.Instantiate(traps[3], Objplayer_4.transform.position, Quaternion.identity, Objplayer_4.transform);







    }

    //[PunRPC]
    //private void RPC_InstantiateCloth(int parentViewId)
    //{
    //    GameObject parentObject = PhotonView.Find(parentViewId).gameObject;
    //    GameObject clone = Instantiate(traps[0], spawnLocation.position, Quaternion.identity);
    //    clone.transform.parent = parentObject.transform;
    //}

    //public void SpawnPlayer()
    //{
    //    byte evCode = 123; // Custom Event 1: Used as "MoveUnitsToTargetPosition" event

    //    GameObject player =GameObject.Instantiate(playerTransparentPrefab);
        
    //    GameObject.Instantiate(traps[0], player.transform.position, Quaternion.identity, player.transform);

    //    objectPlayers.Add(player);
    //  PhotonView photonView = player.GetComponent<PhotonView>();

    //    if (PhotonNetwork.AllocateViewID(photonView))
    //    {
    //        object[] data = new object[]
    //        {
    //        player.transform.position, player.transform.rotation, photonView.ViewID
    //        };

    //        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
    //        {
    //            Receivers = ReceiverGroup.All,
    //            CachingOption = EventCaching.AddToRoomCache
    //        };

    //        SendOptions sendOptions = new SendOptions
    //        {
    //            Reliability = true
    //        };

    //        PhotonNetwork.RaiseEvent(evCode, data, raiseEventOptions, sendOptions);
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to allocate a ViewId.");

    //        Destroy(player);
    //    }
    //}

    //public void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == 123)
    //    {
    //        object[] data = (object[])photonEvent.CustomData;

    //        GameObject player = (GameObject)Instantiate(playerTransparentPrefab, (Vector3)data[0], (Quaternion)data[1]);
    //        PhotonView photonView = player.GetComponent<PhotonView>();
    //        photonView.ViewID = (int)data[2];
    //    }
    //}




    public void onFixedUpdate()
    {
       
    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;
        //if whithin 30s 
        //allow them move to certain location
        if (timer > buildStateTimeLimit)
        {

            


            // instruct all the object player to stop moving
            foreach (GameObject objectPly in objectPlayers)
            {
                //halt the move

                objectPly.GetComponent<ObjMovementController>().speed = 0;

                //stop the camera
                objectPly.transform.GetChild(0).gameObject.SetActive(false);
            }

            //if state finnished 
            //callback
            CallBack();

        }


       
    }

    public void onStateExit()
    {

       
      

    }

    //[RPC]
    //void InstantiateFlame()
    //{
    //    fire = (GameObject)Instantiate(flameBaby, transform.position + transform.forward, transform.rotation);

    //    //this is the part that is giving me trouble.
    //    fire.transform.parent = myGuy.transform;
    //}

}

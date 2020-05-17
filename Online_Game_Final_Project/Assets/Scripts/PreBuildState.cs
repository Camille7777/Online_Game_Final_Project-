using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PreBuildState : Istate
{
   public Transform spawnLocation;
    public GameObject playerTransparentPrefab;
    public GameObject[] traps;
    public Action CallBack;
    private GameObject Objplayer_1;
    private GameObject Objplayer_2;
    private GameObject Objplayer_3;
    private GameObject Objplayer_4;
    public float buildStateTimeLimit ;
    private float timer;
    private GameObject[] objectPlayers;

    public void onStateEnter()
    {
        Debug.Log("build state is ready");

        //loadIn all prefabs

        //for (int i = 0; i < 4; i++)
        // {
        //     traps[i] = Resources.Load("prefab_" + (i + 1)) as GameObject;
        //}
        // playerTransparentPrefab = Resources.Load("Invi_player") as GameObject;

        ////spwan four player prefabs to spawnlocation
        InstantiateAssets();

        //setting up players


        ////assign random player to random traps(prefabs);

        timer = 0;



    }

    private void InstantiateAssets()
    {


       Objplayer_1= GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        Objplayer_1.name = "Objplayer_1";
        GameObject trap_1=GameObject.Instantiate(traps[0], Objplayer_1.transform.position, Quaternion.identity,Objplayer_1.transform);
       
        Objplayer_2 =GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        Objplayer_2.name = "Objplayer_2";
         GameObject trap_2 = GameObject.Instantiate(traps[1], Objplayer_2.transform.position, Quaternion.identity, Objplayer_2.transform);

        Objplayer_3 = GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        Objplayer_3.name = "Objplayer_3";
        GameObject trap_3 = GameObject.Instantiate(traps[2], Objplayer_3.transform.position, Quaternion.identity, Objplayer_3.transform);

        Objplayer_4 = GameObject.Instantiate(playerTransparentPrefab, spawnLocation.position, Quaternion.identity);
        Objplayer_4.name = "Objplayer_4";
        GameObject trap_4 = GameObject.Instantiate(traps[3], Objplayer_4.transform.position, Quaternion.identity, Objplayer_4.transform);







    }

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

            objectPlayers = GameObject.FindGameObjectsWithTag("Obj_Player");


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

}

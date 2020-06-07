using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class ResultState : MonoBehaviourPunCallbacks, Istate
{
    public float resultStateTimeLimit;
    private float timer;
    public Action CallBack;

    public void onStateEnter()
    {
        timer = 0;
        /* UI operation
        foreach (Player p in PhotonNetwork.PlayerList)
        {
           string Score=(string) p.CustomProperties["Score"];
        }
        */

        ScoreManager.instance.UICanvasForEveryRoundResult.SetActive(true);
    }

    public void onStateExit()
    {
       
    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;

        if (timer > resultStateTimeLimit)
        {

             ScoreManager.instance.UICanvasForEveryRoundResult.SetActive(false);

            CallBack();
            //go back to game manager
        }
    }

   public void onFixedUpdate()
    {

    }


}

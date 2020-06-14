using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ScoreManager : MonoBehaviourPunCallbacks
{
   
    public float destination_objective_point=100;
    public float bigfanScore=10;
    public float gunScore=20;
    public float current_sequence = 0;
    public GameObject UICanvasForLastResult;
    public GameObject UICanvasForEveryRoundResult;
    object hashTableValue;
    public Player[] pList;


   // public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
   

    public void displayRanking()
    {

        //Debug.Log(p.NickName + "Score :" + p.CustomProperties["Score"]);
        //Debug.Log(p.NickName + "Ranking   :" + p.CustomProperties["Des_sequence"]);
        sortScore();
       
    }

    public void sortScore()
    {
        Debug.Log("sorting score...");
       Player[] pList = PhotonNetwork.PlayerList;
        System.Array.Sort(pList, delegate (Player p1, Player p2) {
            string p1score = p1.CustomProperties["Score"].ToString();
            string p2score = p2.CustomProperties["Score"].ToString();
            return p1score.CompareTo(p2score); });
        //reverse the lost for descending order
        System.Array.Reverse(pList);
        // call the display resut
        displayresult(pList);
    }

    public void displayresult(Player[] pList)
    {
        Debug.Log(" after sort");
        foreach (Player p in pList)
        {
            // this is the current score for each player
            Debug.Log(p.NickName + "Score :" + p.CustomProperties["Score"]);
            // this is the current ranking for this round
            Debug.Log(p.NickName + "Reach Destination Ranking   :" + p.CustomProperties["Des_sequence"]);
        }

        /**************************** for UI *******************************************/
        /*
         * 
         
        //eg.
        // this Plist is alrd sorted, every time the score changed, the highest score will be at pList[0]
    
        for (int i=0; i<pList.Length;i++)
        {
            //assign the NickName to the Display name UI text
            
            **********************************************
            UI.displayname[i].text=pList[i].NickName
            ***********************************************
            

          ** if you want to diaplay name for each player, just use PhotonNetwork.LocalPlayer.NickName
       
        }
        */

        // pList[].NickName;
        // string score=(string)pList[0].CustomProperties["Score"];
        //string round = (string)PhotonNetwork.CurrentRoom.CustomProperties["Round"];

        //***************************************
        // NEW!!!!!!!!!!!!!!!! health UI
        //health=PhotonNetwork.LocalPlayer.CustomProperties["Health"];

    }



    public float OnDestinationReachedScore(int sequence)

    {

        //if x=1 get 100; x=2 get 50; x=3 get=33
        float Score = 1 / sequence * 100;
        Debug.Log("rankscoreadded");
        return  Score; 
    }
}

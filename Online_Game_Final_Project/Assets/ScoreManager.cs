using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviourPunCallbacks
{
   
    public float destination_objective_point=100;
    public float bigfanScore=10;
    public float gunScore=20;
    public float current_sequence = 0;
    

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
      foreach (Player p in PhotonNetwork.PlayerList)
        {
            Debug.Log(p.NickName+"Score :"+p.CustomProperties["Score"]);
            Debug.Log(p.NickName + "Ranking   :" + p.CustomProperties["Des_sequence"]);
        }
    }
}

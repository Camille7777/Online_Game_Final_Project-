using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharSelectState : MonoBehaviour
{
    public double CharSelectState_Timer;
    public double CharSelectState_TimeLimit = 5f;
    public double startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = PhotonNetwork.Time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onStateEnter()
    {
        CharSelectState_Timer = 0;

        //prepare this state assets
        Debug.Log(PlayerPrefs.GetInt("MyCharacter"));

    }

    public void onStateUpdate()
    {
        CharSelectState_Timer = PhotonNetwork.Time - startTime;
        Debug.Log(CharSelectState_Timer);
        if (CharSelectState_Timer > CharSelectState_TimeLimit)
        {
            PhotonNetwork.LoadLevel("Level_1");
        }
    }
}
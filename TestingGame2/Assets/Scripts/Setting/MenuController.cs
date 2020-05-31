using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public float CharSelectState_Timer = 20f;

    public GameObject[] button;
    public Text[] charName;

    //private PhotonView PV;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void start()
    {
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (CharSelectState_Timer >= 0.0f)
            {
                CharSelectState_Timer -= Time.deltaTime;
                Debug.Log(CharSelectState_Timer);
            }
            else if (CharSelectState_Timer < 0.0f)
            {
                this.GetComponent<PhotonView>().RPC("StartTheGame", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void StartTheGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            //button[whichCharacter].SetActive(false);

            this.GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.All, whichCharacter);
        }
    }

    
    [PunRPC]
    void DisableButton(int whichCharacter)
    {
        charName[whichCharacter].text = PhotonNetwork.NickName;
        button[whichCharacter].SetActive(false);
    }


    
}

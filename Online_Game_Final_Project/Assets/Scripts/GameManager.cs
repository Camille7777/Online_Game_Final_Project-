using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private StateMachine stateMachine;
    private PreBuildState build = new PreBuildState();
    private BattleState battle = new BattleState();

    #region Assets for PrebuildScene
    public Transform spawnLocation;
    public GameObject playerTransparentPrefab;
    public GameObject[] traps;
    public float buildStateTimeLimit = 20f;
    #endregion

    #region Assets for Battle
    public Transform BattleStateStartingPoint;
    public Transform BattleStateDestination;
    public GameObject RealplayerPrefab;
    public float BattleState_TimerLimit=30f;
    #endregion


    private void Awake()
    {
       
        

    }

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 1; i < 5; i++)
        //{
        //    traps[i] = Resources.Load("prefab_"+i) as GameObject;
        //}
        //playerTransparentPrefab = Resources.Load("Invi_player") as GameObject;

        stateMachine = GetComponent<StateMachine>();
        GotoPrebuild();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.UpdateState();
    }

    private void GotoPrebuild()
    {
      
        stateMachine.changeState(Build());
    }

    

    public void buildCallBack()
    {
        Debug.Log("build state is end");
        //gotobattle state
        stateMachine.changeState(Battle());
        Debug.Log("batttle state is start");
    }

    public void BattleWinCallBack()
    {
        Debug.Log("Battle state is end");
        Debug.Log("Battle state is win");

        // stateMachine.changeState(Build());
        //go to win state

    }

    public void BattleLostCallBack()
    {
        Debug.Log("Battle state is end");
        Debug.Log("Battle state is lost");

        //go to lost state first

        //then only go build again
      
        stateMachine.changeState(Build());
    }



    private PreBuildState Build()
    {
        build.spawnLocation = spawnLocation;
        build.playerTransparentPrefab=playerTransparentPrefab;
        build.traps=traps;
        build.CallBack = buildCallBack;
        build.buildStateTimeLimit = buildStateTimeLimit;

        return build;
    }

    private BattleState Battle()
    {
         battle.BattleStateStartingPoint=BattleStateStartingPoint;
        battle.BattleStateDestination= BattleStateDestination;
         battle.RealplayerPrefab=RealplayerPrefab;
        battle.BattleState_TimeLimit= BattleState_TimerLimit;
        battle.WinCallBack = BattleWinCallBack;
        battle.LostCallBack = BattleLostCallBack;

        return battle;
    }


}

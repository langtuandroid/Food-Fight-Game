using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleHandler : MonoBehaviour
{
    private static BattleHandler instance;

    public static BattleHandler GetInstance()
    {
        return instance;
    }

    [SerializeField] Transform[] leftPositions;
    [SerializeField] Transform[] rightPositions;
    [SerializeField] private Transform pfCharacter;
    [SerializeField] GameObject[] playerDeck;
    [SerializeField] GameObject[] enemyDeck;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] GameObject SelectAttackTypeWin_pf;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    private List<CharacterBattle> playerCharacterBattles;
    private List<CharacterBattle> enemyCharacterBattles;
    private CharacterBattle activeCharacterBattle;
    private CharacterBattle activeEnemyCharacterBattle;
    private State state;
    private int playerBattleIndex = 0;
    private int enemyBattleIndex = 0;
    private AttackTypes currentAttackType;
    private bool isPlayerTurn = true;
    private SelectAttackTypeWin selectAttackTypeWin;

    private enum State
    {
        WaitingForPlayer,
        Busy
    }
    public enum AttackTypes
    {
        Basic, Special, ultimate
    }

    private void Awake()
    {
        instance = this;
        currentAttackType = AttackTypes.Basic;
    }

    private void Start()
    {
        playerCharacterBattles  = new List<CharacterBattle>();
        enemyCharacterBattles   = new List<CharacterBattle>();

        playerCharacterBattles.Add( SpawnCharacter(playerDeck[0], true, leftPositions[0].position, 0) );
        playerCharacterBattles.Add( SpawnCharacter(playerDeck[1], true, leftPositions[1].position, 1) );
        playerCharacterBattles.Add( SpawnCharacter(playerDeck[2], true, leftPositions[2].position, 2) );

        enemyCharacterBattles = enemyAI.InitAI(playerCharacterBattles);

        enemyAI.AttackDone += ChooseNextActiveCharacter;
        for (int i = 0; i < playerCharacterBattles.Count; i++)
        {
            playerCharacterBattles[i].CharacterSelected += CharacterSelected;
            enemyCharacterBattles[i].CharacterSelected += CharacterSelected;
        }
        if(activeCharacterBattle) SetActiveCharacterBattle(activeCharacterBattle);
        state = State.WaitingForPlayer;

    }

    private void Update()
    {
        if(state == State.WaitingForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Busy;
                //playerCharacterBattles[playerBattleIndex].Attack(enemyCharacterBattles[enemyBattleIndex], ()=> {
                activeCharacterBattle.Attack(activeEnemyCharacterBattle, ()=> {
                    // Called when the attack is finished.                    
                    ChooseNextActiveCharacter();
                });
            }
        }
        
    }

    public CharacterBattle SpawnCharacter(GameObject character, bool isPlayerTeam, Vector3 pos, int posNum)
    {
        Vector3 position = pos;
        GameObject characterTransform = Instantiate(character, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam, posNum);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle charaterBattle)
    {

        if (charaterBattle.IsPlayerTeam())
        {
            if (activeCharacterBattle !=null)
            {
                activeCharacterBattle.HideSelectionCircle();
            }
            activeCharacterBattle = charaterBattle;
            activeCharacterBattle.ShowSelectionCircle();
            ShowSelectAttackTypeWin();
        }     
        else
        {
            if (activeEnemyCharacterBattle != null)
            {
                activeEnemyCharacterBattle.HideSelectionCircle();
            }
            activeEnemyCharacterBattle = charaterBattle;
            activeEnemyCharacterBattle.ShowSelectionCircle();
        }

    }

    // Keith at some point change this to UpdateTurn.
    private void ChooseNextActiveCharacter()
    {
        if (TestBattleOver())
        {
            return;
        }

        ClearActiveCharacters();
        if (isPlayerTurn)
        {
            //    SetActiveCharacterBattle(activeEnemyCharacterBattle);
                state = State.Busy;
            //    activeEnemyCharacterBattle.Attack(activeCharacterBattle, () => {
            //        ChooseNextActiveCharacter();
            //    });
            
            enemyAI.UpdateTurn();
        }
        else
        {
            //SetActiveCharacterBattle(activeCharacterBattle);
            state = State.WaitingForPlayer;
        }
        isPlayerTurn = !isPlayerTurn;
    }

    private void ClearActiveCharacters()
    {
        for (int i = 0; i < playerCharacterBattles.Count; i++)
        {
            playerCharacterBattles[i].HideSelectionCircle();
        }
        for (int i = 0; i < enemyCharacterBattles.Count; i++)
        {
            enemyCharacterBattles[i].HideSelectionCircle();
        }
        
    }

    private void ShowSelectAttackTypeWin()
    {
        GameObject go = Instantiate(SelectAttackTypeWin_pf);
        selectAttackTypeWin = go.GetComponent<SelectAttackTypeWin>();
        selectAttackTypeWin.OnSelectionMade += AttackTypeSelected;
        selectAttackTypeWin.OnWinClosed += SelectAttackTypeWinClosed;
    }

    private void SelectAttackTypeWinClosed()
    {
        selectAttackTypeWin.OnWinClosed -= SelectAttackTypeWinClosed;
        selectAttackTypeWin = null;
    }


    private bool TestBattleOver()
    {
        //if (playerCharacterBattles[playerBattleIndex].IsDead())
        //{
        //    print("player dead "+ playerBattleIndex);
        //    // Player dead, enemy wins
        //    CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");
        //    return true;
        //}
        //if (enemyCharacterBattles[enemyBattleIndex].IsDead())
        //{
        //    print("enemy dead " + enemyBattleIndex);
        //    // Enemy dead, player wins
        //    CodeMonkey.CMDebug.TextPopupMouse("Player Wins!");
        //    return true;
        //}
        return false;
    }

    private void AttackTypeSelected(AttackTypes attackType)
    {
        print("AttackTypeSelected " + attackType);
        if(attackType == AttackTypes.Special && CheckSpecialAttackReady())
        {
            currentAttackType = attackType;
        }

        if (attackType == AttackTypes.ultimate && CheckUltimateAttackReady())
        {
            currentAttackType = attackType;
        }

        if (attackType == AttackTypes.Basic)
        {
            currentAttackType = attackType;
            //selectAttackTypeWin
        }
    } //

    private bool CheckSpecialAttackReady()
    {
        int minAttackLevel = 5;
        if (activeCharacterBattle.currentLevel >= minAttackLevel)
        {
            print("Set attack to special!!!");
            selectAttackTypeWin.UdateDisplay("Set Attack to Special");
            return true;
        }
        else
        {

            selectAttackTypeWin.UdateDisplay("Character needs to be at least level "+minAttackLevel+" for this attack. "+ activeCharacterBattle.currentLevel);
            return false;
        }
    }

    private bool CheckUltimateAttackReady()
    {
        int minAttackLevel = 10;
        if (activeCharacterBattle.currentLevel >= minAttackLevel)
        {
            selectAttackTypeWin.UdateDisplay("Set Attack to Ultimate");
            return true;
        }
        else
        {
            selectAttackTypeWin.UdateDisplay("Character needs to be at least level " + minAttackLevel + " for this attack.");
            return false;
        }
    }



    public void CharacterSelected(CharacterBattle characterBattle)
    {
        print("CharacterSelected "+characterBattle.IsPlayerTeam());
        //if (characterBattle.IsPlayerTeam())
        //{
            SetActiveCharacterBattle(characterBattle);
        //}
    }

    public List<CharacterBattle> GetEnemyTeam()
    {
        return enemyCharacterBattles;
    }
}

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
    [SerializeField] Transform[] LeftDefeatPositions;
    [SerializeField] Transform[] rightDefeatPositions;
    [SerializeField] private Transform pfCharacter;
    [SerializeField] GameObject[] playerDeck;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] GameObject SelectAttackTypeWin_pf;
    [SerializeField] GameObject BattleOverWin_pf;

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

    private int specialAttacksLeft = 5;
    private int ultimateAttacksLeft = 1;
    private SpecialPowerManager specialPowerManager;

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
        specialPowerManager = GetComponent<SpecialPowerManager>();
    }

    private void Start()
    {
        playerCharacterBattles  = new List<CharacterBattle>();
        enemyCharacterBattles   = new List<CharacterBattle>();

        // Keith create arrays of defeatOffScreenPositions and pass it to spawnCharacter

        playerCharacterBattles.Add( SpawnCharacter(playerDeck[0], true, leftPositions[0].position, LeftDefeatPositions[0].position, 0) );
        playerCharacterBattles.Add( SpawnCharacter(playerDeck[1], true, leftPositions[1].position, LeftDefeatPositions[1].position, 1) );
        playerCharacterBattles.Add( SpawnCharacter(playerDeck[2], true, leftPositions[2].position, LeftDefeatPositions[2].position, 2) );

        enemyCharacterBattles = enemyAI.InitAI(playerCharacterBattles);

        enemyAI.AttackDone += ChooseNextActiveCharacter;
        for (int i = 0; i < playerCharacterBattles.Count; i++)
        {
            playerCharacterBattles[i].CharacterSelected += CharacterSelected;
        }
        print("characterbattles " + enemyCharacterBattles.Count);
        for (int j = 0; j < enemyCharacterBattles.Count; j++)
        {
            enemyCharacterBattles[j].CharacterSelected += CharacterSelected;
            enemyCharacterBattles[j].CharacterDoubleClicked += ShowTargetInfo;
        }
        if (activeCharacterBattle) SetActiveCharacterBattle(activeCharacterBattle);
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
                activeCharacterBattle.Attack(currentAttackType, activeEnemyCharacterBattle, 45f,()=> {
                    // Called when the attack is finished.                    
                    ChooseNextActiveCharacter();
                });
            }
        }        
    }

    public void Attack(AttackTypes attackType)
    {
        TurnOffOtherEnemyColliders(activeEnemyCharacterBattle);
        float damage = activeCharacterBattle.GetBaseDamage();
        /*
         *Attacks have a chance to Miss. Ultimate attacks become more accurate the more turns that have taken 
         * place in the battle. This is to prevent players from using Ultimate attacks when they start every 
         * battle and abusing the system.
         */

        if (attackType == AttackTypes.Special && CheckSpecialAttackReady())
        {
            //Special attacks are specific to type (Birds, Cats, Dogs, etc)
            //Special attacks will be limited to five (5) uses per battle and will do 1.5x damage
            //when used on an Ooonimal that is weak against the attacking Ooonimal. When used
            //against an Ooonimal that is strong against the attacker, the attack does 0.5x
            //damage. If the Ooonimal being attacked is neutral to the attacker, the attack does
            //the standard 1.0x damage. 
            currentAttackType = attackType;
            damage = CalculateDamage(activeCharacterBattle, activeEnemyCharacterBattle);
        }

        if (attackType == AttackTypes.ultimate && CheckUltimateAttackReady())
        {
            //Ultimate attacks can only be used one time (1) per Ooonimal per battle. All Ultimate
            //attacks have “splash damage”. When hit, the Ooonimal targeted is damaged 2.0x what a
            //Basic attack would do and the surrounding opposing Ooonimals (if there are any) are
            //hurt at a rate of 1.0x like a Basic attack.
            currentAttackType = attackType;
            damage *= 2f; // figure out an equation 

        }

        if (attackType == AttackTypes.Basic)
        {
            //A Basic attack 
            currentAttackType = attackType;
        }
        
        if (state == State.WaitingForPlayer)
        {            
            state = State.Busy;
            activeCharacterBattle.Attack(currentAttackType, activeEnemyCharacterBattle, damage, () => {
                // Called when the attack is finished.
                TurnOnAllEnemyColliders();
                ChooseNextActiveCharacter();
            });
        }
    }

    public float CalculateDamage(CharacterBattle attacker, CharacterBattle target)
    {
        float multiplier = specialPowerManager.GetSpecialAttackMultiplier(attacker.GetLand(), target.GetLand());
        float damage = attacker.GetBaseDamage() * multiplier;
        // Stronger? Weaker? Neutral?
        return (damage);
    }

    public CharacterBattle SpawnCharacter(GameObject character, bool isPlayerTeam, Vector3 pos, Vector3 defeatPos, int posNum)
    {
        Vector3 position                = pos;
        GameObject characterTransform   = Instantiate(character, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam, defeatPos, posNum);
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
            state = State.Busy;
            return;
        }

        ClearActiveCharacters();
        if (isPlayerTurn)
        {
             state = State.Busy;            
            enemyAI.UpdateTurn();
        }
        else
        {
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

    // Called when you double click on a target.
    private void ShowTargetInfo(CharacterBattle characterBattle)
    {
        print("Show Target Info");
    }


    private bool TestBattleOver()
    {
        int deadEnemies = 0;
        int deadPlayers = 0;
        bool isBattleOver = false;
        for (int i = 0; i < enemyCharacterBattles.Count; i++)
        {
            if (enemyCharacterBattles[i].IsDead())
            {
                deadEnemies++;
            }
        }

        for (int i = 0; i < playerCharacterBattles.Count; i++)
        {
            if (playerCharacterBattles[i].IsDead())
            {
                deadPlayers++;
            }
        }

        if (deadEnemies == enemyCharacterBattles.Count || deadPlayers == playerCharacterBattles.Count)
        {
            bool isPlayerTheWinner = false;
            if(deadEnemies == enemyCharacterBattles.Count)
            {
                isPlayerTheWinner = true;
            }
            GameObject go = Instantiate(BattleOverWin_pf);
            BattleOverWindow battleOverWindow = go.GetComponent<BattleOverWindow>();
            battleOverWindow.Init("John Doe", isPlayerTheWinner);
            isBattleOver = true;   
        }
        return isBattleOver;
    }


    private bool CheckSpecialAttackReady()
    {
        int minAttackLevel = 5;
        if (activeCharacterBattle.currentLevel >= minAttackLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckUltimateAttackReady()
    {
        int minAttackLevel = 10;
        if (activeCharacterBattle.currentLevel >= minAttackLevel)
        {
            return true;
        }
        else
        {           
            return false;
        }
    }

    public void CharacterSelected(CharacterBattle characterBattle)
    {
        SetActiveCharacterBattle(characterBattle);
    }

    private void TurnOffOtherEnemyColliders(CharacterBattle target)
    {
        for (int i = 0; i < enemyCharacterBattles.Count; i++)
        {
            if (enemyCharacterBattles[i].gameObject != target.gameObject)
            {
                enemyCharacterBattles[i].TurnOffCollider();
            }                      
        }
    }

    private void TurnOnAllEnemyColliders()
    {
        for (int i = 0; i < enemyCharacterBattles.Count; i++)
        {
            enemyCharacterBattles[i].TurnOnCollider();            
        }
    }

    public List<CharacterBattle> GetEnemyTeam()
    {
        return enemyCharacterBattles;
    }
}


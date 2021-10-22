using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject[] AIDeck;
    [SerializeField] Transform[] leftPositions;
    [SerializeField] Transform[] rightPositions;

    [Tooltip("Percentage of the time to attempt a special attack.")]
    [SerializeField] int chanceOfSpecialAttack;


    private States state;
    private enum States { WaitingForOpponent, SelectAttacker, SelectTarget, Attacking, AttackDone, busy}
    [SerializeField] private List<CharacterBattle> AICharacterBattles;
    private List<CharacterBattle> playerCharacterBattles;
    private CharacterBattle activeAICharacter;
    private CharacterBattle activePlayerCharacter;
    public UnityAction AttackDone;


    private void Update()
    {
        switch (state)
        {
            case States.SelectAttacker:

                break;
            case States.SelectTarget:

                break;

            case States.Attacking:

                break;

            case States.AttackDone:

                break;
            default:
                break;
        }
    }

    public List<CharacterBattle> InitAI(List<CharacterBattle> playerCharacterBattles)
    {
        this.playerCharacterBattles = playerCharacterBattles;
        //AICharacterBattles = new List<CharacterBattle>();
        //AICharacterBattles.Add( SpawnCharacter(AIDeck[0], false, rightPositions[0].position, 0) );
        //AICharacterBattles.Add( SpawnCharacter(AIDeck[1], false, rightPositions[1].position, 1) );
        //AICharacterBattles.Add( SpawnCharacter(AIDeck[2], false, rightPositions[2].position, 2) );

        for (int i = 0; i < AICharacterBattles.Count; i++)
        {
            AICharacterBattles[i].Setup(AICharacterBattles[i].IsPlayerTeam(), AICharacterBattles[i].positionNumber);
        }
        return AICharacterBattles;
    }

    //public CharacterBattle SpawnCharacter(GameObject character, bool isPlayerTeam, Vector3 pos, int posNum)
    //{
    //    Vector3 position = pos;
    //    GameObject characterTransform = Instantiate(character, position, Quaternion.identity);
    //    CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
    //    characterBattle.Setup(isPlayerTeam, posNum);

    //    return characterBattle;
    //}


    // Called from BattleHandler when it's the A.I.'s turn.
    public void UpdateTurn()
    {
        state = States.SelectAttacker;
        // Keith, select an attack wait 1 second, select a target wait 1 second, call attack.
        Invoke("ChooseAttacker", 1f);
        Invoke("ChooseTarget", 2f);
        Invoke("Attack", 3f);
        // Set up a unityAction for turn done and invoke it 
    }

    private void ChooseAttacker()
    {
        //AICharacterBattles = KG_Utils.ShuffleList(AICharacterBattles);
        List<CharacterBattle> livingCharacters = GetRemainingChracters(AICharacterBattles);
        int ranNum = Random.Range(0, livingCharacters.Count);
        activeAICharacter = livingCharacters[ranNum];
        activeAICharacter.ShowSelectionCircle();
    }

    private void ChooseTarget()
    {
        // Choose special attack for first attack and use probability to attempt it for each attack.
        // If AI does not have a good special attack target do not attempt.
        // If no special attack target then check if AI has a character with splash damage.
        // Attempt ultimate attack after 2-4 turns.
        // Figure out how to serialize these params so they can be adjusted for harder levels.

        List<CharacterBattle> livingCharacters = GetRemainingChracters(playerCharacterBattles);
        int ranNum = Random.Range(0, livingCharacters.Count);
        activePlayerCharacter = livingCharacters[ranNum];
        activePlayerCharacter = FindWeakestCharacter(livingCharacters);
        activePlayerCharacter.ShowSelectionCircle(); 
    }

    private void Attack()
    {
        float damage = BattleHandler.GetInstance().CalculateDamage(activeAICharacter, activePlayerCharacter);
        activeAICharacter.Attack(BattleHandler.AttackTypes.Basic, activePlayerCharacter, damage, () => {
           AttackDone?.Invoke();
        });
    }

    // Returns a list of characters that are still alive
    private List<CharacterBattle> GetRemainingChracters(List<CharacterBattle> characterBattles)
    {
        List<CharacterBattle> theLiving = new List<CharacterBattle>();
        for (int i = 0; i < characterBattles.Count; i++)
        {
            if (!characterBattles[i].IsDead())
            {
                theLiving.Add(characterBattles[i]);
            }
        }
        return theLiving;
    }

    private CharacterBattle FindWeakestCharacter(List<CharacterBattle> characterBattles)
    {
        CharacterBattle weakChar;
        //int ranNum = Random.Range(0, characterBattles.Length);
        weakChar = characterBattles[0];
        for (int i = 0; i < characterBattles.Count; i++)
        {
            if (characterBattles[i].GetHealthAmount() < weakChar.GetHealthAmount() )
            {
                weakChar = characterBattles[i];
            }
        }
        return weakChar;
    }
}

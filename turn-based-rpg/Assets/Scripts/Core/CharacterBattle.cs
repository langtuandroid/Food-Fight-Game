using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using CodeMonkey.Utils;
using MoreMountains.Feedbacks;

public class CharacterBattle : MonoBehaviour
{
    private Character_Base characterBase;
    private State state;
    [SerializeField] AttackType charAttackType;
    [SerializeField] SpecialPowerManager.Lands battleLand;
    [SerializeField] MMFeedbacks blinkFeedback;
    [SerializeField] AudioSource hurt_as;

    [SerializeField]
    [Tooltip("Chance attack will miss. Lower for better accuracy")]
    int chanceOfMiss = 60;
    [SerializeField]
    [Tooltip("Chance of critical attack. Higher for better chance of landing a critical blow")]
    int chanceOfCrit = 2;

    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private bool isPlayerTeam;
    private GameObject selectionCircleGameObject;
    private HealthSystem healthSystem;
    private World_Bar healthBar;
    Vector3 startingPosition;
    Vector3 attackDir;
    Action onAttackComplete;
    private bool isDamageAOE = false;
    public enum AttackType { Range, Melee}
    public int positionNumber;
    [SerializeField] public int currentLevel = 0;

    public float clickDelta = 0.35f;  // Max between two click to be considered a double click
    private bool click = false;
    private float clickTime;

    private float currAttackDamage = 50f;
    [SerializeField] private float baseDamage = 10f;
    //[SerializeField] SpecialPowerManager.Lands homeLand;

    public UnityAction<CharacterBattle> CharacterSelected;
    public UnityAction<CharacterBattle> CharacterDoubleClicked;
    private enum State
    {
        Idle,
        Sliding,
        Busy
    }

    private void Awake()
    {
        characterBase = GetComponent<Character_Base>();
        selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
        startingPosition = GetPosition();
        HideSelectionCircle();
        state = State.Idle;
    }

    private void OnMouseDown()
    {

        if (click && Time.time <= (clickTime + clickDelta))
        {
            click = false;
            CharacterDoubleClicked?.Invoke(this);
        }
        else
        {
            click = true;
            clickTime = Time.time;
            CharacterSelected?.Invoke(this);
        }


    }

    public void Setup(bool isPlayerTeam, int positionNumber)
    {
        this.isPlayerTeam = isPlayerTeam;
        this.positionNumber = positionNumber;
        currentLevel = 11;
        if (isPlayerTeam)
        {
            characterBase.SetAnimsSwordTwoHandedBack();

            // Change the texture to show a different look for the sprite.
            // Example: archer, knight, giant. They can all use the same code.
            // characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().playerSpritesheet;
        }
        else
        {
            characterBase.SetAnimsSwordShield();
            //characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().enemySpritesheet;
        }

        healthSystem = new HealthSystem(100);
        healthBar = new World_Bar(transform, new Vector3(0, 1.2f), new Vector3(1.5f, .2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = .1f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        PlayAnimIdle();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        healthBar.SetSize(healthSystem.GetHealthPercent());
    }

    private void PlayAnimIdle()
    {
        if (isPlayerTeam)
        {
            characterBase.PlayAnimIdle(new Vector3(+1, 0));
        }
        else
        {
            characterBase.PlayAnimIdle(new Vector3(-1, 0));
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:

                break;
            case State.Busy:
                break;
            case State.Sliding:
                float slideSpeed = 4f;
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if ( Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance )
                {
                    // Arrived at slide target position
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
            default:
                break;
        }


        if (click && Time.time > (clickTime + clickDelta))
        {
            click = false;
        }

    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Damage(CharacterBattle attacker, float damageAmount, bool missed = false, bool isCrit = false)
    {
        //missed = true;
        if (missed)
        {
            DamagePopup.Create(GetPosition(), "MISS!", false, true);
            return;
        }
        healthSystem.Damage(damageAmount);
        //CodeMonkey.CMDebug.TextPopup("Hit " + healthSystem.GetHealthAmount(), GetPosition());
        DamagePopup.Create(GetPosition(), damageAmount.ToString(), false, true);
        Vector3 dirFromToAttacker = (GetPosition() -  attacker.GetPosition() - GetPosition()).normalized;

        //Blood_Handler.SpawnBlood(GetPosition(), dirFromToAttacker);
        CodeMonkey.Utils.UtilsClass.ShakeCamera(.05f, .4f);
        blinkFeedback.PlayFeedbacks();
        Invoke(nameof(DoHurt), .5f);
        if (healthSystem.IsDead())
        {
            //Died
            characterBase.PlayAnimLyingUp();
        }
    }

    private void DoHurt()
    {
        if (hurt_as) hurt_as.Play();
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void Attack(BattleHandler.AttackTypes attackType, CharacterBattle targetCharacterBattle, float damage, Action onAttackComplete)
    {
        currAttackDamage = damage;
        // What is my attack type?
        // if range go to middle of field
        // if melee go to target

        // what is my damage type?
        // if AOE find other enemies around the target.
        //
        this.onAttackComplete = onAttackComplete;
        float attackPos = charAttackType == AttackType.Range ? 4f : 2f;
        Vector3 slideTargetPosition;
        if (charAttackType == AttackType.Range)
        {
            // Range characters fight from a distance, so stay where you are.
            slideTargetPosition = GetPosition();
        }
        else
        {
            slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * attackPos;
        }
        
        

        // Slide to Target
        SlideToPosition(slideTargetPosition, () => {
            // Arrive at Target, attack him
            state = State.Busy;

            attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            characterBase.PlayAnimAttack(this, targetCharacterBattle,() => {
                // Attack animation has caused damage. (could happen multiple times)
                //int ranNum = UnityEngine.Random.Range(0, 3);
                //bool missed = UnityEngine.Random.value < 0.50f ? true : false;
                bool missed = KG_Utils.ProbabilityCheck(chanceOfMiss);
                bool isCrit = KG_Utils.ProbabilityCheck(chanceOfCrit);
                targetCharacterBattle.Damage(this, currAttackDamage, missed, isCrit);
            }, () =>  {
                // Attack completed, slide back
                Invoke("AttackDone", 1f);
            });
        });
    }

    private void AttackDone()
    {
        SlideToPosition(startingPosition, () =>
        {
            // Slide back completed, back to idle.
            state = State.Idle;
            characterBase.PlayAnimIdle(attackDir);
            onAttackComplete();
        });
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlidecomplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlidecomplete;
        state = State.Sliding;
        if (slideTargetPosition.x > 0)
        {
            characterBase.PlayAnimSlideRight();
        }
        else
        {
            characterBase.PlayAnimSlideLeft();

        }
    }

    public void HideSelectionCircle()
    {
        selectionCircleGameObject.SetActive(false);
    }

    public void ShowSelectionCircle()
    {
        selectionCircleGameObject.SetActive(true);
    }

    public bool IsPlayerTeam()
    {
        return isPlayerTeam;
    }

    public AttackType GetAttackType()
    {
        return charAttackType;
    }

    public float GetBaseDamage()
    {
        return baseDamage;
    }

    public SpecialPowerManager.Lands GetLand()
    {
        return battleLand;
    }

    public float GetHealthAmount()
    {
       return healthSystem.GetHealthAmount();
    }

}

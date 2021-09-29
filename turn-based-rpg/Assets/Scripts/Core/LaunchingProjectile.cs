using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingProjectile : Projectile
{
    [SerializeField] CharacterBattle attacker;
    [SerializeField] CharacterBattle target;
    [SerializeField] BlastRadius blastRadius;
    private float tParam = 0f;
    private Vector2 objectPosition;
    private float speedModifier = 0.5f;
    private Vector2 p0,p1,p2,p3;
    private BattleHandler battleHandler;
    private List<CharacterBattle> enemyTeam;
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;
    //private GameObject hit_ps;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
        battleHandler = FindObjectOfType<BattleHandler>();
        enemyTeam = battleHandler.GetEnemyTeam();
        //enemyTeam = new List<CharacterBattle>();
        //blastRadius.OnObjectInRadius += AddCharToHitList;
    }

    void Start()
    {
        objectPosition = transform.position;
        //Setup(attacker, new Vector3(0, 0, 0), target,() => { print("yay"); });
    }

    // Need this update to override Update in base class
    void Update()
    {

    }

    public override void Setup(CharacterBattle attacker, Vector3 shootDir, GameObject hit_ps, CharacterBattle target, Action OnHit)
    {
        this.attacker = attacker;
        targetCharacterBattle = target;
        this.hit_ps = hit_ps;
        OnHitAction = OnHit;
        ProjectileManager attackerPM = attacker.gameObject.GetComponent<ProjectileManager>();
        ProjectileManager targetPM = target.gameObject.GetComponent<ProjectileManager>();
        p0 = attackerPM.GetLaunchPoint();
        p1 = attackerPM.GetControlPoint();
        p3 = targetPM.GetLaunchPoint();
        p2 = targetPM.GetControlPoint();
        StartCoroutine(GoByTheroute());
    }

    private IEnumerator GoByTheroute()
    {
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
        if (characterBattle == targetCharacterBattle)
        {
            collisionSnd_as.Play();
            print("Why not play?");
            OnHitAction();
            //int c = 0;
            List<int> splashPositions = GetSplashPositions(characterBattle.positionNumber);
            List<CharacterBattle> splashList = GetSplashList(splashPositions);
            print("COUNT THE ENEMIES " + enemyTeam.Count);
            print("splashPositions=" + splashPositions.Count+ " splashList=" + splashList.Count);
            for (int i = 0; i < splashList.Count; i++)
            {
                //float distanceSqr = (transform.position - enemyTeam[i].transform.position).sqrMagnitude;
                //if (distanceSqr <= blastRadius) c++;
                //print("distanceSqr =" + distanceSqr+ " blastRadius="+ blastRadius);

                // Keith you need to figure out the splash damage for this projectile
                // damage should be set at launch.
                splashList[i].Damage(attacker, 5);
                if (enemyTeam[i] == characterBattle)
                {
                    print("\n ############################# This should only happen once ##############");
                    Instantiate(hit_ps, collision.gameObject.transform.position, Quaternion.identity);
                }
            }

            //print( " inRadius=" + c +" numberOfEnemies="+enemyTeam.Count);
            spriteColor.a = 0;

            spriteRenderer.color = spriteColor;

            Destroy(gameObject, 2f);
        }
    }

    //// Called when a character is within the blast radius of this projectile.
    //private void AddCharToHitList(CharacterBattle charInRadius)
    //{
    //    if (!charInRadius.IsPlayerTeam())
    //    {            
    //        print("This is an enemy! Add to the list!!!!");
    //        enemyTeam.Add(charInRadius);
    //    }
    //}

    // returns a list of positions affected by a hit from the specified positionNumber
    private List<int> GetSplashPositions(int posNum)
    {
        List<int> splashList = new List<int>();
        if (posNum == 0 || posNum == 2)
        {
            splashList.Add(1);
        }
        else if (posNum == 1)
        {
            splashList.Add(0);
            splashList.Add(2);
        }
        return splashList;
    }

    private List<CharacterBattle> GetSplashList(List<int> posList)
    {
        List<CharacterBattle> splashList = new List<CharacterBattle>();
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            for (int j = 0; j < posList.Count; j++)
            {
                if (enemyTeam[i].positionNumber == posList[j])
                {
                    splashList.Add(enemyTeam[i]);
                }
            }
        }
        return splashList;
    }

}

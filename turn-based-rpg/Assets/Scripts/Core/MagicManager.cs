using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicManager : MonoBehaviour
{
    [SerializeField] private GameObject[] yummies_pf;
    [SerializeField] private GameObject hit_ps;

    private CharacterBattle attacker;
    private CharacterBattle target;
    private Action onHit;
    private int attackCount = 0;
    private int MaxAttacks = 3;
    private float delayBeforeFirstAttack = 1f;
    private float delayBetweenAttacks = .5f;

  

    public void SpawnYummie(CharacterBattle attacker, CharacterBattle target, Action onHit)
    {
        this.attacker   = attacker;
        this.target     = target;
        this.onHit      = onHit;        
        Invoke(nameof(StartDrop), delayBeforeFirstAttack);
    }

    private void StartDrop()
    {
        for (int i = 0; i < MaxAttacks; i++)
        {
            Invoke(nameof(SpawnOneYummy), delayBetweenAttacks * i);
        }
    }

    public void SpawnOneYummy()
    {
        //TODO instantiate at a random x within a range
        //TODO add magic sparkles and vfx when yummy appears
        //TODO add splat sound when hit
        //TODO add splat particle fx
        Transform projectileTransform = Instantiate(yummies_pf[0], target.GetAboveCharPoint(), Quaternion.identity).transform;
        projectileTransform.GetComponent<DroppableYummy>().Setup(attacker, hit_ps, target, CheckAttackDone);

        Vector2 pos = new Vector2(projectileTransform.position.x + UnityEngine.Random.insideUnitCircle.x, projectileTransform.position.y);
        projectileTransform.position = pos;
    }

    private void CheckAttackDone()
    {
        target.Damage(attacker, 1f, false, false);
        attackCount++;
        if (attackCount == MaxAttacks)
        {
            attackCount = 0;
            onHit.Invoke();
        }
    }

}

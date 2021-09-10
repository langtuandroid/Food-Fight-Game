using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using CodeMonkey.Utils;


public class Projectile : MonoBehaviour
{
    protected Vector3 shootDir = new Vector3(1.1f,1.1f,1.1f);
    protected CharacterBattle targetCharacterBattle;
    protected Action OnHitAction;

    public virtual void Setup(CharacterBattle attacker, Vector3 shootDir, CharacterBattle targetCharacterBattle, Action OnHit)
    {
        this.shootDir = shootDir;
        this.targetCharacterBattle = targetCharacterBattle;
        OnHitAction = OnHit;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        float moveSpeed = 1.5f;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("Base Collision!!");
        CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
        if (characterBattle == targetCharacterBattle)
        {
            OnHitAction();
            Destroy(gameObject);
        }
    }
}

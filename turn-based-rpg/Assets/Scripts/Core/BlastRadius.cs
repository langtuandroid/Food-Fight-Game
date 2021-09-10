using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlastRadius : MonoBehaviour
{
    public UnityAction<CharacterBattle> OnObjectInRadius;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
    //    if (!characterBattle.IsPlayerTeam()) print("HIT BLAST RADIUS ");

    //    //print("IsPlayerTeam=" + characterBattle.IsPlayerTeam());

    //    if (characterBattle)
    //    {
    //        OnObjectInRadius?.Invoke(characterBattle);
    //    }

    //}
}

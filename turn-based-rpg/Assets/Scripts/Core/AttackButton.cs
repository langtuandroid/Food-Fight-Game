using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    [SerializeField] BattleHandler battleHandler;
    [SerializeField] BattleHandler.AttackTypes attackType;

   public void OnAttackBtnClicked()
   {
        battleHandler.Attack(attackType);
   }
}

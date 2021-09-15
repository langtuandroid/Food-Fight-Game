using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelectAttackTypeWin : MonoBehaviour
{
    //[SerializeField] UIButton basicAttackBtn;
    //[SerializeField] UIButton specialAttackBtn;
    //[SerializeField] UIButton ultimateAttackBtn;
    [SerializeField] TextMeshProUGUI outputDisplay;

    public UnityAction<BattleHandler.AttackTypes> OnSelectionMade;
    public UnityAction OnWinClosed;

    private void Awake()
    {
        // basicAttackBtn.OnButtonClicked    += SelectBasicAttack;
        // specialAttackBtn.OnButtonClicked  += SelectSpecialAttack;
        // ultimateAttackBtn.OnButtonClicked += SelectSpecialAttack;
    }


    public void SelectBasicAttack()
    {
        ClearDisplay();
        OnSelectionMade?.Invoke(BattleHandler.AttackTypes.Basic);
    }

    public void SelectSpecialAttack()
    {
        ClearDisplay();
        OnSelectionMade?.Invoke(BattleHandler.AttackTypes.Special);
    }

    public void SelectUltimateAttack()
    {
        ClearDisplay();
        OnSelectionMade?.Invoke(BattleHandler.AttackTypes.ultimate);
    }

    public void CloseWin()
    {
        //basicAttackBtn.OnButtonClicked -= SelectBasicAttack;
        //specialAttackBtn.OnButtonClicked -= SelectSpecialAttack;
        //ultimateAttackBtn.OnButtonClicked -= SelectSpecialAttack;
        OnWinClosed?.Invoke();
        Destroy(gameObject);
    }

    public void UdateDisplay(string str)
    {
        outputDisplay.text = str;
    }

    private void ClearDisplay()
    {
        outputDisplay.text = "";
    }

    public void Die()
    {
        Destroy(gameObject);
    }


}

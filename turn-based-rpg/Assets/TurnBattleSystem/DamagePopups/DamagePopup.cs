/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class DamagePopup : MonoBehaviour {

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, string damageAmount, bool isCriticalHit, bool isMiss) {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit, isMiss);

        return damagePopup;
    }

    private static int sortingOrder = 10000;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string damageAmount, bool isCriticalHit, bool missed) {
        textMesh.SetText("-"+ damageAmount.ToString());
        if (isCriticalHit) {
            // Critical hit
            textMesh.fontSize = 30;
            textColor = UtilsClass.GetColorFromString("FF2B00");
        }
        else if (missed) {
            // Missed
            textMesh.fontSize = 10;
            textColor = UtilsClass.GetColorFromString("FF0000");
        }
        else {            
            // Normal hit
            textMesh.fontSize = 20;
            textColor = UtilsClass.GetColorFromString("FFC500");
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(.2f, 1) * 7.5f;
    }

    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            // First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }

}

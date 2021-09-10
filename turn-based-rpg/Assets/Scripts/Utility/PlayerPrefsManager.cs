using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

    public const string APP_NAME = "ooonimals";

    // Notification Keys ----------
    public const string NOTIFICATION_KEY        = APP_NAME         + "_notification";
    //public const string All                     = NOTIFICATION_KEY + "_all";
    //public const string NONE                    = NOTIFICATION_KEY + "_none";
    public const string TRADE_OFFERED           = NOTIFICATION_KEY + "_tradeOffered";
    public const string SOMETHING_SOLD          = NOTIFICATION_KEY + "_somethingSold";
    public const string TRADE_ACCEPTED          = NOTIFICATION_KEY + "_tradeAccepted";
    public const string WISH_LIST_AVAILABLE     = NOTIFICATION_KEY + "_wishListAvailable";
    public const string NEW_MERCH_AVAILABLE     = NOTIFICATION_KEY + "_newMerchAvailable";
    public const string POP_UP_STORE            = NOTIFICATION_KEY + "_popupStore";
    public const string NEW_SCENES_OFFERED      = NOTIFICATION_KEY + "_newScenesOffered";
    public const string SHIPPING_NOTIFICATIONS  = NOTIFICATION_KEY + "_shippingNotifications";
    public const string TRADES                  = NOTIFICATION_KEY + "_trades";
    public const string ORDERS                  = NOTIFICATION_KEY + "_orders";

    // Login Keys --------------
    public const string LOGIN_KEY               = APP_NAME  + "_login";
    public const string PARENT_LOGGEDIN         = LOGIN_KEY + "_parentLoggedIn";
    public const string PARENT_DATA             = LOGIN_KEY + "_parentData";


    const string MASTER_VOLUME_KEY = "master_volume";
	const string DIFFICULTY_KEY = "difficulty";
	const string LEVEL_KEY = "level_unlocked_";


    public static void SetParentLoggedIn(string parentData)
    {
        PlayerPrefs.SetInt(PARENT_LOGGEDIN, 1);
        PlayerPrefs.SetString(PARENT_DATA, parentData);
    }

    public static bool GetParentLoggedIn()
    {
        bool isLoggedIn = false;
        if (PlayerPrefs.GetInt(PARENT_LOGGEDIN) == 1)
        {
            isLoggedIn = true;
        }
        return isLoggedIn;
    }

    public static void ClearParentLoggedIn()
    {
        PlayerPrefs.DeleteKey(PARENT_LOGGEDIN);
        PlayerPrefs.DeleteKey(PARENT_DATA);
    }

    public static string GetParentData()
    {
        return PlayerPrefs.GetString(PARENT_DATA);
    }

    public static void ClearParentLogIn()
    {
        PlayerPrefs.DeleteKey(PARENT_LOGGEDIN);
    }

    public static void SetMasterVolume( float volume){

		if(volume >= 0f && volume <= 1f){
			PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
		}else{
			Debug.LogError("Master volume out of range");
		}
	}

	public static float GetMasterVolume(){

		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
	}

	public static void UnlockLevel (int level){

		if(level <= SceneManager.sceneCountInBuildSettings - 1 ){
			PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); // use 1 for true
		} else {
			Debug.LogError("Trying to unlock level not in build order "+level);
		}
	}

	public static bool IsLevelUnlocked(int level){

		int levelValue = PlayerPrefs.GetInt (LEVEL_KEY + level.ToString());
		bool isLevelUnlocked = (levelValue == 1);

		if(level <= SceneManager.sceneCountInBuildSettings - 1 ){
			return isLevelUnlocked;
		}else{
			Debug.LogError("Trying to query level no in build order");
			return false;
		}

	}


	public static void SetDifficulty(float difficulty){
		if (difficulty >= 1f && difficulty <= 3f) {
			PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
		}else{
			Debug.LogError("Difficulty out of range");
		}

	}

	public static float GetDifficulty(){
		return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
	}

}

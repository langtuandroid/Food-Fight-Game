using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapManager : MonoBehaviour
{

    public enum LandsTypes {
        home,
        star_mountain,
        cyrstal_reef,
        sizzling_bayou,
        aves_city,
        sweet_potato_farm,
        scorching_plains,
        purple_jungle,
        woo_bamboo_forest,
        middleville,
        irk_gardens,
        rextown,
        hill_pop_sahara,
        artic_towne,
        festivites_forest,
        red_fire_mountain,
        civilville
    }

    //private Dictionary<LandsTypes, string> lands;
    [SerializeField] LandBehavior[] lands;

    // Start is called before the first frame update
    void Start()
    {

        //Land artic = new Land { land = WorldMapManager.LandsTypes.artic_towne, sceneName = "Artic!", isLocked = false };

        //lands.Add(artic);

        //print("testLand "+lands[0].sceneName);

        for (int i = 0; i < lands.Length; i++)
        {
            lands[i].LandClicked += HandleClickedOnLand;
        }

        //lands = new Dictionary<LandsTypes, string>();
        //lands.Add(LandsTypes.artic_towne,       "");
        //lands.Add(LandsTypes.aves_city,         "");
        //lands.Add(LandsTypes.civilville,        "");
        //lands.Add(LandsTypes.cyrstal_reef,      "");
        //lands.Add(LandsTypes.festivites_forest, "");
        //lands.Add(LandsTypes.hill_pop_sahara,   "");
        //lands.Add(LandsTypes.irk_gardens,       "");
        //lands.Add(LandsTypes.middleville,       "");
        //lands.Add(LandsTypes.purple_jungle,     "");
        //lands.Add(LandsTypes.red_fire_mountain, "");
        //lands.Add(LandsTypes.rextown,           "");
        //lands.Add(LandsTypes.scorching_plains,  "");
        //lands.Add(LandsTypes.sizzling_bayou,    "");
        //lands.Add(LandsTypes.star_mountain,     "");
        //lands.Add(LandsTypes.sweet_potato_farm, "");
        //lands.Add(LandsTypes.woo_bamboo_forest, "");

    }


    // Called when user clicks on a land
    public void HandleClickedOnLand(LandBehavior land)
    {
        print("HandleClickedOnLand " + land.landType);
        //
        SceneManager.LoadScene(land.sceneName);
    }
}

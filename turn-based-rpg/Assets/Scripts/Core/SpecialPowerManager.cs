using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPowerManager : MonoBehaviour
{
    public enum Lands {
        Aves_City,
        Irk_Gardens,
        Civilville,
        Crystal_Reef,
        Artic_Towne,
        Festivities_Forest,
        Hill_Pop_Sahara,
        Sweet_Potato_farm,
        Scorching_Plains,
        Middleville,
        Woo_Bamboo_Forest,
        Sizzling_Bayou,
        Purple_Jungle,
        Rextown,
        Red_Fire_Mountain,
        Star_Mountain
    }
    List<LandPowerChart> landstrengths;
    Dictionary<Lands, LandPowerChart> landPowerStructure;

    // Start is called before the first frame update
    void Awake()
    {
        landPowerStructure = new Dictionary<Lands, LandPowerChart>();
        //landstrengths = new List<LandPowerChart>();
        landPowerStructure.Add(Lands.Aves_City, new LandPowerChart { land = Lands.Aves_City, strongAgainst = Lands.Irk_Gardens, weakAgainst = Lands.Middleville });
        landPowerStructure.Add(Lands.Irk_Gardens, new LandPowerChart { land = Lands.Irk_Gardens, strongAgainst = Lands.Civilville, weakAgainst = Lands.Aves_City });
        landPowerStructure.Add(Lands.Civilville, new LandPowerChart { land = Lands.Civilville, strongAgainst = Lands.Crystal_Reef, weakAgainst = Lands.Irk_Gardens });
        landPowerStructure.Add(Lands.Crystal_Reef, new LandPowerChart { land = Lands.Crystal_Reef, strongAgainst = Lands.Artic_Towne, weakAgainst = Lands.Civilville });
        landPowerStructure.Add(Lands.Artic_Towne, new LandPowerChart { land = Lands.Artic_Towne, strongAgainst = Lands.Festivities_Forest, weakAgainst = Lands.Crystal_Reef });
        landPowerStructure.Add(Lands.Festivities_Forest, new LandPowerChart { land = Lands.Festivities_Forest, strongAgainst = Lands.Hill_Pop_Sahara, weakAgainst = Lands.Artic_Towne });
        landPowerStructure.Add(Lands.Hill_Pop_Sahara, new LandPowerChart { land = Lands.Hill_Pop_Sahara, strongAgainst = Lands.Sweet_Potato_farm, weakAgainst = Lands.Festivities_Forest });
        landPowerStructure.Add(Lands.Sweet_Potato_farm, new LandPowerChart { land = Lands.Sweet_Potato_farm, strongAgainst = Lands.Scorching_Plains, weakAgainst = Lands.Hill_Pop_Sahara });
        landPowerStructure.Add(Lands.Scorching_Plains, new LandPowerChart { land = Lands.Scorching_Plains, strongAgainst = Lands.Star_Mountain, weakAgainst = Lands.Sweet_Potato_farm });
        landPowerStructure.Add(Lands.Star_Mountain, new LandPowerChart { land = Lands.Star_Mountain, strongAgainst = Lands.Red_Fire_Mountain, weakAgainst = Lands.Scorching_Plains });
        landPowerStructure.Add(Lands.Red_Fire_Mountain, new LandPowerChart { land = Lands.Red_Fire_Mountain, strongAgainst = Lands.Rextown, weakAgainst = Lands.Star_Mountain });
        landPowerStructure.Add(Lands.Rextown, new LandPowerChart { land = Lands.Rextown, strongAgainst = Lands.Purple_Jungle, weakAgainst = Lands.Red_Fire_Mountain });
        landPowerStructure.Add(Lands.Purple_Jungle, new LandPowerChart { land = Lands.Purple_Jungle, strongAgainst = Lands.Sizzling_Bayou, weakAgainst = Lands.Rextown });
        landPowerStructure.Add(Lands.Sizzling_Bayou, new LandPowerChart { land = Lands.Sizzling_Bayou, strongAgainst = Lands.Woo_Bamboo_Forest, weakAgainst = Lands.Purple_Jungle });
        landPowerStructure.Add(Lands.Woo_Bamboo_Forest, new LandPowerChart { land = Lands.Woo_Bamboo_Forest, strongAgainst = Lands.Middleville, weakAgainst = Lands.Woo_Bamboo_Forest });

        // create a getWeakAgains and GetstrongAgaints function. Or maybe a function that just returns 0, 1.5f, .5
    }

    public float GetSpecialAttackMultiplier(Lands attackLand, Lands targetLand)
    {
        float damage = 1;
        LandPowerChart l = landPowerStructure[attackLand];

        if (targetLand == l.strongAgainst)
        {
            damage = 1.5f;
        }
        else if (targetLand == l.weakAgainst)
        {
            damage = .5f;
        }
        //for (int i = 0; i < landstrengths.Count; i++)
        //{
        //    if (landstrengths[i].land == attackLand)
        //    {
        //        l = landstrengths[i];
        //    }
        //}

        
        return damage;
    }
}

struct LandPowerChart
{
    public SpecialPowerManager.Lands land;
    public SpecialPowerManager.Lands weakAgainst;
    public SpecialPowerManager.Lands strongAgainst;

}

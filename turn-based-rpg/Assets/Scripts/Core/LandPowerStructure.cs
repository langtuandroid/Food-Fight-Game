using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPowerStructure : MonoBehaviour
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
    List<LandStrengths> landstrengths;

    // Start is called before the first frame update
    void Start()
    {
        landstrengths = new List<LandStrengths>();
        landstrengths.Add(new LandStrengths { land = Lands.Aves_City, strongAgainst = Lands.Irk_Gardens, weakAgainst = Lands.Middleville });
        landstrengths.Add(new LandStrengths { land = Lands.Irk_Gardens, strongAgainst = Lands.Civilville, weakAgainst = Lands.Aves_City });
        landstrengths.Add(new LandStrengths { land = Lands.Civilville, strongAgainst = Lands.Crystal_Reef, weakAgainst = Lands.Irk_Gardens });
        landstrengths.Add(new LandStrengths { land = Lands.Crystal_Reef, strongAgainst = Lands.Artic_Towne, weakAgainst = Lands.Civilville });
        landstrengths.Add(new LandStrengths { land = Lands.Artic_Towne, strongAgainst = Lands.Festivities_Forest, weakAgainst = Lands.Crystal_Reef });
        landstrengths.Add(new LandStrengths { land = Lands.Festivities_Forest, strongAgainst = Lands.Hill_Pop_Sahara, weakAgainst = Lands.Artic_Towne });
        landstrengths.Add(new LandStrengths { land = Lands.Hill_Pop_Sahara, strongAgainst = Lands.Sweet_Potato_farm, weakAgainst = Lands.Festivities_Forest });
        landstrengths.Add(new LandStrengths { land = Lands.Sweet_Potato_farm, strongAgainst = Lands.Scorching_Plains, weakAgainst = Lands.Hill_Pop_Sahara });


        // finish added strengths
        // create a getWeakAgains and GetstrongAgaints function. Or maybe a function that just returns 0, 1.5f, .5
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

struct LandStrengths
{
    public LandPowerStructure.Lands land;
    public LandPowerStructure.Lands weakAgainst;
    public LandPowerStructure.Lands strongAgainst;

}

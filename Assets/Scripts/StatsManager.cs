using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance { get; protected set; }

    // experience , level , skillpoints , vitality , endurance, strenght , intelligence , arcane


    //PRIMARY STATS

    [SerializeField]
    private int _experience = 1;
    [SerializeField]
    private int _level = 1;
    [SerializeField]
    private int _skillpoints = 1;
    [SerializeField]
    private int _vitality = 1;
    [SerializeField]
    private int _endurance = 1;
    [SerializeField]
    private int _strenght = 1;
    [SerializeField]
    private int _intelligence = 1;
    //[SerializeField]
   // private int _arcane = 1;


    // CALCULATED STATS
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private int _stamina = 1;
    [SerializeField]
    private int _physicalDamage = 1;
   // [SerializeField]
  //  private int _arcaneDamage = 1;

    [SerializeField]
    private int _bonusPhysicalDamage = 0;
   // [SerializeField]
   // private int _bonusArcaneDamage = 0;

    [SerializeField]
    private int _physicalDefense = 1;
    //DAMAGE STATS


    #region getStats 

    // stats

    public int experience
    {
        get
        {
            return _experience;
        }
    }
    public int level
    {
        get
        {
            return _level;
        }
    }
    public int skillpoints
    {
        get
        {
            return _skillpoints;
        }
    }
    public int vitality
    {
        get
        {
            return _vitality;
        }
    }
    public int endurnace
    {
        get
        {
            return _endurance;
        }
    }
    public int strenght
    {
        get
        {
            return _strenght;
        }
    }
    public int intelligence
    {
        get
        {
            return _intelligence;
        }
    }
  //  public int arcane
   // {
   //     get
   //     {
   //         return _arcane;
    //    }
   // }



    public int health
    {
        get
        {
            return _health;
        }
    }

    public int stamina
    {
        get
        {
            return _stamina;
        }
    }

    public int physicalDamage
    {
        get
        {
            return _physicalDamage;
        }
    }

  // public int arcaneDamage
   // {
   //     get
   //     {
    //        return _arcaneDamage;
    //    }
   //}

   // public int bonusArcaneDamage
  //  {
  //      get
   //     {
   //         return _bonusArcaneDamage;
   //     }
  // }


    public int bonusPhysicalDamage
    {
        get
        {
            return _bonusPhysicalDamage;
        }
    }
    public int physicalDefense
    {
        get
        {
            return _physicalDefense;
        }
    }


    #endregion

    private void Start()
    {
        if (StatsManager.instance == null)
            instance = this;
    }


    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    private void OnGetStatistics(GetPlayerStatisticsResult result)
    {

        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            if (eachStat.StatisticName == "Experience")
            {
                _experience = eachStat.Value;

            }
            else if (eachStat.StatisticName == "Level")
            {
                _level = eachStat.Value;
            }
            else if (eachStat.StatisticName == "Skillpoints")
            {
                _skillpoints = eachStat.Value;

            }
            else if (eachStat.StatisticName == "Vitality")
            {
                _vitality = eachStat.Value;
            }
            else if (eachStat.StatisticName == "Endurance")
            {
                _endurance = eachStat.Value;
            }
            else if (eachStat.StatisticName == "Strenght")
            {
                _strenght = eachStat.Value;
            }
            else if (eachStat.StatisticName == "Intelligence")
            {
                _intelligence = eachStat.Value;
            }
          //  else if (eachStat.StatisticName == "Arcane")
          //  {
         //       _arcane = eachStat.Value;
          //  }
        }

        calculateStats();

        ProfileManager.instance.SetStatistics();
    }

    private void calculateStats()
    {
        int pDMG = 0;
        int aDMG = 0;

        float intelligenceBonus = 0;
        float strenghtBonus = 0;
        //float arcaneBonus = 0;

        float attributeSaturation = 0;

        // Health gained per Vitality level - 25
        // Stamina increase 5 per level
        // Physical DEF increases 3 every level + Armor stats

        _health = 100 + (vitality * 25);
        _stamina = 100 + (endurnace * 5);
        _physicalDefense = 1 + (level * 3);
        
    if(EquipmentManager.instance.GetEquipment(ItemCategory.weapon) != null)
        foreach (KeyValuePair<string, object> attribute in EquipmentManager.instance.GetEquipment(ItemCategory.weapon).ItemAttributes)
        {
            if (attribute.Key == "pDMG")
                pDMG = int.Parse(attribute.Value.ToString());

            else if (attribute.Key == "aDMG")         
                aDMG = int.Parse(attribute.Value.ToString());
            
            else if (attribute.Key == "strenghtBonus")           
                strenghtBonus = float.Parse(attribute.Value.ToString());
            
            else if (attribute.Key == "intelligenceBonus")            
                intelligenceBonus = float.Parse(attribute.Value.ToString());
            
           // else if (attribute.Key == "arcaneBonus")           
           //     arcaneBonus = float.Parse(attribute.Value.ToString());           
        }


        _bonusPhysicalDamage = Mathf.FloorToInt(pDMG * strenghtBonus * getSaturation(strenght))
            + Mathf.FloorToInt(pDMG * intelligenceBonus * getSaturation(intelligence));
     
        // _bonusArcaneDamage = Mathf.FloorToInt(aDMG * arcaneBonus * getSaturation(arcane));


        // calculate physical damage;
        _physicalDamage = pDMG + _bonusPhysicalDamage;

        // calculate arcane damage;
        // _arcaneDamage = aDMG + bonusArcaneDamage;

        Debug.Log("baseDamage " + pDMG + "   " + "strenghtBonus " + strenghtBonus + "   " + "strenghtSaturation " + getSaturation(strenght) + "   " + "physicalDamage " + _physicalDamage);
        menuManager.instance.startLobby();
        // AR = [Base Damage + (Base Damage * Attribute Scaling * Attribute Saturation)] * Gem % + Flat Damage

        // int baseDamage = 
        // Invector - 20 0 0 0 / ( 20 * 0.9 * 0.14)

        // DMG CALCULATION IN GAME

        // Base Attack + Attack from Scaling = Total Attack
        // Total Attack x "Dmg x" = Total Damage (CHARGED ATTACK?)
        // Total Damage - Enemy Defense = Actual Damage

    }

    public float getSaturation(int level) {
        float saturation = 0;

        for (int i = 1; i <= level; i++) {
            if (i <= 10)
            {
                saturation += 0.005f;
            }
            else if (i >= 11 && i <= 25)
            {
                saturation += 0.03f;
            }
            else if (i >= 26 && i <= 50)
            {
                saturation += 0.014f;
            }
            else if (i >= 51) {
                saturation += 0.003f;
            }

        }

        return saturation;
    }

}

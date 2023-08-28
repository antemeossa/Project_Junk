using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum FactionRelationsEnum
{
    TradePartners,
    FriendlyTraders,
    NeutralTradingTerms,
    CautiousRelations,
    LimitedTrade,
    TradeEmbargo
}

[System.Serializable]
public class Faction
{
    public Sprite factionLogo;
    public string factionTitle;
    public FactionRelationsEnum factionRelations;
    public int currentFactionRelationship;

    public void updateCurrentfactionRelations()
    {
        currentFactionRelationship = Mathf.Clamp(currentFactionRelationship, 0, 100);

        if(currentFactionRelationship < 100)
        {
            factionRelations = FactionRelationsEnum.TradePartners;
        }
        else if(currentFactionRelationship > 65 && currentFactionRelationship < 85)
        {
            factionRelations = FactionRelationsEnum.FriendlyTraders;
        }
        else if (currentFactionRelationship > 50 && currentFactionRelationship < 65)
        {
            factionRelations = FactionRelationsEnum.NeutralTradingTerms;
        }
        else if (currentFactionRelationship > 40 && currentFactionRelationship < 50)
        {
            factionRelations = FactionRelationsEnum.CautiousRelations;
        }
        else if (currentFactionRelationship > 10 && currentFactionRelationship < 40)
        {
            factionRelations = FactionRelationsEnum.LimitedTrade;
        }
        else if (currentFactionRelationship > 0 && currentFactionRelationship < 10)
        {
            factionRelations = FactionRelationsEnum.TradeEmbargo;
        }
        
        


    }
}
public class FactionManager : MonoBehaviour
{
    public List<Faction> Factions;
}

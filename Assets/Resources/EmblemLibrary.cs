using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu]
public class EmblemLibrary : ScriptableObject
{
    [SerializeField] PlayerData playerData;

    public string magical_acceleration = "Magical Acceleration";
    public string magical_acceleration_description = "Mana recharges faster";
    public string heavy_blows = "Heavy Blows";
    public string heavy_blows_description = "Attacks stagger enemies faster";
    public string vampiric_strikes = "Vampiric Strikes";
    public string vampiric_strikes_description = "Recover health whenever you kill an enemy";
    public string quickstep_ = "Quickstep";
    public string quickstep_description = "Dodging does not use as much Stamina";
    public string pay_raise = "Pay Raise";
    public string pay_raise_description = "Earn more money each time you kill an enemy";
    public string quick_strikes = "Quick Strikes";
    public string quick_strikes_description = "Increase attack speed but decrease damage of each attack";
    public string shell_company = "Shell Company";
    public string shell_company_description = "Increase the Stamina cost of Dodging, but decrease the Mana cost of Blocking";
    public string close_call = "Close Call";
    public string close_call_description = "Add Arcane damage to all attacks for a short time after a Perfect Dodge";
    public string arcane_remains = "Arcane Remains";
    public string arcane_remains_description = "Add Arcane damage to all attacks when near your Remnant. Collecting your remnant restores you to full health";
    public string arcane_step = "Arcane Step";
    public string arcane_step_description = "You can dodge through enemies. Leave a trail of Arcane damage when you dodge";

    //public string explosive_healing = "Explosive Healing";
    //public string explosive_healing_description = "Deal damage to nearby enemies whenever you use the Heal ability";

    public int ArcaneStepDamage()
    {
        return playerData.dedication * 2;
    }

    public int CloseCallDamage()
    {
        return playerData.dedication * 2 + 10;
    }

    public int ArcaneRemainsDamage()
    {
        return playerData.dedication * 2 + 15;
    }

    public string GetDescription(string name)
    {
        switch (name)
        {
            case "Magical Acceleration":
                return magical_acceleration_description;
            case "Heavy Blows":
                return heavy_blows_description;
            case "Vampiric Strikes":
                return vampiric_strikes_description;
            case "Quickstep":
                return quickstep_description;
            case "Pay Raise":
                return pay_raise_description;
            case "Quick Strikes":
                return quick_strikes_description;
            case "Shell Company":
                return shell_company_description;
            case "Close Call":
                return close_call_description;
            case "Arcane Remains":
                return arcane_remains_description;
            case "Arcane Step":
                return arcane_step_description;
            default:
                return "Error";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu]
public class EmblemLibrary : ScriptableObject
{
    [SerializeField] PlayerData playerData;
    public List<string> firstFloorPatches;

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
    public string confident_killer = "Confident Killer";
    public string confident_killer_description = "Add Arcane damage to all attacks when you have full health";
    public string adrenaline_rush = "Adrenaline Rush";
    public string adrenaline_rush_description = "Regain all Stamina after a Perfect Dodge";
    public string rending_blows = "Rending Blows";
    public string rending_blows_description = "Heavy Attacks inflict damage over time";
    public float rendingBlowsDuration = 5;
    public string durable_gem = "Durable Gem";
    public string durable_gem_description = "Your healing gem can be used an extra time without breaking";
    public string protective_barrier = "Protective Barrier";
    public string protective_barrier_description = "Every 10 seconds gain a protective barrier that blocks one instance of damage";
    public string explosive_healing = "Explosive Healing";
    public string explosive_healing_description = "Deal damage to nearby enemies whenever you use your Healing Gem";
    public float explosiveHealingRange = 5;
    public float explosiveHealingStagger = 1f;

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

    public int ConfidentKillerDamage()
    {
        return playerData.dedication * 2 + 15;
    }

    public int ExplosiveHealingDamage()
    {
        return playerData.dedication * 3 + 20;
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
            case "Confident Killer":
                return confident_killer_description;
            case "Adrenaline Rush":
                return adrenaline_rush_description;
            case "Rending Blows":
                return rending_blows_description;
            case "Durable Gem":
                return durable_gem_description;
            case "Explosive Healing":
                return explosive_healing_description;
            case "Protective Barrier":
                return protective_barrier_description;
            default:
                return "Error";
        }
    }
}

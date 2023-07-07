using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EmblemLibrary : ScriptableObject
{
    [SerializeField] PlayerData playerData;
    [System.NonSerialized] public Dictionary<string, string> emblemDictionary;
    public List<string> firstFloorPatches;
    public List<string> secondFloorPatches;

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
    public string arcane_remains_description = "Add Arcane damage to all attacks when near your Remnant. Collecting your Remnant restores you to full health";
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
    public string charons_obol = "Charon's Obol";
    public string charons_obol_description = "When you die you do not leave behind a Remnant and you retain half of your current money";
    public string arcane_preservation = "Arcane Preservation";
    public string arcane_preservation_description = "If you are reduced to 0 health, lose Mana instead of dying";
    public string burning_reflection = "Burning Reflection";
    public string burning_reflection_description = "Deflecting spells inflicts damage over time";
    public string opportune_strike = "Opportune Strike";
    public string opportune_strike_description = "Attacking an enemy inflicted with damage over time deals extra damage";
    public string burning_cloak = "Burning Cloak";
    public string burning_cloak_description = "Inflict damage over time to any enemy that deals damage to you";
    public string mirror_cloak = "Mirror Cloak";
    public string mirror_cloak_description = "Every 5 seconds your dodge will also deflect spells";
    public string _spellsword = "Spellsword";
    public string _spellsword_description = "Your attacks deal extra Arcane damage but consume Mana";
    public float spellswordManaCost = 15;
    public string death_aura = "Death Aura";
    public string death_aura_description = "Your regain Mana faster when near your Remnant. Collecting your Remnant restores you to full Mana";
    public string arcane_mastery = "Arcane Mastery";
    public string arcane_mastery_description = "Your Special Attacks do extra damage";
    public float arcaneMasteryPercent = 0.3f;
    public string reckless_attack = "Reckless Attack";
    public string reckless_attack_description = "Increase the base damage of your attacks when you are low on health";

    public int CloseCallDamage()
    {
        return playerData.dedication * 2 + 10;
    }

    public int ArcaneRemainsDamage()
    {
        return playerData.dedication * 2 + 10;
    }

    public int ConfidentKillerDamage()
    {
        return playerData.dedication * 2 + 15;
    }

    public int ExplosiveHealingDamage()
    {
        return playerData.dedication * 3 + 20;
    }

    public int SpellswordDamage()
    {
        return playerData.dedication * 2 + 15;
    }

    public string GetDescription(string name)
    {
        if (emblemDictionary == null) DefineDictionary();
        return emblemDictionary[name];
    }

    private void DefineDictionary()
    {
        emblemDictionary = new Dictionary<string, string>
        {
            {magical_acceleration, magical_acceleration_description},
            {heavy_blows, heavy_blows_description },
            {vampiric_strikes, vampiric_strikes_description },
            {quickstep_, quickstep_description},
            {pay_raise, pay_raise_description },
            {quick_strikes, quick_strikes_description },
            {shell_company,shell_company_description },
            {close_call, close_call_description },
            {arcane_remains, arcane_remains_description },
            {arcane_step, arcane_step_description },
            {confident_killer, confident_killer_description },
            {adrenaline_rush, adrenaline_rush_description },
            {rending_blows, rending_blows_description },
            {durable_gem, durable_gem_description },
            {explosive_healing, explosive_healing_description },
            {protective_barrier, protective_barrier_description },
            {charons_obol, charons_obol_description },
            {arcane_preservation, arcane_preservation_description },
            {burning_reflection, burning_reflection_description },
            {opportune_strike, opportune_strike_description },
            {burning_cloak, burning_cloak_description },
            {mirror_cloak, mirror_cloak_description },
            {_spellsword, _spellsword_description },
            {death_aura, death_aura_description },
            {arcane_mastery, arcane_mastery_description },
            {reckless_attack, reckless_attack_description }
        };
    }
}

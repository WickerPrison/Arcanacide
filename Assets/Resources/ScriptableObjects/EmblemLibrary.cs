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
    public List<string> thirdFloorPatches;

    [System.NonSerialized] public string magical_acceleration = "Magical Acceleration";
    [System.NonSerialized] public string magical_acceleration_description = "Mana recharges faster";
    [System.NonSerialized] public string heavy_blows = "Heavy Blows";
    [System.NonSerialized] public string heavy_blows_description = "Attacks stagger enemies faster";
    [System.NonSerialized] public string vampiric_strikes = "Vampiric Strikes";
    [System.NonSerialized] public string vampiric_strikes_description = "Recover health whenever you kill an enemy";
    [System.NonSerialized] public string quickstep_ = "Quickstep";
    [System.NonSerialized] public string quickstep_description = "Dodging does not use as much Stamina";
    [System.NonSerialized] public string pay_raise = "Pay Raise";
    [System.NonSerialized] public string pay_raise_description = "Earn more money each time you kill an enemy";
    [System.NonSerialized] public string shell_company = "Shell Company";
    [System.NonSerialized] public string shell_company_description = "Increase the Stamina cost of Dodging, but decrease the Mana cost of Blocking";
    [System.NonSerialized] public string close_call = "Close Call";
    [System.NonSerialized] public string close_call_description = "Increase all Physical damage for a short time after a Perfect Dodge";
    [System.NonSerialized] public string arcane_remains = "Arcane Remains";
    [System.NonSerialized] public string arcane_remains_description = "Increase damage of all attacks when near your Remnant. Collecting your Remnant restores you to full health";
    [System.NonSerialized] public string arcane_step = "Arcane Step";
    [System.NonSerialized] public string arcane_step_description = "You can dodge through enemies. Leave a trail of Arcane damage when you dodge";
    [System.NonSerialized] public string confident_killer = "Confident Killer";
    [System.NonSerialized] public string confident_killer_description = "Increase the damage of all attacks when you have full health";
    [System.NonSerialized] public string adrenaline_rush = "Adrenaline Rush";
    [System.NonSerialized] public string adrenaline_rush_description = "Regain all Stamina after a Perfect Dodge";
    [System.NonSerialized] public string rending_blows = "Rending Blows";
    [System.NonSerialized] public string rending_blows_description = "Heavy Attacks inflict Arcane damage over time";
    [System.NonSerialized] public float rendingBlowsDuration = 5;
    [System.NonSerialized] public string durable_gem = "Durable Gem";
    [System.NonSerialized] public string durable_gem_description = "Your healing gem can be used an extra time without breaking";
    [System.NonSerialized] public string protective_barrier = "Protective Barrier";
    [System.NonSerialized] public string protective_barrier_description = "Every 10 seconds gain a protective barrier that blocks one instance of damage";
    [System.NonSerialized] public string explosive_healing = "Explosive Healing";
    [System.NonSerialized] public string explosive_healing_description = "Deal Arcane damage to nearby enemies whenever you use your Healing Gem";
    [System.NonSerialized] public string charons_obol = "Charon's Obol";
    [System.NonSerialized] public string charons_obol_description = "When you die you do not leave behind a Remnant and you retain half of your current money";
    [System.NonSerialized] public string arcane_preservation = "Arcane Preservation";
    [System.NonSerialized] public string arcane_preservation_description = "If you are reduced to 0 health, lose Mana instead of dying";
    [System.NonSerialized] public string burning_reflection = "Burning Reflection";
    [System.NonSerialized] public string burning_reflection_description = "Deflecting spells inflicts Arcane damage over time";
    [System.NonSerialized] public string opportune_strike = "Opportune Strike";
    [System.NonSerialized] public string opportune_strike_description = "Attacking an enemy inflicted with damage over time deals extra damage";
    [System.NonSerialized] public string burning_cloak = "Burning Cloak";
    [System.NonSerialized] public string burning_cloak_description = "Inflict damage over time to any enemy that deals damage to you";
    [System.NonSerialized] public string mirror_cloak = "Mirror Cloak";
    [System.NonSerialized] public string mirror_cloak_description = "Every 5 seconds your dodge will also deflect spells";
    [System.NonSerialized] public string _spellsword = "Spellsword";
    [System.NonSerialized] public string _spellsword_description = "Your attacks deal extra Physical damage but consume Mana";
    [System.NonSerialized] public float spellswordManaCost = 15;
    [System.NonSerialized] public string death_aura = "Death Aura";
    [System.NonSerialized] public string death_aura_description = "Your regain Mana faster when near your Remnant. Collecting your Remnant restores you to full Mana";
    [System.NonSerialized] public string arcane_mastery = "Arcane Mastery";
    [System.NonSerialized] public string arcane_mastery_description = "Your Special Attacks do extra damage";
    [System.NonSerialized] public float arcaneMasteryPercent = 0.3f;
    [System.NonSerialized] public string reckless_attack = "Reckless Attack";
    [System.NonSerialized] public string reckless_attack_description = "Increase the damage of all attacks when you are low on health";

    [System.NonSerialized] public string way_faerie = "Way Faerie";
    [System.NonSerialized] public string way_faerie_description = "An ancient Faerie will guide you to your final destination";

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
            {reckless_attack, reckless_attack_description },
            {way_faerie, way_faerie_description }
        };
    }
}

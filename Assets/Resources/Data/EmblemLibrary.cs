using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Patches
{
    NONE,
    MAGICAL_ACCELERATION,
    HEAVY_BLOWS,
    VAMPIRIC_STRIKES,
    QUICKSTEP,
    PAY_RAISE,
    SHELL_COMPANY,
    CLOSE_CALL,
    ARCANE_REMAINS,
    ARCANE_STEP,
    CONFIDENT_KILLER,
    ADRENALINE_RUSH,
    RENDING_BLOWS,
    MAXIMUM_REFUND,
    PROTECTIVE_BARRIER,
    EXPLOSIVE_HEALING,
    STANDARD_DEDUCTION,
    ARCANE_PRESERVATION,
    BURNING_REFLECTION,
    OPPORTUNE_STRIKE,
    BURNING_CLOAK,
    MIRROR_CLOAK,
    SPELLSWORD,
    DEATH_AURA,
    ARCANE_MASTERY,
    RECKLESS_ATTACK
}

public class Patch
{
    public string name;
    public string description;
    public object value;
    public Patch(string patchName, string patchDescription, object otherVal = default(object))
    {
        name = patchName;
        description = patchDescription;
        value = otherVal;
    }
}

[CreateAssetMenu]
public class EmblemLibrary : ScriptableObject
{
    [SerializeField] PlayerData playerData;
    [System.NonSerialized] public Dictionary<string, string> emblemDictionary;
    Dictionary<Patches, Patch> PatchDictionary;
    public Dictionary<Patches, Patch> patchDictionary
    {
        get
        {
            if(PatchDictionary == null)
            {
                PatchDictionary = new Dictionary<Patches, Patch>()
                {
                    {Patches.MAGICAL_ACCELERATION, magicalAcceleration },
                    {Patches.HEAVY_BLOWS, heavyBlows },
                    {Patches.VAMPIRIC_STRIKES, vampiricStrikes },
                    {Patches.QUICKSTEP, quickstep },
                    {Patches.PAY_RAISE, payRaise },
                    {Patches.SHELL_COMPANY, shellCompany },
                    {Patches.CLOSE_CALL, closeCall },
                    {Patches.ARCANE_REMAINS, arcaneRemains },
                    {Patches.ARCANE_STEP, arcaneStep },
                    {Patches.CONFIDENT_KILLER, confidentKiller },
                    {Patches.ADRENALINE_RUSH, adrenalineRush },
                    {Patches.RENDING_BLOWS, rendingBlows },
                    {Patches.MAXIMUM_REFUND, maximumRefund },
                    {Patches.PROTECTIVE_BARRIER, protectiveBarrier },
                    {Patches.EXPLOSIVE_HEALING, explosiveHealing },
                    {Patches.STANDARD_DEDUCTION, standardDeduction },
                    {Patches.ARCANE_PRESERVATION, arcanePreservation },
                    {Patches.BURNING_REFLECTION, burningReflection },
                    {Patches.OPPORTUNE_STRIKE, opportuneStrike },
                    {Patches.BURNING_CLOAK, burningCloak },
                    {Patches.MIRROR_CLOAK, mirrorCloak },
                    {Patches.SPELLSWORD, spellsword },
                    {Patches.DEATH_AURA, deathAura },
                    {Patches.ARCANE_MASTERY, arcaneMastery },
                    {Patches.RECKLESS_ATTACK, recklessAttack },
                };
            };
            return PatchDictionary;
        }
    }
    public List<Patches> firstFloorPatches;
    public List<Patches> secondFloorPatches;
    public List<Patches> thirdFloorPatches;

    [System.NonSerialized] public Patch magicalAcceleration = new Patch("Magical Acceleration",
        "Mana recharges faster", (delay: 0.5f, charge: 1f));

    [System.NonSerialized] public Patch heavyBlows = new Patch("Heavy Blows",
        "Attacks stagger enemies faster", 1.5f);

    [System.NonSerialized] public Patch vampiricStrikes = new Patch("Vampiric Strikes",
        "Refund health whenever you kill an enemy", 0.125f);

    [System.NonSerialized] public Patch quickstep = new Patch("Quickstep",
        "Dodging does not use as much Stamina", 0.8f);

    [System.NonSerialized] public Patch payRaise = new Patch("Pay Raise",
        "Earn more money each time you kill an enemy", 1.25f);

    [System.NonSerialized] public Patch shellCompany = new Patch("Shell Company",
        "Increase the Stamina cost of Dodging, but decrease the Mana cost of Blocking", (dodge: 1.2f, block:0.5f));

    [System.NonSerialized] public Patch closeCall = new Patch("Close Call",
       "Increase all Physical damage for a short time after a Perfect Dodge", 0.2f);

    [System.NonSerialized] public Patch arcaneRemains = new Patch("Arcane Remains",
        "Increase damage of all attacks when near your Remnant. Collecting your Remnant restores you to full health", 0.5f);

    [System.NonSerialized] public Patch arcaneStep = new Patch("Arcane Step",
    "You can dodge through enemies. Leave a trail of Arcane damage when you dodge");

    [System.NonSerialized] public Patch confidentKiller = new Patch("Confident Killer",
    "Increase the damage of all attacks when you have full health", 0.4f);

    [System.NonSerialized] public Patch adrenalineRush = new Patch("Adrenaline Rush",
    "Regain all Stamina after a Perfect Dodge");

    [System.NonSerialized] public Patch rendingBlows = new Patch("Rending Blows",
    "Heavy Attacks inflict damage over time", 5f);

    [System.NonSerialized] public Patch maximumRefund = new Patch("Maximum Refund",
    "Your Refund Stone can be used an extra time without breaking");

    [System.NonSerialized] public Patch protectiveBarrier = new Patch("Protective Barrier",
    "Every 10 seconds gain a protective barrier that blocks one instance of damage", 10f);

    [System.NonSerialized] public Patch explosiveHealing = new Patch("Explosive Healing",
    "Deal Arcane damage to nearby enemies whenever you use your Refund Stone");

    [System.NonSerialized] public Patch standardDeduction = new Patch("Standard Deduction",
    "When you die you do not leave behind a Remnant and you retain half of your current money");

    [System.NonSerialized] public Patch arcanePreservation = new Patch("Arcane Preservation",
    "If you are reduced to 0 health, lose Mana instead of dying");

    [System.NonSerialized] public Patch burningReflection = new Patch("Burning Reflection",
    "Deflecting spells inflicts Arcane damage over time", 9f);

    [System.NonSerialized] public Patch opportuneStrike = new Patch("Opportune Strike",
    "Attacking an enemy inflicted with damage over time deals extra damage", 0.2f);

    [System.NonSerialized] public Patch burningCloak = new Patch("Burning Cloak",
    "Inflict damage over time to any enemy that deals damage to you", 5f);

    [System.NonSerialized] public Patch mirrorCloak = new Patch("Mirror Cloak",
    "Every 5 seconds your dodge will also deflect spells", 5f);

    [System.NonSerialized] public Patch spellsword = new Patch("Spellsword",
    "Your attacks deal extra Physical damage but consume Mana", (mana:15f, damage:0.5f));

    [System.NonSerialized] public Patch deathAura = new Patch("Death Aura",
    "You regain Mana faster when near your Remnant. Collecting your Remnant restores you to full Mana", (delay: 0.5f, charge: 1f));

    [System.NonSerialized] public Patch arcaneMastery = new Patch("Arcane Mastery",
    "Your Special Attacks do extra damage", 0.3f);

    [System.NonSerialized] public Patch recklessAttack = new Patch("Reckless Attack",
    "Increase the damage of all attacks when you are low on health", (threshold: 0.3f, damage: 0.6f));

    public List<Patches> GetPatchesFromStrings(List<string> patches)
    {
        List<Patches> output = new List<Patches>();
        List<Patches> allPatches = patchDictionary.Keys.ToList();
        foreach (string patch in patches)
        {
            try
            {
                output.Add((Patches)System.Enum.Parse(typeof(Patches), patch));
            }
            catch
            {
                output.Add(allPatches.Find(x => patchDictionary[x].name == patch));
            }
        }
        return output;
    }

    public List<string> GetStringsFromPatches(List<Patches> patches)
    {
        return patches.Select(patch => patch.ToString()).ToList();
    }
}

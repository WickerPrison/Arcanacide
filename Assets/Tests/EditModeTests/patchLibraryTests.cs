using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class patchLibraryTests
{
    List<Patches> patchesEnums = new List<Patches>()
    {
        Patches.ADRENALINE_RUSH,
        Patches.BURNING_CLOAK,
        Patches.DEATH_AURA,
    };

    List<string> patchesStrings = new List<string>()
    {
        "ADRENALINE_RUSH",
        "BURNING_CLOAK",
        "DEATH_AURA",
    };

    List<string> oldFormat = new List<string>()
    {
        "Adrenaline Rush",
        "Burning Cloak",
        "Death Aura",
    };

    EmblemLibrary emblemLibrary = Resources.Load<EmblemLibrary>("Data/EmblemLibrary");

    [Test]
    public void GetStringFromEnum()
    {
        Assert.AreEqual(patchesStrings, emblemLibrary.GetStringsFromPatches(patchesEnums));
    }

    [Test]
    public void GetEnumFromStrings()
    {
        Assert.AreEqual(patchesEnums, emblemLibrary.GetPatchesFromStrings(patchesStrings));
    }

    [Test]
    public void GetEnumFromOldFormatStrings()
    {
        Assert.AreEqual(patchesEnums, emblemLibrary.GetPatchesFromStrings(oldFormat));
    }

    [Test]
    public void GetEnumFromOldFormatStringsWithOldName()
    {
        List<string> oldFormatRename = new List<string>(oldFormat);
        oldFormatRename.Add("Charon's Obol");
        List<Patches> enumsRename = new List<Patches>(patchesEnums);
        enumsRename.Add(Patches.STANDARD_DEDUCTION);

        Assert.AreEqual(enumsRename, emblemLibrary.GetPatchesFromStrings(oldFormatRename));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityMetadataRow
{
    public AbilityReference AbilityReference { get; }
    public int Damage { get; }
    public int Heal { get; }
    public int Shield { get; }

    public AbilityMetadataRow(AbilityReference abilityReference, int damage, int heal, int shield)
    {
        AbilityReference = abilityReference;
        Damage = damage;
        Heal = heal;
        Shield = shield;
    }
}

public class CsvReader : MonoBehaviour
{
    private const string AbilityMetadataFilepath = "AbilityMetadata";

    private void Start()
    {
        TextAsset abilityMetadata = Resources.Load<TextAsset>(AbilityMetadataFilepath);

        List<string> dataRows = abilityMetadata.text.Split('\n').ToList();
        RemoveUnneededRows(dataRows);

        List<AbilityMetadataRow> metadataRows = dataRows
            .Where(r =>
            {
                if (IsValidRow(r)) return true;
                Debug.LogError($"Invalid ability metadata row: \"{r}\"");
                return false;
            })
            .Select(GetMetadataRow)
            .ToList();
    }

    private void RemoveUnneededRows(List<string> dataRows)
    {
        dataRows.RemoveAt(0);
        dataRows.RemoveAll(string.IsNullOrEmpty);
    }

    private bool IsValidRow(string dataRow)
    {
        string[] rowElements = dataRow.Split(',');
        return Enum.TryParse<AbilityReference>(rowElements[0], out _) &&
               int.TryParse(rowElements[1], out _) &&
               int.TryParse(rowElements[2], out _) &&
               int.TryParse(rowElements[3], out _);
    }

    private AbilityMetadataRow GetMetadataRow(string dataRow)
    {
        string[] rowElements = dataRow.Split(',');
        AbilityReference abilityReference = (AbilityReference) Enum.Parse(typeof(AbilityReference), rowElements[0]);
        int damage = int.Parse(rowElements[1]);
        int heal = int.Parse(rowElements[2]);
        int shield = int.Parse(rowElements[3]);
        
        return new AbilityMetadataRow(abilityReference, damage, heal, shield);
    }
}

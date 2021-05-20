using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurrencyLookup))]
public class CurrencyLookupEditor : Editor
{
    private CurrencyLookup currencyLookup;
    
    public override void OnInspectorGUI()
    {
        currencyLookup = (CurrencyLookup) target;

        EditorUtils.ShowScriptLink(currencyLookup);
        
        EditEnemyCurrencyValueLookup();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditEnemyCurrencyValueLookup()
    {
        EditorUtils.Header("Enemy Currency Values");
        EditorGUI.indentLevel++;
    
        EnumUtils.ForEach<ActorType>(actorType =>
        {
            if (actorType == ActorType.Player) return;

            currencyLookup.EnemyCurrencyValueLookup[actorType] = EditorGUILayout.IntField(
                actorType.ToString(),
                currencyLookup.EnemyCurrencyValueLookup[actorType]
            );
        });
        
        EditorGUI.indentLevel--;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public BattleMenuItem BattleMenuItemPF;
    private List<BattleMenuItem> ActiveBattleMenuItems { get; set; } = new List<BattleMenuItem> ();

    public Transform ButtonParent;

    public void AddMenuItem(string label, IEnumerator buttonAction)
    {
        BattleMenuItem newItem = Instantiate(BattleMenuItemPF, ButtonParent);
        newItem.ButtonTextLabel = label;
        newItem.ButtonInstance.onClick.AddListener(() => StartCoroutine(buttonAction));
        ActiveBattleMenuItems.Add (newItem);
    }

    public void ClearItems()
    {
        for (int ii = ActiveBattleMenuItems.Count - 1; ii >= 0; ii--)
        {
            // TODO: Object pooling
            Destroy(ActiveBattleMenuItems[ii].gameObject);
        }

        ActiveBattleMenuItems.Clear();
    }
}

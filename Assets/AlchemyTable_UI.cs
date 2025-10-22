using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlchemyTable_UI : MonoBehaviour
{
    [System.Serializable]
    public class AlchemyTable_StatUIs
    {
        public GameObject UI_Obj;
        public Sprite icon_normal;
        public Sprite icon_unavailable;
        public PlayerStats.PlayerStatsTypes stat;
        [HideInInspector] public bool isSelected;
    }
    public List<AlchemyTable_StatUIs> statUIs;
    public TextMeshProUGUI lifeEnergyDisplay;
    public TextMeshProUGUI lifeEnergyCostDisplay;
}

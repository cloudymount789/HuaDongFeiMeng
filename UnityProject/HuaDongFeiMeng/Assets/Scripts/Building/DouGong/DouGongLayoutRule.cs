using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HuadongFeimeng/DouGong/LayoutRule", fileName = "DouGongLayoutRule_")]
public class DouGongLayoutRule : ScriptableObject
{
    public enum LayoutType
    {
        ColumnHead,
        BetweenColumns,
        Corner
    }

    [Serializable]
    public class SlotRule
    {
        public string slotId;
        public DouGongPartData.AtomType[] allowedAtoms;
        public bool required;
    }

    public LayoutType layoutType;
    public List<SlotRule> slots = new List<SlotRule>();
}


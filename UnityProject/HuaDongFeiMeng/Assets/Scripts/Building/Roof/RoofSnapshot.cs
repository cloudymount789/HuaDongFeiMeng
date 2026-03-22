using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoofSnapshot
{
    public enum ElementType
    {
        Beam,
        Rafter,
        Tile,
        RidgeDecor
    }

    public class Element
    {
        public ElementType type;
        public Vector3 position;
        public Quaternion rotation;
    }

    public readonly List<Element> elements = new List<Element>();

    public int Count(ElementType type) => elements.Count(e => e.type == type);

    public Element LastOfType(ElementType type)
    {
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            if (elements[i].type == type) return elements[i];
        }
        return null;
    }

    public RoofSnapshot Clone()
    {
        var c = new RoofSnapshot();
        foreach (var e in elements)
        {
            c.elements.Add(new Element { type = e.type, position = e.position, rotation = e.rotation });
        }
        return c;
    }
}


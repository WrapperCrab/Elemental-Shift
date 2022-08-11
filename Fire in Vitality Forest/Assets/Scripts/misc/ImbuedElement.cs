using UnityEngine;

public enum Element { NONE, WATER, EARTH, FIRE, AIR }


[CreateAssetMenu(fileName = "New Item", menuName = "Element")]
public class ImbuedElement : ScriptableObject
{
    public Element element;


    public string getName()
    {
        switch (element)//Will this work?
        {
            case (Element.NONE):
            default:
                return "none";
            case (Element.WATER):
                return "water";
            case (Element.EARTH):
                return "earth";
            case (Element.FIRE):
                return "fire";
            case (Element.AIR):
                return "air";
        }
    }
}


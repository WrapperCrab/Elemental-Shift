using UnityEngine;

public enum Element { k, R, G, B, c, y, m }


[CreateAssetMenu(fileName = "New Item", menuName = "Element")]
public class ImbuedElement : ScriptableObject
{
    public Element element;


    public string getAbrev()
    {
        return element.ToString();
    }

    public string getName()
    {
        switch (element)
        {
            case (Element.k):
            default:
                return "Black";
            case (Element.R):
                return "Red";
            case (Element.G):
                return "Green";
            case (Element.B):
                return "Blue";
            case (Element.c):
                return "Cyan";
            case (Element.y):
                return "Yellow";
            case (Element.m):
                return "Magenta";
        }
    }
}


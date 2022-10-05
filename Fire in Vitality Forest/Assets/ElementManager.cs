using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { k, R, G, B, c, y, m }

public class ElementManager : MonoBehaviour
{
    #region Singleton

    public static ElementManager instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ElementManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    //This class controls element addition, element names and abbreviations and hues.
    Dictionary<Element, Tuple<string, string, Color, int, bool>> elementDict = new Dictionary<Element, Tuple<string, string, Color, int, bool>>()
    {
        //{Element, (abrev, name, hue, rgb base 4 number, isPrimary)}
        {Element.k, ("k", "Black", Color.black, 0, false) },
        {Element.R, ("R", "Red", Color.red, 4, true) },
        {Element.G, ("G", "Green", Color.green, 2, true) },
        {Element.B, ("B", "Blue", Color.blue, 1, true) },
        {Element.c, ("c", "Cyan", Color.cyan, 3, false) },
        {Element.y, ("y", "Yellow", Color.yellow, 6, false) },
        {Element.m, ("m", "Magenta", Color.magenta, 5, false) }
    };
    Dictionary<int, Element> numToElement = new Dictionary<int, Element>()
    {
        //{rgb base 4 number, Element}
        {0, Element.k},
        {1, Element.B },
        {2, Element.G },
        {3, Element.c },
        {4, Element.R },
        {5, Element.m },
        {6, Element.y }
    };

    public Element combineColors(Element color1, Element color2)
    {
        //check if same color
        if (color1 == color2)
        {
            return color1;
        }

        //find if each color is primary
        bool isPrim1 = elementDict[color1][4];
        bool isPrim2 = elementDict[color2][4];

        //add the color's mod 7
        int num1 = elementDict[color1][3];
        int num2 = elementDict[color2][3];

        int sum = (num1 + num2) % 7;

        if (isPrim1 == isPrim2)
        {//these are both primary or both secondary
            //return element with sum as their base 4 representation
            return numToElement[sum];
        }
        else
        {//these are a primary and a secondary
            if (sum == 0)
            {
                return numToElement[sum];
            }
            else
            {//these are adjacent elements. Return the primary one
                if (isPrim1)
                {
                    return color1;
                }
                else
                {
                    return color2;
                }
            }
        }
    }



}

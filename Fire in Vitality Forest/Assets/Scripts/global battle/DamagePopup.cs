using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    float timeLeftAlive = 1f;//time before this disappears in seconds
    Color color;

    TextMeshPro textMesh;
    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }


    public void setup(int damage, Color color)
    {
        textMesh.SetText(damage.ToString());
        textMesh.color = color;
    }

    /*public DamagePopup spawnPopup(Vector3 position, int damage, Color color)
    {//spawns popup at specified position with damage and color
        Transform popup = Instantiate(gameObject.transform, position, Quaternion.identity);
        DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
        damagePopup.setup(damage, color);
        return damagePopup;
    }*/

    void Update()
    {
        timeLeftAlive -= Time.deltaTime;
        if (timeLeftAlive < 0)
        {
            Destroy(gameObject);
        }
    }

}

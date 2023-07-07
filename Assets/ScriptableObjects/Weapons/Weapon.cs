using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon" , menuName = "CustomMenu / Weapon")]
public class Weapon : ScriptableObject
{
    public string Name;
    public float BloodDamage;
    public float StunDamage;
    public Sprite DisplayImage;

}

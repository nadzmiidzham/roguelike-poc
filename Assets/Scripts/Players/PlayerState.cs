using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "State/PlayerState")]
public class PlayerState : ScriptableObject
{
    public float hp, ep;
    public float maxHp, maxEp;
    public float atk, def, spd;
}

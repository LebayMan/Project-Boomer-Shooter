using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonEnemy : MonoBehaviour
{
    [Header("Ref")]
    public Enemy enemyscript;

    public void Hit(float Damage_Point)
    {
        if (enemyscript != null)
        {
            enemyscript.Hit(Damage_Point);
        }
    }
}

using UnityEngine;
using System.Collections;

public class TurretAttackCone : MonoBehaviour {

    public TurretAI turret;
    public bool isRight = false;

    void Awake()
    {
        turret = gameObject.GetComponentInParent<TurretAI>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            turret.Attack(isRight);
        }
    }
}

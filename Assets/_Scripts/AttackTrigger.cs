using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

    public int damage = 2;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger && col.CompareTag("Enemy")){
            col.SendMessageUpwards("Damage", damage);
        }
    }
}

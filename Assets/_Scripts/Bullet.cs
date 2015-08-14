using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger)
        {
            if (col.CompareTag("Player") || col.CompareTag("Enemy"))
            {
                col.SendMessageUpwards("Damage", 1);
            }
            Destroy(gameObject);
        }
        else
        {
        }
    }

}

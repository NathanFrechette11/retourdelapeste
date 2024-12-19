using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    // permet d'indiquer combien de temps ca prend avant que le projectile disparaisse
    public int tempsDisparition = 0;

    void Start()
    {
        // on detruit ce projectile apres que le temps avant disparition est écoulé
        Destroy(gameObject, tempsDisparition);
    }

    void OnCollisionEnter(Collision collision)
    {
        // quand le projectile entre en collision avec quoique ce soit, on le détruit
        Destroy(gameObject);
    }
}

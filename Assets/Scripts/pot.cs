using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // si cest le personnage qui active le trigger du pot...
        if (other.gameObject.CompareTag("Player"))
        {
            // on augmente le nombre de pot que le joueur a
            InfosMonde.instance.nbPot ++;
            // on appelle la fonction dans InfosMonde qui permet d'afficher l'inventaire du joueur
            InfosMonde.instance.AfficherInventaire();
            // on détruit ce pot
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleur : MonoBehaviour //la fleur donne un bonus de vitesse et augmente temporairement le rayon d'infection du personnage au contact du pollen.
{
   void OnTriggerEnter(Collider other)
    {
	    // si un gameobject avec comme tag "Player" active le trigger...
        if (other.CompareTag("Player"))
        {
	    // on va chercher le script MovePerso du gameobject
            MovePerso movePerso = other.GetComponent<MovePerso>();
	    // s'il y a bien un script MovePerso sur le gameobject, on lui active sa fonction DonnerBonus
            if(movePerso != null) movePerso.DonnerBonus();
            Debug.Log("bonus de vitesse et de contamination!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
	    // quand la fleur rentre en collision avec le sol...
        if (collision.gameObject.CompareTag("sol"))
        {
	        // le tag de la fleur passe de "fleur" à "fleurPosé"
            this.gameObject.tag = "fleurPosé";

	        // on indique aux informations du monde que l'item a été possé
            InfosMonde.instance.itemPosee = true;
            
	        // on lance la coroutine qui permet de détruire la fleur après un certain temps
            StartCoroutine(CoroutineAttente());
        }

	    // si cette fleur a comme tag "fleur" et qu'elle touche a une fleur qui a été posée...
        if (collision.gameObject.CompareTag("chariotPosé") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a un puit qui a été posé...
        else if (collision.gameObject.CompareTag("puitPosé") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a un fromage qui a été posé...
        else if (collision.gameObject.CompareTag("fromagePosé") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a une fleur qui a été posée...
        else if (collision.gameObject.CompareTag("fleurPosé") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a une fleur pas posée...
        else if (collision.gameObject.CompareTag("fleur") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a un chariot pas posé...
        else if (collision.gameObject.CompareTag("chariot") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a un fromage pas posé...
        else if (collision.gameObject.CompareTag("fromage") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
	    // si cette fleur a comme tag "fleur" et qu'elle touche a un puit pas posé...
        else if (collision.gameObject.CompareTag("puit") && this.gameObject.tag == "fleur")
        {
	        // on detruit cette fleur
            Destroy(this.gameObject);
        }
    }

    // coroutine qui permet d'attendre la destruction de la fleur une fois son travail fait
    IEnumerator CoroutineAttente()
    {
	    // permet de definir le temps qui sest écoulé depuis le lancement de la coroutine
        float tempsEcoule = 0f;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 10)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }

	    // on detruit cette fleur
        Destroy(gameObject);
    }
}

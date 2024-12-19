using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fromage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // quand le fromage rentre en collision avec le sol...
        if (collision.gameObject.CompareTag("sol") && InfosMonde.instance.vie <=2)
        {
            // on va chercher le rigidbody du fromage
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            // le tag du fromage passe de "fromage" à "fromagePosé"
            this.gameObject.tag = "fromagePosé";
        
            // on donne la position du fromage dans le script d'informations du monde
            InfosMonde.instance.posFromage = gameObject.transform.position;
            // on indique aux informations du monde que l'item a été possé
            InfosMonde.instance.itemPosee = true;
        }
        // on lance la coroutine qui permet de détruire le fromage après un certain temps
        StartCoroutine(AttenteFromage());

        // si ce fromage a comme tag "fromage" et quil touche a un chariot qui a été posé...
        if (collision.gameObject.CompareTag("chariotPosé") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a un puit qui a été posé...
        else if (collision.gameObject.CompareTag("puitPosé") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a un fromage qui a été posé...
        else if (collision.gameObject.CompareTag("fromagePosé") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a une fleur qui a été posée...
        else if (collision.gameObject.CompareTag("fleurPosé") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a un puit pas posé...
        else if (collision.gameObject.CompareTag("puit") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a un chariot pas posé...
        else if (collision.gameObject.CompareTag("chariot") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a un fromage pas posé...
        else if (collision.gameObject.CompareTag("fromage") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
        // sinon si ce fromage a comme tag "fromage" et quil touche a une fleur pas posée...
        else if (collision.gameObject.CompareTag("fleur") && this.gameObject.tag == "fromage")
        {
            // on detruit ce fromage
            Destroy(this.gameObject);
        }
    }

    // coroutine qui permet d'attendre la destruction du fromage une fois son travail fait
    IEnumerator AttenteFromage()
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

        // on detruit ce fromage
        Destroy(this.gameObject);
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Chariot : MonoBehaviour
{
    // permet de fournir la sphere qui infecte les cube dans l'inspecteur
    public GameObject sphere;
    // permet d'indiquer cest a quelle taille que l'on veut que la sphere a au debut
    public float tailleInitiale = 0f;
    // permet d'indiquer cest a quelle taille que l'on veut que la sphere a la fin
    public float tailleFinale = 8f;
    // permet d'indiquer cest quoi la duree sur laquelle on veut le changement de taille
    public float duree = 30f;

    void Start()
    {
        // quand le chariot est instancier, il lance la coroutine qui lui permet de faire grandir sa zone d'infection
        StartCoroutine(CoroutineGrandir());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // quand le chariot rentre en collision avec le sol...
        if (collision.gameObject.CompareTag("sol"))
        {
            // on accede au rigidbody du chariot
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            // si le rigidbody nest pas null...
            if (rigidbody != null)
            {
                // on bloque la rotation du chariot sur l'axe des x
                rigidbody.constraints |= RigidbodyConstraints.FreezeRotationX;

                // on bloque la rotation du chariot sur l'axe des y
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
            }
            // le tag du chariot passe de "chariot" à "chariotPosé"
            this.gameObject.tag = "chariotPosé";
            
            // on indique aux informations du monde que l'item a été possé
            InfosMonde.instance.itemPosee = true;
        }

        // si ce chariot a comme tag "chariot" et quil touche a un chariot qui a été posé...
        if (collision.gameObject.CompareTag("chariotPosé") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a un puit qui a été posé...
        else if (collision.gameObject.CompareTag("puitPosé") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a un fromage qui a été posé...
        else if (collision.gameObject.CompareTag("fromagePosé") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a une fleur qui a été posée...
        else if (collision.gameObject.CompareTag("fleurPosé") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a un puit pas posé...
        else if (collision.gameObject.CompareTag("puit") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a un chariot pas posé...
        else if (collision.gameObject.CompareTag("chariot") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a un fromage pas posé...
        else if (collision.gameObject.CompareTag("fromage") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
        // sinon si ce chariot a comme tag "chariot" et quil touche a une fleur pas posée...
        else if (collision.gameObject.CompareTag("fleur") && this.gameObject.tag == "chariot")
        {
            // on detruit ce chariot
            Destroy(this.gameObject);
        }
    }

    // coroutine qui, une fois appellé, permet de faire grandir la sphere du chariot a partir de la taille initial souhaité
    // a la taille finale souhaité, et ce, sur la duree indiqué
    IEnumerator CoroutineGrandir()
    {
        // on garde combien de temps il s'est écoulé depuis le début de la coroutine
        float tempsEcoule = 0f;

        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < duree)
        {
            // Calcul de l'interpolation linéaire entre la taille initiale et la taille finale
            float tailleActuelle = Mathf.Lerp(tailleInitiale, tailleFinale, tempsEcoule / duree);
            // on transforme le scale de la sphere selon la taille actuelle quelle devrait avoir avec le temps
            sphere.transform.localScale = new Vector3(tailleActuelle, tailleActuelle, tailleActuelle);

            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }

        // S'assurer que le scale de la sphere est egal a la taille final souhaité
        sphere.transform.localScale = new Vector3(tailleFinale, tailleFinale, tailleFinale);

        // on attend pendant 2 secondes
        yield return new WaitForSeconds(2f);

        // et on détruit le chariot une fois son travail accomplie
        Destroy(this.gameObject);
    }
}

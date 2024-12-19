using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    // permet de definir limage du chariot pour linventaire
    public RawImage imageChariot;
    // permet de definir limage du puit pour linventaire
    public RawImage imagePuit;
    // permet de definir limage du fromage pour linventaire
    public RawImage imageFromage;
    // permet de definir limage de la fleur pour linventaire
    public RawImage imageFleur;
    // permet de definir limage du pot pour linventaire
    public RawImage imagePot;

    // definit que si le chariot est en attente pour etre instancié
    bool chariotEnAttente = false;
    // definit que si le puit est en attente pour etre instancié
    bool puitEnAttente = false;
    // definit que si le fromage est en attente pour etre instancié
    bool fromageEnAttente = false;
    // definit que si la fleur est en attente pour etre instancié
    bool fleurEnAttente = false;

    // permet de definir le generateur de lile dans l'inspecteur
    public GenerateurDile generateurDile;

    // Update is called once per frame
    void Update()
    {
        // si le joueur appuie sur la touche 1 et que le chariot nest pas en attente et que le joueur a au moins 1 chariot...
        if(Input.GetButtonDown("1") && chariotEnAttente == false && InfosMonde.instance.nbChariot > 0)
        {
            // appelle la fonction qui permet de placer un item en lui disant que cest le chariot
            PlacerObjet("chariot");
            // on commence la coroutine qui part lattente entre 2 placement de chariot
            StartCoroutine(CoroutineAttenteChariot());
        }
        // si le joueur appuie sur la touche 2 et que le puit nest pas en attente et que le joueur a au moins 1 puit...
        if(Input.GetButtonDown("2") && puitEnAttente == false && InfosMonde.instance.nbPuit > 0)
        {
            // appelle la fonction qui permet de placer un item en lui disant que cest le puit
            PlacerObjet("puit");
            // on commence la coroutine qui part lattente entre 2 placement de puit
            StartCoroutine(CoroutineAttentePuit());
        }
        // si le joueur appuie sur la touche 3 et que le fromage nest pas en attente et que le joueur a au moins 1 fromage...
        if(Input.GetButtonDown("3") && fromageEnAttente == false && InfosMonde.instance.nbFromage > 0)
        {
            // appelle la fonction qui permet de placer un item en lui disant que cest le fromage
            PlacerObjet("fromage");
            // on commence la coroutine qui part lattente entre 2 placement de fromage
            StartCoroutine(CoroutineAttenteFromage());
        }
        // si le joueur appuie sur la touche 4 et que la fleur nest pas en attente et que le joueur a au moins 1 fleur...
        if(Input.GetButtonDown("4") && fleurEnAttente == false && InfosMonde.instance.nbFleur > 0)
        {
            // appelle la fonction qui permet de placer un item en lui disant que cest la fleur
            PlacerObjet("fleur");
            // on commence la coroutine qui part lattente entre 2 placement de fleur
            StartCoroutine(CoroutineAttenteFleur());
        }

        // si le nombre de pot est dau moins de 1...
        if(InfosMonde.instance.nbPot > 0)
        {
            // on met lalpha de limage a 100%
            imagePot.color = new Color(1,1,1,1);
        }
        // sinon...
        else
        {
            // on met lalpha de limage a 75%
            imagePot.color = new Color(1,1,1, 0.75f);
        }

        // si le nombre de chariot est dau moins de 1...
        if(InfosMonde.instance.nbChariot > 0)
        {
            // si le chariot nest pas en attente...
            if(chariotEnAttente == false)
            {
                // on met lalpha de limage a 100%
                imageChariot.color = new Color(1, 1, 1, 1);
            }
        }
        // sinon...
        else
        {
            // on met lalpha de limage a 75%
            imageChariot.color = new Color(1, 1, 1, 0.75f);
        }

        // si le nombre de puit est dau moins de 1...
        if(InfosMonde.instance.nbPuit > 0)
        {
            // si le puit nest pas en attente...
            if(puitEnAttente == false)
            {
                // on met lalpha de limage a 100%
                imagePuit.color = new Color(1, 1, 1, 1);
            }
        }
        // sinon...
        else
        {
            // on met lalpha de limage a 75%
            imagePuit.color = new Color(1, 1, 1, 0.75f);
        }

        // si le nombre de fromage est dau moins de 1...
        if(InfosMonde.instance.nbFromage > 0)
        {
            // si le fromage nest pas en attente...
            if(fromageEnAttente == false)
            {
                // on met lalpha de limage a 100%
                imageFromage.color = new Color(1, 1, 1, 1);
            }
        }
        // sinon...
        else
        {
            // on met lalpha de limage a 75%
            imageFromage.color = new Color(1, 1, 1, 0.75f);
        }

        // si le nombre de fleur est dau moins de 1...
        if(InfosMonde.instance.nbFleur > 0)
        {
            // si la fleur nest pas en attente...
            if(fleurEnAttente == false)
            {
                // on met lalpha de limage a 100%
                imageFleur.color = new Color(1, 1, 1, 1);
            }
        }
        // sinon...
        else
        {
            // on met lalpha de limage a 75%
            imageFleur.color = new Color(1, 1, 1, 0.75f);
        }
    }

    // fonction qui permet de placer un objet sur la scene selon le string quil recoit
    public void PlacerObjet(string typeObjet)
    {
        // on instancie l'objet recu a la position du perso
        Instantiate((GameObject)Resources.Load("objets/"+typeObjet),
                new Vector3(generateurDile.perso.transform.position.x, generateurDile.perso.transform.position.y,
                generateurDile.perso.transform.position.z), Quaternion.identity);
    }

    // coroutine qui permet de gerer lattente de lapparition du prochain chariot
    IEnumerator CoroutineAttenteChariot()
    {
        // definit le temps ecoulé depuis le debut de la coroutine
        float tempsEcoule = 0f;
        // le chariot est en attente
        chariotEnAttente = true;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 10)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        // le chariot nest plus en attente
        chariotEnAttente = false;
    }

    // coroutine qui permet de gerer lattente de lapparition du prochain puit
    IEnumerator CoroutineAttentePuit()
    {
        // definit le temps ecoulé depuis le debut de la coroutine
        float tempsEcoule = 0f;
        // le puit est en attente
        puitEnAttente = true;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 10)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        // le puit nest plus en attente
        puitEnAttente = false;
    }

    // coroutine qui permet de gerer lattente de lapparition du prochain fromage
    IEnumerator CoroutineAttenteFromage()
    {
        // definit le temps ecoulé depuis le debut de la coroutine
        float tempsEcoule = 0f;
        // le fromage est en attente
        fromageEnAttente = true;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 10)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        // le fromage nest plus en attente
        fromageEnAttente = false;
    }

    // coroutine qui permet de gerer lattente de lapparition du prochain fleur
    IEnumerator CoroutineAttenteFleur()
    {
        // definit le temps ecoulé depuis le debut de la coroutine
        float tempsEcoule = 0f;
        // la fleur est en attente
        fleurEnAttente = true;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 10)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        // la fleur nest plus en attente
        fleurEnAttente = false;
    }
}

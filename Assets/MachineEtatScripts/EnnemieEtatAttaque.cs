using System.Collections;
using UnityEngine;

public class EnnemieEtatAttaque : EnnemieEtatsBase
{
    
    public override void initEtat(EnnemieEtatsManager ennemie)
    {
        // on lance lanimation dattaque de lennemie
        ennemie.animator.SetTrigger("attaque");
        // on lance la coroutine qui gere le comportement dattaque de lennemie
        ennemie.StartCoroutine(Attaque(ennemie));
    }
    
    public override void updateEtat(EnnemieEtatsManager ennemie)
    {

    }
    
    public override void TriggerEnterEtat(EnnemieEtatsManager ennemie, Collider col)
    {
        // quand lennemie est triggered par un projectile du perso...
        if(col.CompareTag("projectilePerso"))
        {
            // on diminue sa vie de 1
            ennemie.nbVie--;
            // on fait jouer le son de degat de lennemie a partir de DontDestroyWhenLoad
            DontDestroyWhenLoad.instance.JouerEffetSonore(ennemie.sonDegats);
            // si la vie de lennemie est a 0...
            if(ennemie.nbVie == 0)
            {
                // on appelle la fonction qui permet de replacer lennemie a sa zone initial
                ennemie.ReplacerEnnemie();
            }
        }
        // si lennemie est triggered par le puit
        if(col.CompareTag("puit"))
        {
            // on fait jouer le son splash de lennemie a partir de DontDestroyWhenLoad
            DontDestroyWhenLoad.instance.JouerEffetSonore(ennemie.sonSplash);
            // on fait partir la coroutine qui permet de tuer lennemie
            ennemie.StartCoroutine(FaireMourrirEnnemie(ennemie, col));
        }
    }

    public override void TriggerExitEtat(EnnemieEtatsManager ennemie, Collider col)
    {
        // si lennemie sort du trigger du puit...
        if(col.CompareTag("puit"))
        {
            // on arrete toutes les coroutines
            ennemie.StopAllCoroutines();
            // on appelle la fonction qui permet de changer detat vers repos
            ennemie.ChangerEtat(ennemie.repos);
        }
    }

    // coroutines qui permet de tuer l'ennemie sil reste trop longtemps dans la zone du puit apres un certain temps
    IEnumerator FaireMourrirEnnemie(EnnemieEtatsManager ennemie, Collider col)
    {
        // on attends 3 secondes
        yield return new WaitForSeconds(3f);
        // on detruit le puit qui a tué l'ennemie
        Object.Destroy(col.gameObject.transform.parent.gameObject);
        // et on appele la fonction qui change l'etat de lennemie pour letat de sa mort et qui arrete toutes les coroutines
        ChangerEtatMort(ennemie);
    }

    // coroutine qui gere le comportement dattaque de lennemie
    private IEnumerator Attaque(EnnemieEtatsManager ennemie)
    {
        // on attends une seconde
        yield return new WaitForSeconds(1f);
        // Instancie le projectile à une position légèrement devant le personnage
        GameObject projectile = Object.Instantiate(InfosMonde.instance.projectileEnnemie, ennemie.gameObject.transform.position + new Vector3(0,2f,0) + ennemie.gameObject.transform.forward * 1.5f, ennemie.gameObject.transform.rotation);
        // on va chercher le rigidody du projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        // si le rigidbody nest pas null...
        if (rb != null)
        {
            // on ajuste la vitesse a laquelle le projectile va se deplacer
            float vitesseProjectile = 8f;
            // Ajoute une force pour lancer le projectile vers l'avant
            rb.velocity = ennemie.gameObject.transform.forward * vitesseProjectile;
        }
        // on appelle la fonction qui permet de changer detat vers chasse
        ennemie.ChangerEtat(ennemie.chasse);
    }

    // fonction qui change l'etat de lennemie pour letat de sa mort et qui arrete toutes les coroutines
    void ChangerEtatMort(EnnemieEtatsManager ennemie)
    {
        // arrete toutes les coroutines
        ennemie.StopAllCoroutines();
        // appelle la fonction qui change letat de lennemie pour passer a letat de mort
        ennemie.ChangerEtat(ennemie.mort);
    }
}

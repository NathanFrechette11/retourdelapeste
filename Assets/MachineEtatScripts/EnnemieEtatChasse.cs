using System.Collections;
using UnityEngine;

public class EnnemieEtatChasse : EnnemieEtatsBase
{
    
    public override void initEtat(EnnemieEtatsManager ennemie)
    {
        // on met la piste musicale de base active
        DontDestroyWhenLoad.instance.tPistes[0].estActif = false;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
        // on met la piste musicale de l'attaque a inactive
        DontDestroyWhenLoad.instance.tPistes[1].estActif = true;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
        // lance lanimation de course de lennemie
        ennemie.animator.SetBool("enCourse", true);
        // on fait en sorte quil se dirige vers la position du personnage
        ennemie.agent.destination = ennemie.infos["perso"].transform.position;
        // on lance la coroutine qui gere le comportement de lennemie pendant son etat chasse
        ennemie.StartCoroutine(Chasse(ennemie));
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
        // si lennemie est triggered par le puit...
        if(col.CompareTag("puit"))
        {
            // on fait jouer le son splash de lennemie a partir de DontDestroyWhenLoad
            DontDestroyWhenLoad.instance.JouerEffetSonore(ennemie.sonSplash);
            // on fait partir la coroutine qui permet de tuer lennemie
            ennemie.StartCoroutine(FaireMourrirEnnemie(ennemie , col));
        }
    }

    public override void TriggerExitEtat(EnnemieEtatsManager ennemie, Collider col)
    {
        // si lennemie sort du trigger du puit...
        if(col.CompareTag("puit"))
        {
            // on appelle la fonction qui permet de changer detat et qui permet darreter toutes les coroutines
            ChangerEtatRepos(ennemie);
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

    // coroutine qui gere ce que lennemie fais dans son etat de chasse
    private IEnumerator Chasse(EnnemieEtatsManager ennemie)
    {
        // tant que l'etat actuel est letat de chasse...
        while(ennemie.etatActuel == ennemie.chasse)
        {
            // on met la piste musicale de base active
            DontDestroyWhenLoad.instance.tPistes[0].estActif = false;
            // on ajuste le volume de cette piste musicale
            DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
            // on met la piste musicale de l'attaque a inactive
            DontDestroyWhenLoad.instance.tPistes[1].estActif = true;
            // on ajuste le volume de cette piste musicale
            DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
            // on attend 1 seconde
            yield return new WaitForSeconds(1f);
            // on fait bouger lennemie vers la position actuelle du perso
            ennemie.agent.destination = ennemie.infos["perso"].transform.position;
            // on attend une seconde
            yield return new WaitForSeconds(1f);
            // si la distance entre ou lennemie est et sa destination est plus petite ou egal a 10...
            if(ennemie.agent.remainingDistance <= 10)
            {
                // on appelle la fonction qui permet de changer letat de lennemie pour son etat dattaque
                ennemie.ChangerEtat(ennemie.attaque);
            }
            // si la distance entre ou lennemie est et sa destination est plus grande que sa vision...
            if(ennemie.agent.remainingDistance > ennemie.infos["vision"])
            {
                // on fait en sorte que l'ennemie reste sur place
                ennemie.agent.destination = ennemie.agent.transform.position;
                // on appelle la fonction qui permet de changer detat vers repos et qui permet darreter toutes les coroutines
                ChangerEtatRepos(ennemie);
            }
        }
    }

    // fonction qui permet de changer detat vers repos et qui permet darreter toutes les coroutines
    void ChangerEtatRepos(EnnemieEtatsManager ennemie)
    {
        // arrete toutes les coroutines
        ennemie.StopAllCoroutines();
        // appelle la fonction qui change letat de lennemie pour passer a letat de repos
        ennemie.ChangerEtat(ennemie.repos);
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

using System.Collections;
using UnityEngine;

public class EnnemieEtatRepos : EnnemieEtatsBase
{
    
    public override void initEtat(EnnemieEtatsManager ennemie)
    {
        // on met la piste musicale de base active
        DontDestroyWhenLoad.instance.tPistes[0].estActif = true;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
        // on met la piste musicale de l'attaque a inactive
        DontDestroyWhenLoad.instance.tPistes[1].estActif = false;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
        // on arrete l'animation de course du lennemie
        ennemie.animator.SetBool("enCourse", false);

        // on lance la coroutine de repos de l'ennemie
        ennemie.StartCoroutine(Repos(ennemie));
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
            // on appelle la fonction qui permet de changer detat et qui permet darreter toutes les coroutines
            ChangerEtatChasse(ennemie);
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

    // coroutine qui gere ce que lennemie fais dans son etat de repos
    private IEnumerator Repos(EnnemieEtatsManager ennemie)
    {
        // tant que l'etat actuel est letat de repos...
        while(ennemie.etatActuel == ennemie.repos)
        {
            // si la distance entre lennemie et le personnage est plus petite que la vision de lennemie...
            if(Vector3.Distance(ennemie.transform.position, ennemie.infos["perso"].transform.position) < ennemie.infos["vision"])
            {
                // on appelle la fonction qui permet de changer detat vers chasse et qui permet darreter toutes les coroutines
                ChangerEtatChasse(ennemie);
            }
            // on attends 2 secondes
            yield return new WaitForSeconds(2f);
        }
    }

    // fonction qui permet de changer detat vers repos et qui permet darreter toutes les coroutines
    void ChangerEtatChasse(EnnemieEtatsManager ennemie)
    {
        // arrete toutes les coroutines
        ennemie.StopAllCoroutines();
        // appelle la fonction qui change letat de lennemie pour passer a letat de chasse
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

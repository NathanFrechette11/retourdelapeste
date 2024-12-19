using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieEtatMort : EnnemieEtatsBase
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
        // on enleve le tag de lennemie
        ennemie.tag = "Untagged";
        // on fait en sorte quil reste sur place
        ennemie.agent.destination = ennemie.agent.transform.position;
        // on arrete son animation de course
        ennemie.animator.SetBool("enCourse",false);
        // on part son animation de mort
        ennemie.animator.SetBool("estMort",true);
        // on commence la coroutine qui permet de faire disparaitre lennemie
        ennemie.StartCoroutine(AttenteMort(ennemie));
    }

    public override void updateEtat(EnnemieEtatsManager ennemie)
    {

    }
    
    public override void TriggerEnterEtat(EnnemieEtatsManager ennemie, Collider col)
    {
        
    }

    public override void TriggerExitEtat(EnnemieEtatsManager ennemie, Collider col)
    {
        
    }

    // coroutine qui permet de faire disparaitre l'ennemie apres 5 secondes et de faire apparaitre un pot la ou il se trouvait
    IEnumerator AttenteMort(EnnemieEtatsManager ennemie)
    {
        // on attend 5 secondes
        yield return new WaitForSeconds(5f);
        // on instancie un pot la ou lennemie est
        Object.Instantiate((GameObject)Resources.Load("objets/pot"),new Vector3(ennemie.transform.position.x, ennemie.transform.position.y + .5f,ennemie.transform.position.z), Quaternion.identity);
        // on detruit cet ennemie
        Object.Destroy(ennemie.gameObject);
    }
}

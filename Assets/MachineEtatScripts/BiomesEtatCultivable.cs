using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BiomesEtatCultivable : BiomesEtatsBase
{
    // quand le script est lancé...
   public override void initEtat(BiomesEtatsManager biome)
    {
        // on lance la coroutine d'attente
        biome.StartCoroutine(CoroutineAttente(biome));
    }
    // quand on update le script...
    public override void updateEtat(BiomesEtatsManager biome)
    {
        
    }
    // quand le joueur touche au cubes...
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider col)
    {

    }

    // coroutine qui gere lattente pendant lanimation du passage de letat activable a final
    public IEnumerator CoroutineAttente(BiomesEtatsManager biome)
    {
        // on attends 2.5 secondes
        yield return new WaitForSeconds(2.5f);
        // on change le materiel du cube pour son variant de debut dinfection
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Mats/b"+ biome.infos["quelBiome"] + "_2");
        // on attends 2.5 secondes
        yield return new WaitForSeconds(2.5f);
        // on attend pour une duree aleatoire entre 2 et 5 secondes
        yield return new WaitForSeconds(Random.Range(2f,5f));
        // on lance lanimation dinfection du cube
        biome._animator.SetTrigger("infection");
        // on attends 2.1 secondes
        yield return new WaitForSeconds(2.1f);
        // on appelle la fonction qui permet de changer letat du cube vers letat final
        biome.ChangerEtat(biome.final);
    }
}

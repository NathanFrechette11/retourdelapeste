using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BiomesEtatSemable : BiomesEtatsBase
{
    // permet de definir le gameobject de la bacterie
    GameObject bacterie;
    // permet de definir le gameobject de la bacterie qui est en developpenent
    GameObject bacterieEnDeveloppement;

    // quand le script est lancé...
   public override void initEtat(BiomesEtatsManager biome)
    {
        // on lance lanimation semable du cube
        biome._animator.SetTrigger("semable");
        // si on est dans le 10% des chances...
        if(Random.value*100 <= 10)
        {
            // on va chercher un type de bacterie aleatoire
            int typeBacterie = Random.Range(0,10);
            // on instancie la bactérie selon son type par dessus le cube
            bacterieEnDeveloppement = Object.Instantiate((GameObject)Resources.Load("Bacteries/b" + typeBacterie), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y + 3f, biome.infos["cube"].transform.position.z), Quaternion.identity);
            // on met son scale a 0
            bacterieEnDeveloppement.transform.localScale = Vector3.zero;
        }
        // on lance la coroutine qui permet de faire pousser la bacterie
        biome.StartCoroutine(CoroutinePousser(biome));

    }
    // quand on update le script...
    public override void updateEtat(BiomesEtatsManager biome)
    {

    }
    // quand le joueur touche au cubes...
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider col)
    {
        // si la sphere de l'ennemie touche au cube...
        if(col.CompareTag("sphereEnnemie"))
        {
            // on remet son materiel pour celui non contaminé
            biome.GetComponent<Renderer>().material = (Material)Resources.Load("Mats/b"+ biome.infos["quelBiome"] + "_1");
            // si le cube contient un item...
            if(biome.infos.ContainsKey("item") != false)
            {
                // si litem est litem 2...
                if(biome.infos["item"].name == "c2_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                    // on instancie la version de litem non infectee a la meme place
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c2_1"), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y + 1.6f, biome.infos["cube"].transform.position.z), Quaternion.identity);
                    // on fournie litem a la liste dinfos du cube
                    biome.infos["item"] = item;
                    // on met le parent de litem le cube
                    biome.infos["item"].transform.parent = biome.infos["cube"].transform;
                }
                // si litem est litem 3...
                if(biome.infos["item"].name == "c3_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                    // on instancie la version de litem non infectee a la meme place
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c3_1"), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y + 0.8f, biome.infos["cube"].transform.position.z), Quaternion.identity);
                    // on fournie litem a la liste dinfos du cube
                    biome.infos["item"] = item;
                    // on met le parent de litem le cube
                    biome.infos["item"].transform.parent = biome.infos["cube"].transform;
                }
                // si litem est litem 123456...
                if(biome.infos["item"].name == "c4_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                    // on instancie la version de litem non infectee a la meme place
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c4_1"), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y, biome.infos["cube"].transform.position.z), Quaternion.identity);
                    // on fournie litem a la liste dinfos du cube
                    biome.infos["item"] = item;
                    // on met le parent de litem le cube
                    biome.infos["item"].transform.parent = biome.infos["cube"].transform;
                }
                // si litem est litem 123456...
                if(biome.infos["item"].name == "c5_2(Clone)" || biome.infos["item"].name == "c6_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                    // on instancie la version de litem non infectee a la meme place
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c5_1"), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y + 0.8f, biome.infos["cube"].transform.position.z), Quaternion.identity);
                    // on fournie litem a la liste dinfos du cube
                    biome.infos["item"] = item;
                    // on met le parent de litem le cube
                    biome.infos["item"].transform.parent = biome.infos["cube"].transform;
                }
                // si litem est litem 123456...
                if(biome.infos["item"].name == "c1_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                    // on instancie la version de litem non infectee a la meme place
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c1_1"), new Vector3(biome.infos["cube"].transform.position.x + .8f, biome.infos["cube"].transform.position.y + 0.8f, biome.infos["cube"].transform.position.z - .5f), Quaternion.identity);
                    // on fournie litem a la liste dinfos du cube
                    biome.infos["item"] = item;
                    // on met le parent de litem le cube
                    biome.infos["item"].transform.parent = biome.infos["cube"].transform;
                }
                // si litem est litem 123456...
                if(biome.infos["item"].name == "c7_1(Clone)" || biome.infos["item"].name == "c7_2(Clone)")
                {
                    // on detruit litem
                    Object.Destroy(biome.infos["item"]);
                }

                // si le cube contient toujours un item...
                if(biome.infos["item"] != null)
                {
                    // on remet sa rotation a 0
                    biome.infos["item"].transform.rotation = Quaternion.Euler(0, 0, 0);
                    // on lui donne une rotation aleatoire sur laxe des y
                    biome.infos["item"].transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    // on reactive litem
                    biome.infos["item"].SetActive(true);
                }
                // sinon...
                else
                {
                    // on enleve litem de la liste dinfos
                    biome.infos.Remove("item");
                }
            }

            // on arrete toute les coroutines
            biome.StopAllCoroutines();
            // on detruit la bactérie
            Object.Destroy(bacterie);
            // on appelle la fonction qui permet de changer letat du cube vers activable
            biome.ChangerEtat(biome.activable);
        }
    }

    // coroutine qui permet de faire pousser la bacterie
    public IEnumerator CoroutinePousser(BiomesEtatsManager biome)
    {
        // definit un temps aleatoire entre 6 et 10 secondes
        float temps = Random.Range(6f, 10f);
        // on attend pendant ce temps aleatoire
        yield return new WaitForSeconds(temps);
        // s'il y a une bacterie en developpement...
        if(bacterieEnDeveloppement != null)
        {
            // Définit la cible d'échelle
            Vector3 targetScale = new Vector3(0.25f, 0.25f, 0.25f);
            // si cest le type 3 ou le type 4...
            if(bacterieEnDeveloppement.name == "b3(Clone)" || bacterieEnDeveloppement.name == "b4(Clone)")
            {
                // on diminue la grandeur finale de ces bacteries
                targetScale = new Vector3(0.10f, 0.10f, 0.10f);
            }

            // Continue jusqu'à ce que l'échelle atteigne la cible
            while(bacterieEnDeveloppement.transform.localScale != targetScale)
            {
                // Ajoute une petite quantité à l'échelle actuelle pour créer un effet de transition
                bacterieEnDeveloppement.transform.localScale = Vector3.MoveTowards(bacterieEnDeveloppement.transform.localScale, targetScale, 0.010f);

                // Pause pour le rafraîchissement de la scène
                yield return null;
            }
            // on donne la bacterie en developpement a bacterie
            bacterie = bacterieEnDeveloppement;
            // on indique que le cube contient une bacterie
            biome.contientBacterie = true;
            // on fournit la bacterie au cube
            biome.bacterie = bacterie;
        }
        // appelle la fonction qui permet de changer letat du cube vers letat recoltable
        biome.ChangerEtat(biome.recoltable);
    }
}

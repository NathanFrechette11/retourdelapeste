using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BiomesEtatRecoltable : BiomesEtatsBase
{

    // quand le script est lancé...
   public override void initEtat(BiomesEtatsManager biome)
    {
        
    }
    // quand on update le script...
    public override void updateEtat(BiomesEtatsManager biome)
    {

    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider col)
    {

        // quand le joueur touche au cubes...
        if(col.CompareTag("Player"))
        {
            // si le cube contient une bacterie
            if(biome.contientBacterie == true)
            {
                // on appelle la fonction qui permet dajouter des bacteries au nombre de bacteries ammasser
                biome.infos["monde"].GetComponent<InfosMonde>().AjouterBacterie();
                // on detruit cette bacterie
                Object.Destroy(biome.bacterie);
                // on fait jouer le son pop a partir de DontDestroyWhenLoad
                DontDestroyWhenLoad.instance.JouerEffetSonore(biome.sonPop);
                // on dit que le cube ne contient pas de bacterie
                biome.contientBacterie = false;
            }
            // appelle la fonction qui permet de changer letat du cube vers letat semable
            biome.ChangerEtat(biome.semable);
        }
        
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

            // si le cube contient une bacterie...
            if(biome.contientBacterie == true)
            {
                // on detruit cette bactérie
                Object.Destroy(biome.bacterie);
                // on indique que le cube ne contient pas de bacterie
                biome.contientBacterie = false;
            }
            // appelle la fonction qui permet de changer letat du cube vers letat activable
            biome.ChangerEtat(biome.activable);
        }
    }
}

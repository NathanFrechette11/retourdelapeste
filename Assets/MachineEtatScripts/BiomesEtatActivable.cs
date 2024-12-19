using UnityEngine;

public class BiomesEtatActivable : BiomesEtatsBase
{
    // quand le script est lancé...
    public override void initEtat(BiomesEtatsManager biome)
    {
        // on change le materiel du cube pour celui du biome dans lequel il se trouve et choisit le premier variant de ce biome
        biome.GetComponent<Renderer>().material = (Material)Resources.Load("Mats/b"+ biome.infos["quelBiome"] + "_" + biome.infos["quelVariant"]);
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
            // si le cube ne contient pas d'item sur lui...
            if(biome.infos.ContainsKey("item") == false)
            {
                // si il a la posibilité de faire pousser quelque chose...
                if(Random.value*100 <= biome.infos["fertilite"])
                {
                    // on choisit un nombre aléatoire entre 1 et 2 avec une chance de 50% pour chaque
                    string itemRand = Random.value>0.5 ? "1" : "2";
                    // on fait apparaitre l'item aleatoire avec comme position de base le cube qui le fait apparaitre
                    GameObject item = Object.Instantiate((GameObject)Resources.Load("Items/c7_" + itemRand), new Vector3(biome.infos["cube"].transform.position.x, biome.infos["cube"].transform.position.y + 0.5f, biome.infos["cube"].transform.position.z), Quaternion.identity);
                    // on tourne l'item selon un certain angle aleatoire sur l'axe des y
                    item.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    // on fournie cet item a la liste d'infos du cube
                    biome.infos.Add("item", item);
                    // on lui donne comme parent un empty gameobject dans la scene
                    biome.infos["item"].transform.parent = biome.infos["items"].transform;
                    // on le fait disparaitre
                    biome.infos["item"].SetActive(false);
                }
            }

            // fait lancé l'animation pour la passation de l'étape activable a cultivable
            biome._animator.SetTrigger("ActivableACultivable");
            // // change l'état pour l'état cultivable
            biome.ChangerEtat(biome.cultivable);
        }
    }
}

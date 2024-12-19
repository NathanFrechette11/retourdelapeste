using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GenerateurDile : MonoBehaviour
{
    // definit les infos du monde dans l'inspecteur
    public GameObject infosMonde;
    // definit le prefab du quai dans l'inspecteur
    public GameObject prefabQuai;
    // definit le gameobject du quai
    GameObject quai;

    // permet de definir limage du ui dinfection dans l'inspecteur
    public Image UIInfection;
    // permet de definir le texte du ui dinfection dans l'inspecteur
    public TextMeshProUGUI textUIInfection;
    // permet de definir les images du ui dinfection dans l'inspecteur
    public List<Sprite> listeImageUIInfection = new List<Sprite>();

    // permet de definir le ui du magasin dans l'inspecteur
    public GameObject panelMarchee;
    // crée une liste qui va contenir les cubes instancié
    private List<BiomesEtatsManager> biomesListe = new List<BiomesEtatsManager>();
    // permet de definir le ui pour interaction dans l'inspecteur
    public GameObject interaction;
    // permet de definir le nombre dennemie present sur la scene dans l'inspecteur
    public int nbEnnemies = 6;
    // permet de definir un empty gameobject pour les items dans l'inspecteur
    public GameObject items;
    // permet de definir le personnage une fois instantié
    public GameObject perso;
    
    // permet de definir le marchand une fois instantié
    GameObject marchant;
    // permet de definir si le marchand est deja sur la scene ou non
    bool marchantDejaPresent = false;
    // le marchant a fournir dans l'inspecteur
    public GameObject prefabMarchant;
    // permet de définir le pourcentage de chance que un bloc de l'ile soit fertile
    [Range(0,100)]
    public int fertiliteItemInfectee = 6;

        // crée une liste dans laquelle on mettra les cubes visiles
        List<GameObject> cubesVisibles = new List<GameObject>();

    // permet de définir le pourcentage de chance que un bloc de l'ile soit fertile
    [Range(0,100)]
    public int fertilite = 4;

    // permet de definir differente valeur dans l'inspecteur pour la largeur et la profondeur de l'ile,
    // la force perlin, ainsi que le coefficient de hauteur
    [Range(10,1000)]
    public int ileLargeur = 100;
    [Range(10,1000)]
    public int ileProfondeur = 100;
    [Range(0,1000)]
    public float forcePerlin = 14f;
    [Range(10,30)]
    public int coefH = 10;

    // permet de fournir dans l'inspecteur le prefab du cube pour faire l'ile
    public GameObject prefabCube;

    // permet de fournir la texture du plane dans la scène à partir de l'inspecteur
    [SerializeField] Renderer textureRenderer;

    // permet de definir dans l'inspecteur la grosseur de l'ile qui sera hors de l'eau
    [Range(0,150)]
    public float k = 25; // k = degre dimmersion de lile

    // permet de definir dans l'inspecteur quel pourcentage de l'ile sera hors de leau
    [Range(0,1)]
    public float c = 0.74f; // c = % ile hors de leau
    
    // le perso a fournir dans l'inspecteur
    public GameObject prefabPerso;

    // nomnre de perso a faire apparaitre, a definir dans l'inspecteur
    public int nombreDePersos = 1;

    // on definit une liste qui contiendra une liste de materiel
    private List<List<Material>> biomesMats = new List<List<Material>>();

    // fonction qui va permette d'aller chercher toutes les ressources qu'on aura besoin et qui sont placer dans le dossier Ressources
    private void LoadResources()
    {
        // on definit une variable qui indique a quel biome on est rendu
        int nbBiomes = 1;
        // on definit une variable qui indique a quel variant on est rendu
        int nbVariants = 1;
        // on definit une variable qui permet de savoir s'il reste des materiaux
        bool resteDesMats = true;
        // on crée une nouvelle liste de matériaux
        List<Material> tpBiome = new List<Material>();
        // tant qu'il reste des matériaux...
        do
        {
            // on va chercher le materiel qui selon le biome et variant que lon est rendu
            UnityEngine.Object mats = Resources.Load("mats/b"+nbBiomes+"_"+nbVariants);
            // s'il y a un materiel...
            if(mats)
            {
                // on ajoute ce materiel a la liste
                tpBiome.Add((Material)mats);
                // on crée un nouveau variant
                nbVariants++;
            }
            // sinon...
            else
            {
                // si le nombre de variant est égal a 1...
                if(nbVariants == 1)
                {
                    // il ne reste plus de matériaux
                    resteDesMats = false;
                }
                // sinon...
                else
                {
                    // on ajoute la liste de materiaux a la liste de liste de materiaux
                    biomesMats.Add(tpBiome);
                    // on crée une nouvelle liste de materiaux
                    tpBiome = new List<Material>();
                    // on modifie le nombre du biome que l'on est rendu
                    nbBiomes++;
                    // on definit le variant a 1
                    nbVariants = 1;
                }
            }
        } while (resteDesMats);
    }

    void Awake()
    {
        // augmente la largeur de lile selon le niveau quon est
        ileLargeur = ileLargeur + (50 * DontDestroyWhenLoad.instance.nbNiveau);
        // augmente la profondeur de lile selon le niveau quon est
        ileProfondeur = ileProfondeur + (50 * DontDestroyWhenLoad.instance.nbNiveau);
        // augmente le nombre dennemie selon le niveau quon est
        nbEnnemies = nbEnnemies + (3 * DontDestroyWhenLoad.instance.nbNiveau);
        // augmente la fertilite de lile selon le niveau quon est
        fertilite = fertilite + (2 * DontDestroyWhenLoad.instance.nbNiveau);
        // augmente la fertilite lile infectee selon le niveau quon est
        fertiliteItemInfectee = fertiliteItemInfectee + (2 * DontDestroyWhenLoad.instance.nbNiveau);

        // appelle la fonction qui va chercher les materiaux des cubes
        LoadResources();
        // appelle la fonction qui genere la map
        GenererCarte();

        // commence la coroutine qui affiches les informations du monde sur la scene
        StartCoroutine(GenererInfosMonde());
    }

    void Update()
    {
        // on active le panneau du marche selon si le perso interagit avec le marchant ou non
        panelMarchee.SetActive(perso.GetComponent<MovePerso>().interagitAvecMarchant);
        // sil interagit avec lui...
        if(perso.GetComponent<MovePerso>().interagitAvecMarchant)
        {
            // on desactive le ui dinteraction
            interaction.SetActive(false);
            // on appelle la fonction qui active/desactive les boutons de la boutique selon le nombre de bacteries
            InfosMonde.instance.ChangerBoutons();
            // on met le temps du jeu a 0 pour le mettre sur pause
            Time.timeScale = 0;
        }
        // sinon...
        else
        {
            // on active le panneau d'interaction selon si le perso peut interagir avec le marchant ou non
            interaction.SetActive(perso.GetComponent<MovePerso>().peutInteragirAvecMarchant);
        }

        // si le temps du jeu est a 0...
        if(Time.timeScale == 0)
        {
            // le perso est mit sur pause
            perso.GetComponent<MovePerso>().estEnPause = true;
        }
        // sinon...
        else
        {
            // le perso nest plus sur pause
            perso.GetComponent<MovePerso>().estEnPause = false;
        }
    }

    // fonction qui permet de fermer la boutique quand on en a plus besoin
    public void FermerBoutique()
    {
        // definit que le perso ninteragit plus avec la boutique
        perso.GetComponent<MovePerso>().interagitAvecMarchant = false;
        // remet le temps du jeu a 1 pour reprendre les evenements du jeu
        Time.timeScale = 1;
    }

    void Start()
    {
        // permet de fabriquer un navmesh sur les cubes qui forment lile
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // fonction qui permet de générer l'ile
    private void GenererCarte()
    {
        // appelle la fonction terraforme en lui envoyant la largeur et la profondeur de l'ile,
        // ainsi que la force perlin, qui elle lui renvoit un tableau de float
        float[,] uneCarte = Terraforme(ileLargeur,ileProfondeur,forcePerlin);
        // appelle la fonction aquaforme en lui fournisant le tableau de float de la carte fournie par terraforme,
        // et lui réasigne sa valeur avec son retour
        uneCarte = AquaformeCercle(uneCarte);
        // uneCarte = Aquaforme(uneCarte);

        // fait la meme chose que uneCarte, mais est utilisé pour y mettre les variants???
        float[,] uneCarteVariants = Terraforme(ileLargeur,ileProfondeur,forcePerlin);

        // appelle la fonction qui permet d'afficher l'ile sur la scene et sur le plane
        AfficherIle(uneCarte, uneCarteVariants);

        // appelle la fonction qui permet d'instentier les persos, en lui fournissant le nombre de persos a faire apparaitre
        PlacerPersos(nombreDePersos);

        // appelle la fonction qui place les ennemies sur lile
        PlacerEnnemies();
    }

    // fonction qui permet aux cubes générés de leur donné une hauteur, de faire le terrain de l'ile
    // grace a la largeur et la profondeur de l'ile, et la force perlin, et retourne un tableau de float du terrain
    private float[,] Terraforme(int largeur, int profondeur, float fP)
    {
        // definit un tableau de float de terrain et lui fournie la largeur et la profondeur de l'ile
        float [,] terrain = new float[largeur,profondeur];
        // definit que le bruit(?) est egal a 0
        int bruit =0;
        // pour chaque bloc de profondeur et chaque bloc de largeur...
        for(int z = 0; z < profondeur; z++)
        {
            for(int x = 0; x < largeur; x++)
            {
                // perlin calcul
                float y = Mathf.Clamp01(Mathf.PerlinNoise((x/fP)+bruit,(z/fP)+bruit));
                // affecte perlin a la carte
                terrain[x,z]=y;
            }
        }
        // retourne le terrain
        return terrain;
    }

    // fonction qui permet aux cubes généré qui seraient a une hauteur de 0 ou moins de resté plat, de faire l'eau
    // avec le terrain qui lui est fournie, tout en formant un carre avec ceux-ci
    private float[,] Aquaforme(float[,] terrain)
    {
        // on definit les valeurs de largeur et de profondeur du tableau terrain dans 2 differentes variables
        int l = terrain.GetLength(0);
        int p = terrain.GetLength(1);
        // pour chaque valeur en profondeur et pour chaque valeur en largeur...
        for(int z = 0; z < p; z++)
        {
            for(int x = 0; x < l; x++)
            {
                // on va chercher la valeur absolue du cube selon sa position sur la largeur et la profondeur
                float dx = x/(float)l*2-1;
                float dz = z/(float)p*2-1;
                float val = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dz));
                // on envoie cette valeur a la fonction Sigmoid
                val = Sigmoid(val);
                // on definit la valeur a une place dans le tableau terrain selon son x et son z
                terrain[x,z] = Mathf.Clamp01(terrain[x,z] - val);
            }
        }
        // retourne le terrain
        return terrain;
    }

    // fonction qui permet aux cubes généré qui seraient a une hauteur de 0 ou moins de resté plat, de faire l'eau
    // avec le terrain qui lui est fournie, tout en formant un cercle avec ceux-ci
    private float[,] AquaformeCercle(float[,] terrain)
    {
        // on definit les valeurs de largeur et de profondeur du tableau terrain dans 2 differentes variables
        int l = terrain.GetLength(0);
        int p = terrain.GetLength(1);

        // on calcule le centre de lile selon la largeur max et la profondeur max, les 2 divisés par 2
        float centreX = l/2f;
        float centreZ = p/2f;
        // on calcule la distance du centre au bord selon la racine carrée des valeurs du centre * 2
        float distanceMax = Mathf.Sqrt(centreX*centreX + centreZ*centreZ);

        // pour chaque valeur en profondeur et pour chaque valeur en largeur...
        for(int z = 0; z < p; z++)
        {
            for(int x = 0; x < l; x++)
            {
                // on calcule la distance du point sur la largeur et la profondeur de l'ile
                float dx = x - centreX;
                float dz = z - centreZ;
                // on calcule la distance entre le point donnée et le centre de lile
                float distance = Mathf.Sqrt(dx*dx + dz*dz)/distanceMax;
                // on envoie cette valeur a la fonction Sigmoid
                float val = Sigmoid(distance);
                // on definit la valeur a une place dans le tableau terrain selon son x et son z
                terrain[x,z] = Mathf.Clamp01(terrain[x,z] - val);
            }
        }
        // retourne le terrain
        return terrain;
    }

    // fonction qui permet de gérérer l'ile selon les valeurs recu du tableau de terrain, de faire apparaitre les
    // blocs qui forment l'ile, ainsi d'afficher la forme de l'ile sur le plane
    private void AfficherIle(float[,] uneCarte, float[,] uneCarteVariant)
    {
        // on definit les valeurs de largeur et de profondeur du tableau terrain dans 2 differentes variables
        int l = uneCarte.GetLength(0);
        int p = uneCarte.GetLength(1);

        // on crée une texture avec la valeur de la largeur et de la profondeur pour son nombre de pixels
        Texture2D texture = new Texture2D(l, p);
        // pour chaque valeur en profondeur et pour chaque valeur en largeur...
        for(int z = 0; z < p; z++)
        {
            for(int x = 0; x < l; x++)
            {
                // on va chercher la hauteur du bloc selon la valeur qui est storée a la position dans le tableau selon
                // la position du bloc sur la largeur et la profondeur
                float y = uneCarte[x,z];

                // si la hauteur du cube est au dessus de 0...
                if(y > 0)
                {
                    // on instentie le cube
                    GameObject unCube = Instantiate(prefabCube , transform.position + new Vector3(x,y*coefH,z) - new Vector3(ileLargeur / 2, 0f, ileProfondeur / 2),Quaternion.identity);

                    // on va chercher le biome que ce cube va avoir selon sa hauteur
                    int quelBiome = Mathf.RoundToInt(y*(biomesMats.Count-1));

                    // on fournit au informations du cube le biome du cube
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("quelBiome", quelBiome+1);
                    // on fournit au informations du cube le variant du cube
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("quelVariant", 1);
                    // on fournit au informations du cube litem du cube
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("items", items);
                    // on fournit au informations du cube le gameobject du cube
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("cube", unCube);
                   
                    // on ajoute le cube a la liste des biomes
                    biomesListe.Add(unCube.GetComponent<BiomesEtatsManager>());

                    // on fournit au informations du cube la fertilite du cube
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("fertilite", fertiliteItemInfectee);
                    // on fournit au informations du cube les infos du monde
                    unCube.GetComponent<BiomesEtatsManager>().infos.Add("monde", infosMonde);

                    // on ajoute un boxCollider au cube
                    unCube.AddComponent<BoxCollider>();
                    // on place le cube dans son parent
                    unCube.transform.parent = transform;

                    // si la valeur aleatoire obtenu est egal ou superieur au pourcentage de fertilité choisit...
                    if(UnityEngine.Random.value*100 <= fertilite)
                    {
                        // on va chercher un item selon le biome du cube
                        GameObject item = (GameObject)Resources.Load("Items/c"+(quelBiome+1)+"_"+(1));
                        // un definit le gameobject de litem
                        GameObject unItem;

                        // verifie toutes les possibilitées de nom pour les items pour les instancier a la position qu'il leur faut individuellement
                        if(item.name == "c4_1")
                        {
                            unItem = Instantiate((GameObject)Resources.Load("Items/c"+(quelBiome+1)+"_"+(1)), new Vector3(unCube.transform.position.x, unCube.transform.position.y, unCube.transform.position.z), Quaternion.identity);
                        }
                        else if(item.name == "c2_1")
                        {
                            unItem = Instantiate((GameObject)Resources.Load("Items/c"+(quelBiome+1)+"_"+(1)), new Vector3(unCube.transform.position.x, unCube.transform.position.y + 1.6f, unCube.transform.position.z), Quaternion.identity);
                        }
                        else if(item.name == "c1_1")
                        {
                            unItem = Instantiate((GameObject)Resources.Load("Items/c"+(quelBiome+1)+"_"+(1)), new Vector3(unCube.transform.position.x + .8f, unCube.transform.position.y + 0.8f, unCube.transform.position.z - .5f), Quaternion.identity);
                        }
                        else{
                            unItem = Instantiate((GameObject)Resources.Load("Items/c"+(quelBiome+1)+"_"+(1)), new Vector3(unCube.transform.position.x, unCube.transform.position.y + 0.8f, unCube.transform.position.z), Quaternion.identity);
                        }

                        // on donne une rotation aleatoire a l'item
                        unItem.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
                        // on place l'item en tant qu'enfant du cube
                        unItem.transform.parent = unCube.transform;

                        // on fournit au informations du cube l'item du cube
                        unCube.GetComponent<BiomesEtatsManager>().infos.Add("item", unItem);
                    }
                }

            }
        }
        // on applique la texture créer
        texture.Apply();
        // on fait afficher cette texture sur le plane dans la scene
        textureRenderer.sharedMaterial.mainTexture = texture;

        // instancie le quai selon a la moitier de la profondeur de lile, et de facons a ce que ce soit sur le bord de lile
        quai = Instantiate(prefabQuai, new Vector3(0, 3, ileProfondeur/2 + 3), Quaternion.identity);
    }

    // fonction qui applique la fontion sigmoid sur la valeur recu, et la renvoit ensuite
    private float Sigmoid(float value)
    {
        return 1/(1+Mathf.Exp(-k*(value-c)));
    }

    // fonction qui permet de placer un nombre de perso donné sur des cubes aleatoires de l'ile
    public void PlacerPersos(int nbP)
    {

        // pour chaque cube dans l'objet...
        foreach (Transform cube in transform)
        {
            // si le cube est au dessus de 0 en hauteur...
            if (cube.position.y > 0)
            {
                // on ajoute le cube a la liste des cubes visibles
                cubesVisibles.Add(cube.gameObject);
            }
        }

        // Calculer la limite pour nbP (minimum 1, maximum la moitié des cubes visibles)
        int maxPersos = Mathf.Max(1, cubesVisibles.Count / 2);
        // on clamp le nombre recu de nbP
        nbP = Mathf.Clamp(nbP, 1, maxPersos);

        // pour chaque nbP...
        for (int i = 0; i < nbP; i++)
        {
            // choisit un nombre aleatoire entre 0 et le nombre de cube sur la scene
            int indexCube = UnityEngine.Random.Range(0, cubesVisibles.Count);
            // Choisir un cube aléatoire
            GameObject cubeChoisi = cubesVisibles[indexCube];

            // On place le perso juste au-dessus du cube
            Vector3 positionPerso = cubeChoisi.transform.position + Vector3.up; 
            // Instancier le personnage sur ce cube
            perso = Instantiate(prefabPerso, positionPerso, Quaternion.identity);

            // retire le cube de la liste pour éviter d'y placer un autre perso
            cubesVisibles.RemoveAt(indexCube);

            // donne au personnage son point d'apparition
            perso.GetComponent<MovePerso>().spawnPoint = positionPerso;

        }
    }

    // fonction qui permet de placer des ennemies sur lile
    public void PlacerEnnemies()
    {
        // on appelle la fonction qui melange la liste des cubes de lile pour avoir une liste melangé
        List<BiomesEtatsManager> shuffledBiomesListe = Shuffle(biomesListe);
        // tant que i est plus petit que le nombre dennemies...
        for(int i = 0; i < nbEnnemies; i++)
        {
            // on va chercher un cube a la position i dans la liste melanger
            GameObject unCube = shuffledBiomesListe[i].gameObject;
            // on instancie un ennemie a la position du cube
            GameObject unEnnemie = Instantiate((GameObject)Resources.Load("ennemi/Ennemie"),
                new Vector3(unCube.transform.position.x, unCube.transform.position.y + 1f,
                unCube.transform.position.z), Quaternion.identity);
            // on ajoute au information de lennemie son cube dapparition
            unEnnemie.GetComponent<EnnemieEtatsManager>().infos.Add("maison", unCube);
            // on ajoute au information de lennemie son lile
            unEnnemie.GetComponent<EnnemieEtatsManager>().infos.Add("ile", this);
            // on ajoute au information de lennemie son le personnage
            unEnnemie.GetComponent<EnnemieEtatsManager>().infos.Add("perso", perso);
        }
    }

    // fonction qui permet de melanger la liste des cubes qui forment lile
    private List<BiomesEtatsManager> Shuffle<BiomesEtatsManager>(List<BiomesEtatsManager> _list)
    {
        // tant que i est plus petit que la grandeur de la liste des cubes...
        for(int i = 0; i < _list.Count; i++)
        {
            // on va chercher un cube temporaire
            BiomesEtatsManager temp = _list[i];
            // on va chercher un index aleatoire entre 0 et la grandeur de la liste des cubes
            int randomIndex = UnityEngine.Random.Range(0, _list.Count);
            // a la position i de la liste on met la valeur du cube aleatoire
            _list[i] = _list[randomIndex];
            // a la position du cube aleatoire on met le cube temporaire
            _list[randomIndex] = temp;
        }

        // on retourne la liste melanger
        return _list;
    }

    // méthode qui reçoit une clé et une valeur et qui retourne une liste de tous les biomes qui ont
    // cette corespondance clé/valeur dans leur dictionnaire d'infos
    public List<BiomesEtatsManager> ChercheBiomes(string info, dynamic valeur)
    {
        // Création d'une ile temporaire vide tempListe de biomesEtatsManager
        List<BiomesEtatsManager> tempListe = new List<BiomesEtatsManager>();
        // itération for(i...) à travers biomeListe (la liste de tous les biomes de l'ile)
        for (int i = 0; i < biomesListe.Count; i++)
        {
            // si le dictionnaire infos du biome contient la clé passée en paramètre - string info
            if(biomesListe[i].infos.ContainsKey(info))
            {
                // et si la valeur stockée dans cette clé est égal a la valeur passée en paramètre - dynamic valeur
                if(biomesListe[i].infos[info].Equals(valeur))
                {
                    // alors ajoute le biome à la liste temporaire
                    tempListe.Add(biomesListe[i]);
                }
            }

        }
        
        // retourne la liste temporaire qui contient tous les biomes qui ont la valeur recherchée dans la clé demandée
        return tempListe;
    }

    // méthode qui retourne un pourcentage de biomes d'un certain état, en utilisant le paramètre string typeBiome
    private float PourcentageBiomes(string typeBiome)
    {
        // on ChercheBiome clé 'etat' avec un string typeBiome (activable, cultivable, etc...)
        // on récupere le Count de la liste résultate on le cast float et on le divise par le nombre de biomes total
        // le résultat est un float entre 0 et 1. on le multiplie par 1000, on le Round pour enlever les décimaux
        // on le divise par 10 pour avoir un pourcentage avec 1 décimal (pour 2 décimaux : *10000 / 100...etc)
        return Mathf.Round((float)ChercheBiomes("etat", typeBiome).Count / (float)biomesListe.Count * 1000) / 10;
    }

    // Coroutine qui roule toutes les 5 secondes et qui affiche le % de biomes de chacun des états possibles
    private IEnumerator GenererInfosMonde()
    {
        // definit si la coroutine devrait marcher
        bool coroutineEnMarche = true;

        // tant que la coroutine est en marche...
        while(coroutineEnMarche)
        {
            // on appelle la fonction qui permet de calculer le pourcentage des cubes qui sont a un certain etat pour calculer les
            // cubes qui ont passé letat final
            float pourcentageInfectee = PourcentageBiomes("Final") + PourcentageBiomes("Semable") + PourcentageBiomes("Recoltable");

            // si le pourcentage de lile infectee est plus grand ou egal a 10%...
            if(pourcentageInfectee >= 10 && marchantDejaPresent == false)
            {
                // on appelle la fonction qui place le marchant sur le quai
                PlacerMarchant();
            }
            // sinon si le marchant est deja present et que le pourcentage est plus petit que 10%...
            else if(marchantDejaPresent == true && pourcentageInfectee <= 10)
            {
                // on detruit le marchant
                Destroy(marchant);
                // on definit que le marchand nest pas deja present
                marchantDejaPresent = false;
            }

            // si le pourcentage de lile infectee est plus grand ou egal a 25%...
            if(pourcentageInfectee >= 25)
            {
                // on change limage de limfection pour limage d'infection a 25%
                UIInfection.sprite = listeImageUIInfection[1];
            }
            // si le pourcentage de lile infectee est plus grand ou egal a 50%...
            if(pourcentageInfectee >= 50)
            {
                // on change limage de limfection pour limage d'infection a 50%
                UIInfection.sprite = listeImageUIInfection[2];
            }
            // si le pourcentage de lile infectee est plus grand ou egal a 75%...
            if(pourcentageInfectee >= 75)
            {
                // on change limage de limfection pour limage d'infection a 75%
                UIInfection.sprite = listeImageUIInfection[3];
            }
            // si le pourcentage de lile infectee est plus grand ou egal a 100%...
            if(pourcentageInfectee >= 100)
            {
                // on change limage de limfection pour limage d'infection a 100%
                UIInfection.sprite = listeImageUIInfection[4];
            }

            // si le pourcentage de lile infectee est egal a 100%...
            if(pourcentageInfectee == 100)
            {
                // on arrete la coroutine
                coroutineEnMarche = false;
                // on appelle la coroutine qui permet de changer de scene vers la scene victoire
                StartCoroutine(InfosMonde.instance.AttenteChangementSceneVictoire());
            }

            /// affiche dans un canvas TextMeshPro, les infos de % de chacun des états de biomes sur l'ile complète
            textUIInfection.text = pourcentageInfectee + "%";
            // on attend un 5 secondes
            yield return new WaitForSeconds(5);
        }
    }

    // fonction qui permet de placer le marchand sur le quai de lile
    public void PlacerMarchant()
    {
            // On place le marchant juste au-dessus du quai
            Vector3 positionMarchant = quai.transform.position + new Vector3(0,0.3f,0); 
            // Instancier le marchant sur ce quai
            marchant = Instantiate(prefabMarchant, positionMarchant, Quaternion.Euler(0, 180, 0));

            // on definit que le marchand est deja present sur la scene
            marchantDejaPresent = true;
    }
}
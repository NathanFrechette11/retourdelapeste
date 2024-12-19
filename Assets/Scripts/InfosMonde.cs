using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InfosMonde : MonoBehaviour
{
    public AudioClip sonFromage;
    static InfosMonde _instance;
    static public InfosMonde instance => _instance;

    public navigation nav;

    public GameObject projectileEnnemie;
    public RawImage UIViePerso;
    public List<Sprite> listeImageUIVie = new List<Sprite>();
    int vieMax = 3;
    public int vie = 3;
    int nbBacteries = 0;
    public int nbChariot = 0;
    public int nbPuit = 0;
    public int nbFleur = 0;
    public int nbFromage = 0;
    public int nbPot = 0;

    public TextMeshProUGUI champTextePrixChariot;
    public TextMeshProUGUI champTextePrixPuit;
    public TextMeshProUGUI champTextePrixFromage;
    public TextMeshProUGUI champTextePrixFleur;
    public TextMeshProUGUI champTexteInventaireChariot;
    public TextMeshProUGUI champTexteInventairePuit;
    public TextMeshProUGUI champTexteInventaireFromage;
    public TextMeshProUGUI champTexteInventaireFleur;
    public TextMeshProUGUI champTexteInventairePot;
    public TextMeshProUGUI champTexteBoutonPot;

    public Button boutonChariot;
    public Button boutonPuit;
    public Button boutonFromage;
    public Button boutonFleur;
    public Button boutonPot;

    public int prixChariot = 4000;
    public int prixPuit = 6000;
    public int prixFromage = 10000;
    public int prixFleur = 12000;
    public int prixBocal = 8000;
    bool aPot = false;
    public TextMeshProUGUI champTexteBacteries;

    public bool itemPosee = false;
    public Vector3 posFromage;

    public GameObject npcPrefab; // Préfabriqué du NPC
    public int npcCount = 10;    // Nombre de NPC à générer
    public float spawnRadius = 15f; // Rayon autour de l’item

    void Start()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        vie = vieMax;
        nbBacteries = 0;
        nbChariot = 0;
        nbPuit = 0;
        nbFleur = 0;
        nbFromage = 0;
        nbPot = 0;

        champTexteBacteries.text = "" + nbBacteries;
        AffichageViePerso();
        AfficherPrix();
    }

    public void AjouterBacterie()
    {
        nbBacteries = nbBacteries + 1000;
        champTexteBacteries.text = ""+nbBacteries;
    }

    public void AfficherPrix()
    {
        champTextePrixChariot.text = "Prix : " + prixChariot + " bactéries";
        champTextePrixPuit.text = "Prix : " + prixPuit + " bactéries";
        champTextePrixFromage.text = "Prix : " + prixFromage + " bactéries";
        champTextePrixFleur.text = "Prix : " + prixFleur + " bactéries";
    }

    public void AcheterItem(string nomItem)
    {
        if(nomItem == "chariot" && nbBacteries >= prixChariot)
        {
            nbChariot++;
            nbBacteries = nbBacteries - prixChariot;
            champTexteBacteries.text = ""+nbBacteries;
            if(aPot)
            {
                aPot = false;
                nbPot--;
                RemettrePrixNormal();
            }
            AfficherInventaire();
        }
        else if(nomItem == "puit" && nbBacteries >= prixPuit)
        {
            nbPuit++;
            nbBacteries = nbBacteries - prixPuit;
            champTexteBacteries.text = ""+nbBacteries;
            if(aPot)
            {
                aPot = false;
                nbPot--;
                RemettrePrixNormal();
            }
            AfficherInventaire();
        }
        else if(nomItem == "fromage" && nbBacteries >= prixFromage)
        {
            nbFromage++;
            nbBacteries = nbBacteries - prixFromage;
            champTexteBacteries.text = ""+nbBacteries;
            if(aPot)
            {
                aPot = false;
                nbPot--;
                RemettrePrixNormal();
            }
            AfficherInventaire();
        }
        else if(nomItem == "fleur" && nbBacteries >= prixFleur)
        {
            nbFleur++;
            nbBacteries = nbBacteries - prixFleur;
            champTexteBacteries.text = ""+nbBacteries;
            if(aPot)
            {
                aPot = false;
                nbPot--;
                champTexteBoutonPot.text = "Utiliser pot";
                RemettrePrixNormal();
            }
            AfficherInventaire();
        }
        ChangerBoutons();
    }

    public void AfficherInventaire()
    {
        champTexteInventaireChariot.text = "" + nbChariot;
        champTexteInventairePuit.text = "" + nbPuit;
        champTexteInventaireFromage.text = "" + nbFromage;
        champTexteInventaireFleur.text = "" + nbFleur;
        champTexteInventairePot.text = "" + nbPot;
    }

    void ChangerPrix()
    {
        prixChariot = prixChariot/2;
        prixPuit = prixPuit/2;
        prixFromage = prixFromage/2;
        prixFleur = prixFleur/2;
        AfficherPrix();
    }

    void RemettrePrixNormal()
    {
        prixChariot = prixChariot * 2;
        prixPuit = prixPuit * 2;
        prixFromage = prixFromage * 2;
        prixFleur = prixFleur * 2;
        AfficherPrix();
    }
    
    public void UtiliserPot()
    {
        if(aPot != true && nbPot > 0)
        {
            aPot = true;
            champTexteBoutonPot.text = "Ranger le pot";
            ChangerPrix();
            ChangerBoutons();
        }
        else if(aPot)
        {
            aPot = false;
            champTexteBoutonPot.text = "Utiliser pot";
            RemettrePrixNormal();
            ChangerBoutons();
        }
    }

    public void AffichageViePerso()
    {
        UIViePerso.texture = listeImageUIVie[vie].texture;
        if(vie == 0)
        {
            StartCoroutine(AttenteChangementSceneGameOver());
        }
    }

    public void EnleverItemInventaire(string typeObjet)
    {
        if(typeObjet == "chariot")
        {       
            nbChariot--;
        }
        if(typeObjet == "puit")
        {       
            nbPuit--;
        }
        if(typeObjet == "fromage")
        {       
            nbFromage--;
            PlaceItem(posFromage); 
            vie ++;
            AffichageViePerso();
            DontDestroyWhenLoad.instance.JouerEffetSonore(sonFromage);
        }
        if(typeObjet == "fleur")
        {       
            nbFleur--;
        }
        AfficherInventaire();
    }

    public void ChangerBoutons()
    {
        if(nbBacteries < prixChariot)
        {
            boutonChariot.interactable = false;
        }
        else if(nbBacteries > prixChariot)
        {
            boutonChariot.interactable = true;
        }
        if(nbBacteries < prixPuit)
        {
            boutonPuit.interactable = false;
        }
        else if(nbBacteries > prixPuit)
        {
            boutonPuit.interactable = true;
        }
        if(nbBacteries < prixFromage)
        {
            boutonFromage.interactable = false;
        }
        else if(nbBacteries > prixFromage)
        {
            boutonFromage.interactable = true;
        }
        if(nbBacteries < prixFleur)
        {
            boutonFleur.interactable = false;
        }
        else if(nbBacteries > prixFleur)
        {
            boutonFleur.interactable = true;
        }
        if(nbPot <= 0)
        {
            boutonPot.interactable = false;
        }
        else  if(nbPot >= 0)
        {
            boutonPot.interactable = true;
        }
    }

    public void PlaceItem(Vector3 itemPosition)
    {
        // Générer les NPC
        for (int i = 0; i < npcCount; i++)
        {
            // Générer une position aléatoire autour de l’item
            Vector3 randomPos = itemPosition + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // Vérifier si la position est navigable
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                // Instancier le NPC
                GameObject npc = Instantiate(npcPrefab, hit.position, Quaternion.identity);
                // Faire naviguer le NPC vers l’item
                npc.GetComponent<NavMeshAgent>().SetDestination(itemPosition);
            }
        }
    }

    IEnumerator AttenteChangementSceneGameOver()
    {
        float tempsEcoule = 0f;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 1)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        nav.AllezSceneGameOver();
    }

    public IEnumerator AttenteChangementSceneVictoire()
    {
        float tempsEcoule = 0f;
        // Tant que le temps écoulé est inférieur à la durée totale
        while (tempsEcoule < 3)
        {
            // Incrémenter le temps écoulé en fonction du temps réel
            tempsEcoule += Time.deltaTime;

            // Attendre la prochaine frame avant de continuer
            yield return null;
        }
        DontDestroyWhenLoad.instance.nbNiveau ++;
        nav.AllezSceneVictoire();
    }
}

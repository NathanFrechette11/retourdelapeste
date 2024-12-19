using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemieEtatsManager : MonoBehaviour
{
    // permet de fournir un effet sonore qui sera jouer quand l'ennemie prend des dégats
    public AudioClip sonDegats;
    // permet de fournir un effet sonore qui sera jouer quand l'ennemie rentrera dans la zone d'un puit
    public AudioClip sonSplash;

    // permet de definir le nombre de vie initial d'un ennemie
    int vieInit = 3;
    // permet de mettre en place un acces a la vie de lennemie pour tout les états
    public int nbVie;

    // etat actuel qui permettra de lire les états
    public EnnemieEtatsBase etatActuel;

    // permet de mettre en place l'acces a l'etat d'attaque
    public EnnemieEtatAttaque attaque = new EnnemieEtatAttaque();
    // permet de mettre en place l'acces a l'etat de repos
    public EnnemieEtatRepos repos = new EnnemieEtatRepos();
    // permet de mettre en place l'acces a l'etat de chasse
    public EnnemieEtatChasse chasse = new EnnemieEtatChasse();
    // permet de mettre en place l'acces a l'etat de mort
    public EnnemieEtatMort mort = new EnnemieEtatMort();

    // permet de mettre en place un getter/setter de l'agent du navmesh pour l'ennemie
    public NavMeshAgent agent {get; set;} 
    // permet de mettre en place un getter/setter de lanimator de l'ennemie
    public Animator animator {get; set;}
    // permet de mettre en place un dictionnaire qui regroupera toutes les informations nécessaire pour la gestion des ennemies
    public Dictionary<string, dynamic> infos { get; set; } = new Dictionary<string, dynamic>();
    
    void Start()
    {   
        // quand le script lance, la vie de l'ennemie est egal a sa vie initial
        nbVie = vieInit;
        // on va chercher l'agent de l'ennemie
        agent = GetComponent<NavMeshAgent>();
        // on va chercher l'animator de l'ennemie
        animator = GetComponent<Animator>();
        // on fait tourner l'ennemie selon un angle aleatoire sur laxe des y
        transform.Rotate(new Vector3(0,UnityEngine.Random.Range(0,360),0));
        // on met en place la vision de l'ennemie
        infos.Add("vision", 20f);
        // on lance l'etat de repos de lennemie
        ChangerEtat(repos);
    }

    // fonction qui permet de changer letat des ennemies pour letat donnée
    public void ChangerEtat(EnnemieEtatsBase etat)
    {
        // l'etat actuel est egal a letat qui est fournie
        etatActuel = etat;
        // on lance le debut de l'etat actuel
        etatActuel.initEtat(this);
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // appel le triggerenter de letat actuel
        etatActuel.TriggerEnterEtat(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        // appel le triggerexit de letat actuel
        etatActuel.TriggerExitEtat(this, other);
    }

    // fonction qui permet de replacer l'ennemie la ou il vient quand sa vie tombe a 0
    public void ReplacerEnnemie()
    {
        // on remet sa vie a sa valeur initial
        nbVie = vieInit;
        // on desactive son agent
        agent.enabled = false;
        // on le replace la ou il a apparut sur lile
        agent.Warp(infos["maison"].transform.position);
        // on reactive son agent
        agent.enabled = true;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesEtatsManager : MonoBehaviour
{
    // permet de fournir un effet sonore qui sera jouer quand le perso ramasse une bacterie
    public AudioClip sonPop;

    // permet de mettre en place un dictionnaire qui regroupera toutes les informations nécessaire pour la gestion des cubes
    public Dictionary<string, dynamic> infos { get; set; } = new Dictionary<string, dynamic>();

    // etat actuel qui permettra de lire les états
    public BiomesEtatsBase etatActuel;
    // permet de mettre en place l'acces a l'etat activable
    public BiomesEtatActivable activable = new BiomesEtatActivable();
    // permet de mettre en place l'acces a l'etat cultivable
    public BiomesEtatCultivable cultivable = new BiomesEtatCultivable();
    // permet de mettre en place l'acces a l'etat final
    public BiomesEtatFinal final = new BiomesEtatFinal();
    // permet de mettre en place l'acces a l'etat semable
    public BiomesEtatSemable semable = new BiomesEtatSemable();
    // permet de mettre en place l'acces a l'etat recoltable
    public BiomesEtatRecoltable recoltable = new BiomesEtatRecoltable();

    // permet de definir lanimator du cube
    public Animator _animator;
    // permet de definir si le cube contient une bacterie ou non
    public bool contientBacterie = false;
    // permet de definir le gameobject de la bacterie du cube
    public GameObject bacterie;

    // Start is called before the first frame update
    void Start()
    {
        // on ajoute letat actuel dans la liste des infos des cubes
        infos.Add("etat", etatActuel);
        // definit que le variant de biome initial est le premier
        infos["variant"] = 1;

        // on va chercher lanimator du cube
        _animator = GetComponent<Animator>();
        // on lance l'etat activable du cube
        ChangerEtat(activable);
    }

    // fonction qui permet de changer letat des cubes pour letat donnée
    public void ChangerEtat(BiomesEtatsBase etat)
    {
        // l'etat actuel est egal a letat qui est fournie
        etatActuel = etat;
        // change le nom detat dans le dictionnaire dinfos des cubes
        infos["etat"] = etatActuel.GetType().Name.Replace("BiomesEtat", "");
        // on lance le debut de l'etat actuel
        etatActuel.initEtat(this);
    }

    private void OnTriggerEnter(Collider col)
    {
        // appel le triggerenter de letat actuel
        etatActuel.TriggerEnterEtat(this, col);
    }
}

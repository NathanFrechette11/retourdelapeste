using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyWhenLoad : MonoBehaviour
{
    // permet de créer une instance de la classe DontDestroyWhenLoad
    static DontDestroyWhenLoad _instance;
    // permet de créer un acces publique pour que d'autre classe ai accès à l'instance
    static public DontDestroyWhenLoad instance => _instance;

    // permet de savoir a quel niveau le joueur est rendu
    public int nbNiveau = 0;

    // permet d'indiquer cest quoi le volume auquel on veut que les pistes audio jouent
    [SerializeField] float _volumeMusicalRef = 1f;
    // permet de creer un acces publique au float volumeMusicalRef pour que les autres classes y ai acces
    public float volumeMusicalRef => _volumeMusicalRef;
    // crée un tableau de pistes qui va contenir toutes les pistes musicales
    PisteMusicale[] _tPistes;
    // crée un acces publique a la liste des pistes musicales pour que dautre classes y ai acces
    public PisteMusicale[] tPistes => _tPistes;
    // audiosource qui va permettre de faire jouer les effets sonores du jeu
    AudioSource _sourceEffetsSonores;

    // pendant le lancement du jeu...
    void Awake()
    {
	// on va chercher les enfants du gameobject qui a le script DontDestroyWhenLoad, qui eux
	// on le script PisteMusicale, et on les donne au tableau des piste musicales
        _tPistes = GetComponentsInChildren<PisteMusicale>();
	// on rajoute un audiosource au gameobject avec ce script, et on le fournit a _sourcesEffetsSonores
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
	// une fois le jeu commencé, on verifie sil n'existe pas deja une instance de ce script sur la scene
        if(_instance != null)
        {
	    // s'il y a une instance, on detruit le nouvel instance de ce script
            Destroy(gameObject);
	    // on arrete la lecture du script pour l'instance détruit
            return;
        }
	// on indique que l'instance est égal au gameobject qui contient ce script
        _instance = this;

	// on met ce gameobject en mode dont destroy on load pour pouvoir garder l'instance entre chaque scene
        DontDestroyOnLoad(gameObject);
    }

    // fonction qui permet de faire jouer les pistes musicales selon le volume fournie
    public void ChangerVolumeGeneral(float volume)
    {
	    // fait en sorte que le volumeMusicalRef soit egal a la valeur du volume fournie
        _volumeMusicalRef = volume;
	    // pour chaque piste musicale dans le tableau des pistes, on leur met le volume au meme niveau que volumeMusicalRef
        foreach (PisteMusicale piste in _tPistes) piste.AjusterVolume();
        
    }

    // fonction qui permet de changer l'état des pistes musicales selon si c'est la bonne piste et si elle doit etre active ou non
    public void ChangerEtatLecturePiste(TypePiste type, bool estActif)
    {
	    // pour chaque piste musicale dans le tableau de pistes...
        foreach (PisteMusicale piste in _tPistes)
        {
	        // s'il sagit de la bonne piste musicale, on lui dit que sont estActif est egal a la valeur du estActif fournie
            if (piste.type == type) piste.estActif = estActif;
	        // sinon, on lui donne le contraire de la valeur du estActif fournie
            else piste.estActif = !estActif;
        }
    }

    // fonction qui permet de faire jouer un effet sonore qui lui est fournie
    public void JouerEffetSonore(AudioClip clip)
    {
	// on fait jouer une seul fois l'effet sonore reçus
        _sourceEffetsSonores.PlayOneShot(clip);
    }
}

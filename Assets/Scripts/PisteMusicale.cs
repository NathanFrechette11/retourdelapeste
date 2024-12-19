using UnityEngine;

public class PisteMusicale : MonoBehaviour
{
    // permet de definir de quel type de piste (de base ou attaque) est la musique
    [SerializeField] TypePiste _type; 
    // permet de créer un acces au type de la piste a partir de nimporte quel script
    public TypePiste type => _type;
    // permet de definir si la piste est sensé jouer dès le début ou non
    [SerializeField] bool _estActifParDefaut; 
    // permet de définir si la piste est active ou non
    [SerializeField] bool _estActif; 
    // permet de faire un setter / getter pour estActif pour y acceder a partir de nimporte quel script
    public bool estActif
    {
        get => _estActif;
        set
        {
            // on y modifie la valeur de _estActif
            _estActif = value;
            // quand il y a changement on ajuste le volume de la piste
            AjusterVolume();
        }
    }

    // permet de definir un audiosource pour faire jouer les musiques
    AudioSource _source;
    // permet de faire un acces depuis nimporte quel script de cet audiosource
    public AudioSource source => _source;

    void Awake() 
    {
        // quand le jeu ce charge, on va chercher l'audiosource
        _source = GetComponent<AudioSource>();
        // on definit que la piste est active seulement si elle est sensé etre active par défaut
        _estActif = _estActifParDefaut;
        // on s'assure que la musique va jouer en boucle
        _source.loop = true;
        // on part la musique dès le awake, pas seulement une fois appelé
        _source.playOnAwake = true;
    }

    void Start() 
    {
        // on ajuste le volume de la piste musicale dès le début du jeu
        AjusterVolume();
    }

    // fonction qui permet d'ajuster le volume de la piste musicale selon si elle est active ou non
    public void AjusterVolume() 
    {
        // si la piste musicale est active, on met son volume a 1 (plein son)
        if (_estActif) _source.volume = 1f;
        // sinon, on met son volume a 0 (pas de son)
        else _source.volume = 0f;
    }
}
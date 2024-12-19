using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePerso : MonoBehaviour
{
    // permet de fournir un effet sonore qui sera jouer quand le perso aura pris des degats
    public AudioClip sonDegats;
    // permet de fournir un effet sonore qui sera jouer quand le perso tire un projectile
    public AudioClip sonTire;
    // permet de fournir un effet sonore qui sera jouer quand le perso touche a leau
    public AudioClip sonSplash;

    // permet de definir le charactercontroller du perso
    public CharacterController perso;

    // permet de definir le point dapparition du perso
    public Vector3 spawnPoint;
    // permet de definir le prefab du projectile du perso dans l'inspecteur
    public GameObject prefabProjectile;
    // permet de definir si le perso peut interagir avec le marchant
    public bool peutInteragirAvecMarchant = false;
    // permet de definir si le perso est en train dinteragir avec le marchand
    public bool interagitAvecMarchant = false;
    // permet de definir si le jeu a ete mis en pause
    public bool estEnPause = false;

    // Temps entre chaque son de marche en secondes
    [SerializeField] private float _delaiEntrePas = 0.5f;
    // Stocke le temps du dernier son de marche
    private float _tempsDernierPas = 0f;
    // permet de definir si le perso a son bonus de vitesse en cours
    bool _aBonus = false;
    // permet de definir si le perso peut tirer un projectile
    bool peutTirer = true;

    // permet de fournir un effet sonore qui sera jouer quand le perso marche
    [SerializeField] AudioClip _marche;
    // permet de definir un audiosource
    AudioSource _audio;
    // permet de fournir la camera dans l'inspecteur
    [SerializeField] Camera _cam;
    // permet de fournir la sphere qui suit infecte les cubes dans l'inspecteur
    [SerializeField] private Transform _transformSphere;

    // definit la vitesse de mouvement du perso
    [SerializeField] private float _vitesseMouvement = 20.0f;
    // definit la vitesse de rotation du perso
    [SerializeField] private float _vitesseRotation = 3.0f;
    // definit l'impulsion de saut du perso
    [SerializeField] private float _impulsionSaut = 30.0f;
    // definit la gravité du perso
    [SerializeField] private float _gravite = 0.2f;

    // definit la vitesse de saut du perso
    [SerializeField] private float _vitesseSaut;
    // permet de definir la direction dans laquelle le perso va se deplacer
    private Vector3 _directionsMouvement = Vector3.zero;

    // permet de definir lanimator du perso
    Animator _animator;
    // permet de definir le charactercontroller du perso 
    CharacterController _controller;

    // permet de determiner si le perso peut detecter s'il peut etre touché par les ennemies
    bool detectionDegats = true;

    void Awake()
    {
        // quand le jeu charge, on va chercher le charactercontroller du perso
        perso = GetComponent<CharacterController>();
        // quand le jeu charge, on va chercher l'audiosource du perso
        _audio = GetComponent<AudioSource>();
        // quand le jeu charge, on va chercher l'animator du perso
        _animator = GetComponent<Animator>();
        // quand le jeu charge, on va chercher le charactercontroller du perso
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // si le jeu n'est pas en pause...
        if(estEnPause == false)
        {
            // si le joueur appuie sur la touche f et que le perso peut tirer...
            if (Input.GetKeyDown(KeyCode.F) && peutTirer)
            {
                // on appelle la fonction qui permet de tirer des projectiles
                TirerProjectile();
            }

            // si le perso na plus de vie...
            if(InfosMonde.instance.vie == 0)
            {
                // on desactive son controller 
                _controller.enabled = false;
                // on desactive son collider
                GetComponent<Collider>().enabled = false;
            }

        //permet de faire pivoter le personnage sur l'axe y selon sa vitesse de rotation par la valeur droite ou gauche si le joueur appuie sur 'a' ou 'd'
        transform.Rotate(0, Input.GetAxis("Horizontal") * _vitesseRotation, 0);
        // variable qui permet de calculer la vitesse du perso selon sa vitesse de mouvement par la valeur avant ou arrière si le joueur appuie sur 'w' ou 's'
        float vitesse = _vitesseMouvement * Input.GetAxis("Vertical");

        // permet de définir la valeur du champ de vision de la caméra a 60 par défault, et quand le perso bouge on y rajoute sa vitesse
        _cam.fieldOfView = 60 + vitesse;
        // si le perso bouge, permet de faire apparaitre la sphere. sinon, la sphere n'apparait pas
        if(vitesse > 0){
            // si le temps actuelle - le temps du dernier pas est plus grand ou egal au delai entre chaque pas...
            if (Time.time - _tempsDernierPas >= _delaiEntrePas)
            {
                // fait jouer le son de marche a un volume de .5
                _audio.PlayOneShot(_marche, 0.5f);
                // Met à jour le temps du dernier son
                _tempsDernierPas = Time.time; // Met à jour le temps du dernier son
            }

            _transformSphere.gameObject.SetActive(true);
        }
        else _transformSphere.gameObject.SetActive(false);

        // si la vitesse du perso est plus petite que 0...
        if(vitesse < 0)
        {
            // la vitesse est a 0
            vitesse = 0;
        }

        // quand la vitesse du perso est supérieur à 0, permt de définir le bool "enCourse" de l'animator du perso à true, sinon on le met à false
        _animator.SetBool("enCourse", vitesse > 0);
        // permet de définir la direction de mouvement du perso est vers l'avant ou l'arriere selon la vitesse du joueur
        _directionsMouvement = new Vector3(0, 0, vitesse);
        // permet de faire en sorte que le déplacement est fait selon le monde, et non le personnage (?)
        _directionsMouvement = transform.TransformDirection(_directionsMouvement);

        // si le joueur appuie sur la touche 'espace' et que le perso touche au sol, permet de mettre la vitesse de saut du perso égal a sa force de saut
        // if(Input.GetButtonDown("Jump") && _controller.isGrounded) _vitesseSaut = _impulsionSaut;
        // si le perso ne touche plus au sol et que la vitesse de saut est plus ou moins (?) grande que la force de saut,
        // permet de définir le bool "enSaut" de l'animator du perso à true, sinon on le met à false
        _animator.SetBool("enSaut", !_controller.isGrounded && _vitesseSaut >- _impulsionSaut);
        // permet de rajouter la vitesse de saut à l'axe y de la direction de mouvement du perso pour qu'il puisse sauter
        _directionsMouvement.y += _vitesseSaut;
        // si le perso touche au sol, permet à la vitesse de saut du perso de diminuer petit a petit selon la gravité
        if(!_controller.isGrounded) _vitesseSaut -= _gravite;
        // permet de faire bouger le personnage selon le temps réel et sa direction de mouvement
        _controller.Move( Time.deltaTime * _directionsMouvement);
        }

        // si le perso peut interagir avec le marchant, que le jeu nest pas en pause et que le joueur appuie sur la touche e...
        if(peutInteragirAvecMarchant == true && estEnPause != true && Input.GetButtonDown("Interact"))
        {
            // le perso interagit avec le marchand
            interagitAvecMarchant = true;
        }
        // si le perso est en dessous du point 0 sur laxe y...
        if(this.gameObject.transform.position.y <= -2)
        {
            // on met sa vitesse de saut a -10 pour pas que ca continue de diminuer a l'infinie
            _vitesseSaut = -10;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // quand le perso touche le marchand...
        if(other.CompareTag("Marchant"))
        {
            // le perso peut interagir avec lui
            peutInteragirAvecMarchant = true;
        }
        // si le perso touche le point de respawn en dessous de la map...
        if(other.CompareTag("Respawn"))
        {
            // on desactive son controller
            perso.enabled = false;
            // renvoit le perso a son point dapparition
            this.gameObject.transform.position = spawnPoint;
            // on reactive son controller
            perso.enabled = true;
            // on diminue le nombre de vie du perso
            InfosMonde.instance.vie--;
            // on update laffichage du nombre de vie du perso
            InfosMonde.instance.AffichageViePerso();
        }
        // si le perso touche a lennemie ou sil ce fait toucher par un projectile...
        if(other.CompareTag("ennemie") || other.CompareTag("projectile"))
        {
            // si la detection des degats est actif...
            if(detectionDegats)
            {
                // on diminue le nombre de vie du perso
                InfosMonde.instance.vie--;
                // on fait jouer le son de degats a partir de dontdestroywhenload
                DontDestroyWhenLoad.instance.JouerEffetSonore(sonDegats);
                // on update laffichage du nombre de vie du perso
                InfosMonde.instance.AffichageViePerso();
                // on part la coroutine qui empeche le perso de ce refaire attaquer
                StartCoroutine(AttenteDegats());
            }
        }
        // si le perso touche a leau...
        if(other.CompareTag("eau"))
        {
            // on fait jouer le son splash a partir de dontdestroywhenload
            DontDestroyWhenLoad.instance.JouerEffetSonore(sonSplash);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // si le perso quitte le trigger du marchand...
        if(other.CompareTag("Marchant"))
        {
            // le perso ne peux plus interagir avec lui
            peutInteragirAvecMarchant = false;
            // le perso nest plus en train dinteragir avec lui
            interagitAvecMarchant = false;
        }
    }

    // fonction qui permet de donner le bonus de vitesse au perso
    public void DonnerBonus()
    {
        // si le perso na pas le bonus...
        if(_aBonus == false) 
        {
            // le perso a le bonus
            _aBonus = true;
            // on part la coroutine qui permet de gerer le comportement du bonus
            StartCoroutine(CoroutineBonus());
        }
    }

    // coroutine qui gere le comportement du bonus de vitesse du perso
    IEnumerator CoroutineBonus()
    {
        // on attend pendant 0.1 seconde
        yield return new WaitForSeconds(0.1f);
        // on definit la vitesse bonus du perso
        float vitesseBonus = 22f;
        // on met la vitesse de mouvement du perso egal a celle de bonus
        _vitesseMouvement = vitesseBonus;
        // on attend 5 secondes
        yield return new WaitForSeconds(5f);
        // on lui retire le bonus
        _aBonus = false;
        // la vitesse de mouvement du perso retourne a la normal
        _vitesseMouvement = _vitesseMouvement/2;
        // la vitesse de saut du perso retourne a la normal
        _vitesseSaut = _vitesseSaut/2;
        // on attend le prochain frame
        yield return null;
    }

    // coroutine qui gere lattente entre 2 tires
    IEnumerator CoroutineTirer()
    {
        // le perso ne peut plus tirer
        peutTirer = false;
        // on attend 2 secondes
        yield return new WaitForSeconds(2f);
        // le perso peut tirer
        peutTirer = true;
    }

    // coroutine qui gere lattente pour le prochain degat
    IEnumerator AttenteDegats()
    {
        // on desactive la detection des degats
        detectionDegats = false;
        // on attend 2 secondes
        yield return new WaitForSeconds(2f);
        // on active la detection des degats
        detectionDegats = true;
    }

    // fonction qui permet de gerer l'apparition du projectile et son mouvement
    public void TirerProjectile()
    {

        // Instancie le projectile à une position légèrement devant le personnage
        GameObject projectile = Instantiate(prefabProjectile, transform.position + new Vector3(0,1.3f,0) + transform.forward * 1.5f, transform.rotation);

        // on va chercher le rigidody du projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        // si le rigidbody nest pas null...
        if (rb != null)
        {
            // on ajuste la vitesse a laquelle le projectile va se deplacer
            float vitesseProjectile = 7f;
            // Ajoute une force pour lancer le projectile vers l'avant
            rb.velocity = transform.forward * vitesseProjectile;
        }
        // on fait jouer le son de tire a partir de dontdestroywhenload
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonTire);
        // on commence la coroutine qui gere lattente entre 2 tires
        StartCoroutine(CoroutineTirer());
    }
}

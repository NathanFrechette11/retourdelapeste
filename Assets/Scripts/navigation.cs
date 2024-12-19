using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class navigation : MonoBehaviour
{
    // permet de fournir un audioclip que les boutons vont jouer une fois appuyé
    public AudioClip sonBouton;

    // fonction qui permet d'aller a la scene de jeu en remettant les choses a 0
    public void AllezSceneJeu()
    {
        // on met la piste musicale de base active
        DontDestroyWhenLoad.instance.tPistes[0].estActif = true;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
        // on met la piste musicale de l'attaque a inactive
        DontDestroyWhenLoad.instance.tPistes[1].estActif = false;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
        // envoie le son des boutons a DontDestroyWhenLoad pour pouvoir le faire jouer
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonBouton);
        // remet le temps du jeu a 1 pour reprendre les evenements du jeu
        Time.timeScale = 1;
        // charge la scene "Jeu"
        SceneManager.LoadScene("Jeu");
    }

    // fonction qui permet d'aller a la scene d'acceuil en remettant les choses a 0
    public void AllezSceneAcceuil()
    {
        // on met la piste musicale de base active
        DontDestroyWhenLoad.instance.tPistes[0].estActif = true;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
        // on met la piste musicale de l'attaque a inactive
        DontDestroyWhenLoad.instance.tPistes[1].estActif = false;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
        // envoie le son des boutons a DontDestroyWhenLoad pour pouvoir le faire jouer
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonBouton);
        // on remet le niveau actuel a 0;
        DontDestroyWhenLoad.instance.nbNiveau = 0;
        // charge la scene "MenuPrincipal"
        SceneManager.LoadScene("MenuPrincipal");
    }

    // fonction qui permet d'aller a la scene des crédits
    public void AllezSceneCredits()
    {
        // envoie le son des boutons a DontDestroyWhenLoad pour pouvoir le faire jouer
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonBouton);
        // charge la scene "Credits"
        SceneManager.LoadScene("Credits");
    }

    // fonction qui permet d'aller a la scene de l'histoire du jeu
    public void AllezSceneHistoire()
    {
        // envoie le son des boutons a DontDestroyWhenLoad pour pouvoir le faire jouer
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonBouton);
        // charge la scene "Histoire"
        SceneManager.LoadScene("Histoire");
    }

    // // fonction qui permet d'aller a la scene de des instructions
    public void AllezSceneInstructions()
    {
        // envoie le son des boutons a DontDestroyWhenLoad pour pouvoir le faire jouer
        DontDestroyWhenLoad.instance.JouerEffetSonore(sonBouton);
        // charge la scene "Instructions"
        SceneManager.LoadScene("Instructions");
    }

    // fonction qui permet d'aller a la scene de gameover tout en remettant les choses a 0
    public void AllezSceneGameOver()
    {
        // on met la piste musicale de base active
        DontDestroyWhenLoad.instance.tPistes[0].estActif = true;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[0].AjusterVolume();
        // on met la piste musicale de l'attaque a inactive
        DontDestroyWhenLoad.instance.tPistes[1].estActif = false;
        // on ajuste le volume de cette piste musicale
        DontDestroyWhenLoad.instance.tPistes[1].AjusterVolume();
        // on remet le niveau actuel a 0;
        DontDestroyWhenLoad.instance.nbNiveau = 0;
        // charge la scene "GameOver"
        SceneManager.LoadScene("GameOver");
    }

    // fonction qui permet d'aller a la scene de victoire
    public void AllezSceneVictoire()
    {
        // charge la scene "Victoire"
        SceneManager.LoadScene("Victoire");
    }
}

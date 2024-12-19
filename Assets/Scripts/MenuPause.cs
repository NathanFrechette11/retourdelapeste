using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    // permet de fournir dans l'inspecteur le menu pause
    public GameObject panelMenuPause;
    // permet de savoir si le jeu est sur pause ou non
    bool jeuSurPause = false;

    // Update is called once per frame
    void Update()
    {
        // si le joueur appuie sur la touche escape et que le jeu n'ai pas sur pause...
        if(Input.GetButtonDown("esc") && jeuSurPause == false)
        {
            // on affiche le menu de pause sur la scene
            panelMenuPause.SetActive(true);
            // on indique que le jeu est en pause
            jeuSurPause = true;
            // on met le temps du jeu a 0 pour que le jeu soit sur pause
            Time.timeScale = 0;
        }
        // sinon si le joueur appuie sur la touche escape et que le jeu est déja sur pause...
        else if(Input.GetButtonDown("esc") && jeuSurPause)
        {
            // on n'affiche plus le menu de pause sur la scene
            panelMenuPause.SetActive(false);
            // on indique que le jeu n'est plus en pause
            jeuSurPause = false;
            // on met le temps du jeu a 1 pour que les évenements du jeu reprennent
            Time.timeScale = 1;
        }
    }
}

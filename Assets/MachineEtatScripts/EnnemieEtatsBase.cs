using UnityEngine;

public abstract class EnnemieEtatsBase
{
    // sera appeler a chaque début de lecture des états
    public abstract void initEtat(EnnemieEtatsManager biome);
    // sera appeler a chaque update qui se passera dans les états
    public abstract void updateEtat(EnnemieEtatsManager biome);
    // sera appeler quand lennemie sera triggered dans les états
    public abstract void TriggerEnterEtat(EnnemieEtatsManager biome, Collider col);
    // sera appeler quand lennemie sortira dun trigger dans les états
    public abstract void TriggerExitEtat(EnnemieEtatsManager biome, Collider col);
}

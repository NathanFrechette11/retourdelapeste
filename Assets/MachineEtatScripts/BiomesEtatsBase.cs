using UnityEngine;

public abstract class BiomesEtatsBase
{
    // sera appeler a chaque début de lecture des états
    public abstract void initEtat(BiomesEtatsManager biome);
    // sera appeler a chaque update qui se passera dans les états
    public abstract void updateEtat(BiomesEtatsManager biome);
    // sera appeler quand lennemie sera triggered dans les états
    public abstract void TriggerEnterEtat(BiomesEtatsManager biome, Collider other);
}

using UnityEngine;

[CreateAssetMenu(fileName = "AllPools", menuName = "Pools/AllPools")]
public class AllPools : ScriptableObject
{
    public RandomPool ZombiesPool;
    public Pool BulletMissilesPool;
}

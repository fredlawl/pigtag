using UnityEngine;

interface IBuildable<T>
{
    T Spawn(GameObject owner);
}

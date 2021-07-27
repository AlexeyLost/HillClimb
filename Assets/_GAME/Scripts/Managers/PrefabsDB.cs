using GAME.Bikes;
using GAME.Grounds;
using UnityEngine;

namespace GAME.Managers
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "GAME/Prefabs")]
    public class PrefabsDB : ScriptableObject
    {
        public GroundView groundPrefab;
        public BikeView bikePrefab;
    }
}
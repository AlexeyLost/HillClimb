using GAME.Bikes;
using GAME.Grounds;
using UnityEngine;

namespace GAME.Managers
{
    public class SpawnManager
    {
        private static SpawnManager instance;
        
        public PrefabsDB Prefabs { get; set; }
        
        public static SpawnManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SpawnManager();

                return instance;
            }
        }
        
        public object GetObjectView(string typeName)
        {
            switch (typeName)
            {
                case nameof(GroundView):
                    GroundView _groundView = GameObject.Instantiate(Prefabs.groundPrefab);
                    return _groundView;
                case nameof(BikeView):
                    BikeView _bikeView = GameObject.Instantiate(Prefabs.bikePrefab);
                    return _bikeView;
                default:
                    return null;
            }
        }
    }
}
using UnityEngine;

namespace MineMiner
{
    public class MineSceneAccessor : MonoBehaviour
    {
        [SerializeField] private Transform _levelParent;
        [SerializeField] private Transform _levelCenterTransform;
        [SerializeField] private CracksBlock _cracksBlockPrefab;

        public Transform LevelParent
        {
            get { return _levelParent; }
        }

        public Transform LevelCenterTransform 
        {
            get { return _levelCenterTransform; } 
        }

        public CracksBlock CracksBlockPrefab
        {
            get { return _cracksBlockPrefab; }
        }
    }
}
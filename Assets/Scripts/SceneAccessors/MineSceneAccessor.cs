using UnityEngine;

namespace MineMiner
{
    public class MineSceneAccessor : MonoBehaviour
    {
        [SerializeField] private Transform levelParent;
        [SerializeField] private Transform levelCenterTransform;
        [SerializeField] private CracksBlock cracksBlockPrefab;

        public Transform LevelParent
        {
            get { return levelParent; }
        }

        public Transform LevelCenterTransform 
        {
            get { return levelCenterTransform; } 
        }

        public CracksBlock CracksBlockPrefab
        {
            get { return cracksBlockPrefab; }
        }
    }
}
using DefaultNamespace;
using SimpleJSON;
using UnityEngine;

namespace MineMiner
{
    public class DestroyableBlockData : BlockData
    {
        private float _strengthLeft;
        private Vector3Int _position;

        public DestroyableBlockData(DestroyableBlockMetaData blockMetaData, DestroyableBlockView blockView, Vector3Int position) : base(blockMetaData, blockView)
        {
            _strengthLeft = blockMetaData.Strength;
            _position = position;
        }

        public DestroyableBlockData(JSONNode jsonNode) : base(null, null)
        {
            _strengthLeft = jsonNode[JSONKeys.BlockStrength].AsFloat;
            _position = new Vector3Int(jsonNode[JSONKeys.X].AsInt, 
                jsonNode[JSONKeys.Y].AsInt, jsonNode[JSONKeys.Z].AsInt);
        }

        public bool Hit(float damage)
        {
            _strengthLeft -= damage;
            if (_strengthLeft <= 0)
            {
                return true;
            }

            return false;
        }

        public override JSONNode ToJson()
        {
            JSONNode jsonNode = base.ToJson();
            jsonNode.Add(JSONKeys.BlockStrength, _strengthLeft);
            jsonNode.Add(JSONKeys.X, _position.x);
            jsonNode.Add(JSONKeys.Y, _position.y);
            jsonNode.Add(JSONKeys.Z, _position.z);
            jsonNode.Add(JSONKeys.BlockDataType, BlockDataType.ToString());
            return jsonNode;
        }

        public float StrengthLeft => _strengthLeft;
        public Vector3Int Position => _position;
        public override BlockDataType BlockDataType => BlockDataType.Destroyable;
    }
}
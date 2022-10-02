using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MineMiner
{
    public class LevelGenerator
    {
        [Inject] private BlocksController _blocksController;
        [Inject] private BlocksFactory _blocksFactory;
        
        private int _xMax;
        private int _yMax;
        private int _zMax;
        
        private bool _lastBlockWasStone;
        private List<BlockView> _currentBlockViews = new List<BlockView>();
        private Image _noizeImage;
        private Dictionary<DestroyableBlockView, Vector3Int> _blockViewToPosition = new Dictionary<DestroyableBlockView, Vector3Int>();
        private Dictionary<Vector2Int, int> _blockPositionToMaxY = new Dictionary<Vector2Int, int>();

        public void GenerateLevel(int xMax, int yMax, int zMax)
        {
            _currentBlockViews.Clear();
            
            _xMax = xMax;
            _yMax = yMax;
            _zMax = zMax;
            GenerateHeightMap();
            GenerateTerrainBlocks();
            AddCoinFilter();
        }

        private void GenerateHeightMap()
        {
            Texture2D texture2D = new Texture2D(_xMax, _zMax);
            NoizeGenerator noizeGenerator = new NoizeGenerator();
            noizeGenerator.scale = 0.5f;
            int seedValue = Random.Range(0, 10000);
            noizeGenerator.CalcNoise(texture2D, seedValue);
            if (_noizeImage != null)
            {
                _noizeImage.sprite = Sprite.Create(texture2D, new Rect(0f,0f,texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            }

            Debug.Log($"xMax: {_xMax}, yMax: {_yMax}");

            for (int xPosition = 0; xPosition < _xMax; xPosition++)
            {
                for (int yPosition = 0; yPosition < _yMax; yPosition++)
                {
                    for (int zPosition = 0; zPosition < _zMax; zPosition++)
                    {
                        int blockMaxY = (int) Mathf.Clamp(
                            Mathf.Ceil(texture2D.GetPixel(xPosition, zPosition).grayscale * _yMax),
                            0, _yMax
                        );
                        bool passYMaxFilter = yPosition < blockMaxY;
                        _blockPositionToMaxY[new Vector2Int(xPosition, zPosition)] = blockMaxY;
                        // noizeGenerator.CalcNoise(texture2D, Random.Range(0, 10000));
                        //
                        // int blockMinY = (int) Mathf.Clamp(
                        //     Mathf.Ceil(texture2D.GetPixel(xPosition, zPosition).grayscale * _yMax),
                        //     0, _yMax * 0.4f);
                        // bool passYMinFilter = yPosition > blockMinY;

                        
                        if (passYMaxFilter /*&& passYMinFilter*/)
                        {
                            Vector3Int blockPosition = new Vector3Int(xPosition, yPosition, zPosition);
                            DestroyableBlockView destroyableBlockView = _blocksController.CreateBlock(blockPosition,
                                (DestroyableBlockMetaData)_blocksFactory.GetBlockMetaData(BlockId.Stone));
                            _currentBlockViews.Add(destroyableBlockView);
                            _blockViewToPosition[destroyableBlockView] = blockPosition;
                        }

                    }
                }
            }
        }

        private void AddCoinFilter()
        {
            bool isLastBlockWasCoin = false;
            foreach (DestroyableBlockView blockView in _currentBlockViews)
            {
                if (blockView.DestroyableBlockMetaData.Id == BlockId.Stone)
                {
                    if (Random.Range(0, 8) == 0 || isLastBlockWasCoin && Random.Range(0,3) == 1)
                    {
                        blockView.SetMetaData(_blocksFactory.GetBlockMetaData(BlockId.Golds));
                        isLastBlockWasCoin = true;
                        continue;
                    }
                }

                isLastBlockWasCoin = false;
            }
        }

        private void GenerateTerrainBlocks()
        {
            foreach (BlockView currentBlockView in _currentBlockViews)
            {
                currentBlockView.SetMetaData((DestroyableBlockMetaData)_blocksFactory.GetBlockMetaData(
                    GetBlockIdByPosition(_blockViewToPosition[(DestroyableBlockView)currentBlockView])));
            }
        }

        private BlockId GetBlockIdByPosition(Vector3Int blockPosition)
        {
            int maxY = _blockPositionToMaxY[new Vector2Int(blockPosition.x, blockPosition.z)];
            if (blockPosition.y < maxY - 2)
            {
                if (Random.Range(0, 10) == 1)
                {
                    _lastBlockWasStone = false;
                    return BlockId.Dirt00;
                }
                _lastBlockWasStone = true;
                return BlockId.Stone;
            }
            if (blockPosition.y < maxY - 1)
            {
                if (Random.Range(0, 10) == 1 || _lastBlockWasStone && Random.Range(0,3) == 1)
                {
                    _lastBlockWasStone = true;
                    return BlockId.Stone;
                }
                _lastBlockWasStone = false;
                return BlockId.Dirt00;
            }
            if (blockPosition.y == maxY - 1)
            {
                _lastBlockWasStone = false;
                return BlockId.Dirt01;
            }
            _lastBlockWasStone = true;
            return BlockId.Stone;
        }

        public void SetNoiseRenderer(Image noizeImage)
        {
            _noizeImage = noizeImage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Type Resources", menuName = "ScriptableObjects/Type Resources", order = 1)]
    public class TypeResources : ScriptableObject
    {
        [Serializable]
        public struct Resource<T, R>
        {
            public T type;
            public R res;
        }


        public List<Resource<Shape, SpriteRenderer>> shapeRes;

        public List<Resource<Animal, Sprite>> animalRes;

        public List<Resource<FrameColor, Color>> colorlRes;

        public (SpriteRenderer shape, Sprite animal, Color color) GetResourcesOfType(TileType type)
        {
           var shapes = ToDictionary(shapeRes);
           var animals = ToDictionary(animalRes);
           var colors = ToDictionary(colorlRes);

            return (shapes[type.shape], animals[type.animal], colors[type.color]);
        }

        private Dictionary<T, R> ToDictionary<T, R> (List<Resource<T, R>> res) =>
            res.ToDictionary(res => res.type, res => res.res);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [Serializable]
    class TypedCriterion
    {
        [SerializeField]
        [SerializedTypeFilter(typeof(Criterion))]
        public SerializedType type;

        [SerializeField]
        public Criterion criterion;

        public TypedCriterion(SerializedType type, Criterion criterion)
        {
            this.type = type;
            this.criterion = criterion;
        }
    }

    [Serializable]
    class TypedCriterionCollection : CollectionWrapper<TypedCriterion>
    {
        public TypedCriterionCollection() : base()
        {
        }

        public TypedCriterionCollection(IList<TypedCriterion> items) : base(items)
        {
        }
    }
}

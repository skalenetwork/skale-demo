using System;
using UnityEngine;

namespace Unity.InteractiveTutorials.Tests
{
    static class TestComponents
    {
        // Nest type to avoid it showing up in the "Add Component" menu
        public class ComponentWithNestedValues : MonoBehaviour
        {
            public A componentWithNestedValuesFieldA;

            [Serializable]
            public struct A
            {
                public int componentWithNestedValuesFieldB;
            }
        }
    }
}

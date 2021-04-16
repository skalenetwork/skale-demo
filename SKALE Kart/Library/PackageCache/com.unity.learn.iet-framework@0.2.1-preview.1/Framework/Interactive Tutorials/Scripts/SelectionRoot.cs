using UnityEngine;

namespace Unity.InteractiveTutorials
{
    [SelectionBase, ExecuteInEditMode]
    public class SelectionRoot : MonoBehaviour
    {
        void Update()
        {
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
            this.transform.localScale = Vector3.one;
        }
    }
}

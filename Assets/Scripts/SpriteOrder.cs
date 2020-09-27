using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
     using UnityEditor;
#endif

public class SpriteOrder : MonoBehaviour
{
    private void Start()
    {
        Reorder();
    }

    void Update()
    {
        if (!gameObject.isStatic)
            Reorder();
    }
    
    public void Reorder()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteOrder))]
class SpriteOrderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpriteOrder spriteOrder = (SpriteOrder)target;
        if (spriteOrder == null) return;
        if (GUILayout.Button("Reorder"))
        {
            spriteOrder.Reorder();
        }
    }
}

#endif

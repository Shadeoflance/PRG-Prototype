using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileEditor))]
public class GridEditor : Editor
{
    TileEditor tileEditor;
    Transform parent;
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Width");
        tileEditor.width = EditorGUILayout.FloatField(tileEditor.width, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Height");
        tileEditor.height = EditorGUILayout.FloatField(tileEditor.height, GUILayout.Width(50));
        GUILayout.EndHorizontal();
        tileEditor.color = EditorGUILayout.ColorField("Grid Color", tileEditor.color);
        EditorGUILayout.ObjectField("Current Object", tileEditor.current, typeof(GameObject), true);
    }
    void Awake()
    {
        tileEditor = (TileEditor)target;
        tileEditor.Init();
        parent = GameObject.Find("TilesTemp").transform;
        //SceneView.onSceneGUIDelegate = GridUpdate;
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if (tileEditor.current == null)
        {
            GUI.Label(r, "null");
            return;
        }
        SpriteRenderer sr = tileEditor.current.GetComponent<SpriteRenderer>();
        if(sr == null)
            sr = tileEditor.current.transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            return;
        }
        Rect srr = sr.sprite.rect;
        float width = r.width;
        r.width = r.height * srr.width / srr.height;
        r.x = width / 2 - r.width / 2;
        GUI.DrawTexture(r, sr.sprite.texture);
    }

    void OnSceneGUI()
    {
        Event e = Event.current;
        OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);

        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = r.origin;
        Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / tileEditor.width) * tileEditor.width + tileEditor.width / 2.0f,
                                      Mathf.Floor(mousePos.y / tileEditor.height) * tileEditor.height + tileEditor.height / 2.0f, 0.0f);
        if (e.isKey)
        {
            if (e.character == 'a')
            {
                DestroyTileAt(aligned);
                GameObject obj;
                Object prefab = tileEditor.current == null ? PrefabUtility.GetPrefabParent(Selection.activeObject) : tileEditor.current;
                if (prefab)
                {
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    obj.transform.position = aligned;
                    obj.transform.SetParent(parent);
                    Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                }
            }
            else if (e.character == 'd')
            {
                DestroyTileAt(aligned);
            }
            else if(e.character == 'e')
            {
                tileEditor.SelectPrev();
                e.Use();
            }
        }
        if(e.type == EventType.ScrollWheel)
        {
            if (e.delta.y > 0)
                tileEditor.SelectNext();
            if (e.delta.y < 0)
                tileEditor.SelectPrev();
            e.Use();
        }

    }

    void DestroyTileAt(Vector3 vec)
    {
        foreach (Transform obj in parent)
        {
            if (obj.position == vec)
                Undo.DestroyObjectImmediate(obj.gameObject);
        }
    }
}

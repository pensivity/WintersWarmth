using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EditorRandomObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToPlace;
    [SerializeField] private Vector2 mapSize = new Vector2(10, 10);
    [SerializeField] private Vector2 placementOffset = Vector2.zero;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float minDistanceBetweenObjects = 1f;
    [SerializeField] private int quantityMultiplier = 1;
    [SerializeField] private int maxPlacementAttempts = 100;

    public void PlaceObjects()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int totalAttempts = 0;
        int placedObjects = 0;
        int targetObjectCount = objectsToPlace.Count * quantityMultiplier;

        while (placedObjects < targetObjectCount && totalAttempts < maxPlacementAttempts * targetObjectCount)
        {
            GameObject prefab = objectsToPlace[placedObjects % objectsToPlace.Count];
            if (prefab == null) continue;

            Vector2 position = GetRandomPosition();
            if (IsValidPosition(position))
            {
                GameObject placedObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                placedObject.transform.position = position;
                placedObject.transform.SetParent(transform);
                Undo.RegisterCreatedObjectUndo(placedObject, "Place Random Object");
                placedObjects++;
            }
            totalAttempts++;
        }

        if (placedObjects < targetObjectCount)
        {
            Debug.LogWarning($"Could only place {placedObjects} out of {targetObjectCount} objects due to space constraints or too many placement attempts.");
        }
    }

    private Vector2 GetRandomPosition()
    {
        float x = Random.Range(-mapSize.x / 2, mapSize.x / 2) + placementOffset.x + transform.position.x;
        float y = Random.Range(-mapSize.y / 2, mapSize.y / 2) + placementOffset.y + transform.position.y;
        return new Vector2(x, y);
    }

    private bool IsValidPosition(Vector2 position)
    {
        // Check if the position is obstructed
        Collider2D hit = Physics2D.OverlapCircle(position, 0.5f, obstacleLayers);
        if (hit != null) return false;

        // Check distance from other placed objects
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(position, minDistanceBetweenObjects);
        foreach (Collider2D nearbyObject in nearbyObjects)
        {
            if (nearbyObject.gameObject.transform.parent == transform)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)placementOffset, mapSize);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EditorRandomObjectPlacer))]
public class EditorRandomObjectPlacerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorRandomObjectPlacer placer = (EditorRandomObjectPlacer)target;

        if (GUILayout.Button("Place Objects"))
        {
            placer.PlaceObjects();
        }
    }
}
#endif
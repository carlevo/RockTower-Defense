using UnityEngine;

public class Route : MonoBehaviour
{
    // Array de posiciones (puedes arrastrar GameObjects vacíos aquí)
    public Transform[] waypoints;

    // Para visualizar la ruta en el editor (ayuda mucho)
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
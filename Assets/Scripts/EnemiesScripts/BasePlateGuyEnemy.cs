using UnityEngine;

public class BasePlateGuyEnemy : MonoBehaviour
{
    [SerializeField] private float vel;

    void Update()
    {
        transform.position += new Vector3(-vel * Time.deltaTime, 0, 0);

        if (transform.position.x <= -9.58f)
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class BasePlateGuyEnemy : MonoBehaviour
{
    [SerializeField] private float vel;
    private Camera camera;
    Vector3 limitInferiorEsquerra;
    //Vector3 limitSuperiorDret;
    void Start()
    {
        camera = Camera.main;

        limitInferiorEsquerra = camera.ViewportToWorldPoint(new Vector2(0, 0));
        //limitSuperiorDret = camera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Update()
    {
        transform.position += new Vector3(-vel * Time.deltaTime, 0, 0);

        if (transform.position.x <= limitInferiorEsquerra.x)
        {
            Destroy(gameObject);
        }
    }
}
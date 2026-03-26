using UnityEngine;

public class Teclaenemy : MonoBehaviour
{
    [SerializeField] private float vel;
    private Camera mainCamera;
    Vector3 limitInferiorEsquerra;
    //Vector3 limitSuperiorDret;
    void Start()
    {
        mainCamera = Camera.main;

        limitInferiorEsquerra = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        //limitSuperiorDret = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
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

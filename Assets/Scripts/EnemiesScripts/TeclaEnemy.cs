using UnityEngine;

public class Teclaenemy : MonoBehaviour
{
    [SerializeField] private float vel;
    private Camera mainCamera;
    Vector3 limitInferiorEsquerra;
    SpriteRenderer hitPLayerGm;
    private float counterTilDestroy = 0.3f;
    private bool arrivedTorRoca = false;
    private RocaHandler player;
    //Vector3 limitSuperiorDret;
    void Start()
    {
        mainCamera = Camera.main;

        limitInferiorEsquerra = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        //limitSuperiorDret = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        hitPLayerGm = GameObject.Find("GotDamaged").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Roca").GetComponent<RocaHandler>();
    }

    void Update()
    {
        if (arrivedTorRoca)
        {
            counterTilDestroy-=Time.deltaTime;

            if(counterTilDestroy <= 0)
            {
                HitPlayer(false);
                Destroy(this.gameObject);
            }
        }
        transform.position += new Vector3(-vel * Time.deltaTime, 0, 0);

        if (transform.position.x <= limitInferiorEsquerra.x && !arrivedTorRoca)
        {
            arrivedTorRoca = true;
            HitPlayer(true);
            RocaHandler.Instance.TakeDamage(20f);
        }
    }

    private void HitPlayer(bool activeUIEffect)
    {
        player.TakeDamage(0.8f);
        hitPLayerGm.enabled=activeUIEffect;
    }
}

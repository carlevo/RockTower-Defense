using UnityEngine;


public class Teclaenemy : MonoBehaviour, IDamageable
{
    //Campo de velocidad de este enemigo
    [SerializeField] private float vel;

//Creamos el hp para este tipo de enemigo
    public float HP;

//daño que hace el enemigo
public float damage;
    private Camera mainCamera;
    Vector3 limitInferiorEsquerra;
    SpriteRenderer hitPLayerGm;
    private float counterTilDestroy = 0.3f;
    private bool arrivedTorRoca = false;
    private RocaHandler player;

    //Vector3 limitSuperiorDret;
    void Start()
    {
        //Devuelve la camara marcada como Main Camera en la escena
        mainCamera = Camera.main;
        //Empiezan con 100
       if(HP==0f) HP = 100.0f;

        //Daño que hace
        if(damage==0f) damage = 10f;

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
            RocaHandler.Instance.TakeDamage(damage);
        }
    }

    private void HitPlayer(bool activeUIEffect)
    {
        hitPLayerGm.enabled=activeUIEffect;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0f)
        {
            RocaHandler.Instance.RegisterKill();
            Destroy(gameObject);
        }
    }
}

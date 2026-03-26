using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabEnemicBase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //El nombre de la función comentado y primer parametro es
        //el tiempo desde que se inicia y el segundo
        // parametro es el tiempo desde el primero
        InvokeRepeating("generarEnemics", 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void generarEnemics()
    {
       GameObject enemicGenerat = Instantiate(prefabEnemicBase);
       enemicGenerat.transform.position = transform.position; 
       //transform.position es equivalent a la posició del objecte que te
       //aquest script
    }
}

using UnityEngine;

public class VolverAlIdle : StateMachineBehaviour
{
    [SerializeField] private float tiempoDeEspera = 2.5f; // El tiempo que dura tu rayo Kamehameha
    private float cronometro;

    // Se ejecuta en el primer frame cuando el Animator entra en Goku_Fijo
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cronometro = 0f;
    }

    // Se ejecuta en cada frame mientras el Animator siga en Goku_Fijo
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cronometro += Time.deltaTime;

        // Cuando pasa el tiempo del ataque, le obligamos a volver al Idle
        if (cronometro >= tiempoDeEspera)
        {
            animator.Play("Idle"); // Asegúrate de que tu animación normal se llame exactamente "Idle"
        }
    }
}
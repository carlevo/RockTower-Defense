using UnityEngine;

public class BotonSombreado : MonoBehaviour
{
    public void SetShadowActive(bool active)
    {
        Transform sombreadoTransform = transform.Find("Sombreado");
        if (sombreadoTransform == null)
        {
            Debug.LogWarning("BotonSombreado: No se encontró el hijo 'Sombreado'.");
            return;
        }

        sombreadoTransform.gameObject.SetActive(active);
    }

    public void DisableShadowing()
    {
        SetShadowActive(false);
    }

    public void EnableShadowing()
    {
        SetShadowActive(true);
    }
}

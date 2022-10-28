using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarBooster>(out CarBooster car))
        {
            Time.timeScale = 0;
        }
    }
}

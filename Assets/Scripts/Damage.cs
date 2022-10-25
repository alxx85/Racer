using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarBooster>(out CarBooster car1))
        {
            Time.timeScale = 0;
        }
    }
}

using UnityEngine;

public class BoostRocket : Booster
{
    [SerializeField] private float _boostSpeed;

    public override void GetBoost(CarBooster car)
    {
        car.AddRocket(_boostSpeed);
    }
}

using UnityEngine;

public class BoostWings : Booster
{
    [SerializeField] private float _addWingSize;

    public override void GetBoost(CarBooster car)
    {
        car.AddWing(_addWingSize);
    }
}

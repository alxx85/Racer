using System.Collections;
using UnityEngine;

public class Properties : MonoBehaviour
{
    [SerializeField] private EnergyBoostViewer _energyBoost;
    [SerializeField] private float _speed;
    [SerializeField] private Movement _car;
    [SerializeField] private CarBooster _booster;
    [SerializeField] private RezultScreenViewer _finishScreen;
    [SerializeField] private Finish _finish;

    private WaitForSeconds _delay;

    public EnergyBoostViewer EnergyBoost => _energyBoost;
    public CarBooster CarBooster => _booster;
    public Finish Finish => _finish;
    public float Speed => _speed;

    public static Properties Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddSpeed(float speed)
    {
        _speed += speed;
    }

    public void Finishes(WaitForSeconds delay)
    {
        _car.StopMove();
        _delay = delay;
        StartCoroutine(ShowFinishedScreen());
    }

    private IEnumerator ShowFinishedScreen()
    {
        yield return _delay;

        _finishScreen.Show();
    }
}

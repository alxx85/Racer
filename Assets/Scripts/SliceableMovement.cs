using System.Collections.Generic;
using UnityEngine;

public class SliceableMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _moveVector;
    [SerializeField] private float _forcePower;

    private List<SliceableAction> _sliceableActions = new List<SliceableAction>();

    public static SliceableMovement Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        foreach (var item in _sliceableActions)
        {
            item.transform.Translate(_moveVector * _forcePower * Time.deltaTime);
        }
    }

    public void AddSlicedObject(SliceableAction sliceable) => _sliceableActions.Add(sliceable);
}

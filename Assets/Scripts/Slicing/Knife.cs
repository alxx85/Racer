using UnityEngine;

public class Knife : MonoBehaviour
{
	public int SliceID { get; private set; }
	Vector3 _prevPos;
	Vector3 _pos;

	[SerializeField]
	private Vector3 _direction = Vector3.up;

	public Vector3 Origin
	{
		get
		{
			Vector3 localShifted = transform.InverseTransformPoint(transform.position);
			return transform.TransformPoint(localShifted);
		}
	}

	public Vector3 BladeDirection => transform.rotation * _direction.normalized;
	public Vector3 MoveDirection => (_pos - _prevPos).normalized;

	private void Update()
	{
		_prevPos = _pos;
		_pos = transform.position;
	}
}


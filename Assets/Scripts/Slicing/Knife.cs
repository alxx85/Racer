using UnityEngine;

public class Knife : MonoBehaviour
{
	[SerializeField] private Vector3 _direction = Vector3.up;

	private Vector3 _origin
	{
		get
		{
			Vector3 localShifted = transform.InverseTransformPoint(transform.position);
			return transform.TransformPoint(localShifted);
		}
	}

	public Vector3 Origin => _origin;
	public Vector3 BladeDirection => transform.rotation * _direction.normalized;
}


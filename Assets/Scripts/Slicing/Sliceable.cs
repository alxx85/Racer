using BzKovSoft.ObjectSlicer;
using UnityEngine;
using UnityEngine.Events;

public class Sliceable : MonoBehaviour
{
	private Vector3 _point;
	private Knife _knife;

	public event UnityAction<Vector3> Sliced;

	void OnTriggerEnter(Collider other)
	{
		_knife = other.gameObject.GetComponent<Knife>();
		if (_knife == null)
			return;

		_point = GetCollisionPoint(_knife);
    }

    private void OnTriggerExit(Collider other)
    {
		if (_knife == null)
			return;

        Cut(_knife);

		SliceTextViewer viewer = other.GetComponent<SliceTextViewer>();
		if (viewer != null)
			viewer.Show();
    }

    private void Cut(Knife knife)
	{
		Plane plane = new Plane(knife.BladeDirection, _point);

		var sliceable = GetComponent<IBzSliceable>();
		
		if (sliceable == null)
			return;

		sliceable.Slice(plane, null);
		Sliced?.Invoke(_point);
	}

	private Vector3 GetCollisionPoint(Knife knife)
	{
		Vector3 collisionPoint = knife.Origin;
		return collisionPoint;
	}
}

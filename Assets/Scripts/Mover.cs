using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
	[SerializeField] private Vector3 _start;
	[SerializeField] private Vector3 _end;

    [SerializeField] private float _speed;
    [SerializeField] private float _delay;

    private IEnumerator Start()
    {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		Vector3[] points = new Vector3[] { _start, _end };
		int targetIndex = points.Length - 1;
        Vector3 direction = transform.position - points[targetIndex];

        while (true)
		{
			yield return new WaitForFixedUpdate();
			rigidbody.velocity = direction.normalized * _speed;

			if (Vector3.Distance(transform.position, points[targetIndex]) < 0.1f)
			{
				rigidbody.position = points[targetIndex];
				yield return new WaitForSeconds(_delay);

				targetIndex = (targetIndex + 1) % points.Length;
                direction = transform.position - points[targetIndex];
            }
        }
    }
}

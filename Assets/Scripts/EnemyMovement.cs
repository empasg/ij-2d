using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;

    private Transform[] _pathPoints;
    private int _currentPathPoint;
    private bool _canMove;

    private void Start()
    {
        _canMove = true;

        _pathPoints = new Transform[_path.childCount];

        for(int i = 0; i < _pathPoints.Length; i++)
        {
            _pathPoints[i] = _path.GetChild(i);
        }

        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        while(_canMove)
        {
            Vector2 currentPathPointPosition = _pathPoints[_currentPathPoint].position;

            while (currentPathPointPosition != (Vector2)transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, currentPathPointPosition, _speed * Time.deltaTime);

                yield return null;
            }

            if (++_currentPathPoint >= _pathPoints.Length)
                _currentPathPoint = 0;
        }
    }
}

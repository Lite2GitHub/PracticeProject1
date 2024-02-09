using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    //sets initial direction, this triggers initial movement due to movement being FixedUpdate;

    private List<Transform> _segments = new List<Transform>();
    //Keeps track of segments in a list;
    //List needs using System.Collections.Generic to function;

    public Transform segmentPrefab;
    //tags prefab;

    public int initialSize = 3;

    private void Start()
    {
        ResetState();
        //adds segments when triggered?
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w"))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s"))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
        {
            _direction = Vector2.right;
        }
        //Movement, added in support for WASD using "||" = OR, BUT
        // from left to right and passes if former has a pass, single pipe (|) checks all;
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i-1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        //this spawns new segments;
        segment.position = _segments[_segments.Count - 1].position;
        //position is the last on the list (end of snake) marked as "-1" in list;

        _segments.Add(segment);
        //adds segments;
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            Debug.Log("+1");
        } else if (other.tag == "Obstacle")
        {
            ResetState();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UltimateAttributesPack;

public class CameraEntity : MonoBehaviour
{
    [Header("Players")]
    public GameObject _player1 = null;
    public GameObject _player2 = null;

    [HideInInspector]
    public GameObject[] players = new GameObject[2];

    [Space]
    [Title("Exploration", "light blue", "white")]

    [Header("Rotation")]
    [SerializeField] Vector3 _camExplorationRotation;

    [Header("Offset Forward")]
    [SerializeField] float _camMinForwardOffset = -10f;
    [SerializeField] float _camMaxForwardOffset = -15f;

    [Header("Zoom / Dezoom")]
    [SerializeField] float _maxZoomYDist = 10;
    [SerializeField] float _maxDezoomYDist = 20;
    [Space]
    [SerializeField] float _startDezoomDistance = 5;
    [SerializeField] float _maxDezoomDistance = 15;

    Vector3 _lastCameraTargetPosition;

    [Header("Interpolation")]
    [SerializeField] float _camExplorationPosInterpTime = 0.5f;
    [SerializeField] float _camExplorationRotInterpSpeed = 20;

    [Space]
    [Title("Puzzle", "light green", "white")]
  
    [SerializeField] float _camPuzzlePosInterpTime = 0.5f;
    [SerializeField] float _camPuzzleRotInterpSpeed = 20;

    CameraModes _currentCameraMode;
    public enum CameraModes
    {
        Exploration, Puzzle
    }
    Transform _puzzleCameraTargetPoint;
    Vector3 _velocityRef = Vector3.zero;

    [Space]
    [Title("Camera border", "yellow", "white")]
    [SerializeField] bool _useBordersInExploration = true;
    [SerializeField] bool _useBordersInPuzzle = false;
    bool _camBordersEnabled = true;
    [Space]
    [SerializeField, Tooltip("The plane that is used to calculate camera corners point at players y position")] GameObject _horizontalPlaneObject;
    [SerializeField] LayerMask _horizontalPlaneLayerMask;
    float _averrageYPlayers;
    [Space]
    [SerializeField] GameObject[] _bordersWalls = new GameObject[4];
    [SerializeField] float _bordersWallsWidth;
    [SerializeField] float _bordersWallsHeight;

    [Space]
    [Title("Debug", "grey", "white")]

    [SerializeField] bool _drawDebugGizmos = true;
    [Space]
    [SerializeField] Color _bordersWallsDebugColor = Color.blue;
    [SerializeField] Color _bordersTriggersDebugColor = Color.green;
    [Space]
    [SerializeField] Color _centerLineDebugColor = Color.red;
    [SerializeField] Color _cornersLinesDebugColor = Color.yellow;
    [SerializeField] Color _groundVisionDebugColor = Color.yellow;

    Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();       
    }

    private void Start()
    {
        _currentCameraMode = CameraModes.Exploration;
        _camBordersEnabled = _useBordersInExploration;
    }

    void Update()
    {
        // Dont use camera if the players are not in game
        if(_player1 == null || _player2 == null)
            return;

        // Use camera by type
        switch(_currentCameraMode)
        {
            case CameraModes.Exploration:
                ExplorationCamera();
                break;
            case CameraModes.Puzzle:
                PuzzleCamera();
                break;
        }
        
        PlaceCamBorder();
    }

    // ---------------------  Camera states ----------------------- //

    void ExplorationCamera()
    {
        // Set cam borders enabled
        if (_camBordersEnabled != _useBordersInExploration)
            SetCameraBorderEnabled(_useBordersInExploration);

        // Calculate players mid point
        float distancePlayers = Vector3.Distance(_player1.transform.position, _player2.transform.position);
        Vector3 playersMidCamPosition = Vector3.Lerp(_player1.transform.position, _player2.transform.position, 0.5f);

        // Set camera position and rotation
        if (distancePlayers > _startDezoomDistance && distancePlayers < _maxDezoomDistance) // If players are not too far but far enough to dezoom
        {
            float percent = Mathf.InverseLerp(_startDezoomDistance, _maxDezoomDistance, distancePlayers);
            float yAddDezoom = Mathf.Lerp(_maxZoomYDist, _maxDezoomYDist, percent);
            float currentForwardDezoom = Mathf.Lerp(_camMinForwardOffset, _camMaxForwardOffset, percent);

            _lastCameraTargetPosition = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + yAddDezoom, playersMidCamPosition.z + currentForwardDezoom);
            transform.position = Vector3.SmoothDamp(transform.position, _lastCameraTargetPosition, ref _velocityRef, _camExplorationPosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_camExplorationRotation), _camExplorationRotInterpSpeed * Time.deltaTime);
        }
        else if(distancePlayers <= _startDezoomDistance) // If players are not far enough to dezoom
        {
            _lastCameraTargetPosition = new Vector3(playersMidCamPosition.x, playersMidCamPosition.y + _maxZoomYDist, playersMidCamPosition.z + _camMinForwardOffset);
            transform.position = Vector3.SmoothDamp(transform.position, _lastCameraTargetPosition, ref _velocityRef, _camExplorationPosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_camExplorationRotation), _camExplorationRotInterpSpeed * Time.deltaTime);
        }
        else if(distancePlayers >= _maxDezoomDistance) // If players are too far for the camera dezoom
        {
            if(transform.position != _lastCameraTargetPosition)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _lastCameraTargetPosition, ref _velocityRef, _camExplorationPosInterpTime);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_camExplorationRotation), _camExplorationRotInterpSpeed * Time.deltaTime);
        }
    }

    void PuzzleCamera()
    {
        // Set cam borders enabled
        if (_camBordersEnabled != _useBordersInPuzzle)
            SetCameraBorderEnabled(_useBordersInPuzzle);

        // Lerp cam to target position
        if(_puzzleCameraTargetPoint != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _puzzleCameraTargetPoint.position, ref _velocityRef, _camPuzzlePosInterpTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _puzzleCameraTargetPoint.rotation, _camPuzzleRotInterpSpeed * Time.deltaTime);
        }
    }

    // Enable or disable cam borders
    void SetCameraBorderEnabled(bool state)
    {
        foreach(GameObject go in _bordersWalls)
        {
            go.SetActive(state);
        }
        _camBordersEnabled = state;
    }

    public void AddPlayer(GameObject player)
    {
        if (_player1 == null && _player2 == null)
        {
            _player1 = player;
            players[0] = player;
            return;
        }
        else if(_player1 != null && _player2 == null && player != _player1)
        {            
            _player2 = player;
            players[1] = player;
            return;
        }
    }

    public void SetNewCamTargetPoint(Transform target)
    {
        _currentCameraMode = CameraModes.Puzzle;
        _puzzleCameraTargetPoint = target;
    }

    public void SetExplorationCam()
    {
        _currentCameraMode = CameraModes.Exploration;
    }

    // ---------------------  Camera border ----------------------- //

    void PlaceCamBorder()
    {
        // Set horizontal plane at players averrage Y position
        _averrageYPlayers = (_player1.transform.position.y + _player2.transform.position.y) / 2;
        _horizontalPlaneObject.transform.position = new Vector3(transform.position.x, _averrageYPlayers, transform.position.z);
        _horizontalPlaneObject.transform.up = Vector3.up;

        // Create lists and calculate camera corners directions
        List<Vector3> camCornersPoints = new List<Vector3>();
        Vector3[] camCornersRays = new Vector3[4];
        _cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, camCornersRays); // Ordre des corners : bas gauche, haut gauche, haut droite, bas droite

        // Get camera corners world point at averrage y pos
        foreach (Vector3 cornerRay in camCornersRays)
        {
            if (Physics.Raycast(transform.position, transform.rotation * cornerRay.normalized, out RaycastHit hitInfo, 500f, _horizontalPlaneLayerMask))
            {
                camCornersPoints.Add(hitInfo.point);
            }
            else // If raycast touch nothing, add temporary points with 25 of lenght
            {
                Vector3 newTempCornerPoint = transform.position + (transform.rotation * cornerRay.normalized * 25f);
                camCornersPoints.Add(newTempCornerPoint);
            }
        }

        // Place camera borders walls and triggers
        for (int i = 0; i < 4; i++)
        {
            int nextIndex = i + 1;
            if (i == 3) // If it's thelast index, set the next index to first index
            {
                nextIndex = 0;
            }

            // Set border wall and trigger position
            Vector3 midPoint = Vector3.Lerp(camCornersPoints[i], camCornersPoints[nextIndex], 0.5f);
            midPoint.y = _averrageYPlayers;
            _bordersWalls[i].transform.position = midPoint;

            // If border wall and trigger must be rotated, rotate it and set collider size
            if (i == 0 || i == 2)
            {
                // Set wall and trigger rotation
                Vector3 lookAtTarget = camCornersPoints[nextIndex];
                lookAtTarget.y = _bordersWalls[i].transform.position.y;
                _bordersWalls[i].transform.LookAt(lookAtTarget);

                // Set collider size
                float cornersPointsDist = Vector3.Distance(camCornersPoints[i], camCornersPoints[nextIndex]);
                _bordersWalls[i].GetComponent<BoxCollider>().size = new Vector3(_bordersWallsWidth, _bordersWallsHeight, cornersPointsDist + 2);
            }
            else
            {
                // Set collider size
                float cornersPointsDist = Vector3.Distance(camCornersPoints[i], camCornersPoints[nextIndex]);
                _bordersWalls[i].GetComponent<BoxCollider>().size = new Vector3(cornersPointsDist + 2, _bordersWallsHeight, _bordersWallsWidth);
            }
        }
    }

    // ---------------------  Debug ----------------------- //

    private void OnDrawGizmos()
    {
        if (_drawDebugGizmos)
        {
            DrawDebugCameraCenterLine();

            if (Application.isPlaying && !(_player1 == null || _player2 == null))
            {
                DrawPlayersDistanceLine();
                DrawDebugLines();
                DrawDebugCameraBorders();
            }
        }
    }

    void DrawPlayersDistanceLine()
    {
        // Calculate players distance and set line color
        float playersDist = Vector3.Distance(_player1.transform.position, _player2.transform.position);
        if(playersDist <= _maxDezoomDistance)
        {
            // Draw only green gizmos
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_player1.transform.position, _player2.transform.position);
        }
        else
        {
            float extraDist = Vector3.Distance(_player1.transform.position, _player2.transform.position) - _maxDezoomDistance;

            // Draw green gizmos
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_player1.transform.position, _player2.transform.position);

            // Draw red gizmos
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_player1.transform.position, _player1.transform.position + ((_player2.transform.position - _player1.transform.position).normalized * extraDist / 2));
            Gizmos.DrawSphere(_player1.transform.position + ((_player2.transform.position - _player1.transform.position).normalized * extraDist / 2), 0.5f);

            Gizmos.DrawLine(_player2.transform.position, _player2.transform.position - ((_player2.transform.position - _player1.transform.position).normalized * extraDist / 2));
            Gizmos.DrawSphere(_player2.transform.position + ((_player1.transform.position - _player2.transform.position).normalized * extraDist / 2), 0.5f);
        }
    }

    void DrawDebugCameraCenterLine()
    {
        Gizmos.color = _centerLineDebugColor;
        Gizmos.DrawRay(transform.position, transform.forward * 30f);
    }

    void DrawDebugLines()
    {
        // Create lists and calculate camera corners directions
        List<Vector3> camCornersPoints = new List<Vector3>();
        Vector3[] camCornersRays = new Vector3[4];
        _cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, camCornersRays);

        // Get camera corners world point at averrage y pos
        foreach (Vector3 cornerRay in camCornersRays)
        {
            if (Physics.Raycast(transform.position, transform.rotation * cornerRay.normalized, out RaycastHit hitInfo, 500f, _horizontalPlaneLayerMask))
            {
                camCornersPoints.Add(hitInfo.point);

                // Draw corners lines
                Gizmos.color = _cornersLinesDebugColor;
                Gizmos.DrawLine(transform.position, hitInfo.point);
            }
            else
            {
                // Draw temp corners lines
                Vector3 newTempCornerPoint = transform.position + (transform.rotation * cornerRay.normalized * 25f);
                camCornersPoints.Add(newTempCornerPoint);
            }
        }

        Gizmos.color = _groundVisionDebugColor;
        for(int i = 0; i < 4; i++)
        {
            // Set next index
            int nextIndex = i + 1;
            if(i == 3)
            {
                nextIndex = 0;
            }

            // Draw line
            Gizmos.DrawLine(camCornersPoints[i], camCornersPoints[nextIndex]);
        }
    }

    void DrawDebugCameraBorders()
    {
        for (int i = 0; i < 4; i++)
        {
            // Set gizmo matrix
            Matrix4x4 matrix = _bordersWalls[i].transform.localToWorldMatrix;
            Gizmos.matrix = matrix;

            // Draw border wall
            Gizmos.color = _bordersWallsDebugColor;
            Gizmos.DrawCube(Vector3.zero, _bordersWalls[i].GetComponent<BoxCollider>().size);

            // Draw border trigger
            Gizmos.color = _bordersTriggersDebugColor;
        }
    }
}

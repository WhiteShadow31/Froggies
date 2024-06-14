using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected CameraEntity _cameraEntity;
    [Space]
    [SerializeField] protected GameObject _prefabPlayerEntity;
    protected PlayerEntity _playerEntity;
    public PlayerEntity Player => _playerEntity;
    [Space]
    
    public int playerNbr = 0;
    public Vector3 spawnPoint;

    UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();

        if (_cameraEntity == null)
        {
            _cameraEntity = Camera.main.GetComponent<CameraEntity>();
        }
    }

    void OnReloadScene_TGS(InputValue ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //RespawnZoneSelector.Instance.TeleportPlayerNextRespawn();
    }

    void OnMove(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            // Direction
            Vector2 dir = ctx.Get<Vector2>();

            // Is not in the menu
            _playerEntity.RotaInput = dir;
        }
    }

    void OnSmallJump(InputValue ctx)
    {
        if (_playerEntity != null)
            _playerEntity.SmallJumpInput = true;
    }

    void OnLongJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            _playerEntity.LongJumpInput = true;
        }
    }

    void OnStartTongueAim(InputValue ctx)
    {
        if( _playerEntity != null)
        {
            _playerEntity.StartTongueAimInput = true;
        }
    }

    void OnEndTongueAim(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            _playerEntity.EndTongueAimInput = true;          
        }
    }

    void OnMount(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            _playerEntity.MountInput = true;
        }
    }

    void OnMenuPause(InputValue ctx)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            _uiManager.SetPauseMenu(!_uiManager.InPauseMenu);
    }

    // Arrows

    void OnUpArrow(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            if (AudioGenerator.Instance != null)
            {
                AudioGenerator.Instance.PlayClipAt(_playerEntity.transform.position, "GRE_CROI_01");
            }
        }
    }

    void OnBottomArrow(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            if (AudioGenerator.Instance != null)
            {
                AudioGenerator.Instance.PlayClipAt(_playerEntity.transform.position, "GRE_CROI_02");
            }
        }
    }

    void OnRightArrow(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            if (AudioGenerator.Instance != null)
            {
                AudioGenerator.Instance.PlayClipAt(_playerEntity.transform.position, "GRE_CROI_03");
            }
        }
    }

    void OnLeftArrow(InputValue ctx)
    {
        if(_playerEntity != null)
        {
            if (AudioGenerator.Instance != null)
            {
                AudioGenerator.Instance.PlayClipAt(_playerEntity.transform.position, "GRE_CROI_04");
            }
        }
    }

    public void SpawnPlayer()
    {
        if (_prefabPlayerEntity != null)
        {
            // Instantiate the gameObject frog
            GameObject go = Instantiate(_prefabPlayerEntity);

            // Get the component to control it
            _playerEntity = go.GetComponent<PlayerEntity>();

            // Set it's starting position to this
            go.transform.position = this.transform.position;

            // Change it's name
            _playerEntity.gameObject.name = "PlayerFrog" + playerNbr.ToString();


            if (_cameraEntity == null)
            {
                _cameraEntity = Camera.main.GetComponent<CameraEntity>();
            }
            _cameraEntity.AddPlayer(go);
        }
    }
    public void SpawnPlayer(Vector3 pos)
    {
        if (_prefabPlayerEntity != null)
        {
            GameObject go = Instantiate(_prefabPlayerEntity);
            _playerEntity = go.GetComponent<PlayerEntity>();
            go.transform.position = pos;

            _playerEntity.gameObject.name = _prefabPlayerEntity.name + " " + playerNbr.ToString();
            if(_cameraEntity == null)
            {
                if(Camera.main.GetComponent<CameraEntity>() != null)
                    _cameraEntity = Camera.main.GetComponent<CameraEntity>();
            }

            if(_cameraEntity != null)
                _cameraEntity.AddPlayer(go);

            _playerEntity.controller = this;
        }
    }

    public void SpawnPlayer(Vector3 pos, PlayerEntity player)
    {
        _playerEntity = player;
        player.transform.position = pos;

        if (_cameraEntity == null)
        {
            if (Camera.main.GetComponent<CameraEntity>() != null)
                _cameraEntity = Camera.main.GetComponent<CameraEntity>();
        }

        if (_cameraEntity != null)
            _cameraEntity.AddPlayer(player.gameObject);

        _playerEntity.controller = this;
    }

    public void SetPlayerColor(Color col)
    {
        if(_playerEntity != null && _playerEntity.model != null)
        {
            _playerEntity.playerColor = col;
            SetPlayerColorRecursive(col, _playerEntity.model);
        }
    }

    protected void SetPlayerColorRecursive(Color col, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer mrChild))
            {
                Material mat = mrChild.material;
                mat.color = col;
                mrChild.material = mat;
            }

            SetPlayerColorRecursive(col, child);
        }
    }
    public void RespawnPlayer()
    {
        if(_prefabPlayerEntity != null)
        {
            _playerEntity.transform.position = spawnPoint;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
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
    [Space]
    
    public int playerNbr = 0;
    public Vector3 spawnPoint;

    bool _tryToChangeButton = false;




    private void Awake()
    {
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
            if (!MenuManager.Instance.IsInMenu)
                _playerEntity.RotaInput = dir;
            else
            {
                // Wasnt trying to change button and pushed joystick
                if(!_tryToChangeButton && dir.magnitude > 0.5f)
                {
                    _tryToChangeButton = true;

                    //Debug.Log("Try to change selected button");
                    // MENU USE DIRECTION FOR SELECTION OF BUTTONS
                    MenuManager.Instance.ChangeSelectedButton(dir);

                }
                // Has tried to change and almost stopped pushing joystick
                else if (_tryToChangeButton && dir.magnitude < 0.3f)
                {

                    _tryToChangeButton = false;
                }
            }
        }
    }

    void OnSmallJump(InputValue ctx)
    {
        if (_playerEntity != null)
        {
            if (!MenuManager.Instance.IsInMenu)
                _playerEntity.SmallJumpInput = true;
            else
            {
                // PRESS A BUTTON FROM MENU
                MenuManager.Instance.PressSelectedButton();
            }
        }
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
        if( _playerEntity != null && !MenuManager.Instance.IsInMenu)
        {
            _playerEntity.StartTongueAimInput = true;
        }
    }

    void OnEndTongueAim(InputValue ctx)
    {
        if(_playerEntity != null && !MenuManager.Instance.IsInMenu)
        {
            _playerEntity.EndTongueAimInput = true;          
        }
    }

    void OnMount(InputValue ctx)
    {
        if(_playerEntity != null && !MenuManager.Instance.IsInMenu)
        {
            _playerEntity.MountInput = true;
        }
    }

    void OnMenuPause(InputValue ctx)
    {
        MenuManager.Instance.TryToOpenMenu();
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
                _cameraEntity = Camera.main.GetComponent<CameraEntity>();
            }
            _cameraEntity.AddPlayer(go);

            _playerEntity.controller = this;
        }
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

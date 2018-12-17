using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;

    bool isFiring;

    [Tooltip("The current Health of our player")]
    public float Health = 1f;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("The Player's UI Gameobject Prefab")]
    [SerializeField]
    public GameObject PlayerUIPrefab;

    void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(this.gameObject);

        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }
    }

    void Start()
    {
        if (PlayerUIPrefab != null)
        {
            InstantiateUIGameObject();
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        if (_cameraWork != null)
        {
            if (photonView.IsMine || !PhotonNetwork.IsConnected)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            ProcessInputs();
        }

        if (Health <= 0)
        {
            GameManager.Instance.LeaveRoom();
        }

        if (beams != null && isFiring != beams.activeSelf)
        {
            beams.SetActive(isFiring);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (isFiring)
            {
                isFiring = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }
        Health -= 0.1f;
    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }
        Health -= 0.1f * Time.deltaTime;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0, 5f, 0);
        }

        InstantiateUIGameObject();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFiring);
            stream.SendNext(Health);
        }
        else
        {
            this.isFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    private void InstantiateUIGameObject()
    {
        GameObject _UIGO = Instantiate(this.PlayerUIPrefab);
        _UIGO.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
}

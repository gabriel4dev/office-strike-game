using Assets.Entities;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager instance;
    public Canvas canvas;
    public SocketIOComponent socket;
    public InputField playerNameInput;
    public GameObject player;
    public CameraFollow cameraFollow;

    private void Awake()
    {
        if(NetworkManager.instance == null)
        {
            NetworkManager.instance = this;
        }
        else if(NetworkManager.instance != this)
        {
            NetworkManager.Destroy(gameObject);
        }
        NetworkManager.DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.socket.On("enemies", this.OnEnemies);
        this.socket.On("other_player_connected", this.OnOtherPlayerConnected);
        this.socket.On("other_player_disconnected", this.OnOtherPlayerDisconnected);
        this.socket.On("play", this.OnPlay);
        this.socket.On("player_move", this.OnPlayerMove);
        this.socket.On("player_turn", this.OnPlayerTurn);
        this.socket.On("player_shoot", this.OnPlayerShoot);
        this.socket.On("health", this.OnHealth);

    }

    public void JoinGame()
    {
        StartCoroutine(this.ConnectToServer());
    }

    #region Remote Commands

    private IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

        this.socket.Emit("player_connect");

        yield return new WaitForSeconds(1f);

        string playerName = this.playerNameInput.text;
        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        List<SpawnPoint> enemySpawnPoints = GetComponent<EnemySpawner>().enemySpawnPoints;

        PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints, enemySpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        this.socket.Emit("play", new JSONObject(data));
        canvas.gameObject.SetActive(false);
    }

    public void CommandMove(Vector3 pVector3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(pVector3));
        this.socket.Emit("player_move", new JSONObject(data));
    }

    public void CommandTurn(Quaternion pQuaternion)
    {
        string data = JsonUtility.ToJson(new RotationJSON(pQuaternion));
        this.socket.Emit("player_turn", new JSONObject(data));
    }

    public void CommandShoot()
    {
        print("Shooting");
        this.socket.Emit("player_shoot");
    }

    public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange, bool isEnemy)
    {
        HealthChangeJSON healthChangeJSON = new HealthChangeJSON(playerTo.name, healthChange, playerFrom.name, isEnemy);
        this.socket.Emit("health", new JSONObject(JsonUtility.ToJson(healthChangeJSON)));
    }

    #endregion

    #region Remote Listening
    private void OnEnemies(SocketIOEvent e)
    {
        EnemiesJSON enemiesJSON = EnemiesJSON.CreateFromJSON(e.data.ToString());
        Debug.Log(e.data.ToString());
        EnemySpawner es = GetComponent<EnemySpawner>();
        es.SpawnEnemies(enemiesJSON);
    }
    private void OnOtherPlayerConnected(SocketIOEvent e)
    {
        print("Someone else joined");
        string data = e.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        GameObject o = GameObject.Find(userJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(this.player, position, rotation) as GameObject;
        PlayerController vController = p.GetComponent<PlayerController>();

        Transform t = p.transform.Find("HealthbarCanvas");
        Transform t1 = t.transform.Find("PlayerName");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = userJSON.name;
        vController.isLocalPlayer = false;
        p.name = userJSON.name;
        Health h = p.GetComponent<Health>();
        h.currentHealth = userJSON.health;
        h.OnChangeHealth();
    }
    private void OnOtherPlayerDisconnected(SocketIOEvent s)
    {
        print("user disconnected");
        string data = s.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Destroy(GameObject.Find(userJSON.name));
    }
    private void OnPlay(SocketIOEvent e)
    {
        print("you joined");
        string data = e.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject p = Instantiate(player, position, rotation) as GameObject;

        PlayerController vController = p.GetComponent<PlayerController>();

        Transform t = p.transform.Find("HealthbarCanvas");
        Transform t1 = t.transform.Find("PlayerName");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = currentUserJSON.name;
        vController.isLocalPlayer = true; 
        p.name = currentUserJSON.name;
        cameraFollow.InitializateCamera();
    }
    private void OnPlayerMove(SocketIOEvent e)
    {
        string data = e.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 pos = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);

        if (userJSON.name == this.playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if(p != null)
        {
            p.transform.position = pos;
        }
    }
    private void OnPlayerTurn(SocketIOEvent e)
    {
        string data = e.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Quaternion rot = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        if (userJSON.name == this.playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.rotation = rot;
        }
    }
    private void OnPlayerShoot(SocketIOEvent e)
    {
        string data = e.data.ToString();
        ShootJSON shoot = ShootJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(shoot.name);
        Debug.Log("Activando Shooting: " + shoot.name);
        PlayerController pc = p.GetComponent<PlayerController>();
        pc.Shoot();
    }

    private void OnHealth(SocketIOEvent e)
    {
        string data = e.data.ToString();
        UserHealthJSON userHealthJSON = UserHealthJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(userHealthJSON.name);

        Health h = p.GetComponent<Health>();
        h.currentHealth = userHealthJSON.health;
        h.OnChangeHealth();

    }

    #endregion

    #region JSON Objects

    #endregion
}

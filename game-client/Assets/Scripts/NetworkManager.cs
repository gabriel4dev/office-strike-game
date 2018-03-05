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
        this.socket.On("play",this.OnPlay);
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

    #endregion

    #region Remote Listening
    private void OnEnemies(SocketIOEvent e)
    {

    }
    private void OnOtherPlayerConnected(SocketIOEvent e)
    {
        print("Someone else joined");
        string data = e.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        GameObject o = GameObject.Find(userJSON.name) as GameObject;
        if(o != null)
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

    }
    private void OnPlay(SocketIOEvent e)
    {

    }
    private void OnPlayerMove(SocketIOEvent e)
    {

    }
    private void OnPlayerTurn(SocketIOEvent e)
    {

    }
    private void OnPlayerShoot(SocketIOEvent e)
    {

    }

    private void OnHealth(SocketIOEvent e)
    {

    }

    #endregion

    #region JSON Objects

    #endregion
}

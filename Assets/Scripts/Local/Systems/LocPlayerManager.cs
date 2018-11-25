using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocPlayerManager : MonoBehaviour {

    #region Singleton
    public static LocPlayerManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple LocPlayerManager. Something went wrong");
        instance = this;
    }
    #endregion


    public static int numPlayers;
    public static List<Player> players = new List<Player>();

    static string[] names;
    static Color[] colors;

    public GameObject playerPrefab;

    static Vector3[] spawnPosOffsets = {
        new Vector3(0.26f, 0.05f, -1),
        new Vector3(-0.2f, -0.25f, -1),
        new Vector3(-0.13f, 0.15f, -1),
        new Vector3(0.12f, -0.25f, -1),
        new Vector3(0, 0, -1),
        new Vector3(0.2f, 0, -1)
    };

    public static void ReceivePlayersData(string[] names, Color[] colors)
    {
        LocPlayerManager.names = names;
        LocPlayerManager.colors = colors;
    }

    public void Initialize()
    {
        numPlayers = names.Length;
        Vector3 startPos = BoardManager.startPos;

        for (int i = 0; i < numPlayers; i++)
        {
            players.Add(CreatePlayer(startPos + spawnPosOffsets[i], names[i], colors[i]));
        }

        PlayerPanelController.instance.Initialize(players);
    }

    public Player CreatePlayer(Vector3 pos, string name, Color color)
    {
        GameObject p = Instantiate(playerPrefab, pos, Quaternion.identity);
        p.GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 1);

        Player player = p.GetComponent<Player>();
        player.Initialize(name, LocGameManager.initialMoney, color);

        return player;
    }

    public void DebugInitialize()
    {
        numPlayers = 4;

        Vector3 startPos = BoardManager.startPos;

        players.Add(CreatePlayer(startPos + new Vector3(0.26f, 0.05f, -1), "帅大黄", Color.red));
        players.Add(CreatePlayer(startPos + new Vector3(-0.2f, -0.25f, -1), "臭臭白", Color.blue));
        players.Add(CreatePlayer(startPos + new Vector3(-0.13f, 0.15f, -1), "小灰蛋", Color.grey));
        players.Add(CreatePlayer(startPos + new Vector3(0.12f, -0.25f, -1), "黄太子", Color.yellow));

        PlayerPanelController.instance.Initialize(players);
    }
}

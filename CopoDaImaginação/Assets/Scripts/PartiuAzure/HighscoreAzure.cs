using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity3dAzure.AppServices;
using System;
using UnityEngine.UI;
using System.Net;
using System.Linq;

public class HighscoreAzure : MonoBehaviour
{
    [SerializeField]
    private string _appUrl = "PASTE_YOUR_APP_URL";

    private MobileServiceClient _client;
    private MobileServiceTable<Highscore> _table;

    public bool isReadyComplete;

    private Highscore _score;
    //Leaderboard
    public List<Highscore> _scores = new List<Highscore>();

    //Variáveis de Scrolling
    private bool _isPaginated = false;
    private uint _skip = 0;
    // load once when scroll buffer is hit
    private const uint _noPageResults = 50;
    // no of records to skip
    private uint _totalCount = 0;
    private bool _isLoadingNextPage = false;

    void Start()
    {
        //Cria um AppService client
        _client = new MobileServiceClient(_appUrl);

        //Pega a tabela 'Highscores'
        _table = _client.GetTable<Highscore>("Highscores");
    }

    #region Insert 

    public void Insert() //Vai inserir um score na tabela. Função pra por num botão
    {
        Highscore score = GetScore();
        if (Validate(score))
        {
            StartCoroutine(_table.Insert<Highscore>(score, OnInsertCompleted));
        }
    }

    private void OnInsertCompleted(IRestResponse<Highscore> response)
    {
        if (!response.IsError && response.StatusCode == HttpStatusCode.Created)
        {
            Debug.Log("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
            Highscore item = response.Data; //Se der certo, o item vai ter um ID 
            _score = item;
        }
        else
        {
            Debug.LogWarning("Insert Error Status:" + response.StatusCode + " Url: " + response.Url);
        }
    }

    private Highscore GetScore()
    {
        string name = PlayerPrefs.GetString("Nome"); //NOME DIGITADO PELO PLAYER
        string score = PlayerPrefs.GetFloat("Score").ToString();// playerscore
        string id = GameObject.Find("Id").GetComponent<Text>().text; //GERA UM ID
        Highscore highscore = new Highscore();
        highscore.username = name;
        if (!String.IsNullOrEmpty(score))
        {
            highscore.score = Convert.ToUInt32(score);
        }
        if (!String.IsNullOrEmpty(id))
        {
            highscore.id = id;
            Debug.Log("Existing Id:" + id);
        }
        return highscore;
    }
    #endregion

    #region Pegar top 5 player
    //Por hora tá pegando todos
    public void Top5()
    {
        ResetList();
        GetPageHighscores();
    }

    private void GetPageHighscores()
    {
        isReadyComplete = false;
        CustomQuery query = new CustomQuery("", "score desc", _noPageResults, _skip, "id,username,score");
        StartCoroutine(_table.Query<Highscore>(query, OnReadNestedResultsCompleted));
    }

    private void OnReadNestedResultsCompleted(IRestResponse<NestedResults<Highscore>> response)
    {
        if (!response.IsError)
        {
            Debug.Log("OnReadNestedResultsCompleted: " + response.Url + " data: " + response.Content);
            Highscore[] items = response.Data.results;
            _totalCount = response.Data.count;
            Debug.Log("Read items count: " + items.Length + "/" + response.Data.count);
            _isPaginated = true; // nested query will support pagination
            isReadyComplete = true;
            if (_skip != 0)
            {
                _scores.AddRange(items.ToList());
            }
            else
            {
                _scores = items.ToList(); // set for first page of results
            }

        }
        else
        {
            Debug.LogWarning("Read Nested Results Error Status:" + response.StatusCode.ToString() + " Url: " + response.Url);
        }
        _isLoadingNextPage = false; // allows next page to be loaded
    }

    #endregion


    #region OutrasFunções 

    #region usadas na inserção 

    private bool Validate(Highscore highscore)
    {
        bool isUsernameValid = true;

        //Valida Nome
        if (String.IsNullOrEmpty(highscore.username))
        {
            isUsernameValid = false;
            Debug.LogWarning("Error, player username required");
        }
        UpdateText("Player", isUsernameValid);
        return (isUsernameValid);
    }

    private void UpdateText(string gameObjectName, bool isValid = true)
    {
        Text text = GameObject.Find(gameObjectName).GetComponent<Text>();
        if (text)
        {
            text.color = isValid ? Color.white : Color.red;
        }
    }

    #endregion


    #region usadas na busca dos 5 maiores

    private void ResetList()
    {
        _skip = 0;
        _scores = new List<Highscore>();
    }

    #endregion

    #endregion
}

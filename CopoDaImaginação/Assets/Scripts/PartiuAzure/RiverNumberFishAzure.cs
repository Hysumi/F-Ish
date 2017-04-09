using System.Collections.Generic;
using UnityEngine;
using Unity3dAzure.AppServices;
using System;
using System.Net;
using System.Linq;

public class RiverNumberFishAzure : MonoBehaviour
{

    [SerializeField]
    private string _appUrl = "http://docerio.azurewebsites.net"; //"PASTE_YOUR_APP_URL";
    private MobileServiceTable<RiverNumberFish> _table;
    private MobileServiceClient _client;
    private RiverNumberFish _fish;
    public List<RiverNumberFish> _allFishInDB = new List<RiverNumberFish>();
    public bool isReadCompleted = false;

    private uint _skip = 0;
    private const uint _noPageResults = 50;
    private uint _totalCount = 0;
    private bool _isPaginated = false;
    private bool _isLoadingNextPage = false;

    void Start()
    {
        //Cria um AppService client
        _client = new MobileServiceClient(_appUrl);

        //Pega a tabela 'Highscores'
        _table = _client.GetTable<RiverNumberFish>("RiverFish");
    }

    #region UpdateFishTable

    public void UpdateScore(string nameFish, uint nFish)
    {
        GetAllFishes();
        foreach (RiverNumberFish fish in _allFishInDB)
        {
            if (nameFish == fish.fishName)
            {
                fish.nFish -= nFish;
                StartCoroutine(_table.Update<RiverNumberFish>(fish, OnUpdateScoreCompleted));
            }
        }
    }

    private void OnUpdateScoreCompleted(IRestResponse<RiverNumberFish> response)
    {
        if (!response.IsError)
        {
            Debug.Log("OnUpdateItemCompleted: " + response.Content);
        }
        else
        {
            Debug.LogWarning("Update Error Status:" + response.StatusCode + " " + response.ErrorMessage + " Url: " + response.Url);
        }
    }

    public void GetAllFishes()
    {
        isReadCompleted = false;

        _allFishInDB = new List<RiverNumberFish>();
        CustomQuery query = new CustomQuery("", "nFish desc", _noPageResults, _skip, "id,fishName, nFish");
        StartCoroutine(_table.Query<RiverNumberFish>(query, OnReadNestedResultsCompleted));
    }

    private void OnReadNestedResultsCompleted(IRestResponse<NestedResults<RiverNumberFish>> response)
    {
        if (!response.IsError)
        {
            Debug.Log("OnReadNestedResultsCompleted: " + response.Url + " data: " + response.Content);
            RiverNumberFish[] items = response.Data.results;
            _totalCount = response.Data.count;
            Debug.Log("Read items count: " + items.Length + "/" + response.Data.count);
            isReadCompleted = true;
            _isPaginated = true; // nested query will support pagination
            if (_skip != 0)
            {
                _allFishInDB.AddRange(items.ToList());
            }
            else
            {
                _allFishInDB = items.ToList(); // set for first page of results
            }
        }
        else
        {
            Debug.LogWarning("Read Nested Results Error Status:" + response.StatusCode.ToString() + " Url: " + response.Url);
        }
        _isLoadingNextPage = false; // allows next page to be loaded
    }

    #endregion

    /*
    #region Insert 

    public void Insert() //Vai inserir um score na tabela. Função pra por num botão
    {
        RiverNumberFish nfishs = GetFish();
        if (Validate(nfishs))
        {
            StartCoroutine(_table.Insert<RiverNumberFish>(nfishs, OnInsertCompleted));
        }
    }

    private void OnInsertCompleted(IRestResponse<RiverNumberFish> response)
    {
        if (!response.IsError && response.StatusCode == HttpStatusCode.Created)
        {
            Debug.Log("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
            RiverNumberFish item = response.Data; //Se der certo, o item vai ter um ID 
            _fishs = item;
        }
        else
        {
            Debug.LogWarning("Insert Error Status:" + response.StatusCode + " Url: " + response.Url);
        }
    }

    private RiverNumberFish GetFish()
    {
        string name = GameObject.Find("InputName").GetComponent<InputField>().text; //NOME DIGITADO PELO PLAYER
        string nFish = GameObject.Find("Player").GetComponent<Boat>().playerPoints.ToString(); //PEGA OS PONTOS TOTAIS DO PLAYER 
        string id = GameObject.Find("Id").GetComponent<Text>().text; //GERA UM ID
        RiverNumberFish riverFish = new RiverNumberFish();
        riverFish.fishName = name;
        if (!String.IsNullOrEmpty(nFish))
        {
            riverFish.nFish = Convert.ToUInt32(nFish);
        }
        if (!String.IsNullOrEmpty(id))
        {
            riverFish.id = id;
            Debug.Log("Existing Id:" + id);
        }
        return riverFish;
    }
    #endregion

    private bool Validate(RiverNumberFish nfish)
    {
        bool isUsernameValid = true;

        //Valida Nome
        if (String.IsNullOrEmpty(nfish.fishName))
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
    */
}

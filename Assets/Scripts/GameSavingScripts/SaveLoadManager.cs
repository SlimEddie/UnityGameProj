using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour {

    public Text status_ui_txt;
    public bool saving = false;
    public string fileName = "WSASsave";
    public bool initialization = true;
    public Database gameData = new Database();

    public bool resetCloudData = false;
    public bool saved = false;
    public bool editorMode = false;
    private bool firstTimeLocal = false;
    private bool firstTimeCloud = false;

    private DateTime startTime;
    private TimeSpan mTotalPlayingTime;
    private TimeSpan playedTimeLocal;

    // Use this for initialization
    void Start () {
        status_ui_txt.text = "Connecting to Google play games";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        startTime = DateTime.Now;
        //PlayerPrefs.DeleteAll();
        TryLoadLocal();

        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                if (resetCloudData)
                    DeleteGameData(fileName);
                else
                {
                    status_ui_txt.text = "Online mode. "; /// Do stuff if connected
                    OpenSavedGame(fileName); // Stuff that opens save game - do this also when saving I guess.
                }
            }
            else
            {
                //will happen in editor or if authentication fails. Should also go on and play game.
                editorMode = true;
                status_ui_txt.text = "Unable to connect to Google play games or in editor. Loading game.";
                SceneManager.LoadScene(1);
            }
        });
    }



    #region OpenSave
    public void Save()
    {
        saved = false;
        OpenSavedGame(fileName);
    }

    void OpenSavedGame(string filename)
    {
        if (!editorMode)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
        else
        {
            mTotalPlayingTime = playedTimeLocal.Add(DateTime.Now.Subtract(startTime));
            SaveLocal<Database>(gameData, mTotalPlayingTime);
        }
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            status_ui_txt.text = "Success opening saved game";
            if (initialization && !saving)
            {
                initialization = false;
                if (game.TotalTimePlayed.TotalSeconds < 1)
                    firstTimeCloud = true;
                status_ui_txt.text += game.TotalTimePlayed.TotalSeconds.ToString();

                if (!firstTimeCloud && !firstTimeLocal)
                {
                    if (game.TotalTimePlayed.TotalSeconds >= playedTimeLocal.TotalSeconds)
                        LoadGameData(game);
                    status_ui_txt.text += " - Both saves exist. ";
                }
                else if(!firstTimeCloud)
                {
                    LoadGameData(game);
                    status_ui_txt.text += " - Loading from cloud. ";
                }
                else 
                    SaveGame(game, Serialize<Database>(gameData), mTotalPlayingTime);

            }
            else if(!saving)
            {
                saving = true;
                if (game.TotalTimePlayed.TotalSeconds > playedTimeLocal.TotalSeconds)
                    mTotalPlayingTime = game.TotalTimePlayed.Add(DateTime.Now.Subtract(startTime));
                else
                    mTotalPlayingTime = playedTimeLocal.Add(DateTime.Now.Subtract(startTime));

                SaveLocal<Database>(gameData, mTotalPlayingTime);
                SaveGame(game, Serialize<Database>(gameData), mTotalPlayingTime);   
            }
        }
        else if(!saving)
        {
            status_ui_txt.text = " error in opening cloud save. - Working with local "; /// Do stuff if didn't connect
            if (initialization)
            {
                initialization = false;
                SceneManager.LoadScene(1);
            }
            else
                SaveLocal<Database>(gameData,playedTimeLocal);    
        }
    }
    #endregion


    #region SaveGameData Cloud  
    void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            status_ui_txt.text += "Success saving to cloud";
            // handle reading or writing of saved game.
            saved = true;
            saving = false;
            if (firstTimeCloud)
                SceneManager.LoadScene(1);
        }
        else
        {
            status_ui_txt.text += "error finishing save to cloud"; 
            // handle error
        }
    }
    #endregion


    #region ReadSavedData Cloud
    void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        mTotalPlayingTime = game.TotalTimePlayed;
    }

    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            gameData = Deserialize<Database>(data);
            status_ui_txt.text += " Received data from cloud ";
            SceneManager.LoadScene(1);
        }
        else
        {
            // handle error
        }
    }
    // Stuff that reads game data---------------------------------------------------------------------------
    #endregion


    #region SerializeDeserialize
    public static byte[] Serialize<T>(T obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(byte[] byteArray) where T : class
    {
        if (byteArray == null)
        { 
            return null;
        }
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        ms.Write(byteArray, 0, byteArray.Length);
        ms.Seek(0, SeekOrigin.Begin);
        T obj = (T)bf.Deserialize(ms);
        return obj;
    }
    #endregion

    #region Save/Load Local
    private T LoadLocal<T>() where T : class
    {
        string localDataStr = PlayerPrefs.GetString(fileName);
        byte[] localByteArr = System.Convert.FromBase64String(localDataStr);
        playedTimeLocal.Add(new TimeSpan(0, 0, PlayerPrefs.GetInt("TimePlayed")));
        return Deserialize<T>(localByteArr);
    }

    void SaveLocal<T>( T data, TimeSpan TimePlayed) {
        byte[] localByteArr = Serialize<Database>(gameData);
        string localDataStr = System.Convert.ToBase64String(localByteArr);
        PlayerPrefs.SetInt("TimePlayed", (int)TimePlayed.TotalSeconds);
        PlayerPrefs.SetString(fileName, localDataStr);
        PlayerPrefs.Save();
        saving = false;
        PlayerPrefs.Save();
    }


    private void TryLoadLocal()
    {
        //tells us if it's the first time that this game has been launched after install
        if (!PlayerPrefs.HasKey("IsFirstTime")) 
        {
            PlayerPrefs.SetInt("IsFirstTime", 1);
            PlayerPrefs.SetInt("TimePlayed", 0);
            PlayerPrefs.SetString(fileName, "0");
            firstTimeLocal = true;
            SaveLocal<Database>(gameData, new TimeSpan(0,0,0));
        }
        else
        {
            gameData = LoadLocal<Database>();
        }
    }
    #endregion

    #region DeleteCloud
    void DeleteGameData(string filename)
    {
        // Open the file to get the metadata.
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
    }

    public void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.Delete(game);
            SceneManager.LoadScene(1);
        }
        else
        {
            // handle error
        }
    }
    #endregion




}

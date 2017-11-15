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

public class SaveLoadManager : MonoBehaviour {

    public Text status_ui_txt;
    public bool saving = false;
    public string fileName = "WSASsave";
    public bool initialization = true;
    public Database gameData;

    private bool firstTimeLocal = false;
    private bool firstTimeCloud = false;

    private DateTime mStartTime;
    private TimeSpan mTotalPlayingTime;
    private TimeSpan playedTime;

    // Use this for initialization
    void Start () {
        status_ui_txt.text = "Connecting to Google play games";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        #region FirstTimeSaving
        if (!PlayerPrefs.HasKey(fileName)) // If no save - set key to clean
            PlayerPrefs.SetString(fileName, "0");
        if (!PlayerPrefs.HasKey("IsFirstTime")) //tells us if it's the first time that this game has been launched after install - 0 = no, 1 = yes 
        {
            PlayerPrefs.SetInt("IsFirstTime", 1);
            firstTimeLocal = true;
        }
        #endregion
       


        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                status_ui_txt.text = "Online mode."; /// Do stuff if connected
          
                OpenSavedGame(fileName); // Stuff that opens save game - do this also when saving I guess.
            }
            else
            {
                // will happen inside editor
               
                status_ui_txt.text = "Unable to connect to Google play games."; /// Do stuff if didn't connect
            }
        });


    }



    #region OpenSave
    // Stuff that opens save---------------------------------------------------------------------------
    void OpenSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (initialization && !saving)
            {
                status_ui_txt.text += "Success opening saved game";
                initialization = false;
                if (game.TotalTimePlayed.TotalSeconds < 1 )
                {
                    // no save on cloud.
                    status_ui_txt.text += " - Nothing on cloud.";
                    status_ui_txt.text += game.TotalTimePlayed.TotalSeconds.ToString();
                    firstTimeCloud = true;
                    //add first save to cloud if local save is empty too
                    if (firstTimeLocal)
                    {
                        status_ui_txt.text += " - First time local.";
                        saving = true;
                        SaveGame(game, Serialize<Database>(gameData), mTotalPlayingTime.Add(DateTime.Now.Subtract(mStartTime)));
                        SaveLocal<Database>(gameData);
                    }
                    else
                    {
                        status_ui_txt.text += " - rading local and saving to cloud.";
                        gameData = LoadLocal<Database>();
                        SaveGame(game, Serialize<Database>(gameData), mTotalPlayingTime.Add(DateTime.Now.Subtract(mStartTime)));
                        //read local save and write it to cloud
                    }
                }
                else
                {
                    status_ui_txt.text += " - found save on cloud... Loading...";
                    //read game save - only called if successful authentication.
                    LoadGameData(game); // ---------------------------------------------------------------------------------- Need to compare which save is older.
                }

            }
            else 
            {
                saving = true;
                SaveGame(game, Serialize<Database>(gameData), mTotalPlayingTime.Add(DateTime.Now.Subtract(mStartTime)));
                SaveLocal<Database>(gameData);
            }
            // handle reading or writing of saved game.
        }
        else
        {
            if (initialization)
            {
                gameData = LoadLocal<Database>();
                initialization = false;
            }
            else
                 SaveLocal<Database>(gameData);
                // handle error
                status_ui_txt.text += "error in read/write cloud of save game. - saved to local"; /// Do stuff if didn't connect
        }
    }
    // Stuff that opens save---------------------------------------------------------------------------
    #endregion


    #region SaveGameData
    // Stuff that saves game data---------------------------------------------------------------------------
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
        }
        else
        {
            status_ui_txt.text += "error finishing save to cloud"; 
            // handle error
        }
    }
    // Stuff that saves game data---------------------------------------------------------------------------
    #endregion


    #region ReadSavedData
    // Stuff that reads game data---------------------------------------------------------------------------
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

            mStartTime = DateTime.Now;
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
        {
            return null;
        }
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);
        return ms.ToArray();
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



    private T LoadLocal<T>() where T : class
    {
        if (firstTimeLocal)
        {
            SaveLocal<Database>(gameData);
        }
        else
        {
            string localDataStr = PlayerPrefs.GetString(fileName);
            byte[] localByteArr = Encoding.ASCII.GetBytes(localDataStr);
            return Deserialize<T>(localByteArr);
        }
        return null;
    }

    void SaveLocal<T>( T data) {
        byte[] localByteArr = Serialize<Database>(gameData);
        string localDataStr = Encoding.ASCII.GetString(localByteArr);
        PlayerPrefs.SetString(fileName, localDataStr);
        //- also save timeplayed.
    }


}

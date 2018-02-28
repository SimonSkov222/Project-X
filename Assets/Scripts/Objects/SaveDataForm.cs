using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Kan gemme værdi i en fil som så kan hentes senere.
//  filen kan ikke læse eller ændres af andre end denne
//  klasse, da der bruges kryptering.
//
////////////////////////////////////////////////////////////////////
public class SaveDataForm
{

    ///////////////////////////////
    //      Private Static Fields
    ///////////////////////////////
    private static byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8 }; // The secret key to be used for the symmetric algorithm. 
    private static byte[] iv = { 1, 2, 3, 4, 5, 6, 7, 8 };  // The IV to be used for the symmetric algorithm.
    private static DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    
    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public string Filename { get; private set; }
    public string FullFilename { get { return string.Format("{0}/{1}.dat", Application.persistentDataPath, Filename); } }
    public bool IsEmpty { get { return data.Count == 0; } }

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Dictionary<string, object> data = new Dictionary<string, object>();
    
    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Gør at man skal give klassen et filnavn.
    /// </summary>
    public SaveDataForm(string filename)
    {
        Filename = filename;
    }
    /// <summary>
    /// Tjekker om en nøgle for en værdi findes.
    /// </summary>
    public bool HasKey(string key) { return data.ContainsKey(key); }

    /// <summary>
    /// Tilføjer en ny værdi der skal gemmes i filen.
    /// hvis nøglen ikke findes bliver den tilføjet,
    /// hvis den findes bliver den overskrevet.
    /// </summary>
    public void PutValue(string key, object value)
    {
        if (HasKey(key))
        {
            data[key] = value;
        }
        else
        {
            data.Add(key, value);
        }
    }

    /// <summary>
    /// Henter en værdi fra filen. man kan definere hvad type
    /// værdi skal være. Fejl vil ske ved forkert type værdi.
    /// </summary>
    public T GetValue<T>(string key) { return (T)data[key]; }
    
    /// <summary>
    /// Gemmer dataen som en fil der kan blive læst senere.
    /// Der bruges encryption.
    /// </summary>
    public void Save()
    {
        // Encryption
        using (var fs = new FileStream(FullFilename, FileMode.Create, FileAccess.Write))
        using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(cryptoStream, data);
        }
    }

    #endregion

    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Læser en fil der er blevet gem ved hjælp af denne class.
    /// Gemmer alt dataen fra filen ind i en ny SaveDataForm class.
    /// 
    /// null hvis filen ikke findes.
    /// </summary>
    public static SaveDataForm Load(string filename)
    {

        string fullName = string.Format("{0}/{1}.dat", Application.persistentDataPath, filename);

        if (File.Exists(fullName))
        {
            var sdf = new SaveDataForm(filename);
            // Decryption
            using (var fs = new FileStream(fullName, FileMode.Open, FileAccess.Read))
            using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // This is where you deserialize the class
                sdf.data = (Dictionary<string, object>)formatter.Deserialize(cryptoStream);
            }

            return sdf;
        }

        return null;
    }

    #endregion
}
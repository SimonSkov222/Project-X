using System.Collections.Generic;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Denne helper klasse gør det nemt at gemme 
//  og hente data fra forskellige filer.
//
////////////////////////////////////////////////////////////////////
public static class SaveDataHelper
{
    ///////////////////////////////
    //      Private Static Fields
    ///////////////////////////////
    private static Dictionary<string, SaveDataForm> files = new Dictionary<string, SaveDataForm>();

    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Henter en fil hvis den findes bliver den læst ind, 
    /// hvis den ikke findes kan man vælge at oprette den.
    /// </summary>
    public static SaveDataForm GetFileForm(string filename, bool create = false)
    {
        if (files.ContainsKey(filename))
        {
            return files[filename];
        }
        else
        {
            //Læs filen sdf vil være null hvis den ikke findes
            SaveDataForm sdf = SaveDataForm.Load(filename);

            //Hvis den findes eller skal oprettes bliver den gemt i listen
            if (sdf != null)
            {
                files.Add(filename, sdf);
                return files[filename];
            }
            else if (create)
            {
                files.Add(filename, new SaveDataForm(filename));
                return files[filename];
            }
        }

        return null;
    }

    /// <summary>
    /// Henter en værdi fra filen. man kan definere hvad type
    /// værdi skal være. Fejl vil ske ved forkert type værdi.
    /// </summary>
    public static T GetValue<T>(string filename, string key)
    {
        SaveDataForm sdf = GetFileForm(filename);
        return sdf.GetValue<T>(key);
    }

    /// <summary>
    /// Tilføjer en ny værdi der skal gemmes i filen.
    /// hvis nøglen ikke findes bliver den tilføjet,
    /// hvis den findes bliver den overskrevet.
    /// </summary>
    public static void PutValue(string filename, string key, object value)
    {
        SaveDataForm sdf = GetFileForm(filename, true);
        sdf.PutValue(key, value);
    }
   
    /// <summary>
    /// Gemmer dataen for en fil der kan blive læst senere.
    /// </summary>
    public static void SaveFile(string filename)
    {
        SaveDataForm sdf = GetFileForm(filename, true);
        sdf.Save();
    }

    /// <summary>
    /// Gemmer alle filer der blivet hentet via. GetFileForm()
    /// </summary>
    public static void SaveAll()
    {
        foreach (var file in files)
        {
            file.Value.Save();
        }
    }

    #endregion
}



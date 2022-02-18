using System.Collections.Generic;
using UnityEngine;


public class SoundLoader : MonoBehaviour
{
    Dictionary<string, AudioClip> clips;
    string soundFolderName = "Sounds";
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        LoadAll();
    }

    void LoadAll()
    {
        clips = new Dictionary<string, AudioClip>();

        var temp = Resources.LoadAll<AudioClip>(soundFolderName);

        for (int i = 0; i < temp.Length; i++)
            clips.Add(temp[i].name, temp[i]);
    }

    public AudioClip GetClip(string name)
    {
        if (clips.ContainsKey(name)) return clips[name];
        return null;
    }


}

using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    Dictionary<string, AudioClip> clips;
    string soundFolderName = "Sounds";

    
    static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<SoundManager>();

        if (objs.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        if (instance == null) instance = this;

        LoadAll();
    }

    void LoadAll()
    {
        clips = new Dictionary<string, AudioClip>();

        var temp = Resources.LoadAll<AudioClip>(soundFolderName);

        for (int i = 0; i < temp.Length; i++)
        {
            clips.Add(temp[i].name, temp[i]);
        }
    }

    public AudioClip GetClip(string name)
    {
        if (clips.ContainsKey(name)) return clips[name];
        return null;
    }


}

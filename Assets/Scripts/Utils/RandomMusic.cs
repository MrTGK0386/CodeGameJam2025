#if UNITY_EDITOR
using UnityEditor; // Nécessaire pour utiliser AssetDatabase
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] mySounds;

    private AudioClip activeSound;

#if UNITY_EDITOR
    [ContextMenu("Load Audio From Folder")]
    void LoadAudioFromFolder()
    {
        string folderPath = "Assets/Art/Audio/Music"; // Chemin du dossier

        // Trouver tous les assets de type AudioClip dans le dossier spécifié
        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { folderPath });
        mySounds = new AudioClip[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            mySounds[i] = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
        }

        Debug.Log($"{mySounds.Length} fichiers audio chargés depuis {folderPath}.");
    }
#endif

    void Start()
    {
        LoadAudioFromFolder();
    }
    
    public void PlayRandomSound()
    {
        if (mySounds.Length > 0)
        {
            audioSource.Stop();
            activeSound = mySounds[Random.Range(0, mySounds.Length)];
            audioSource.PlayOneShot(activeSound);
            Debug.Log($"Joue le son : {activeSound.name}");
        }
        else
        {
            Debug.LogWarning("Le tableau mySounds est vide. Chargez des fichiers audio.");
        }
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
using System.IO;

public class directoryDeleterCreator : MonoBehaviour
{

    public List<string> directories;
    public List<string> disposableDirectories;

    public void deleteDirectories()
    {
        for (int i = 0; i < directories.Count; i++)
        {
            deleteAllFilesInADirectory(directories[i]);
            readWriteData.deleteFolder(directories[i]);
        }
    }

    public void deleteDisposableDirectories()
    {
        for (int i = 0; i < disposableDirectories.Count; i++)
        {
            deleteAllFilesInADirectory(disposableDirectories[i]);
            readWriteData.deleteFolder(disposableDirectories[i]);
        }
    }

    public void deleteFilesInDirectories()
    {
        for (int i = 0; i < disposableDirectories.Count; i++)
        {
            deleteAllFilesInADirectory(disposableDirectories[i]);
        }
    }

    public void createDirectories()
    {
        for (int i = 0; i < directories.Count; i++)
        {
            readWriteData.Createfolder(directories[i]);
        }
    }

    public void deleteAllFilesInADirectory(string folder)
    {
        string[] files = Directory.GetFiles(readWriteData.GetPath(folder));
        if (files.Length == 0)
            return;

        for (int i = 0; i < files.Length; i++)
        {
            Debug.Log(files[i] + " is deleted!");
            File.Delete(files[i]);
        }

    }

}


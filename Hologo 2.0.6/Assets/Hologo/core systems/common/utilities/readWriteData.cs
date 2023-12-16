using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using AvoEx;


namespace Hologo2
{
    public static class readWriteData
    {

        //writing file to disk string data
        public static bool WriteFileToDisk(string data,bool checkFileExits, params string[] pathStrings)
        {
            string filepath = GetPath(pathStrings);
            // string encryptedData = data;
            string encryptedData = AesEncryptor.Encrypt(data);
            IosNoBackupFlagging(filepath);

            // creating the folder if the folder doesnt exist
            if (!Createfolder(CombineFolderExcludeLast(pathStrings)))
                return false;

            // if checkFileExits bool is checked then it wont overrite the file and returns a false
            if (checkFileExits && CheckIfFileExists(filepath))
            {

                return false;

            }
            else
            {

                //this method will write all data to disk and then close and then proceed to next 
                File.WriteAllText(filepath, encryptedData);
                // after writing the files checks if its actualy written 
                if (CheckIfFileExists(filepath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //returns a true if the folder is created or false if not
        public static bool Createfolder(string folderName)
        {
            string p = folderName + "/";
            string filepath = Path.Combine(Application.persistentDataPath, p);
//            Debug.Log(filepath);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
                if (Directory.Exists(filepath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        // delete a folder. returns true if successFull;
        public static bool deleteFolder(string folderName)
        {
            string p = folderName + "/";
            string filepath = Path.Combine(Application.persistentDataPath, p);
            if(Directory.Exists(filepath))
            {
               // Directory.Delete(filepath,true);
                Directory.Delete(filepath);
            }

            if (Directory.Exists(filepath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //read file from disk
        public static string ReadFileFromDisk(params string[] pathStrings)
        {
            string filePath = GetPath(pathStrings);
            string contents = File.ReadAllText(filePath);
            contents = AesEncryptor.DecryptString(contents);
            // return decryptData;
            return contents;
            //Decryption part
        }

        public static Texture2D LoadAnImageFromDisk(int width, int height, string stringPath)
        {

           // string filePath = GetPath(stringPath);
            byte[] bytes = File.ReadAllBytes(stringPath);

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.LoadImage(bytes);
            return texture;
        }

        public static async Task<Texture2D> ReadImage(int width, int height,params string[] stringPath)
        {
            string filePath = GetPath(stringPath);
            Task<byte[]> task = new Task<byte[]>(() => readBytes(filePath));
            task.Start();
            byte[] bytes = await task;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.LoadImage(bytes);
            await Task.Yield();
            return texture;
        }

        static byte[] readBytes(string stringPath)
        {
            byte[] bytes = File.ReadAllBytes(stringPath);
            return bytes;
        }


        //partial async Task<string> readFileAsync(string fileName, params string[] folders)
        //{
        //    string filePath = GetPath(CombineFolders(folders), fileName);
        //    string contents = await File.ReadAllText(filePath);
        //}


        // deleting file on storage
        public static bool DeleteFileOnDisk(params string[] pathStrings)
        {
           
            string filePath = GetPath(pathStrings);
            Debug.Log("file to delete "+filePath);
            if (CheckIfFileExists(pathStrings))
           {
                File.Delete(filePath);
                //TODO: check for error and couldnt delete flag
            }
            return true;
        }


        public static void IosNoBackupFlagging(string path)
        {
#if(PLATFORM_IOS)
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
        }



        // for just file
        //public static string GetPath(string fileName)
        //{
        //    return (Path.Combine(Application.persistentDataPath, fileName));
        //}

        //// for folder/folders and file
        //public static string GetPath(string fileName, params string[] folders)
        //{
        //    string p = CombineFolders(folders) + "/" + fileName;
        //    return (Path.Combine(Application.persistentDataPath, p));
        //}

        // for folder/folders and file at the end
        public static string GetPath(params string[] foldersAndFile)
        {
            string p = CombineFolders(foldersAndFile);
            return (Path.Combine(Application.persistentDataPath, p));
        }

        //combining a list of folders and returns them as a string
        static string CombineFolders(params string[] folders)
        {
            string foldersString = "";
            if (folders.Length <= 0)
                return foldersString;

            if (folders.Length == 1)
            {
                foldersString = folders[0];
            }
            else
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    string a = "";
                    if (i < folders.Length - 1)
                    {
                        a = folders[i] + "/";
                    }
                    else
                    {
                        a = folders[i];
                    }
                    foldersString = foldersString + a;
                }

            }

            return foldersString;
        }

        public static string CombineFolderExcludeLast(params string[] folders)
        {
            string foldersString = "";
            if (folders.Length <= 0)
                return foldersString;

            if (folders.Length == 1)
            {
                foldersString = folders[0];
            }
            else
            {
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    string a = "";
                    if (i < folders.Length - 1)
                    {
                        a = folders[i] + "/";
                    }
                    else
                    {
                        a = folders[i];
                    }
                    foldersString = foldersString + a;
                }

            }

            return foldersString;
        }



        //checking to see if a file exits in persistant datapath. 
        //params list will accept folders in series and a file at the end.
        //eg CheckIfFileExists("folder01","folder02","text.txt");
        public static bool CheckIfFileExists(params string[] list)
        {
            string a = CombineFolderExcludeLast(list);
            string folderPath = Path.Combine(Application.persistentDataPath, a);

            if (!Directory.Exists(folderPath))
                return false;

            string p = CombineFolders(list);
            

            string filePath = Path.Combine(Application.persistentDataPath, p);

            //checking if file exists
            if (!File.Exists(filePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}

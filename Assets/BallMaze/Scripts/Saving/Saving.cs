using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Utilities
{
    public enum SerializerType
    {
        XML,
        BINARY,
    }

    public static class Saving
    {
        public static string Path
        {
            get
            {
                return "";
            }
        }
        private static Serializer<Class> GetSerializer<Class>(SerializerType type)
        {
            switch (type)
            {
                case SerializerType.BINARY:
                    return new MyBinaryFormatter<Class>();
                case SerializerType.XML:
                    return new MyXmlSerializer<Class>();
                default:
                    Debug.LogError("There is no serializer corresponding to this enum type : " + type);
                    return null;
            }
        }

        public static void Save<Class>(string path, Class objectToSerialise)
        {
            Save(path, objectToSerialise, SerializerType.XML);
        }

        public static void Save<Class>(string path, Class objectToSerialise, SerializerType type)
        {
            Serializer<Class> serializer = GetSerializer<Class>(type);
            path = GetPath(path, serializer);
            FileStream file = File.Create(path);

            serializer.Serialize(file, objectToSerialise);
            file.Close();
            //Debug.Log("Saved to " + path);
        }

        public static bool TryLoad<Class>(string path, out Class value)
        {
            return TryLoad<Class,Class>(path, out value, SerializerType.XML);
        }

        public static bool TryLoad<Class>(string path, out Class value, SerializerType type)
        {
            return TryLoad<Class, Class>(path, out value, type);
        }

        public static bool TryLoad<SubClass,Class>(string path,out Class value) where SubClass : Class
        {
            return TryLoad<SubClass,Class>(path,out value, SerializerType.XML);
        }

        public static bool TryLoad<SubClass,Class>(string path, out Class value, SerializerType type) where SubClass : Class
        {
            Serializer<SubClass> serializer = GetSerializer<SubClass>(type);
            path = GetPath(path, serializer);
            //Debug.Log("Loading : " + path);
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);

                value = serializer.Deserialize(file);
                file.Close();
                return true;
            }
            Debug.LogWarning("File doesn't exist : " + path);
            value = default(Class);
            return false;
        }

        private static string GetPath<Class>(string path, Serializer<Class> serializer)
        {
            path = path + serializer.Extension;
            return path;
        }
    }

    internal interface Serializer<Class>
    {
        string Extension
        {
            get;
        }

        void Serialize(FileStream file, Class objectToSerialise);

        Class Deserialize(FileStream file);
    }

    internal class MyXmlSerializer<Class> : Serializer<Class>
    {

        XmlSerializer serializer = new XmlSerializer(typeof(Class));

        public string Extension
        {
            get
            {
                return ".xml";
            }
        }

        public Class Deserialize(FileStream file)
        {
            return (Class)serializer.Deserialize(file);
        }

        public void Serialize(FileStream file, Class objectToSerialise)
        {
            serializer.Serialize(file, objectToSerialise);
        }
    }

    internal class MyBinaryFormatter<Class> : Serializer<Class>
    {

        BinaryFormatter serializer = new BinaryFormatter();
        public string Extension
        {
            get
            {
                return ".dat";
            }
        }


        public Class Deserialize(FileStream file)
        {
            return (Class)serializer.Deserialize(file);
        }

        public void Serialize(FileStream file, Class objectToSerialise)
        {
            serializer.Serialize(file, objectToSerialise);
        }
    }
}


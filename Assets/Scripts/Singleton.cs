using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robotry.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static object syncobj = new object();
        private static bool applsClosing = false;

        public static T Instance
        {
            get
            {
                if (applsClosing)
                {
                    return null;
                }
                lock (syncobj)
                {
                    if (instance == null)
                    {
                        T[] objs = FindObjectsOfType<T>();

                        if(objs.Length > 0)
                        {
                            instance = objs[0];
                        }

                        if(objs.Length > 1)
                        {
                            Debug.LogError("There is more than one " + typeof(T).Name);
                        }

                        if(instance == null)
                        {
                            string objName = typeof(T).ToString();
                            GameObject obj = GameObject.Find(objName);
                            if(obj == null)
                            {
                                obj = new GameObject(objName);
                            }
                            instance = obj.AddComponent<T>();
                        }
                    }
                    return instance;
                }
            }
        }

        public virtual void OnDestroy()
        {
            instance = null;
        }
    }
}

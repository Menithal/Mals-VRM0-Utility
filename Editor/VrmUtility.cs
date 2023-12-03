/*
 *    Copyright 2023 Matti Lahtinen

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
#if UNITY_EDITOR && UNITY_2019
using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UniGLTF;
using UnityEditor;
using UnityEngine;
using VRM;
using Object = UnityEngine.Object;

namespace Malactus.VrmTools.Editor
{   
    
    public class MalToolException: Exception
    {
        public MalToolException(string message): base(message)
        {
        }
    }
    

    public static class VrmUtility 
    {
        public static void RemoveComponents<T>(T[] components) where T:UnityEngine.Component
        {
            foreach (var component in components)
            {
                Object.Destroy(component);
            }
        }
        public static void DeleteVrmBlendShapeClip(VRMBlendShapeProxy proxy, BlendShapeClip clip)
        {
            string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(proxy.BlendShapeAvatar));
            string path = Path.Combine(dir,  clip.BlendShapeName + ".asset").ToUnityRelativePath();
            proxy.BlendShapeAvatar.Clips.Remove(clip);
            AssetDatabase.DeleteAsset(path); 
        }

        [NotNull]
        public static VRMBlendShapeProxy GetVrmBlendshapeProxy(GameObject gameObject)
        { 
            if (gameObject == null) throw new MalToolException("No object selected.\n\n Select a VRM Model");
            
            VRMBlendShapeProxy blendshapeProxy = gameObject.GetComponent<VRMBlendShapeProxy>();
            
            if(blendshapeProxy == null) throw new MalToolException("Selected Object has not processed by UniVRM.\n\nMake sure VRMBlendShapeProxy is present, or that the Object Root is selected");

            return blendshapeProxy;
        }
       
        
    }
     
    ///
    /// <summary> Object which contains all relevant information for most operations in the Utility..</summary>
    ///
    public class VrmBindings
    {
        public GameObject FpvBoneOffset {  get; set; }
        public Dictionary<string, List<Transform>> SpringBoneGroups { get; set; }
        public GameObject SpringBoneRoot { get; set; }
        public Dictionary<string, GameObject> ColliderGroups { get; set; }

        public VrmBindings()
        {
            SpringBoneGroups = new Dictionary<string, List<Transform>>();
            SpringBoneGroups.Add("Default", new List<Transform>());
            ColliderGroups = new Dictionary<string, GameObject>();
        }
    }

    
    
    
}
#endif
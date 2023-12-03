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

using System;

using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UniGLTF;
using UnityEditor;
using UnityEngine;
using VRM;

#if UNITY_EDITOR && UNITY_2019
namespace Malactus.VrmTools.Editor
{
    public static class BlendShapeUtility {
         private static Regex DECORATIVE_FILTER = new Regex("^[=\\-*{|[(\\s]+.+[=\\-*}|\\])\\\\\\/]");
        ///
        /// <summary> Gets all the Blendshapes as a collected list of names, with a possibility to filter some out.</summary>
        ///
        public static List<string> FindAllInChildren(GameObject parent, string[] blacklist = null, string[] whitelist = null)
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers = parent.GetComponentsInChildren<SkinnedMeshRenderer>();
            List<string> foundBlendshapes = new List<string>();

            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                Mesh mesh = skinnedMeshRenderer.sharedMesh;
                
                if(mesh.blendShapeCount == 0) continue; // Skip mesh if no blendshapes exists
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    string blendshapeName = mesh.GetBlendShapeName(i);
                    string lowerCase = blendshapeName.ToLower();

                    if (DECORATIVE_FILTER.Match(lowerCase).Success)
                    {
                        Debug.Log("Found Decorating Blendshape, Skipping");
                        continue;
                    } 
                    if (whitelist != null && !whitelist.Contains(lowerCase))
                    {
                        Debug.Log("FindAllBlendshapesInChildren: Could not find " + blendshapeName + " in whitelist, Skipping.");
                        continue;
                    } else if (blacklist != null && blacklist.Contains(lowerCase))
                    {
                        Debug.Log("FindAllBlendshapesInChildren: Found " + blendshapeName + " in blacklist, Skipping.");
                        continue;
                    }

                    if (!foundBlendshapes.Contains(blendshapeName))
                    {
                        foundBlendshapes.Add(blendshapeName);
                    }
                }
            }
            return foundBlendshapes;
        }
    }

    public static class BlendShapeClipUtility
    {
        public static void Nuke()
        {
            VRMBlendShapeProxy blendShapeProxy = VrmUtility.GetVrmBlendshapeProxy(Selection.activeGameObject);

            BlendShapeClip[] clips = blendShapeProxy.BlendShapeAvatar.Clips.ToArray();
            foreach (var clip in clips)
            {
                VrmUtility.DeleteVrmBlendShapeClip(blendShapeProxy,clip);
            }
        }
        
        ///
        /// <summary> Binds All Found Blendshapes to the VRM BlendshapeClips. Does not set if BlendshapeClip already exists and has existing values. </summary>
        ///
        public static void BindBlendShapesToClips()
        {
            GameObject obj = Selection.activeGameObject;
            VRMBlendShapeProxy blendshapeProxy = VrmUtility.GetVrmBlendshapeProxy(obj);

            Debug.LogFormat("found blendshape proxy {0}", obj.name.ToString());

            // Following Conditions must be met before
            // Object must be selected 
            // VRMBlendshapeProxy must be present
            SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

            BlendShapeAvatar blendshapeAvatar = blendshapeProxy.BlendShapeAvatar;

            // For each Blendshape Assignment (BlendShapeClip)
            foreach (BlendShapeClip clip in blendshapeAvatar.Clips)
            {
                if (clip.Values.Length == 0)
                {

                    List<BlendShapeBinding> bindings = new List<BlendShapeBinding>();

                    foreach (SkinnedMeshRenderer skinnedMesh in skinnedMeshRenderers)
                    {
                        // Iterate every mesh
                        // Check if mesh has blendshapes with name or vrc.<name> lowercase

                        string objectName = skinnedMesh.gameObject.name;
                        Mesh mesh = skinnedMesh.sharedMesh;

                        if (mesh.blendShapeCount == 0) continue; // Skip mesh if no blendshapes exists

                        // do a broad search

                        int index = -1;
                        for (int i = 0; i < mesh.blendShapeCount; i++)
                        {
                            string blendshapeName = mesh.GetBlendShapeName(i);

                            if (BlendshapeDictionary.VrcToVrmBinding.TryGetValue(blendshapeName,
                                    out string vrmBlendshape))
                            {
                                if (vrmBlendshape == clip.BlendShapeName.ToLower())
                                {
                                    index = i;
                                    break;
                                }
                            }
                            else if (string.Equals(blendshapeName.Replace("_", "").Replace(" ", "").ToLower(),
                                         clip.BlendShapeName.Replace(" ", "").ToLower(),
                                         StringComparison.Ordinal))
                            {
                                // We are  using ToLower for the match, since it is UNLIKELY for a content creator to both in their naming schema

                                // There are however CamelCase and under_score schools of thought so lets just make sure EVERYTHING is same
                                // There is NO consistancy between VRM, VRC and VSF Language as VSF accepts "Brows up", 
                                //  meanwhile VRC uses OVR Standards where viseme might be written with vrc.v_<viseme> and avoids spaces
                                // It is unlikely for a content creator to mix names in their workflow however, unless they do not know what they are doing, but in this case we just want to bind
                                // for the clip.BlendShapeName, we can use _, so that matchup should be valid.
                                // Match made, fall off with index 
                                index = i;
                                break;
                            }
                        }

                        // Match was found, then do binding, otherwise,  skip.
                        if (index >= 0)
                        {
                            // Assume one to one bind
                            BlendShapeBinding binding = new BlendShapeBinding();
                            binding.Index = index;
                            binding.Weight = 100.0f;
                            binding.RelativePath = objectName;

                            bindings.Add(binding);
                        }
                    }

                    if (bindings.Count > 0)
                    {
                        // Move new instance values to old
                        clip.Values = bindings.ToArray();
                        Debug.LogFormat("Applying: {0}", clip.BlendShapeName);
                    }
                }
                else
                {
                    Debug.Log("Skipping Clip as previously set: - " + clip.BlendShapeName);
                }
            }
        }
        
        ///
        /// <summary> Generates VRM BlendshapeClips to the VRMBlendShapeProxy. This Will not Override existing BlendshapeProxies! </summary>
        ///
        public static void GenerateClips(string[] blendshapes){
            GameObject obj = Selection.activeGameObject;
            
            VRMBlendShapeProxy blendshapeProxy = VrmUtility.GetVrmBlendshapeProxy(obj);

            Debug.Log("Found BlendShapeProxy");
            BlendShapeAvatar blendshapeAvatar = blendshapeProxy.BlendShapeAvatar;
            
            // If no clips exists, error out! - mal (2020)
            // Wait why? - mal (2023)
            /*if(blendshapeAvatar.Clips.Count == 0){
                throw new MalToolException("Selected object does not seem to have any valid BlendshapeClips to bind to!\n\n Either Generate New Clips, or make sure you have exported the Model once as VRM before binding.");
            }*/
            
            Debug.Log("Found BlendShapeAvatars " + blendshapeAvatar);
            Debug.Log("Opening Path" + AssetDatabase.GetAssetPath(blendshapeAvatar));
            string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(blendshapeAvatar));
            Debug.Log(dir);
            foreach(string blendshape in blendshapes){
                // TODO: Find a better way to do this.
                bool createClip = false;
                try {
                    BlendShapeClip clip = blendshapeAvatar.Clips.Find(x=> blendshape.Equals(x.Key.Name));
                    if (clip == null){
                        Debug.Log("Generating BlendshapeClip " + blendshape);
                        createClip = true;
                    }
                    // this search will throw and Exception if clip cannot be found. Not the BEST way to do this, and I rather not do it via a try catch 
                    // but its the main way to make sure any thing new will be bound correctly to a new file.
                } catch(Exception){
                    createClip = true;
                }

                if(createClip){
                    Debug.Log("Generating BlendshapeClip " + blendshape);
                    string path = Path.Combine(dir, blendshape + ".asset").ToUnityRelativePath();
                    BlendShapeClip clip = BlendShapeAvatar.CreateBlendShapeClip(path);
                    blendshapeAvatar.Clips.Add(clip);
                }
            }
        }
    }
}
#endif
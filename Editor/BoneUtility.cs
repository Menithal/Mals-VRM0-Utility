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
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using VRM;

namespace Malactus.VrmTools.Editor
{
    public static class BoneUtility {
        private static Regex PHYSICS_BONE_REXP = new Regex(".+[_\\.]phys_?(\\d*)$", RegexOptions.Singleline);
        private static Regex SPRING_BONE_COMPONENT_NAMING =
            new Regex("^MalTools:\\s?(Default|(Group\\s(\\d+)))([\\s:-].*)?$", RegexOptions.Singleline);

        ///
        /// <summary> Searches through the GameObject's Hierarchy for Vrm Specific components and returns an object with ready bindings </summary>
        ///
        public static VrmBindings GetVrmBindings(GameObject parent)
        {
            VrmBindings bindings = new VrmBindings();
            VRMSpringBone[] vrmSpringBones = parent.GetComponentsInChildren<VRMSpringBone>();

            if (vrmSpringBones.Length == 0)
            {
                throw new MalToolException(
                    "Could not find any springbones set, is the VRM Model Selected in Scene? If so, add single springbone to a child gameobject to establish where you want spring bones to go to.");
            }

            bindings.SpringBoneRoot = vrmSpringBones[0].gameObject;

            VRMHumanoidDescription avatarDescription = parent.GetComponent<VRMHumanoidDescription>();

            Avatar avatar = avatarDescription.Avatar;
            HumanBone[] bones = avatar.humanDescription.human;
            Transform[] objects = parent.GetComponentsInChildren<Transform>();

            Dictionary<string, string> colliderDictionary = new Dictionary<string, string>();
        
            foreach (HumanBone bone in bones)
            {
                if (bone.humanName.Contains("Hips") || 
                    bone.humanName.Contains("Chest") ||
                    bone.humanName.Contains("Head") || 
                    bone.humanName.Contains("Hand") || 
                    bone.humanName.Contains("Distal"))
                {
                    colliderDictionary.Add(bone.boneName, bone.humanName);
                }
            }
            
            foreach (Transform transform in objects)
            {
                string name = transform.name.ToLower();

                if (name == "firstpersonbone" || name ==
                        "fpv" || name == "firstpersonoffset")
                {
                    bindings.FpvBoneOffset = transform.gameObject;
                } else if (name == "secondary")
                {
                    bindings.SpringBoneRoot = transform.gameObject;
                }

                if (colliderDictionary.ContainsKey(transform.name))
                {
                    colliderDictionary.TryGetValue(transform.name, out string value);
                    System.Diagnostics.Debug.Assert(value != null, nameof(value) + " != null");
                    bindings.ColliderGroups.Add(value, transform.gameObject);
                }
                
                Match matchedBones = PHYSICS_BONE_REXP.Match(name);

                if (matchedBones.Success)
                {
                    if (matchedBones.Groups[1].Success)
                    {
                        // Generate All Physics Groupings
                        string index = matchedBones.Groups[1].ToString();

                        if (bindings.SpringBoneGroups.TryGetValue("Group " + index, out List<Transform> group))
                        {
                            group.Add(transform);
                        }
                        else
                        {
                            List<Transform> newGroup = new List<Transform>();
                            newGroup.Add(transform);
                            bindings.SpringBoneGroups.Add("Group " + index, newGroup);
                        }
                    }
                    else
                    {
                        // Else Default Physics Group
                        // Below should *always* be present.
                        bindings.SpringBoneGroups.TryGetValue("Default", out List<Transform> defaults);
                        System.Diagnostics.Debug.Assert(defaults != null, nameof(defaults) + " != null");
                        defaults.Add(transform);
                    }
                }
            }
            
            return bindings;
        }

        
        ///
        /// <summary> Simple Class to contain the matched regular expression with the VRMSpringBone component </summary>
        ///
        private class SpringBoneComponentInfo
        {
            public Match Match { get; }
            public VRMSpringBone Component { get;  }
            public SpringBoneComponentInfo(Match match, VRMSpringBone component)
            {
                Match = match;
                Component = component;
            }
        }
        
        
        ///
        /// <summary> A VrmBoneBindings object, adjusts VRM Component settings to match the skeleton, Creates SpringBone Bindings as well </summary>
        ///
        public static void GenerateSpringBones(VrmBindings bindings)
        {
            GameObject obj = Selection.activeGameObject;
            
            // Set FPV
            VRMFirstPerson fpvScript = obj.GetComponent<VRMFirstPerson>();
            fpvScript.FirstPersonBone = bindings.FpvBoneOffset.transform;

            // Set Spring Bones 
            VRMSpringBone[] existingSpringBones = bindings.SpringBoneRoot.GetComponentsInChildren<VRMSpringBone>();
            int meshSpringBoneGroupCount = bindings.SpringBoneGroups.Count;
            
            Dictionary<string, SpringBoneComponentInfo> matchedDictionary = new Dictionary<string, SpringBoneComponentInfo>();

            if (existingSpringBones.Length == 1)
            {
                if (existingSpringBones[0].m_comment == "")
                {
                    existingSpringBones[0].m_comment = "MalTools: Default";
                }
            }
            // First Find Existing Groups
            foreach (VRMSpringBone group in existingSpringBones)
            {
                Match match = SPRING_BONE_COMPONENT_NAMING.Match(group.m_comment);
                        
                if (match.Success)
                {
                    matchedDictionary.Add(match.Groups[1].Value, new SpringBoneComponentInfo(match, group));
                }
            }

            // Then Create new groups if Missing
            if (meshSpringBoneGroupCount != matchedDictionary.Count)
            {
                List<string> existingGroups = new List<string>(matchedDictionary.Keys);
                List<string> targetGroups = new List<string>(bindings.SpringBoneGroups.Keys);

                var missingGroups = targetGroups.Except(existingGroups);

                foreach (var group in missingGroups)
                {
                    VRMSpringBone springBone = bindings.SpringBoneRoot.AddComponent<VRMSpringBone>();
                    springBone.m_comment = "MalTools: " + group;
                    
                    bindings.SpringBoneGroups.TryGetValue(group, out List<Transform> bones);
                    springBone.RootBones = bones;
                }
            }
            
            // Bind Bones to Groups
            foreach (var pairs in bindings.SpringBoneGroups)
            {
                string name = pairs.Key;
                // Default, Group #

                if (matchedDictionary.TryGetValue(name, out SpringBoneComponentInfo componentInfo))
                {
                    // So Found the match name,
                    componentInfo.Component.RootBones = pairs.Value;
                }
            }
        }

        private static void BindColliderWithRadiusIfNoneExist(GameObject obj, float radius)
        {
            if (obj.GetComponent<VRMSpringBoneColliderGroup>() == null)
            {
                VRMSpringBoneColliderGroup group = obj.AddComponent<VRMSpringBoneColliderGroup>();
                group.Colliders[0].Radius = radius;
            }
        }
        
        ///
        /// <summary> Given an object and A VrmBoneBindings object, adds ColliderGroup Components to bones. </summary>
        ///
        public static void GenerateSpringBoneColliders(GameObject obj, VrmBindings bindings, bool additional = false)
        {
            foreach (var colliderKey in bindings.ColliderGroups.Keys)
            {
                bindings.ColliderGroups.TryGetValue(colliderKey, out GameObject colliderParent);
                
                if (colliderKey.Contains("Index Distal") || (additional && colliderKey.Contains("Distal") )) 
                {
                    BindColliderWithRadiusIfNoneExist(colliderParent, 0.015f);
                } else if (colliderKey.Contains("Hand"))
                {
                    BindColliderWithRadiusIfNoneExist(colliderParent, 0.03f);
                } else if (colliderKey.Contains("Head") || (additional && colliderKey.Contains("Chest") ) ) 
                {
                    BindColliderWithRadiusIfNoneExist(colliderParent, 0.1f);
                }
            }

            VRMSpringBoneColliderGroup[] colliderGroups = obj.GetComponentsInChildren<VRMSpringBoneColliderGroup>();
            VRMSpringBone[] springBones = obj.GetComponentsInChildren<VRMSpringBone>();
            
            // Bind Springbone
            foreach (var springBone in springBones)
            {
                if (springBone.ColliderGroups.Length == 0)
                {
                    VRMSpringBoneColliderGroup[] groups = new VRMSpringBoneColliderGroup[colliderGroups.Length];
                    Array.Copy( colliderGroups,groups, colliderGroups.Length);
                    springBone.ColliderGroups = groups;
                }
            }

        }

      
    }
}
#endif
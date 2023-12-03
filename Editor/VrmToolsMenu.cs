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
using UnityEditor;
using UnityEngine;
using VRM;

namespace Malactus.VrmTools.Editor {

    /**
     * Custom VRM0 Pipeline Tools by Matti "Menithal/Malactus" Lahtinen
     * 
     * Supports UniVRM version 0.89 to 0.99, Specifically VRM0 context.
     * Built to Work with do bindings to make VSF Avatars.
     */
    [ExecuteInEditMode]
    public class VrmToolsMenu : MonoBehaviour
    {
        const string MenuPrefix = "MalAv VRM Utility";
        const string Version = "0.1.0";

        
        /***
         * Helper Funciton which does the following:
         * - Generates BlendshapeClips based on provided array
         * - Binds Blendshapes from Mesh to BlendshapeClips
         * - 
         */
        private static void AutoBind(GameObject selectedObject, List<string> blendshapes, bool detailed = false)
        {
            BlendShapeClipUtility.BindBlendShapesToClips();
                
            VrmBindings bindings = BoneUtility.GetVrmBindings(selectedObject);
            BoneUtility.GenerateSpringBones(bindings);
            BoneUtility.GenerateSpringBoneColliders(selectedObject, bindings, detailed);
        }
        
        /***
         * AutoBind Tooling 
         */
        [MenuItem(MenuPrefix + "/AutoBind/Existing Blendshapes/Filtered", false, 1)]
        private static void AutoBindExisting()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject, 
                    Array.Empty<string>(), BlendshapeDictionary.BlendshapeWhiteList);
                
                BlendShapeClipUtility.GenerateClips(allBlendshapes.ToArray());
                AutoBind(selectedObject, allBlendshapes);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/AutoBind/Existing Blendshapes/All (Advanced)", false, 1)]
        private static void AutoBindExistingAll()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject);

                if (EditorUtility.DisplayDialog("Binding Warning",
                        "About to bind *" + allBlendshapes.Count + "* Blendshapes on the Avatar and its components. \n" +
                        "This includes any Body and Clothing Customizations, and Additional Expressions. \n \nThis may take a while to complete.", "OK", "Cancel"))
                {
                    BlendShapeClipUtility.GenerateClips(allBlendshapes.ToArray());
                    AutoBind(selectedObject, allBlendshapes);
                }
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/AutoBind/VSFe Blendshapes (Suggested)", false, 1)]
        private static void AutoBindVsfExtended()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ExtraVsfBlendShapes);
               
                AutoBind(selectedObject, allBlendshapes);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/AutoBind/ARKit+VSFe Blendshapes", false, 1)]
        private static void AutoBindArKit()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ExtraVsfBlendShapes);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ArKitBlendShapes);
                
                AutoBind(selectedObject, allBlendshapes);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/AutoBind/HTC+VSFe Blendshapes", false, 1)]
        private static void AutoBindHTC()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ExtraVsfBlendShapes);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.HtcBlendshapes);
                
                AutoBind(selectedObject, allBlendshapes);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/AutoBind/Meta+VSFe Blendshapes", false, 1)]
        private static void AutoBindMeta()
        {
            try
            {
                GameObject selectedObject = Selection.activeGameObject;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(selectedObject);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ExtraVsfBlendShapes);
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.MetaBlendShapes);
                
                AutoBind(selectedObject, allBlendshapes);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }

        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/From Mesh/Filtered")]
        private static void GenerateMeshBlendshapesFiltered(){
            try
            {
                string[] test = BlendshapeDictionary.BlendshapeWhiteList;
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(Selection.activeGameObject, Array.Empty<string>(), test);
                BlendShapeClipUtility.GenerateClips(allBlendshapes.ToArray());
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/From Mesh/All (Advanced)")]
        private static void GenerateMeshBlendshapesAll(){
            try
            {
                List<string> allBlendshapes = BlendShapeUtility.FindAllInChildren(Selection.activeGameObject,
                    Array.Empty<string>());
                if (EditorUtility.DisplayDialog("Binding Warning",
                        "About to create *" + allBlendshapes.Count +
                        "* Blendshapes on the Avatar Mesh and its children. \n" +
                        "This includes any Body and Clothing Customizations, and Additional Expressions.",
                        "OK", "Cancel"))
                {
                    BlendShapeClipUtility.GenerateClips(allBlendshapes.ToArray());
                    BlendShapeClipUtility.BindBlendShapesToClips();
                }
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/Defined VSeeFace Extra")]
        private static void GenerateVSeeFaceBlendshapes(){
            try
            {
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ExtraVsfBlendShapes);
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/Defined ARKit")]
        private static void GenerateArKitBlendshapes(){ 
            try
            {
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.ArKitBlendShapes);
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/Defined HTC")]
        private static void GenerateHtcBlendshapes(){ 
            try
            {
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.HtcBlendshapes);
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Create BlendshapeClips/Defined Meta")]
        private static void GenerateMetaBlendshapes(){ 
            try
            {
                BlendShapeClipUtility.GenerateClips(BlendshapeDictionary.MetaBlendShapes);
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }


        
        [MenuItem(MenuPrefix + "/Blendshapes/Bind Blendshapes to Clips")]
        private static void AssignBlendShapes()
        {
            try
            {
                BlendShapeClipUtility.BindBlendShapesToClips();
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Blendshapes/Clean All BlendshapeClips (Advanced)")]
        private static void NukeBlendshapeClips()
        {
            try
            {
                if (EditorUtility.DisplayDialog("Delete Warning",
                        "This will DELETE ALL the Blendshapes Clips on the VRM Avatar. Are You sure?", "Yes", "No"))
                {
                    BlendShapeClipUtility.Nuke();
                }
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        [MenuItem(MenuPrefix + "/Spring Bones/Bind SpringBones" )]
        private static void SetSpringBoneBindings()
        {
            try
            {
                VrmBindings bindings = BoneUtility.GetVrmBindings(Selection.activeGameObject);
                BoneUtility.GenerateSpringBones(bindings);
            }
            catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }

        [MenuItem(MenuPrefix + "/Spring Bones/Bind SpringBoneColliders/Basic (Suggested)")]
        private static void GenerateBasicSpringBoneColliders()
        {
            try
            {
                GameObject obj = Selection.activeGameObject;
                VrmBindings bindings = BoneUtility.GetVrmBindings(obj);
                BoneUtility.GenerateSpringBoneColliders(obj, bindings);
            } catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Spring Bones/Bind SpringBoneColliders/Finger Tips, Chest, Head")]
        private static void GenerateAdvancedSpringBoneColliders()
        {
            try
            {
                GameObject obj = Selection.activeGameObject;
                VrmBindings bindings = BoneUtility.GetVrmBindings(obj);
                BoneUtility.GenerateSpringBoneColliders(obj, bindings, true);
            } catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }

        [MenuItem(MenuPrefix + "/Spring Bones/Clean Spring Bones (Advanced)")]
        private static void NukeSpringBones()
        {
            try
            {
                GameObject obj = Selection.activeGameObject;
                VRMSpringBone[] springBones = obj.GetComponentsInChildren<VRMSpringBone>();
                if (EditorUtility.DisplayDialog("Delete Warning",
                        "This will DELETE ALL *" + springBones.Count() + 
                        "* Spring Bones on the VRM Avatar. Are You sure?", "Yes", "No"))
                {
                    VrmUtility.RemoveComponents<VRMSpringBone>(springBones);
                }
            }catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix + "/Spring Bones/Clean Spring Bone Colliders (Advanced)")]
        private static void NukeSpringBoneColliders()
        {
            try
            {
                GameObject obj = Selection.activeGameObject;
                VRMSpringBoneColliderGroup[] colliderGroups = obj.GetComponentsInChildren<VRMSpringBoneColliderGroup>();
                if (EditorUtility.DisplayDialog("Delete Warning",
                        "This will DELETE ALL *" + colliderGroups.Count() + 
                        "* Spring Bone Collider Groups on the VRM Avatar. Are You sure?", "Yes", "No"))
                {
                    VrmUtility.RemoveComponents<VRMSpringBoneColliderGroup>(colliderGroups);
                }
            }catch (MalToolException e)
            {
                EditorUtility.DisplayDialog("Error", e.Message, "OK");
            }
        }
        
        [MenuItem(MenuPrefix+ "/Version " + Version)]
        private static void VersionDisplay() {}
        
        [MenuItem(MenuPrefix+ "/Version " + Version, true)]
        private static bool ValidateVersionDisplay()
        { return false; }
    }
}
#endif
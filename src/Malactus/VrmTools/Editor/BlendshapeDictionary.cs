using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR && UNITY_2019

namespace Malactus.VrmTools.Editor {
    public class BlendshapeDictionary {
        // Binding Dictionary for VRC Shapekeys to VRM BlendshapeClips

        public static Dictionary<string, string> VRMBlendshapeBindings = new Dictionary<string, string>()
        {

        };
        
        public static Dictionary<string, string> VrcToVrmBinding = new Dictionary<string, string>(){
            {"vrc.v_aa", "a"},
            {"vrc.v_ch", "ch"},
            {"vrc.v_dd", "dd"},
            {"vrc.v_e", "e"},
            {"vrc.v_ee", "e"},
            {"vrc.v_ff", "ff"},
            {"vrc.v_ih", "i"},
            {"vrc.v_kk", "kk"},
            {"vrc.v_oh", "o"},
            {"vrc.v_pp", "pp"},
            {"vrc.v_nn", "nn"},
            {"vrc.v_rr", "rr"},
            {"vrc.v_sil", "sil"},
            {"vrc.v_ss", "ss"},
            {"vrc.v_th", "th"},
            {"vrc.v_ou", "u"},
            {"vrc.blink", "blink"},
            {"vrc.blink_left", "blink_l"},
            {"vrc.blink_right", "blink_r"},
        };

        // Array of Blendshapes OVR FaceExpressions (Meta Quest VR) Supports. Future Proofing?
        public static string[] MetaBlendShapes = new string[]{
            "Brow_Lowerer_L", // BrowDownLeft
            "Brow_Lowerer_R", // BRownDownRight
            "Cheek_Puff_L", // CheekPuff Split 2
            "Cheek_Puff_R",
            "Cheek_Raiser_L", //CheekSquintLeft
            "Cheek_Raiser_R", // CheekSquintRight
            "Cheek_Suck_L", // 
            "Cheek_Suck_R",
            "Chin_Raiser_B",
            "Chin_Raiser_T",
            "Dimpler_L", // MouthDimpleLeft
            "Dimpler_R", // MouthDimpleRight
            "Eyes_Closed_L", // EyeBlinkLeft
            "Eyes_Closed_R", // EyeBlinkRight
            "Eyes_Look_Down_L", // EyeLookDownLeft
            "Eyes_Look_Down_R", //  EyeLookDownRight
            "Eyes_Look_Left_L", // EyeLookOutLeft
            "Eyes_Look_Left_R", // EyeLookInRight
            "Eyes_Look_Right_L", // EyeLookInLeft
            "Eyes_Look_Right_R", // EyeLookOutRight
            "Eyes_Look_Up_L", // EyeLookUpLeft
            "Eyes_Look_Up_R", // EyeLookUpRight
            "Inner_Brow_Raiser_L", // BrowInnerUp Split 2
            "Inner_Brow_Raiser_R",
            "Jaw_Drop", // Jaw Open
            "Jaw_Sideways_Left", // JawLeft
            "Jaw_Sideways_Right", // JawRight
            "Jaw_Thrust", // JawForward
            "Lid_Tightener_L", //MouthPressLeft
            "Lid_Tightener_R",  // MouthPressRight
            "Lip_Corner_Depressor_L", // MouthStretchLeft
            "Lip_Corner_Depressor_R", // MouthStretchRight
            "Lip_Corner_Puller_L", // MouthSmileLeft
            "Lip_Corner_Puller_R", // MouthSmileRight
            "Lip_Funneler_LB", // MouthFunnel Split 4
            "Lip_Funneler_LT",
            "Lip_Funneler_RB",
            "Lip_Funneler_RT",
            "Lip_Pressor_L", // MouthPressLeft
            "Lip_Pressor_R", // MouthPressRight
            "Lip_Pucker_L", // MouthPucker  Split 2
            "Lip_Pucker_R", 
            "Lip_Stretcher_L", // MouthStretchLeft
            "Lip_Stretcher_R", // MouthStretchRight
            "Lip_Suck_LB", //
            "Lip_Suck_LT",
            "Lip_Suck_RB",
            "Lip_Suck_RT",
            "Lip_Tightener_L",  // MouthRollUpper split
            "Lip_Tightener_R", 
            "Lips_Toward", // MouthRollUpper
            "Lower_Lip_Depressor_L",
            "Lower_Lip_Depressor_R",
            "Mouth_Left", //MouthLeft
            "Mouth_Right", // MouthRight
            "Nose_Wrinkler_L", // NoseSneerLeft
            "Nose_Wrinkler_R", // NoseSneerRight
            "Outer_Brow_Raiser_L", // BrowOuterUpLeft
            "Outer_Brow_Raiser_R", // BrowOuterUpRight
            "Upper_Lid_Raiser_L", // EyeWideLeft
            "Upper_Lid_Raiser_R", // EyeWideRight
            "Upper_Lip_Raiser_L", //MouthUpperUpLeft
            "Upper_Lip_Raiser_R", //MouthUpperUpRight
            "Max"
        };

        // Array of additional Blendshapes various apps (mainly VSF) can support if present. Mainly compatible with vrc shapekeys.
        public static string[] ExtraVsfBlendShapes = new string[]
        {
            "A",
            "CH",
            "DD",
            "E",
            "FF",
            "I",
            "KK",
            "O",
            "PP",
            "NN",
            "RR",
            "SIL",
            "SS",
            "TH",
            "U",
            "Blink",
            "Blink_L",
            "Blink_R",
            "Joy",
            "Fun",
            "Angry",
            "Sorrow",
            "LookDown",
            "LookLeft",
            "LookRight",
            "LookUp",
            "Suprised",
            "Brows up",
            "Brows down"
        };
       
        // Array of additional Blendshapes various apps can use if ARKit data is used to track.
        public static string[] ArKitBlendShapes = new string[] {
            "BrowDownLeft",
            "BrowDownRight",
            "BrowInnerUp",
            "BrowOuterUpLeft",
            "BrowOuterUpRight",
            "CheekPuff",
            "CheekSquintLeft",
            "CheekSquintRight",
            "EyeBlinkLeft",
            "EyeBlinkRight",
            "EyeLookDownLeft",
            "EyeLookDownRight",
            "EyeLookInLeft",
            "EyeLookInRight",
            "EyeLookOutLeft",
            "EyeLookOutRight",
            "EyeLookUpLeft",
            "EyeLookUpRight",
            "EyeSquintLeft",
            "EyeSquintRight",
            "EyeWideLeft",
            "EyeWideRight",
            "JawForward",
            "JawLeft",
            "JawOpen",
            "JawRight",
            "MouthClose",
            "MouthDimpleLeft",
            "MouthDimpleRight",
            "MouthFrownLeft",
            "MouthFrownRight",
            "MouthFunnel",
            "MouthLeft",
            "MouthLowerDownLeft",
            "MouthLowerDownRight",
            "MouthPressLeft",
            "MouthPressRight",
            "MouthPucker",
            "MouthRight",
            "MouthRollLower",
            "MouthRollUpper",
            "MouthShrugLower",
            "MouthShrugUpper",
            "MouthSmileLeft",
            "MouthSmileRight",
            "MouthStretchLeft",
            "MouthStretchRight",
            "MouthUpperUpLeft",
            "MouthUpperUpRight",
            "NoseSneerLeft",
            "NoseSneerRight",
            "TongueOut",
        };

        public static string[] HtcBlendshapes = new string[]
        {
            "Jaw_Left",
            "Jaw_Right",
            "Jaw_Forward",
            "Jaw_Open",
            "Mouth_Ape_Shape",
            "Mouth_Upper_Left",
            "Mouth_Upper_Right",
            "Mouth_Lower_Left",
            "Mouth_Lower_Right",
            "Mouth_Upper_Overturn",
            "Mouth_Lower_Overturn",
            "Mouth_Pout", 
            "Mouth_Smile_Left",
            "Mouth_Smile_Right",
            "Mouth_Sad_Left",
            "Mouth_Sad_Right",
            "Cheek_Puff_Left",
            "Cheek_Puff_Right",
            "Cheek_Suck",
            "Mouth_Upper_UpLeft",
            "Mouth_Upper_UpRight",
            "Mouth_Lower_DownLeft",
            "Mouth_Lower_DownRight",
            "Mouth_Upper_Inside",
            "Mouth_Lower_Inside",
            "Mouth_Lower_Overlay",
            "Eye_Left_Blink",
            "Eye_Left_Wide",
            "Eye_Left_Right",
            "Eye_Left_Left",
            "Eye_Left_Up",
            "Eye_Left_Down",
            "Eye_Right_Blink",
            "Eye_Right_Wide",
            "Eye_Right_Right",
            "Eye_Right_Left",
            "Eye_Right_Up",
            "Eye_Right_Down",
            "Eye_Frown",
            "Eye_Left_Squeeze",
            "Eye_Right_Squeeze",
            "Tongue_LongStep1",
            "Tongue_LongStep2",
            "Tongue_Left",
            "Tongue_Right",
            "Tongue_Up",
            "Tongue_Down",
            "Tongue_Roll",
            "Tongue_UpLeft_Morph",
            "Tongue_UpRight_Morph",
            "Tongue_DownLeft_Morph",
            "Tongue_DownRight_Morph"
        };
        // TODO: Find a Good Guesstimate on HTC Facetracking to ARkitBinding? Heavy RND Required
        public static Dictionary<string, string> HtcToArKitBinding = new Dictionary<string, string>(){
            {"MouthApeShape", "MouthPucker"},
        };

        public static string[] BlendshapeWhiteList = new List<string>()
                .Concat(ArKitBlendShapes.ToList())
                .Concat(HtcBlendshapes.ToList())
                .Concat(MetaBlendShapes.ToList())
                .Concat(ExtraVsfBlendShapes.ToList()).Select(s=> s.ToLower()).ToArray();

    }
}
#endif
# MalAv VRM0 Utility

If you are looking for the Blender Version, You can find it at [Menithal/Mals-Blender-VRM0-Utility](https://github.com/Menithal/Mals-Blender-VRM0-Utility)

This is an utility plugin for the use with **UniVRM 0.89 - 0.99** specifically built to make the pipeline in making VRM0.x format avatars way, WAY less time consuming to create.

Standards mean everything, and avatar creators should be able to just make stuff with naming conventions.

Originates as from a personal plugin that the author has been using over the years to streamline the creation of custom avatars for VTubing with [VSeeFace](https://www.vseeface.icu/) and other VRM0 compatible software such as [VirtualMotionCapture](https://akira.works/VirtualMotionCapture-en/download.html), [LIV](https://www.liv.tv/), [VNyan](https://suvidriel.itch.io/vnyan), [VTuberPlus](https://arzolath.itch.io/vtuber-plus), [Meeps](https://meepskitten.itch.io/meep), and so forth.

You can also use this to debug Avatars, to see what blendshape may be missing from the avatar, or if somethign is misnamed through the Blendshape folder created and the preview.

These tools are specifically built by an Avatar Creator for Avatar Creators. But it everyone can use it just as well, its Free! But as  with anything free, use at your own risk.

If you find something that could use fixing, feel free to submit a Merge Request or Report it :)

## Please Backup your work before using this, or use this only on NEW avatars for your pipeline 

[Here is a youtube video on what this plugin does](https://www.youtube.com/watch?v=yuX6WckNemo)

# What is this?

This Tool allows you to quickly bind the following attributes to a VRM0 Avatar.

- Blendshapes from Multiple Mesh to similar BlendShapeClips.
  - Filtered Mode that only adds defined standards if they Exist on the model
    - VRM Standard Binding
    - VSeeFace Extended Standard Binding
    - ARKit Compatible Bindings
    - Experimental (Have not used them):
      - HTC FC Compatible Binding
      - Meta Compatible Bindings
  - Unfiltered Mode that adds -every- single blendshape that the script can find from the Mesh, that is not decorative (`====== Blendshapes ======` or etc)
- Assign SpringBonesGroups from bone naming conventions, if the bones follow a specific postfix format.
  - if bones end with `_phys` ending, they are added to the default SpringBone.
  - if bones end with `_phys#` ending, they are added to individual groupings of SpringBones (`_phys1` => group 1, `_phys2` => group 2, etc)
- Assign SpringBone Colliders Automatically
  - Humanoid defined Head, Hands, and the Index finger Distal
  - Above but Every Single Finger + Chest
- Bones named `FPV`/`FirstPersonOffset`/`FirstPersonBone` will be bound as the location of the FirstPersonOffset.

# Dependencies

- [UniVRM 0.89.0 - 0.99.x](https://github.com/vrm-c/UniVRM/releases/tag/v0.99.0) and All its dependencies (note later versions do NOT support VRM0)
- Unity 2019.4.31f (Latest Version supported by VSeeFaceSDK)

# Optional

- [VSeeFace Extension](https://github.com/emilianavt/VSeeFaceSDK)

### Lexicon:

- `Blendshape`- Alternative shape for a mesh. Most commonly used to move lips of an avatar, or morph a body shape. Also known as shapekey (blender) or morphtarget (source engine) 
- `BlendshapeClip` - Blendshape Definition that is bound to the VRM Model to be used.
- `SpringBone` - Dynamic, Flexible, or Wiggly Bone Group. These are bones that are simulated by any engine its run on.
- `SpringBoneColliderGroup` - Group that defineds a collider.

# What does this add?

The plugin adds new Navigation Option on Unity's Top Bar. `MalAv VRM Utility`

The menu provides the following options to choose from

- `AutoBind` - Collection of tools to -automatically- Bind majority of our avatars VRM components.  This is probably which will be your new best friend
- `Blendshapes` - Collection of tools that deal with Blendshapes, BlendshapeClips.
- `SpringBones` - Collection of tools that deal with SpringBones and SpringBone Colliders (Dynamic/Wiggly things)



# How  Does this binding work?

Both Blendshapes and Springbones are defined by __Naming convention__

As long as the blendshapes follow any of the above standards (or even **VRC ones that are mapped directly to the VSF Extended**), it will be included in the binding. Note that the binding is **case insensative**

Spring Bones use a bit more of a unique convention defined for this plugin. They must use `_phys`, `_phys_#`, or `_phys#` naming convention inorder to be bound to the bones. *You only have to define this on the Root bones* you want in the group.

SpringBoneColliderGroups are then automatically bound to these SpringBone definitions if they have not yet been defined.

Spring Bone colliders use the Unity Humanoid as reference to define the position of the default Colliders. If these already exist on those bones, no groups will be overriden.

Similarly, Bones named `FPV`/`FirstPersonOffset` or `FirstPersonBone` will be bound as the location of the FirstPersonOffset.

Note this should **NOT override existing BlendshapeClips that have defined Blendshapes in them**, only ones that have not been bind. You will have to delete previously made BlendshapeClips if you want the tools to handle them. spring bones work in similar method, so if you have -any- customization done to a blendshapeClip, that clip is ignored.


# Installation UPM

See [Releases](https://github.com/Menithal/Mals-VRM0-Utility/releases).

# How To use

1. Import the Avatar first into Unity
2. Convert the Avatar Rig to Unity Humanoid
3. Export as VRM into the project, with T Posing Enabled, and author Settings into a separate folder.
4. Reimport Exported VRM Edit.
5. Select *VRM on the Scene*
5. Use `MalAv VRM Utility/Autobind/VSFe Blendshapes (Suggested)` 
6. Adjust Any bindings on the model
7. Export
8. Enjoy.

## What does `Autobind` Option do

Autobind contains various "Magic" tools. These will only work if certain naming conventions are used (as its the only way to reliably bind them) but it will try to match them to a "best guess" based with lower case sensitivity.

- `Existing Blendshapes` - Looks through all the mesh in the selected VRM model, and looks through every single available blendshapes, and makes them into clips.
  - `Filtered` - Selects All blendshapes that match any of the standards.
  - `Unfiltered` - Selects -everything- but decorative Blendshapes. (like `====== Viseme ======` or variants using other symbols)
- `VSFe Blendshapes (Suggested)` - Binds only all blendshapes that match [VSF Extended](https://www.vseeface.icu/#special-blendshapes) Naming Convention
- `Arkit+VSFe` - Binds all blendshapes that match [VSF Extended](https://www.vseeface.icu/#special-blendshapes), AND [Apple's ARKit Standard](https://developer.apple.com/documentation/arkit/arfaceanchor/blendshapelocation).
- `HTC+VSFe` - Binds all blendshapes that match [VSF Extended](https://www.vseeface.icu/#special-blendshapes), AND [HTC Blendshape Standard](https://developer.vive.com/resources/openxr/openxr-pcvr/tutorials/unity/integrate-facial-tracking-your-avatar/).
- `Meta+VSFe` - Binds all blendshapes that match [VSF Extended](https://www.vseeface.icu/#special-blendshapes), AND [Meta Blendshape Standard](https://developer.oculus.com/documentation/unreal/move-ref-blendshapes/).

## What does `Blendshape` Option do?

Similar to autobind, however this does not bind SpringBones at all. This can be used to specifically create and bind new additions to a Model. 

It also allows you to Clean all blendshapeClips off a model with `Clean All BlendshapeClips (Advanced)`

## What does `Springbone` Option do?

Similar to autobind, however only deals with SpringBones. Allows you to bind additional colliders, like all the fingers.

Additional Options Include
- `Clean All SpringBones (Advanced)` - which removes All SpringBone definitions from the VRM.
- `Clean All SpringBoneGroups (Advanced)` - whcih removes all SpringBoneGroups from the VRM.
## FAQ

### Blendshapes not binding!?

Check that blender is exporting the blendshapes. In Unity, you can check this by selecting the mesh, and checking the blendshapes are presentt. FBX Export tends to merge modifiers, and blender doesnt like modifiers being merged with Blendshapes.

If not, double check your modifiers. If you have modifiers, check tools like [przemir's ApplyModifiersForObjectWithShapekeys](https://github.com/przemir/ApplyModifierForObjectWithShapeKeys/) to merge them while keeping your Blendshapes consistant.
 
### My Blendshapes are looking wierd D:

Blender FBX Export normals can go a bit wonky, especially on older versions. When importing the model to unity, make sure `Legacy Blendshape Normals` is enabled in the model `Import` settings.

### My SpringBones are not binding!??

Postfixing a the root bone you want with `_phys` will allow that bone to be mapped to the `Default` Springbone group.
Using `_phys#` will allow you to assign a bone to a grouping.
Forexample the Author uses postfix `_phys1` with everything fluffy, like Hair, Chest Fluff, etc `Hair1_phys1`, `Fluff1_phys1`, and configures them accordingly.

Ears are separate, so they belong in another group thus `LeftEar1_phys2`, etc. The rest of the chain can be named as what ever after (`LeftEar2`, etc).

### My SpringBoneColliders are not binding?!

Make sure to have the hands, chest, fingers defined in the Unity Humanoid. The Finger Springbones are generated on the `Distal` bones.

# Donations

If you think my stuff is neat, feel free to check my other work at my [ko-fi at malactus](https://ko-fi.com/malactus) and maybe buy me a coffee, or two.

# Change Log

### Version 0.1

Initial Public Release.

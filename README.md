# openxrhands
Open XR hand tracking in Unity

This is an implementation of the OpenXR extension
- XR_EXT_hand_tracking (should work on multiple devices)

This project allows you to:
- Get hand joint positions, orientations and radii from some kind of hand tracker, e.g. Oculus Quest, Hololens, VR gloves, Valve Knuckles. 
- Get finger curl values used by OpenVR for Knuckles and some VR gloves.

# Why?
Just so I could do this:

https://user-images.githubusercontent.com/10870921/168443113-64856a78-f05c-41ae-b6ba-707496cdfaf1.mp4

# Usage
Add HandTrackingFeature.cs and HandGetter.cs into your project and enable the Hand tracking Extension under OpenXR plugin settings.

HandGetter lets you get hand joint orientations, positions and radii, and then calculates finger curl values based on the animation used by Knuckles and [VR gloves](https://github.com/LucidVR/opengloves-driver/tree/develop/openglove/resources/anims).

# Drawbacks
- For now, I can't calculate splay values, since I'm still unsure what exactly those are. The [OpenGloves Driver](https://github.com/LucidVR/opengloves-driver/blob/763b6e9e90dcf44f1161a965ccc595e5f725f0b8/src/Bones.cpp#L181) seems to suggest that splays have a range of 20° but it also seems to allow for values from -1 to 1, rather than 0 to 1 [as Valve suggests](https://valvesoftware.github.io/steamvr_unity_plugin/articles/Skeleton-Input.html#finger-curls).
- The Knuckles seem to do some weird things:
1. The skeleton fingers curl as you aim your hand down, even if you don't actually curl your fingers. I don't know of a way to figure out how the controller orientation relates to finger curls.
2. Curling all your fingers except for the index causes the index to stay fully extended no matter where you aim the controller. Not a problem, just need to point out that it cancels out what 1. does.
- The curl calculation is specific to the animation the Knuckles and OpenGloves use. I might add a utility that'll allow you to get closed and open poses from other animations after some cleanup.
- Hands keep being tracked even after you take off your headset, while OpenXR's controller tracking stops. Just something to keep in mind.

# TODO
- Try adding splay value calculations 
- Try figuring out the Knuckles skeletal input stuff

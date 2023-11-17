# AR M64 - Super Mario 64 for Quest 3
>Super Mario 64 running natively on the Quest 3, interacting with the real-world environment.

This repo contains the Unity project of an MR app that allows controlling Mario from Super Mario 64 in virtual and augmented reality. Through passthrough and plane detection, this application allows Mario to navigate and jump around your physical Space. 



https://github.com/JonasJakobi/SM64-Quest-3/assets/93149084/b4726dea-1f7b-451a-a164-3a6be6563c0c




Mario can be controlled with the Quest Controllers and platforms can be created for him to walk and jump on. 
This code base builds upon [libsm64-unity](https://github.com/libsm64/libsm64-unity) and extends its functionality to support the Quest 3 as well as allow Mario to collide with the physical space. 
It also includes [libsm64](https://github.com/libsm64/libsm64) as a compiled library file (.so)  that is compatible with the ARM64 infrastructure of the Quest 3's CPU. The same library file could probably also be used for other projects that want to use libsm64 on an ARM64 infrastructure.
This project requires loading an official SM64 ROM at runtime to get Mario's texture and animation data through asset extraction.





## Controls

<img src="https://github.com/JonasJakobi/SM64-Quest-3/assets/93149084/96916240-0875-4880-8f20-90606001a44c" alt="Controls" width="512">

_(Controllers shown are not the Quest 3 Controllers but Quest 2 Controllers, the Buttons are the same though)_

- When creating or moving platforms, make sure not to place them inside of Mario. This will cause the game to crash. 

- Ideally, start the application in a room where the physical space setup has been done before. 

## Installation

### Unity Installation
Cloning this repository and opening it in Unity 2022.3.8f1 should preconfigure your Unity application and import all packages accordingly. Make sure to add the Android Build Support module to your Unity install.

### Quest 3 Developer Mode
In case this is your first time developing with the Quest 3, you have to set up the headset for development mode.  Refer to [this guide](https://developer.oculus.com/documentation/native/android/mobile-device-setup/) by Oculus.
### Running Project
With the Quest 3 connected to the computer over a USB cable, `Build and Run` the project as an Android application. This should pack the project into an .apk file and load it onto the headset. 
For the application to work, a ROM of the original Super Mario 64 game is needed. Name the rom `baserom.us.z64` and place it in the `/Android/com.DefaultCompany.ARM64/files/` directory.

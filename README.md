# ZXMapper

<p align="left">
  <img src="https://img.shields.io/github/license/rk797/zxmapper" alt="License">
  <img src="https://img.shields.io/github/stars/rk797/zxmapper" alt="Stars">
  <img src="https://img.shields.io/github/forks/rk797/zxmapper" alt="Forks">
  <img src="https://komarev.com/ghpvc/?username=zxmapper&label=Views" alt="Views">
</p>


## Description


ZXMapper is a C# application that allows you to remap your keystrokes 
to controller inputs using the Interception driver and ViGEmBus for 
virtual gamepad emulation.


## Features
```sh-session
- Keystroke -> Controller mapping
- Support for multiple controller types: Xbox 360, DualShock 4 (PS4), and DualSense (PS5)
- Customizable mappings for all keys (not just WASD)
- Cross center axis reset from W->D or D->W
- Low input latency
- Supports southpaw / standard axis configuration
- F1/F2 hotkeys to enable/disable mapping on the fly
- Custom keybinds for game-specific actions
```


## Installation

>[!IMPORTANT]
> Disable steam input for low latency (sometimes causes issues with remapping)

### Option 1
```sh-session
1. Download the latest release from [Releases](https://github.com/rk797/zxmapper/releases)
2. Disable Anti Virus
3. Run zxmapper.exe
```
### Option 2

```sh-session
1. Clone this Repository:
    - Clone the repository to your local machine using the following command

      git clone https://github.com/rk797/zxmapper.git


2. Build the Project:
    - Open the project in your preferred C# development environment (e.g., Visual Studio).
    - Restore the NuGet packages and build the solution.


3. Add Requirements to build dir
    - Add the ViGEmBus.msi file to the build dir
    - Add the interception.dll binary to the build dir
```

>[!NOTE]
> Some antivirus programs may flag ZXMapper as a potential threat. This is a false positive. ZXMapper uses the Interception driver, which involves kernel-level communication to capture and remap keyboard inputs. Because of this, antivirus software might mistakenly identify it as malicious. To ensure ZXMapper functions correctly, you may need to temporarily disable your antivirus software during installation and use. Rest assured, ZXMapper is safe and does not pose any threat to your system.

## Usage Guide

### Controller Selection
ZXMapper now supports multiple controller types:

1. Select your desired controller type from the dropdown:
   - Xbox 360 (default)
   - DualShock 4 (PS4)
   - DualSense (PS5)
2. This selection must be made before enabling mapping
3. Your choice will be saved for future sessions

### Custom Keybinds
ZXMapper now supports custom keybinds for all game actions, not just WASD movements:

1. Click the "Custom Keybinds" button in the main window
2. Click on any action button you want to bind
3. Press the key you want to assign to that action
4. Click "Save" to apply your changes

### Enable/Disable Mapping
You can now toggle the mapping on and off using keyboard shortcuts:

- Press F1 to enable mapping
- Press F2 to disable mapping

These hotkeys can be customized in the Custom Keybinds window.

## Credits

- **Interception Driver**: developers of the Interception driver. Learn more about Interception [here](https://github.com/oblitum/Interception).
- **ViGEmBus**: virtual gamepad emulation driver + wrapper. Learn more about ViGEmBus [here](https://vigem.org/projects/ViGEm/).

  

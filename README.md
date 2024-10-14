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
- Customizable mappings
- Cross center axis reset from W->D or D->W
- Low input latency
- Suppports southpaw / standard axis configuration
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



## Credits

- **Interception Driver**: developers of the Interception driver. Learn more about Interception [here](https://github.com/oblitum/Interception).
- **ViGEmBus**: virtual gamepad emulation driver + wrapper. Learn more about ViGEmBus [here](https://vigem.org/projects/ViGEm/).

  

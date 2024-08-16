# ZXMapper

ZXMapper is a C# application that allows you to remap your keystrokes to controller inputs using the Interception driver and ViGEmBus for virtual gamepad emulation.

## Features

- **Keystrokes to Controller Mapping**: Convert keyboard inputs to controller actions.
- **Interception Driver**: Capture and remap keyboard inputs.
- **ViGEmBus Support**: Emulate a virtual gamepad for broad compatibility.
- **Customizable Mappings**: Easily configure key-to-controller mappings.


## Requirements

- **Win10+**

## Installation
### Option 1
1. Download the latest release from [Releases](https://github.com/rk797/zxmapper/releases)
2. Disable Anti Virus
3. Run zxmapper.exe

### Option 2
1. **Clone this Repository**:
    - Clone the repository to your local machine using the following command:
      ```sh
      git clone https://github.com/rk797/zxmapper.git
      ```

2. **Build the Project**:
    - Open the project in your preferred C# development environment (e.g., Visual Studio).
    - Restore the NuGet packages and build the solution.
3. Add Requirements to build dir
    - Add the ViGEmBus.msi file to the build dir
    - Add the interception.dll binary to the build dir
   
## Important Notice

### Antivirus Software

Please note that some antivirus programs may flag ZXMapper as a potential threat. This is a false positive. ZXMapper uses the Interception driver, which involves kernel-level communication to capture and remap keyboard inputs. Because of this, antivirus software might mistakenly identify it as malicious. To ensure ZXMapper functions correctly, you may need to temporarily disable your antivirus software during installation and use. Rest assured, ZXMapper is safe and does not pose any threat to your system.

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please fork the repository and submit a pull request. For major changes, please open an issue first to discuss what you would like to change.


## Acknowledgements

- **Interception Driver**: developers of the Interception driver. Learn more about Interception [here](https://github.com/oblitum/Interception).
- **ViGEmBus**: virtual gamepad emulation driver + wrapper. Learn more about ViGEmBus [here](https://vigem.org/projects/ViGEm/).

## Donations
- LTC: MHAjWpuFbyQrqYw4GpcHCLHSZ6Go1xdsyq
  
### Disclaimer

This software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages, or other liability, whether in an action of contract, tort, or otherwise, arising from, out of, or in connection with the software or the use or other dealings in the software.

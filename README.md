# samples-mpc_windows / Windows SPC sample codes
This sample code is for **MPC** communication (devices in SPC mode with MPC supported script) on Windows devices.

[Test LINK: SPC communication mode](../doc/communication-modes/mpc)
//TODO Add link See "communication-modes/spc"

## Requirements
* IDE (Visual Studio 2017)
* Micro-Sensys RFID reader with appropriate script running
* RFID transponders

> If you have interest in MPC functionality and do not have a compatible script, please contact us: [Contact](#Contact)

## Implementation
This code shows how to use **MPC.Communication** class to communicate with a device running on MPC mode. 
Using this class the communication port can be open/closed. It automatically detects the connected reader and reads the basic information. There are a couple of functions available to read MPC memory.
Reading process will be notified by *ProgressEvent* and MPC datasets will be provided by *ConversionEvent*

> Class information is available under API documentation. See [Useful Links](#Useful-Links)

## Steps
Import the project into your IDE connect the RFID reader to your computer using USB cable and run the code.

//TODO screenshot!!
<!--- ![Screenshot](screenshot/SampleApp_SpcControl_AndroidJava.png) --->

 1. Press "START" button. This will open the communication port and show the reader information (received in *ReaderInfoEvent*) in the TextBox below.
 2. (Optional) Select the value of the properties using provided CheckBox on top right side
 3. To read MPC memory press "READ" button. Progress and result (received in *ProgressEvent* and *ConversionEvent*) will be shown in TextBox on the right.

## Useful Links

 - [Library and API documentation](https://www.microsensys.de/downloads/DevSamples/Libraries/Windows/iID%20MPC%20-%20.NET%20library/)


## Contact

* For coding questions or questions about this sample code, you can use [support@microsensys.de](mailto:support@microsensys.de)
* For general questions about the company or our devices, you can contact us using [info@microsensys.de](mailto:info@microsensys.de)

## Authors

* **Victor Garcia** - *Initial work* - [MICS-VGarcia](https://github.com/MICS-VGarcia/)

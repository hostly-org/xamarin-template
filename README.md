# Example Smarttime Android Application
[![Build status](https://build.appcenter.ms/v0.1/apps/50d8351f-a22a-4a0d-8784-eb5ee73750af/branches/master/badge)](https://appcenter.ms)
[![Build Status](https://dev.azure.com/AtlasCleverSoftware/Example%20Group/_apis/build/status/SmartTime%20-%20Mobile?branchName=feature%2Fxamarin-rewrite)](https://dev.azure.com/AtlasCleverSoftware/Example%20Group/_build/latest?definitionId=27&branchName=feature%2Fxamarin-rewrite)
### 1. Languages
- C#
- XAML
- Css

### 2. Frameworks
- Xamarin

### 3. Steps to get up and running for developers

1. Install Visual Studio 2019 (Ensure to include the mobile app developement modules in the instalation)

    1.b I have visual studio installed but do not have the mobile app developmnent module installed?
    
        - No worries just open up the visual studio installer, modify your installation and select the module.
2. Run the Example.Mobile.Android project

### 4. Additional configuration for local development

If you want to run the mobile app locally by using your local instance of Example there is some futher actions you need to take:

1. Find out your local IP. Run `ipconfig` in a terminal and make note of the IPv4 address
2. Go to `D:\Git\smarttime\.vs\config\applicationhost.config` and add your IP to the Example.API bindings. It should look something like this:
```
<site name="Example.API" id="3">
    <application path="/" applicationPool="Clr4IntegratedAppPool">
        <virtualDirectory path="/" physicalPath="D:\Git\smarttime\Example.API" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation="*:57002:<YOUR_IP>" />
        <binding protocol="http" bindingInformation="*:57002:localhost" />
    </bindings>
</site>
```
3. Run the Example `Authentication Api` project
4. Run the main Example `smarttime` project (you might have to run this in VS Admin mode)

### 5. Using emulators
1. If using an emaultor be sure to have the correct SDK insatlled and your emulator shoudl appear in visual studio as a debug target.
2. If using bluestacks please follwoing the follwing steps:

    2.a Open bluestacks 4
    
    2.b Open up the settings page and navigate to Preferences
    
    2.c Turn on the "Enable Android Debug Bridge (ADB)" feature
    
    2.d Open up an instace of Command prompt/Powershell and execute `adb connect localhost:5555`
    
    2.e Bluestacks should now show up as a selectable debugging device in visual studio

### 6. using physical devices
1. Turn on debugging mode on your device (This is usually found within Settings->Developer)
2. Plug in your device
3. Select your device in visual studio and run the project

### 3. Opticon bluetooth scanner

There are several areas of the mobile app where we use the opticon bluetooth scanner for input (eg: Product search)

In order to use the scanner while developing. Follow the steps below to pair it with your phone / tablet:

1. We have some instructions for the scanner. You will need these to make the scanner discoverable as a bluetooth device and to manually disconnect it.
2. Scan the `Manual disconnect` barcode from the instructions (Section 5.1)
3. After that, scan the `Reconnection your barcode reader` barcode (Section 5.2)
4. Enable bluetooth on your phone / tablet and pair it with the scanner (The scanner should appear under the name of Scanfob or something similar)
5. Now run the app in visual studio. The scanner should beep once the app has started.

NOTE: The scanner has a tendency to disconnect. This works fine, but there are times when the scanner will fail to reconnect. If this is the case you will need to manually disconenct the scanner, unpair it, disable bluetooth on your phone/tablet and repeat steps 1-5 above.


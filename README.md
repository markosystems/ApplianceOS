# ApplianceOS

The purpose of this project is to make creating single purpose Linux machines easier to start up. Especially for "Windows Babies" like me that don't really like using the CLI or want to learn linux system managers or package handlers, but still want to create a single purpose machine.

## The Flow
This is the current flow, this may change in the future based on what's needed, and what can be made more efficient.
You'll need a flash drive and you'll only need to know 3 commands.

### Appliance_Maker.exe
In order to make mine and your life easier, Appliance_Maker.exe is a WPF that makes building a drive easier with a GUI.
1. Point the program to a flash drive(the drive will be wiped, so be careful)
2. Select the program or program directory that you'll be using as your "appliance"
3. List dependencies.
4. click `build`

### Your Linux Machine
When you plug your drive into the linux machine you'll need to use some commands

1. `cd` for traversing to your flash drive
2. `sudo chmod` for changing the permissions of the appliance-cli tool
3. `sudo appliancecli` to run the setup tool

The appliance app should run on startup after resetting.

## Future development
I have limited knowledge of Linux and the many distros, so as I learn more there'll be more. I'm planning:

- Support for other system managers
- Support for packagehandlers
- Less convolution for dependencies and startup app
- Better Package Resolution
- Dedicated distro(s)... but I'm not too worried about this

## Why?
Linux is a powerful kernel and extremely good for single purpose machines, but desktop environments can be more limiting than minimal. So to have as much control that I need and I can be as lazy as I want, I wrapped what I know about linux into this tool. Feel free to steal and improve as you see fit.

## "Single purpose"?
Feel free to interchange with "headless" or "appliance". There's no literal limit on the amount of startup apps that you can have per machine, that's just the term I wanted to use.

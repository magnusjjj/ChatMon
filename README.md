# ChatMon - The perfect companion for Pokémon nuzzlocke Twitch streams.
![image](https://github.com/magnusjjj/ChatMon/assets/525731/f88b9a32-e693-4836-9720-e3cebdb8319e)

ChatMon lets your viewers talk through your pokemon roster.

<img src="https://github.com/magnusjjj/ChatMon/assets/525731/82b75fce-23ca-496f-927e-0fd8ccafa660"/>


## About

ChatMon is created by SlightlyTango, based on an idea by and commissioned by SkylordZoey.

[![becomeapatron](https://github.com/magnusjjj/ChatMon/assets/525731/62663fdc-0e4a-4e6a-a4cb-a8752ac52080)](https://www.patreon.com/SlightlyTango)

Becoming a patron really does help a whole lot <3.

Having troubles? Join our discord server, https://discord.gg/Z7wHzWWVuU

## Features
- An interface and setup that's easy enough to use that even people that have any streaming experience can set it up in minutes.
- All chatters can have different voices
- You can set up a keybind in settings to abort any in progress TTS messages. Useful if someone decides to post the bee movie script...
- It only requires a channel name as a setting, it doesn't require any tokens or shenanigans.
- The window is transparent, and hides all buttons when the window is not on top. Just click in the window again to show all the buttons.
- Pokemon up to and including gen 7.

## Setup
### The program itself
Run chatmon.exe, and follow onscreen instruction.
Then click one of the pokeballs, stick in your twitch username and select a pokemon.
Type whatever prefix, like !poke you picked in settings followed by a message in your channel chat. You don't need to have an active stream for this.
Tada! You should hear a voice and see some text in a bubble under the pokemon.

### Streamlabs
It's really easy. Just pick the window in window capture, and set capture mode to windows 10.
That's all! You should now see the program with all it's glorious transparent background.

![image](https://github.com/magnusjjj/ChatMon/assets/525731/f7b473d9-36b9-47a4-9240-dace192dcd19)

## How do I build this?
### Requirements:
- visual studio 2022
- nodejs 21.5 with the build tools available in the installer.
- yarn (npm install -g yarn in the terminal)

### Building for debug:
- Clone the repository.
- open the frontend directory in terminal
- run "yarn install" (You *might* have to run "set-executionpolicy remotesigned" in an administrator powershell if it's a fresh install of yarn)
- run "yarn serve"
- open the sln file in the chatmon directory
- make sure that visual studio target set to 'debug', and hit debug. Tadaaa!

### Building for release:
- Set it to 'Release'.
- Delete everything in the 'publish' folder in the root.
- Build all.
- Zip up and enjoy :)

## FAQ, questions that people have asked, and some nobody has asked so far

#### How do I add more voices?
You need to go to speech settings in windows, scroll all the way down, and then add as many voices as you like (go wild!) and then reboot.
Sorry for the reboot, literally a windows thing that can't be fixed.

#### The code quality is rubbish mate
It's slightly better now, but mostly burned out on it :). Hey, if you wanna talk about it, hmu on discord, might get un-burned-out by the attention.

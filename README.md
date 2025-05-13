# PSVrammed
PSVrammed is a PlayStation 1 (PSX) emulator state VRAM viewer and simple editor. With it you can open state files and view the VRAM content and then save it as images or edit it and save the changes back to the state file.
![example](https://github.com/mechaskrom/PSVrammed/blob/main/example.png)

Its primary use is to rip graphics from games or to temporarily change graphics when taking emulator screenshots. I created it when I was working on making maps for a 2D game because I needed a simple way to screenshot different layers.

How well the editor works for changing graphics depends on how often the game reloads VRAM. Usually that only happens when you move between different places in a game.

Supported emulator state files at the moment are:
-ePSXe
-pSX (PSXFin)
-BizHawk Octoshock PSX core
-DuckStation

Raw 1MB VRAM dumps are also supported.

PSVrammed cannot convert files so saved file will be in the same format as the source.

## Installation
No installation is needed. Just run the exe file to start PSVrammed. Microsoft's .NET 6.0 desktop runtime is required though, but if it's missing you should get a prompt directing you to the installer.

# Help Wanted
I'd really appreciate help with implementing support for more state file formats and more robust handling of them. Especially BizHawk, which currently uses a really hacky solution. It doesn't have to be code commits, just a detailed description of an emulator's state file format is very helpful to me.

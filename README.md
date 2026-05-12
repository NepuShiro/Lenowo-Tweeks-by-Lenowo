# Lenowo Tweeks

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that does things we want. :3

## Features
1. Modify Inspector UIX to look prettier and adds the option to expand string and URI inputs to match the size of the content. (this also adds UIX for nullabled matrix's since resonite doesn't have any UIX for it by default)
2. A reset all button next to the reset scale, rotation, postion buttons.
3. A system for collapsing components! which makes the component UI not load unless you click on the component, this makes slots with large amounts of components trivial to open unlike the default behavior.
4. With the collapsing components comes a little bit of UIX customization, as you can change the color of the text when its collapsed AND uncollapsed!
5. List collapsing! We added some config options to make large lists collapse and all lists collapsable mostly to reduce lag when loading components MORE but also because they can be so long its annoying to scoll to what you need!
6. A modified add child button that makes it super easy to start UIX and context menu stuff!
7. Custom protoflux wires and utilities such as disabling the background of Relay's, adding collapsable Protoflux Nodes, and custom connector UIX that is different if you have a wire connected or not! (the expanded string and URI inputs also apply to protoflux!)
8. Fixed a few annoying things with protoflux such as a toggle for physical touch, makeing dynvar inputs and similar fields auto load their text editors.
9. Modified component headers for dynamic variables to include the variable name AND its customizable with what wording you like!

## Installation
1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
1. Place [LenowoTweeks.dll](https://github.com/YourGithubUsername/YourModRepoName/releases/latest/download/LenowoTweeks.dll) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` for a default install. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create this folder for you.
1. Start the game. If you want to verify that the mod is working you can check your Resonite logs.

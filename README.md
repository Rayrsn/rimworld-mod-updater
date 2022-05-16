# RimworldModUpdater

A tool used for automatically updating/downloading mods for rimworld.

This tool is meant to make it easy to maintain a large list of rimworld mods without access to the steam workshop, so this should be useful for anyone with a GOG copy of the game as well as those that have a pirated copy.

It also features a built-in downloader for downloading mods directly from the steam workshop using an embedded browser.

I made this just for myself initially, as manually keeping track of hundreds of mods(or even just dozens) is basically impossible. The way the updater works is fairly simple, and I might consider adapting it to work for multiple games in the future. Certainly not anytime soon though.

## Installation ##

- Download the latest version from [Releases](https://github.com/EnervationRIN/RimworldModUpdater/releases/latest)
- Use your decompression utility of choice (7-zip, winrar, etc) to extract the ModUpdater folder to your rimworld installation.

It's recommended you keep the updater in its subfolder inside of the rimworld install folder; This allows the updater to automatically detect the path as well as not cluttering your installation.

## Basic Usage ##

- Run RimworldModUpdater.exe
- If it's not set already, select your rimworld installation folder by clicking "Open Rimworld Folder"
- Click "Query Mod Files" and wait to finish checking for outdated mods.
- After it's finished, click "Update All" to update all outdated mods.

[Example](https://streamable.com/uubtli)

## How does it work? ##

It's easy to say "update if mod is outdated" but a little more difficult in practice. There is no built-in mod "version" that can just be read and update if it's not the current version. There are several user-created tools that do specify versions and a repo to download updates from, but this is a minority. So instead the updater just gets the time that the mod files were created on your filesystem and compares them to the last time the workshop mod was updated.

Due to the way the updater does this, this requires mods to have a PublishedFileId.txt. Every workshop mod ships with this file, and it contains the file ID corresponding to the file on the workshop. Some mods you download off of github will also include this file, since it's probably just pushed from the original mod folder. Using this ID, we can query steam for information about the mod and use SteamCMD to download it if it's been updated.

## Why is it so big!? ##
I've tried to make the initial download for the updater as small as possible with compression, but most of the size is actually coming from the resources/dependencies for the embedded chromium browser, adding over 100MB to the total size. This is further amplified when you update something for the first time, steamcmd will download all of its stuff and tack on another 100MB or so.

## Note ##

Due to the way the updater checks to see if a mod is outdated, it's not foolproof. If you download a mod from its github page and then the mod author uploads that same version to the workshop a day later, the updater will say it's outdated even though it's the exact same files with a different upload date. Stuff like this can result in some false positives the first time around.

Since the tool checks for the date mods were created on YOUR filesystem, copying existing mods into your mod folder or downloading & extracting an outdated mod into your mod folder will result in the updater not being able to detect that the mod(s) is outdated. You can use the manual update to update these mods, but it will not be done automatically.

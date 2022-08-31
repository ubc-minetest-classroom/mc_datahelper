[![.NET 6 Continuous Integration and Deployment](https://github.com/ubc-minetest-classroom/mc_datahelper/actions/workflows/ci.yml/badge.svg?branch=main&event=push)](https://github.com/ubc-minetest-classroom/mc_datahelper/actions/workflows/ci.yml)

# About

MC_DataHelper is a graphical user interface created to lower the technical boundaries of defining and importing data into the UBC Minetest Classroom game (https://github.com/ubc-minetest-classroom/minetest_classroom), and more generally, into the Minetest engine.

In the simplest way possible, MC_DataHelper is a graphical Minetest Mod maker with the goal to enable anyone to create new content for the engine regardless of experience.

## Built With
MC_DataHelper has been built using:
* The .net 6.0 framework (https://github.com/dotnet)
* Avalonia UI Toolkit (https://github.com/AvaloniaUI/Avalonia)

# Versioning
Every commit that gets pushed to master for this project triggers an automatic build as well as a tagged release called `latest`. Stable tagged releases are also released from time to time.

You can download binaries compatible with most Windows, Mac, and Linux installations from: https://github.com/ubc-minetest-classroom/mc_datahelper/releases/tag/latest. If a binary is not available, please follow the instructions below for manually building the project.

# Building the project
If you would like to contribute to this project or require binaries for another platform, you can follow these general steps to build the binaries:
## CLI
1. Download and install the dotnet 6.0 core SDK for your build platform (https://dotnet.microsoft.com/en-us/download/).
2. Clone this repository (recommended branch: main), and in your CLI, navigate to the repo's root directory.
3. Execute `dotnet publish -c Release` and wait for it to finish compiling & packaging. To compile for another platform, use the runtime identifier (`-r [RID]`) flag. Valid RID's can be found here: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
4. The compiled project will be found in `~\MC_DataHelper\bin\Release\net6.0\publish`

Additional publish command flags can be passed to the `dotnet publish` command to modify build behavior. For example, to build to a single EXE file. (https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish)


## IDE
You can also open the project solution file (`MC_DataHelper.sln`) using Jetbrains Rider (https://www.jetbrains.com/rider/) or Visual Studio (https://visualstudio.microsoft.com/downloads/). From there, you can use the respective platforms build system to generate binaries.
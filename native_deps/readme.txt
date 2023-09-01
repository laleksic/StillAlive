prebuilt native dependencies are provided for windows (32 and 64 bit) and linux (64 bit)

to build the native dependencies

extract the native_deps.zip into this folder

set the environment variables
OS to "windows" or "linux"
ARCH to "64" or "32"

(linux 32-bit not yet supported)

and run "bash build.sh"

on windows you'll need bash (comes with git for windows) and mingw installed to c:\mingw32 (32 bit) and/or c:\mingw64 (64 bit)

on linux you'll need to install a cross-compiler with crosstool-ng targeting centos 6
to the standard install path (~/x-tools/x86_64-centos6-linux-gnu)

on both platforms you'll need a recent version of cmake

in some cases the dependencies' code and/or build systems had to be modified (very minor modifications) to get them to build with this setup,
so when upgrading dependencies, some tweaking will have to be done
# C# script runner

Run any c# file from the command line.

```
# console mode 
nscript.exe script.cs 

# windows mode
nscriptw.exe script.cs
```

## Using

To access other libraries in the c# file add the following to the top of your .cs files:

```c#
// ref : mylib.dll
// ref : c:\dir\mylib2.dll
using System;
...
```

If you like you can compile the script file to an exe with:

```
nscript.exe /c script.cs
```


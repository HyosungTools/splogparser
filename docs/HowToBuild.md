# How To Build splogparser

To build splogparser locally mimic the steps documented in `.github\workflow\build.yml`. These are the build steps that happen on the build server.

1. style:ordered Open a `cmd.exe` in the local repository folder (e.g. `C:\Git\splogparser`).

2. Add the MSBuild tools to the path by running the command `build\SetEnv.cmd`.

3. Build the application by running the MSBuild command from `.github\workflow\build.yml`:

```text
msbuild src\\splogparser.sln /m:1 /t:Clean,Build /p:Configuration="Release" /p:Platform="Any CPU" /nodeReuse:false
```

4. Add to the path the location of `vstest.console.exe` (yes, you shouldnt have to do this but for some reason I couldn't get it to work when I tried adding it to `build\SetEnv.cmd`).

```text
set path=%path%;C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\Common7\IDE\CommonExtensions\Microsoft\TestWindow
```

5. Run the unit tests by running the MSBuild command from `.github\workflow\build.yml`:

```text
msbuild src\\splogparser.sln /t:RunUnitTests /p:Configuration="Release" /p:Platform="Any CPU"
```

The application is distributed to the `splogparser\dist` subfolder.

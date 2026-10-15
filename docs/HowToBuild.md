# How To Build splogparser

To build splogparser locally, mimic the steps in `.github\workflows\build.yml`. These are the same steps the build server runs on every pull request and every merge to `main`.

1. Open a `cmd.exe` in the local repository folder (e.g. `C:\Git\splogparser`).

2. Add the MSBuild tools to the path:

```text
build\SetEnv.cmd
```

3. Restore NuGet packages (first build, or after a package change):

```text
cd src && nuget restore && cd ..
```

4. Make sure the `dist` folder exists:

```text
if not exist dist mkdir dist
```

5. Build the application:

```text
msbuild src\splogparser.sln /m:1 /t:Clean,Build /p:Configuration="Release" /p:Platform="Any CPU" /nodeReuse:false
```

6. Add the location of `vstest.console.exe` to the path. (You shouldn't have to do this, but it didn't work when added to `build\SetEnv.cmd`.)

```text
set path=%path%;C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\Common7\IDE\CommonExtensions\Microsoft\TestWindow
```

7. Run the unit tests:

```text
msbuild src\splogparser.sln /t:RunUnitTests /p:Configuration="Release" /p:Platform="Any CPU"
```

The application is built into the `dist` folder at the root of the repository.

Once the build and tests pass, see [Pull Request Process](./PullRequest.md) to get your change into `main`.

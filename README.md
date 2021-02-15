# Simple Rule Engine

## Purpose

This project contains the source code for a simple rule engine.

The program prints the final record state based on the provided initial data and list of commands.

## Prerequisite

[PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows?view=powershell-7.1) - The tool to run the build script if you are using a Windows machine

[bash](https://www.gnu.org/software/bash/) - The tool to run the build script if you are using a Linus machine

[Docker](https://docs.docker.com/get-docker/) - an open platform for developing, shipping, and running applications

## Inputs

The program accepts two inputs:

1. A JSON file containing an array of object representing the initial state. Each object has a "Key" and "Value" property of type `string`

ex.

```
[{ "Key": "asleep", "Value": "true" }]
```

2. A JSON file containing an arry of objects representing a list of commands. Each object has a "Critera" and "Actions" array. The Criteria array represents a list of criterias for the list of actions, contained in the Actions array, to execute.

ex.

```
[
  {
    "Criteria": [
      { "Key": "asleep", "Operator": "=", "Value": "true" }
    ],
    "Actions": [
      { "Key": "asleep", "Value": "false" },
      { "Key": "get-ready", "Value": "false" }
    ]
  }
]
```

## Building and running the application

1. Navigate to the main `rule-engine` folder after unzipping the .zip file

2. Open Windows PowerShell or command prompt based on the operating system on your machine at the main `rule-engine` folder

3. add the file containg the initial state as `Data.json` in the `data` folder. Alternatively, open `Dockerfile` and provide the full path of the file as the third argument of the `ENTRYPOINT` command

4. add the file containing the rules as `Command.json` in the `data` folder. Alternatively, open `Dockerfile` and provide the full path of the file as the fourth argument of the `ENTRYPOINT` command

5. Enter `.\build-script.ps1` in Windows Powershell or `build-script.sh` in command prompt based on the operating system on your machine

## Expected outputs

The build script tries to remove the docker container `jsiuapp` before building and running the application. When you run it for the first time, you may see a message `Error: No such container: jsiuapp` in the console. The script would still continue to run so it is not a fatal issue.

The script then creates a docker image and container for the application and the build logs are visiible in the console as well. Feel free to look through them if you are curious about the build process, otherwise wait until you see the message `Starting rule engine...`, signifying that the program is running.

If the input files are valid, the program would log the final record state then terminate.

If the program encounters an issue(s) with the input files, it would log the error message(s) in the console then terminate.

### Possible errors

You may see the following errors if you do not provide valid json files

1. You did not provide the correct number of files as input:

```
[Error] Please pass in the InitialRecordState file, then the Rules file as the arguments.
```

2. There is an unsupported operator in the rules file:

```
[Error] The operator ${unsupportedOperator} is not supported. Please verify the rules file content.
```

3. A provided file does not exist:

```
[Error] File: ${filePath} does not exist.
```

4. A provided file is not a JSON file:

```
[Error] File: ${filePath} is not a .json file.
```

5. A provided file is empty:

```
[Error] File: ${filePath} is empty. Please include ""[]"" if you intended to pass in an empty array.
```

6. An object in a provided file cannot be deserialized into a valid object type defined by the program:

```
[Error] An error occured while deserializing the objects in file: ${filePath}
${errorMessage1}
${errorMessage2}
...
```

# Yaml Action Runner (YAMI)

**YAMI** is a simple, extensible C# CLI tool for defining and running workflows using easy-to-read YAML files. Automate simple tasks and orchestrate them in a structured way.

## Installation & Setup

You can easily build the `yami` executable from the source code and make it available as a global CLI command.

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0 "null")
    

### 1. Pack the Tool

First, build the tool package from the source code.

1. **Clone the repository:**
    
    ```
    git clone <your-repo-url>
    cd YamlActionRunnerCli
    ```
    
2. **Run the `dotnet pack` command:**
    
    ```
    dotnet pack -c Release
    ```
    
    This creates a file named `YamlActionRunnerCli.1.0.0.nupkg` inside the `bin/Release/` directory.
    

### 2. Install the Tool Globally

Now, install the tool from the local package you just created.

```
# This command points to the folder containing the .nupkg
dotnet tool install --global --add-source ./bin/Release YamlActionRunnerCli
```

You're all set! You can now open **any new terminal** and use the `yami` command.

### Updating & Uninstalling

- To Update:
    
    If you make changes to the code, you'll need to re-pack and update the tool:
    
    ```
    dotnet pack -c Release
    dotnet tool update --global --add-source ./bin/Release YamlActionRunnerCli
    ```
    
- **To Uninstall:**
    
    ```
    dotnet tool uninstall --global YamlActionRunnerCli
    ```
    

## Usage

Once `yami` is in your system's `PATH`, you can run it from any directory.

```
# The command is now globally available!
yami run [options]
```

### CLI Options

|             |                 |           |                                                              |
| ----------- | --------------- | --------- | ------------------------------------------------------------ |
| **Command** | **Option**      | **Short** | **Description**                                              |
| **run**     |                 |           | Command to run a YAML actions file.                          |
|             | `--file <path>` | `-f`      | **(Required)** The path to the YAML instructions file.       |
|             | `--dry-run`     |           | Parses the YAML and prints the steps without executing them. |
|             | `--verbose`     |           | Enables detailed verbose logging for every step.             |
|             | `--help`        |           | Displays the help screen with all commands and options.      |

### Example

```
# Run a workflow from anywhere
yami run -f "C:\my-instructions\actions.yaml"

# See what would happen without running
yami run -f /home/user/instructions/actions.yaml --dry-run

# Run with detailed logging
yami run -f myFile.yaml --verbose
```

## YAML Action Reference

Instructions workflows are defined by a list of `steps`. Each step must have an `action` and a `parameters` block. Variables can be used in any parameter string with the syntax `${variableName}`.

```
steps:
  - action: <action_type>
    parameters:
      <param_name>: <param_value>
      <param_name>: <param_value>
```

### Available Actions

|   |   |   |
|---|---|---|
|**Action**|**Description**|**Parameters**|
|**log**|Logs a message to the console.|`message`: (string, required) The text to log. Supports `${var}` substitution.<br><br>  <br><br>`level`: (string, optional) Log level (`Verbose`, `Debug`, `Information`, `Warning`, `Error`, `Fatal`). Defaults to `Information`.|
|**setVariable**|Sets a variable in memory for this run.|`name`: (string, required) The name of the variable.<br><br>  <br><br>`value`: (any, required) The value to store.|
|**printVariable**|Logs the value of a variable.|`name`: (string, required) The name of the variable to print.|
|**delay**|Pauses execution for a set duration.|`duration`: (int, required) The time to wait in milliseconds.|
|**shell**|Executes a command in the native shell (`cmd.exe` or `/bin/bash`).|`command`: (string, required) The command to run (e.g., `dotnet build`). Supports `${var}` substitution.|
|**assert**|Evaluates a C# expression. Fails the run if false.|`condition`: (string, required) The C# boolean expression (e.g., `"1 + 1 == 2"` or `"\"${myVar}\" == \"Hello\""`).|
|**http**|Makes an HTTP request.|`method`: (string, required) `Get` or `Post`.<br><br>  <br><br>`url`: (string, required) The target URL. Supports `${var}` substitution.<br><br>  <br><br>`body`: (string, optional) The string content for a `Post` request. Supports `${var}` substitution.|
|**condition**|Runs nested steps _only if_ a condition is true.|`condition`: (string, required) The C# boolean expression to check.<br><br>  <br><br>`steps`: (list, required) A list of steps to run if the condition is true.|
|**retry**|Retries a block of steps if they fail.|`times`: (int, required) The total number of attempts.<br><br>  <br><br>`steps`: (list, required) A list of steps to run and retry on failure.|
|**parallel**|Runs a block of steps concurrently.|`steps`: (list, required) A list of steps to run in parallel.|
|**import**|Runs all steps from another YAML file.|`filePath`: (string, required) The path to the YAML file to import. Supports `${var}` substitution.|

## Example Workflow (`Example.yaml`)

This example demonstrates usage for all commands:

```
steps:   
  - action: setVariable  
    parameters:  
      name: "name"  
      value: "Uri"  
  - action: log  
    parameters:  
      message: "Hello ${name}!"  
  - action: setVariable  
    parameters:  
      name: "delayMs"  
      value: 5000  
  - action: delay  
    parameters:  
      duration: ${delayMs}  
  - action: assert  
    parameters:  
      condition: "${delayMs} == 5000"  
  - action: retry  
    parameters:  
      times: 3  
      steps:  
        - action: http  
          parameters:  
            method: post  
            url: "https://postman-echo.com/post"  
            body: "{\"message\":\"Hello world\"}"  
        - action: http  
          parameters:  
            method: get  
            url: "https://postman-echo.com/get?message=steam"  
  - action: condition  
    parameters:  
      condition: "true"  
      steps:  
        - action: setVariable  
          parameters:  
            name: "text"  
            value: "hello world"  
        - action: printVariable  
          parameters:  
            name: "text"  
  - action: parallel  
    parameters:  
      steps:  
        - action: delay  
          parameters:  
            duration: 1000  
        - action: printVariable  
          parameters:  
            name: "name"  
        - action: shell  
          parameters:   
            command: "echo ${name}"  
  - action: import  
    parameters:  
      filePath: ExampleYamlActionFiles/ImportExample.yaml
```# Yaml Action Runner (YAMI)

**YAMI** is a simple, extensible C# CLI tool for defining and running workflows using easy-to-read YAML files. Automate simple tasks and orchestrate them in a structured way.

## Installation & Setup

You can easily build the `yami` executable from the source code and make it available as a global CLI command.

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0 "null")
    

### 1. Pack the Tool

First, build the tool package from the source code.

1. **Clone the repository:**
    
    ```
    git clone <your-repo-url>
    cd YamlActionRunnerCli
    ```
    
2. **Run the `dotnet pack` command:**
    
    ```
    dotnet pack -c Release
    ```
    
    This creates a file named `YamlActionRunnerCli.1.0.0.nupkg` inside the `bin/Release/` directory.
    

### 2. Install the Tool Globally

Now, install the tool from the local package you just created.

```
# This command points to the folder containing the .nupkg
dotnet tool install --global --add-source ./bin/Release YamlActionRunnerCli
```

You're all set! You can now open **any new terminal** and use the `yami` command.

### Updating & Uninstalling

- To Update:
    
    If you make changes to the code, you'll need to re-pack and update the tool:
    
    ```
    dotnet pack -c Release
    dotnet tool update --global --add-source ./bin/Release YamlActionRunnerCli
    ```
    
- **To Uninstall:**
    
    ```
    dotnet tool uninstall --global YamlActionRunnerCli
    ```
    

## Usage

Once `yami` is in your system's `PATH`, you can run it from any directory.

```
# The command is now globally available!
yami run [options]
```

### CLI Options

|             |                 |           |                                                              |
| ----------- | --------------- | --------- | ------------------------------------------------------------ |
| **Command** | **Option**      | **Short** | **Description**                                              |
| **run**     |                 |           | Command to run a YAML actions file.                          |
|             | `--file <path>` | `-f`      | **(Required)** The path to the YAML instructions file.       |
|             | `--dry-run`     |           | Parses the YAML and prints the steps without executing them. |
|             | `--verbose`     |           | Enables detailed verbose logging for every step.             |
|             | `--help`        |           | Displays the help screen with all commands and options.      |

### Example

```
# Run a workflow from anywhere
yami run -f "C:\my-instructions\actions.yaml"

# See what would happen without running
yami run -f /home/user/instructions/actions.yaml --dry-run

# Run with detailed logging
yami run -f myFile.yaml --verbose
```

## YAML Action Reference

Instructions workflows are defined by a list of `steps`. Each step must have an `action` and a `parameters` block. Variables can be used in any parameter string with the syntax `${variableName}`.

```
steps:
  - action: <action_type>
    parameters:
      <param_name>: <param_value>
      <param_name>: <param_value>
```

### Available Actions

|   |   |   |
|---|---|---|
|**Action**|**Description**|**Parameters**|
|**log**|Logs a message to the console.|`message`: (string, required) The text to log. Supports `${var}` substitution.<br><br>  <br><br>`level`: (string, optional) Log level (`Verbose`, `Debug`, `Information`, `Warning`, `Error`, `Fatal`). Defaults to `Information`.|
|**setVariable**|Sets a variable in memory for this run.|`name`: (string, required) The name of the variable.<br><br>  <br><br>`value`: (any, required) The value to store.|
|**printVariable**|Logs the value of a variable.|`name`: (string, required) The name of the variable to print.|
|**delay**|Pauses execution for a set duration.|`duration`: (int, required) The time to wait in milliseconds.|
|**shell**|Executes a command in the native shell (`cmd.exe` or `/bin/bash`).|`command`: (string, required) The command to run (e.g., `dotnet build`). Supports `${var}` substitution.|
|**assert**|Evaluates a C# expression. Fails the run if false.|`condition`: (string, required) The C# boolean expression (e.g., `"1 + 1 == 2"` or `"\"${myVar}\" == \"Hello\""`).|
|**http**|Makes an HTTP request.|`method`: (string, required) `Get` or `Post`.<br><br>  <br><br>`url`: (string, required) The target URL. Supports `${var}` substitution.<br><br>  <br><br>`body`: (string, optional) The string content for a `Post` request. Supports `${var}` substitution.|
|**condition**|Runs nested steps _only if_ a condition is true.|`condition`: (string, required) The C# boolean expression to check.<br><br>  <br><br>`steps`: (list, required) A list of steps to run if the condition is true.|
|**retry**|Retries a block of steps if they fail.|`times`: (int, required) The total number of attempts.<br><br>  <br><br>`steps`: (list, required) A list of steps to run and retry on failure.|
|**parallel**|Runs a block of steps concurrently.|`steps`: (list, required) A list of steps to run in parallel.|
|**import**|Runs all steps from another YAML file.|`filePath`: (string, required) The path to the YAML file to import. Supports `${var}` substitution.|

## Example Workflow (`Example.yaml`)

This example demonstrates usage for all commands:

```
steps:   
  - action: setVariable  
    parameters:  
      name: "name"  
      value: "Uri"  
  - action: log  
    parameters:  
      message: "Hello ${name}!"  
  - action: setVariable  
    parameters:  
      name: "delayMs"  
      value: 5000  
  - action: delay  
    parameters:  
      duration: ${delayMs}  
  - action: assert  
    parameters:  
      condition: "${delayMs} == 5000"  
  - action: retry  
    parameters:  
      times: 3  
      steps:  
        - action: http  
          parameters:  
            method: post  
            url: "https://postman-echo.com/post"  
            body: "{\"message\":\"Hello world\"}"  
        - action: http  
          parameters:  
            method: get  
            url: "https://postman-echo.com/get?message=steam"  
  - action: condition  
    parameters:  
      condition: "true"  
      steps:  
        - action: setVariable  
          parameters:  
            name: "text"  
            value: "hello world"  
        - action: printVariable  
          parameters:  
            name: "text"  
  - action: parallel  
    parameters:  
      steps:  
        - action: delay  
          parameters:  
            duration: 1000  
        - action: printVariable  
          parameters:  
            name: "name"  
        - action: shell  
          parameters:   
            command: "echo ${name}"  
  - action: import  
    parameters:  
      filePath: ExampleYamlActionFiles/ImportExample.yaml
```
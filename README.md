<div id="top">
    <br />
    <div align="center">
        <a href="./images/NullP-Logo.png">
            <img src="./images/NullP-Logo.png" alt="Logo" width="80" height="80">
        </a>
        <h3 align="center">NULLP INTERPRETER</h3>
        <p align="center">
            An interpreter for the (invented) language NULLP and just made for fun.
            <br />
        </p>
    </div>
</div>

# About
NULLP is a made up programming language created for the only purpose to test out how a language and the interpreter made for it works.

## Example Program

```JavaScript
// Define function with parameters
function ExecuteCode(fruit)			
{
    // Print something to the console
    WriteLine("Do you want an " + fruit + "?");
    // Read a string from the console
    var answer = ReadLine();
    // Write an if statement					
    if (answer == "Yes")						
    {
        WriteLine("Good, here is your " + fruit + ".");					
    }
    else											
    {
        WriteLine("Okay then, no " + fruit + " for you.");
    }
}

function Main()							
{
    // Call functions from an other function
    ExecuteCode("apple");					
}

// Execute code
Main();
```

### Output
```
Do you want an apple?
[Yes] > Good, here is your apple.
[No] > Okay then, no apple for you. 
```

## Language Features

### Variables

#### Definition
```JavaScript
var variableName;

// or

var variableName = initialValue;
```
#### Assignment
```JavaScript
variable = newValue;
variable = FunctionCall();
```

### Functions

#### Definition
```JavaScript
function FunctionName(parameter1, parameter 2, etc)
{
    // Function body here
}
```

#### Call
```JavaScript
FunctionName(param1, "param2", 3, etc);
```

### If-Statement

```JavaScript
// Is equals to
if (variable1 == "expectedValue")
{
    // Code
}
else
{
    // Code
}
```

```JavaScript
// Is not equals to
if (variable1 != "expectedValue")
{
    ...
}
```

### Built-In Functions

|          Name             |       Parameters         |       Return Value         |
|---------------------------|--------------------------|----------------------------|
| WriteLine                 |  String : messageToPrint | None                       |
| ReadLine                  |  None                    | String : Read input        |
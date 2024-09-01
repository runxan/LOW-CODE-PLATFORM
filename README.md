custom-assembly-resolvers-web-api
=================================

#Overview
This solution contains two projects that together enable dynamic, ad-hoc development using C# scripting. The main feature of this solution is the ability to write C# code as scripts, include necessary libraries, and have the script compiled into a DLL at runtime within the main application. This setup is ideal for scenarios where you need to quickly develop and test functionality like API calls, medium-level computations, and more without the overhead of a full development cycle.

#Project Structure
WebApiCodeGenLib Project:
This project allows you to write C# scripts. You can include external libraries and define custom logic. Once the script is ready, it gets compiled into a DLL, which can be used by the main application.

#CustomAssemblyResolver Project:
The main application dynamically loads the script file which in runtime from the WebApiCodeGenLib project. This enables you to execute the scripts as modules within the application, providing a flexible and modular approach to development.

#Key Features
Ad-hoc Development: Write and execute C# scripts on the fly, without needing to recompile the entire application.
Runtime Compilation: Scripts are compiled into DLLs at runtime, allowing for easy integration and execution within the main application.
Modularity: Add scripts as modules, making it easy to extend the functionality of the application with minimal changes to the core codebase.
Library Inclusion: Easily include external libraries in your scripts, enabling complex operations like API calls and data processing.
Use Cases
Rapid API integration and testing.
Medium-level computations and data processing.
Prototyping and experimenting with new features without impacting the main application.

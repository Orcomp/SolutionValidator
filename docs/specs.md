The aim of this project is to build a WPF application which will check existing Visual Studio solutions to make sure they conform to certain standards and display the results in a meaningful way.

# The Problem

We have a lot of C# projects that all use different standards. We want to develop an application which will make it easy and simple for us to check each project and allow us to make the necessary modifications to ensure they do follow the standards.

When a project does not conform to the standards we would like to see a report that will help us address these issues.

The application will also have some functionality to fix some of the issues automatically.

The application should be flexible enough to deal with different sets of standards, depending on each teams requirements.


# Functionality

There are really only 4 things we need to check:

1) The folder structure of the main repository.
2) The output build path of the project.
3) That certain files are present and in the right locations.
4) That the code follows the standards defined in a Stylecop.settings file and/or a Resharper settings file.

The application will have a WPF user interface, but it will also be able to be run at the command line. This is important because I would like to include this tool as a build step in a continuous integration environment.


## Folder Structure

The application will check the folder structure within a repository follows this structure:
./src
./output
./doc
./deployment

Read this post for more details: http://blog.catenalogic.com/post/2012/04/04/Setting-output-directories-of-projects.aspx

The solution file for the project should be found in the ./src folder. Each project belonging to the solution should be contained in its own folder inside the ./src folder.

The application should make it easy for new folder structures to be defined if needed.

## Output Build Path

The application must check the output build path for each project is correct.
i.e. for us it will be ./output/debug/.NET40/projectName if the project is in debug mode or
./output/release/.NET40/projectName if it is in release mode.

## Files

The application must be able to check certain files exist and are located in the right place.
Some of these are:

- stylecop settings
- Resharper settings
- gitignore
- gitattributes
- Readme.md
- License file
- FAKE build script
- etc...

The application must also have the option to create these files if they do not already exist. Or check that the settings are the same as the ones in the template.

## Code Inspection

The application will run stylecop and/or Reshaper's InspectCode to check the code base against the stylecop settings and/or resharper settings.

An important part of this application will be to display the findings (which are XML files) in a intuitive and meaningful way, and make it easy for developers to fix the issues quickly.

If you are curious to know which standards we use, they are defined here: https://csharpguidelines.codeplex.com/ (Except that we allow private fields to have an underscore prefix.)

# Suggestions

- Resharper Inspection Tool : http://confluence.jetbrains.com/display/NETCOM/Introducing+InspectCode

This tool is a command line tool, which is freely available even if you do not have a Resharper license. The InspectCode tool will analyze all your code base and output a report as an XML file.

The application will call the InspectCode tool to run the code inspection.


# In Summary

- Build a flexible WPF and console application to check whether a solution conforms to a set of standards.

SolutionValidator
----------------------------------------------

WIP: 2014/03/26


Validates existing solution:

- Folder structure
- File existance (such as gitignore, license.txt, Stylecop settings etc...)
- Check and correct Output build directory for each project: [hint](http://social.msdn.microsoft.com/Forums/vstudio/en-US/01422233-63d8-48c9-8bbe-148e6a6bef16/need-help-programmatically-setting-projects-output-directory?forum=csharpgeneral)
- Check Code follows conventions.
- Reformat code using NRefactory.

# Folder Structure

Uses a .gitignore style definition file to check the structure of a solution.

Checks that:
- Certain folders and files exist at predefined locations
- Certain folders and files do not exist at certain locations.

# Reformatting Code

Would be nice to integrate with a git library to create a branch before reformatting occurs.
Make staged changes where the code is submitted and use FAKE to build and make sure the reformatting hasn't broken anything. Then rebase all the commits.

- Re-order members
- Put using statements inside the namespace
- Check the copyrights
- Check private fields start with an underscore
- If statements should always have opening and closing curly brackets.
- etc...

Libraries to use:
- NRefactory
- https://github.com/gitextensions/libgit2sharp


# Other

Uses NRefactory to reformat code:
- https://github.com/icsharpcode/NRefactory/blob/master/ICSharpCode.NRefactory.CSharp/Formatter/FormattingOptionsFactory.cs
- https://github.com/icsharpcode/NRefactory/tree/master/ICSharpCode.NRefactory.CSharp
- Tutorial: http://mikemdblog.blogspot.com.au/2013/03/code-formatting-with-nrefactory.html

Formatting tools available:
- Resharper
- Roslyn: http://www.christophdebaene.com/blog/2011/10/26/roslyn-formatting-code/
- Sourceformat.com http://www.sourceformat.com/csharp-formatter.htm

Create Projects using Templates:
- http://msdn.microsoft.com/en-us/library/vstudio/bb126445(v=vs.100).aspx
- http://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
- http://msdn.microsoft.com/en-us/library/ms228767.aspx

C# Parsers:
- http://www.inevitablesoftware.com/Products.aspx
- http://trelford.com/blog/category/C.aspx

Other:
- NugetSolutionValidator: https://github.com/parametric/NuGetSolutionValidator
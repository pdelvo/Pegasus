Pegasus
=======

Pegasus is a PEG-style parser generator for C# that integrates with MSBuild and Visual Studio.

Getting Started
---------------

The easiest way to get a copy of Pegasus is to install the [Pegasus NuGet package](http://nuget.org/packages/Pegasus) in Visual Studio.

    PM> Install-Package Pegasus

Once you have the package installed, files in your project marked as 'PegGrammar' in the properties window will be compiled to their respective `.peg.cs` parser classes before every build.  These parser classes will be automatically included in compilation.

Parsers generated by Pegasus share a few common classes that are copied to the project root automatically.  The namespace of these files can be changed as desired.

For help with grammar syntax, see [the syntax guide wiki entry](https://github.com/otac0n/Pegasus/wiki/Syntax-Guide)

Example
-------

Here is an example of a simple parser for mathematical expressions:

    @namespace MyProject
    @classname ExpressionParser

    additive <decimal>
      = left:multiplicative "+" right:additive { left + right }
      / left:multiplicative "-" right:additive { left - right }
      / multiplicative

    multiplicative <decimal>
      = left:primary "*" right:multiplicative { left * right }
      / left:primary "/" right:multiplicative { left / right }
      / primary

    primary <decimal>
      = decimal
      / "(" additive:additive ")" { additive }

    decimal <decimal>
      = value:([0-9]+ ("." [0-9]+)?) { decimal.Parse(value) }

This will take mathematical expressions as strings and evaluate them with the proper order of operations to produce a result as a decimal.

The above parser would be used like so:

    var parser = new MyProject.ExpressionParser();
    var result = parser.Parse("5.1+2*3");
    Console.WriteLine(result); // Outputs "11.1".
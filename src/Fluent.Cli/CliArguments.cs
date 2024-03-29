﻿using Fluent.Cli.Exceptions;

namespace Fluent.Cli;

public class CliArguments {
    public string Program { get; }
    public string Version { get; }
    public Command? SelectedCommand { get; }
    public List<Option> Options { get; }
    public List<Argument> Arguments { get; }

    public CliArguments(string program, string version, Command? selectedCommand, List<Option> options, List<Argument> arguments) {
        Program = program;
        Version = version;
        SelectedCommand = selectedCommand;
        Options = options;
        Arguments = arguments;
    }

    public bool IsCommandPresent() {
        return SelectedCommand != null;
    }

    public bool IsOptionPresent(char shortName) {
        var option = Option(shortName);
        return option.IsPresent;
    }

    public bool IsOptionPresent(string longName) {
        var option = Option(longName);
        return option.IsPresent;
    }

    public bool IsArgumentPresent(string argumentName) {
        return Arguments.Exists(argument => argument.Name.Equals(argumentName));
    }

    public Command Command() {
        return SelectedCommand;
    }

    public Option Option(char shortName) {
        var option = Options.FirstOrDefault(option => shortName.Equals(option.ShortName));
        if (option != null) return option;
        throw new OptionIsNotConfiguredException($"Option -- '{shortName}' has not been configured yet, add it to the builder first.");
    }

    public Option Option(string longName) {
        var option = Options.FirstOrDefault(option => !string.IsNullOrEmpty(option.Name) && option.Name.Equals(longName));
        if (option != null) return option;
        throw new OptionIsNotConfiguredException($"Option -- '{longName}' has not been configured yet, add it to the builder first.");
    }

    public Argument Argument(string argumentName) {
        var argument = Arguments.FirstOrDefault(argument => argument.Name.Equals(argumentName));
        if (argument == null) throw new ArgumentException($"Argument '{argumentName}' has not been provided.");
        return argument;
    }
}
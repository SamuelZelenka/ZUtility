using System;
using System.Globalization;
using UnityEngine.Events;

/* Create a new command anywhere by Command.NewCommand<Input type>(<string to call command>, (<Input type>) => Method() )
 * eg.  Command.NewCommand<int>("sethp", (newHPValue) => SetHP(newHPValue));
 *      Command.NewCommand<bool>("godmode", (godmodeActive) => _godmode = godmodeActive);
 *      
 *      Questions and problems are directed to Samuel Zelenka af Rolén.
 */

public abstract class Command 
{
    public string description;
    public abstract void Invoke(string input);
    public static void NewCommand(string commandline, Command newCommand, string description)
    {
        if (description == "")
        {
            newCommand.description = $"{commandline.ToLower()} <{newCommand.GetType()}> - No Description.";
        }
        else
        {
            newCommand.description = $"{commandline.ToLower()} <{newCommand.GetType()}> - {description}";
        }
        CommandWindow.AddCommand(commandline, newCommand);
    }
}

/// <summary>
/// Command affecting a boolean.
/// </summary>
public class BoolCommand : Command
{
    private UnityAction<bool> _action;

    public BoolCommand(UnityAction<bool> action)
    {
        _action = action;
    }

    public override void Invoke(string input)
    {
        if (input.ToLower() == "true")
        {
            _action.Invoke(true);
        }
        else if(input.ToLower() == "false")
        {
            _action.Invoke(false);
        }
        else
        {
            CommandLog.PrintMessage($"Invalid parameter. ({input})");
        }
    }
}
/// <summary>
/// Command affecting an integer.
/// </summary>
public class IntCommand : Command
{
    private UnityAction<int> _action;
    public IntCommand(UnityAction<int> action)
    {
        _action = action;
    }
    public override void Invoke(string input)
    {
        int inputInt = 0;
        try
        {
            inputInt = int.Parse(input, NumberStyles.Integer);
        }
        catch (System.Exception)
        {
            CommandLog.PrintMessage($"Invalid parameter. ({input})");
            return;
        }
        _action.Invoke(inputInt);
    }
}

/// <summary>
/// Command affecting a float.
/// </summary>
public class FloatCommand : Command
{
    private UnityAction<float> _action;
    public FloatCommand(UnityAction<float> action)
    {
        _action = action;
    }
    public override void Invoke(string input)
    {
        float inputFloat;
        try
        {
            inputFloat = float.Parse(input, NumberStyles.Float);
        }
        catch (System.Exception)
        {
            CommandLog.PrintMessage($"Invalid parameter. ({input})");
            return;
        }
        _action.Invoke(inputFloat);
    }
}
/// <summary>
/// Command affecting a string.
/// </summary>
public class StringCommand : Command
{
    private UnityAction<string> _action;
    public StringCommand(UnityAction<string> action)
    {
        _action = action;
    }
    public override void Invoke(string input)
    {
        _action.Invoke(input);
    }
}
/// <summary>
/// Command calling a method.
/// </summary>
public class VoidCommand : Command
{
    private UnityAction _action;
    public VoidCommand(UnityAction action)
    {
        _action = action;
    }
    public override void Invoke(string input)
    {
        _action.Invoke();
    }
}
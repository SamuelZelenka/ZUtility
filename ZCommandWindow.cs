using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZCommandWindow : MonoBehaviour
{
    private static UnityAction<string> _commandCall;

    private InputField _inputField;
    private Transform _commandLogView;

    private bool _isActive;

    private string _previousActionCall = "";
    private static Dictionary<string, Command> _commands = new Dictionary<string, Command>();

    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        private set
        {
            _isActive = value;
            _inputField.gameObject.SetActive(IsActive);
            _commandLogView.gameObject.SetActive(IsActive);
        }
    }

    private void Start()
    {
        _inputField = transform.GetChild(0).GetComponent<InputField>();
        _commandLogView = transform.GetChild(1);

        _commandCall += CallCommand;

        _inputField.onEndEdit.AddListener(_commandCall);
        IsActive = false;

        Command.NewCommand("commands", new VoidCommand(() => PrintAllCommands()), "List all available commands.");
        Command.NewCommand("restart", new VoidCommand(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name)), "Restart current level");
        Command.NewCommand("exit", new VoidCommand(() => Application.Quit()), "Restart current level");

    }

    private void Update()
    {
        if (IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsActive = false;
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.UpArrow))
            {
                CallPreviousCommand();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                IsActive = true;
            }
        }
    }
    public void Enable() => _isActive = true;
    public void Disable() => _isActive = false;
    public static void AddCommand(string commandLine, Command action)
    {
        if (!_commands.ContainsKey(commandLine))
        {
            _commands.Add(commandLine, action);
        }
    }
    public void CallCommand(string commandLine)
    {
        commandLine = commandLine.ToLower();
        string[] commandSections = commandLine.Split(' ');
        if (_commands.ContainsKey(commandSections[0]) && commandSections.Length > 1)
        {
            _commands[commandSections[0]].Invoke(commandSections[1]);
            CommandLog.PrintMessage(commandLine);
        }
        else if(_commands.ContainsKey(commandSections[0]))
        {
            _commands[commandSections[0]].Invoke(commandSections[0]);
        }
        else
        {
            CommandLog.PrintMessage("Unknown command");
        }
        _inputField.text = "";
        _previousActionCall = commandLine;
    }
    public void CallPreviousCommand()
    {
        if (_previousActionCall != "")
        {
            CallCommand(_previousActionCall);
        }
    }
    public void PrintAllCommands()
    {
        Command[] commands = _commands.Values.ToArray();
        for (int i = 0; i < commands.Length; i++)
        {
            CommandLog.PrintMessage(commands[i].description);
        }
    }
}

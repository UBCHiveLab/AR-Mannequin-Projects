using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonEvents))]
public class StudentCommandSend : Singleton<StudentCommandSend>
{
    private List<Command> commands;
    private int lastCommand;
    private PhotonEvents photonEv;

    // Start is called before the first frame update
    void Start()
    {
        commands = new List<Command>();
        photonEv = GetComponent<PhotonEvents>();

    }

    // Apply single command - used for network commands that are not attached to UI and have no data to transfer
    public void ApplyCommand(Command command)
    {
        commands.Add(command);
        photonEv.CallCommandEvent(command._evCode, command._data);
        lastCommand++;
    }

    public void ApplyCommands(List<Command> newCommands)
    {
        // Add new commands to the end of buffered list
        foreach (Command cmd in newCommands)
        {
            commands.Add(cmd);
        }
        // iterate through buffered list of commands starting at the command that was last called
        for (int i = lastCommand; i < commands.Count; i++)
        {
            photonEv.CallCommandEvent(commands[i]._evCode, commands[i]._data);
        }
        // update pointer
        lastCommand = commands.Count;
    }

    /// <summary>
    /// Iterates through all buffered commands for when new players enter the room to update status of AR mannequin to match
    /// </summary>
    public void OnNewPlayer()
    {
        foreach (Command cmd in commands)
        {
            photonEv.CallCommandEvent(cmd._evCode, cmd._data);
        }
    }
}

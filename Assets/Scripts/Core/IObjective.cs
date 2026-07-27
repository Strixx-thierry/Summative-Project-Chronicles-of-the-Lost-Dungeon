using System;

// A single level objective the player must satisfy to open the exit
public interface IObjective
{
    string Label { get; }
    string Progress { get; }   // e.g. "1/2"
    bool IsComplete { get; }
    event Action Changed;
}

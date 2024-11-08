using System.Collections.Generic;

public class MelodyData
{
    public const string NoteA = "NoteA";
    public const string NoteB = "NoteB";
    public const string NoteC = "NoteC";
    public const string NoteE = "NoteE";
    public const string NoteF = "NoteF";
    public const int MelodyLength = 4;
    public const string Melody1 = "Melody1";
    public const string Melody2 = "Melody2";
    // TODO: make melodies customizable in the editor
    private static string[] melody1Inputs = new string[MelodyLength]{
        NoteE,
        NoteB,
        NoteC,
        NoteA,
    };
    private static string[] melody2Inputs = new string[MelodyLength]{
        NoteA,
        NoteC,
        NoteB,
        NoteE,
    };
    public static Dictionary<string, string[]> MelodyInputs = new Dictionary<string, string[]>(){
        { Melody1, melody1Inputs },
        { Melody2, melody2Inputs },
    };
}

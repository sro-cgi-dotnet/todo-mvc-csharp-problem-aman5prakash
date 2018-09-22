using System.Collections.Generic;
using Google.Keep;
namespace Classes
{
    public interface INoteClass
    {
        List<Keep> GetAllNotes();
        Keep GetNotesById(int Keepid);
        List<Keep> GetNotesByTitle(string text);
        List<Keep> GetNotesByLabel(string text);
        List<Keep> GetPinnedNotes(string text);
        bool PostNote(Keep value);
        bool PutNote(int id, Keep value);
        bool DeleteNote(int id);
    }
}
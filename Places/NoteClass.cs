using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Google.Keep;

namespace Classes
{
    public class NoteClass : INoteClass
    {
        GoogleContext Notes = null;
        public NoteClass(GoogleContext GK)
        {
            this.Notes = GK;
        }

        public List<Keep> GetAllNotes()
        {
            return Notes.Google.Include(l => l.Lable).Include(c => c.CheckList).ToList();
        }

        public Keep GetNotesById(int Keepid)
        {
            return Notes.Google.Include(l => l.Lable).Include(c => c.CheckList).FirstOrDefault(n => n.KeepId == Keepid);
        }
        public List<Keep> GetNotesByTitle(string text)
        {
            List<Keep> ListTitle = Notes.Google.Where(k => k.Title == text).Include(l => l.Lable).Include(c => c.CheckList).ToList();
            return ListTitle;
        }
        public List<Keep> GetNotesByLabel(string text)
        {
            List<Keep> labeledList = new List<Keep>();
            foreach (Keep label in Notes.Google.Include(l => l.Lable).Include(c => c.CheckList).ToList())
            {
                foreach (LabelNote labelName in label.Lable)
                {
                    if (labelName.items == text)
                    {
                        labeledList.Add(label);
                    }
                }
            }
            return labeledList;
        }
        public List<Keep> GetPinnedNotes(string text)
        {
            bool t1 = true;
            return Notes.Google.Where(k => k.Pinned == t1).Include(l => l.Lable).Include(c => c.CheckList).ToList();
        }
        public bool PostNote(Keep value)
        {
            Keep temp2 = Notes.Google.FirstOrDefault(n => n.KeepId == value.KeepId);
            if (temp2 != null)
            {
                return false;
            }
            Notes.Google.Add(value);
            Notes.SaveChanges();
            return true;
        }
        public bool PutNote(int id, Keep value)
        {
            Notes.Update<Keep>(value);
            Notes.SaveChanges();
            return true;
        }
        public bool DeleteNote(int id)
        {
            List<Keep> ListID = Notes.Google.Where(k => k.KeepId == id).ToList();
            if (ListID.Count > 0)
            {
                Notes.RemoveRange(ListID);
                Notes.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        ~NoteClass()
        {
            Notes.Dispose();
        }
    }
}
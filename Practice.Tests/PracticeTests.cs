using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Practice.Controllers;
using Google.Keep;
using Classes;

namespace Practice.Tests
{
    public class PracticeTests
    {
        private List<Keep> GetMockDatabase()
        {
            return new List<Keep>
            {
                new Keep
                {
                    KeepId = 1,
                    Title = "Note1",
                    Text = "Type your text here",
                    CheckList = new List<Checklist>
                    {
                        new Checklist
                        {
                       // ChecklistID = 1,
                        ChecklistText = "Type your text here"
                        },
                        new Checklist {
                       // ChecklistID = 2,
                        ChecklistText = "Type your text here"
                        }
                    },
                    Lable = new List<LabelNote>
                    {
                        new LabelNote
                        {
                        //Id = 1,
                        items = "aman"
                        },
                        new LabelNote{
                      //  Id = 2,
                        items = "Type your text here"
                        }
                    },
                    Pinned = true
                },
                new Keep
                {
                    KeepId = 2,
                    Title = "Note2",
                    Text = "Type your text here",
                    CheckList = new List<Checklist>
                    {
                        new Checklist
                        {
                       // ChecklistID = 3,
                        ChecklistText = "Type your text here"

                        },
                        new Checklist{
                        //ChecklistID = 4,
                        ChecklistText = "Type your text here"
                        }
                    },
                    Lable = new List<LabelNote>
                    {
                        new LabelNote
                        {
                       // Id = 3,
                        items = "aman"

                        },
                        new LabelNote{
                     //   Id = 4,
                        items = "Type your text here"
                        }
                    },
                    Pinned = false
                }
            };
        }
        public class PositiveTests
        {
            public List<Keep> GetObject()
            {
                PracticeTests practice = new PracticeTests();
                return practice.GetMockDatabase();
            }

            [Fact]
            public void GetAllNotesTest()
            {
                var database = new Mock<INoteClass>();
                List<Keep> notes = GetObject();
                database.Setup(d => d.GetAllNotes()).Returns(notes);
                ValuesController valuesController = new ValuesController(database.Object);

                var actionresult = valuesController.Get();

                var okObjectResult = actionresult as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var model = okObjectResult.Value as List<Keep>;
                Assert.NotNull(model);

                Assert.Equal(notes.Count, model.Count);
            }
            [Fact]
            public void GetNotesById()
            {
                var database = new Mock<INoteClass>();
                List<Keep> notes = GetObject();
                int id = 1;
                database.Setup(d => d.GetNotesById(id)).Returns(notes.Find(n => n.KeepId == id));
                ValuesController valuesController = new ValuesController(database.Object);

                var actionresult = valuesController.Get(id);

                var okObjectResult = actionresult as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var model = okObjectResult.Value as Keep;
                Assert.NotNull(model);

                Assert.Equal(id, model.KeepId);
            }
            [Fact]
            public void GetNotesByTitle()
            {
                var database = new Mock<INoteClass>();
                List<Keep> notes = GetObject();
                string text = "Note1";
                string type = "title";
                database.Setup(d => d.GetNotesByTitle(text)).Returns(notes.FindAll(n => n.Title == text));
                ValuesController valuesController = new ValuesController(database.Object);

                var actionresult = valuesController.Get(text, type);

                var okObjectResult = actionresult as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var model = okObjectResult.Value as List<Keep>;
                Assert.NotNull(model);

                int a = 1;
                Assert.Equal(a, model.Count);
            }
            [Fact]
            public void GetNotesByLabel()
            {
                var database = new Mock<INoteClass>();
                List<Keep> notes = GetObject();
                string text = "aman";
                string type = "label";
                database.Setup(d => d.GetNotesByLabel(text)).Returns(notes.FindAll(n => n.Lable.FindAll(l => l.items == text) != null));
                ValuesController valuesController = new ValuesController(database.Object);

                var actionresult = valuesController.Get(text, type);

                var okObjectResult = actionresult as OkObjectResult;
                Assert.NotNull(okObjectResult);

                var model = okObjectResult.Value as List<Keep>;
                Assert.NotNull(model);

                int a = 2;
                Assert.Equal(a, model.Count);
            }
            [Fact]
            public void Post()
            {
                var database = new Mock<INoteClass>();
                Keep notes = new Keep
                {
                    KeepId = 3,
                    Title = "Note3"
                };
                database.Setup(d => d.PostNote(notes)).Returns(true);
                ValuesController valuesController = new ValuesController(database.Object);

                var actionresult = valuesController.Post(notes);

                var ObjectResult = actionresult as CreatedResult;
                Assert.NotNull(ObjectResult);

                var model = ObjectResult.Value as Keep;
                Assert.Equal(notes.KeepId, model.KeepId);
            }
            // [Fact]
            // public void Put()
            // {

            // }
            //     [Fact]
            //     public void Delete()
            //     {

            //     }
        }
    }
}

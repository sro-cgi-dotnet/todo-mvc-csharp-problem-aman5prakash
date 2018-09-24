using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Keep;
using Microsoft.EntityFrameworkCore;
using Classes;

namespace Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        INoteClass Notess = null;

        public ValuesController(INoteClass gk)
        {
            this.Notess = gk;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var temp = Notess.GetAllNotes();
            if (temp.Count == 0)
            {
                return Ok($"Database is empty");
            }
            else return Ok(temp);
        }

        // GET api/values/5
        [HttpGet("{KeepId:int}")]
        public IActionResult Get(int Keepid)
        {
            var ListID = Notess.GetNotesById(Keepid);
            if (ListID != null)
            {
                return Ok(ListID);
            }
            else
            {
                return BadRequest($"ID doesnot exist");
            }
        }
        [HttpGet("{text}")]
        public IActionResult Get(string text, [FromQuery] string type)
        {
            if (type == "label")
            {
                List<Keep> labeledList = Notess.GetNotesByLabel(text);
                if (labeledList.Count > 0)
                {
                    return Ok(labeledList);
                }
                else
                {
                    return Ok($"Keep with Label Not Found");
                }
            }
            else if (type == "title")
            {
                List<Keep> ListTitle = Notess.GetNotesByTitle(text);
                if (ListTitle.Count > 0)
                {
                    return Ok(ListTitle);
                }
                else
                {
                    return BadRequest($"Title doesnot exist");
                }
            }
            else if (type == "pinned")
            {
                List<Keep> ListPin = Notess.GetPinnedNotes(text);
                if (ListPin.Count > 0)
                {
                    return Ok(ListPin);
                }
                else
                {
                    return BadRequest($" No Note Found");
                }
            }

            return BadRequest($"Note doesnot exist");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Keep value)
        {
            if (ModelState.IsValid)
            {
                bool result = Notess.PostNote(value);
                if (result)
                {
                    return Created($"/api/value/{value.KeepId}", value);

                }
                else
                {
                    return Conflict($"Bad Request");
                }
            }
            return BadRequest($"Invalid Format");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Keep value)
        {
            if (ModelState.IsValid)
            {
                bool result = Notess.PutNote(id, value);
                if (result)
                {
                    return Created("/api/value", value);
                }
                else
                {
                    return NotFound($"Note with {id} not found.");
                }
            }
            return BadRequest("Invalid Format");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = Notess.DeleteNote(id);
            if (result)
            {
                return Ok($"Deleted");
            }
            else
            {
                return BadRequest($"ID not found");
            }
        }
    }
}

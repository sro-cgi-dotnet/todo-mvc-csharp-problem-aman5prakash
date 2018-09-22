using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Keep;
using Microsoft.EntityFrameworkCore;

namespace Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        GoogleContext Notes = null;

        public ValuesController(GoogleContext GK)
        {
            this.Notes = GK;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (Notes)
            {
                List<Keep> temp = Notes.Google.Include(l => l.Lable).Include(c => c.CheckList).ToList();
                if (temp.Count == 0)
                {
                    return Ok($"Database is empty");
                }
                else return Ok(temp);
            }
        }

        // GET api/values/5
        [HttpGet("{KeepId:int}")]
        public ActionResult<int> Get(int Keepid)
        {
            using (Notes)
            {
                List<Keep> ListID = Notes.Google.Where(k => k.KeepId == Keepid).Include(l => l.Lable).Include(c => c.CheckList).ToList();
                if (ListID.Count > 0)
                {
                    return Ok(ListID);
                }
                else
                {
                    return BadRequest($"ID doesnot exist");
                }
            }
        }
        [HttpGet("{text}")]
        // [Route("{Lable}?type=Lable")]
        public ActionResult<string> Get(string text, [FromQuery] string type)
        {
            using (Notes)
            {
                if (type == "label")
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
                    List<Keep> ListTitle = Notes.Google.Where(k => k.Title == text).Include(l => l.Lable).Include(c => c.CheckList).ToList();
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
                    bool t1 = true;
                    List<Keep> ListPin = Notes.Google.Where(k => k.Pinned == t1).Include(l => l.Lable).Include(c => c.CheckList).ToList();
                    if (ListPin.Count > 0)
                    {
                        return Ok(ListPin);
                    }
                    else
                    {
                        return BadRequest($" No Note Found");
                    }
                }
            }
            return BadRequest($"Note doesnot exist");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Keep value)
        {
            using (Notes)
            {
                Keep temp2 = Notes.Google.FirstOrDefault(n => n.KeepId == value.KeepId);
                if (temp2 != null)
                {
                    return Conflict($"Bad Request");
                }
                Notes.Google.Add(value);
                Notes.SaveChanges();
                return Created($"/api/value/{value.KeepId}", value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Keep value)
        {
            using (Notes)
            {
                Notes.Update<Keep>(value);
                Notes.SaveChanges();
                return Ok("Updated");
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (Notes)
            {
                List<Keep> ListID = Notes.Google.Where(k => k.KeepId == id).ToList();
                if (ListID.Count > 0)
                {
                    Notes.RemoveRange(ListID);
                    Notes.SaveChanges();
                    return Ok($"Deleted");
                }
                else
                {
                    return BadRequest($"ID not found");
                }
            }
        }
    }
}

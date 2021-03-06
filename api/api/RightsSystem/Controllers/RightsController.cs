﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BAGCST.api.RightsSystem.Database;
using BAGCST.api.RightsSystem.Models;

namespace BAGCST.api.RightsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RightsController : ControllerBase
    {
        private IRightsDB rightsDB;

        public RightsController(IRightsDB rightsDB)
        {
            this.rightsDB = rightsDB;
        }


        /// <summary>
        /// returns the Right for the given ID. If it's not found, it returns NotFound.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Right</returns>
        [HttpGet("{id}")]
        public ActionResult<RightItem> getRight(int id)
        {
            RightItem right = rightsDB.getRight(id);
            if (right == null)
            {
                return NotFound($"No right found for id: {id}");
            }
            else
            {
                return Ok(right);
            }
        }

        [HttpGet]
        public ActionResult<RightItem[]> getAllRights()
        {
            RightItem[] rights = rightsDB.getAllRights();
            return Ok(rights);
        }

        [HttpPut("{id}")]
        public ActionResult<RightItem> editRight(int id, [FromBody] RightItem right_in)
        {
            //Check if id is valid
            if (rightsDB.getRight(id) == null)
            {
                return NotFound(($"No Right found for ID: {id}"));
            }

            //Check if rights are not null
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //update existing right
            RightItem right_out = rightsDB.editRight(id, right_in);

            //return new item
            return Ok(right_out);
        }

        [HttpDelete("{id}")]
        public ActionResult deleteRight(int id)
        {
            //TODO check for permission
            if (rightsDB.getRight(id) == null)
            {
                return NotFound(($"No Right found for id: {id}"));
            }
            rightsDB.deleteRight(id);
            return Ok();
        }

        /// <summary>
        /// creates a Right based on the given Right. If the given Right is null, it returns BadRequest.
        /// </summary>
        /// <param name="right_in"></param>
        /// <returns>Right|BadRequest</returns>
        [HttpPost]
        public ActionResult<RightItem> createRight(RightItem right_in)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (right_in == null || right_in.Path == "")
            {
                return BadRequest("Right not found");
            }

            foreach(RightItem right in rightsDB.getAllRights())
            {
                if(right_in.Path == right.Path)
                {
                    return BadRequest("Path is already existing.");
                }
            }

            if(right_in.RightID.ToString() == "")
            {
                return BadRequest("Failure. No RightID entered.");
            }
            RightItem right_out = rightsDB.createRight(right_in);
            return Created("", right_out);
        }

    }
}

/**************************************************************
 *  Filename:    UsersController.cs
 *  Copyright:    Co., Ltd.
 *
 *  Description: UsersController ClassFile.
 *
 *  @author:     Dongliang Yi
 *  @version     2021/11/8 17:30:53  @Reviser  Initial Version
 **************************************************************/
using Microsoft.AspNetCore.Mvc;
using NPlatform.Domains.Entity.Sys;
using NPlatform.Domains.IService.Sys;
using NPlatform.DTO.Sys;
using NPlatform.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService userSvc;
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<INPResult >Get()
        {
            return await userSvc.GetAllAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

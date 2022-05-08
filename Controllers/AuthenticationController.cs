using System;
using Microsoft.AspNetCore.Mvc;
using PisscordServer.Objects;
using PisscordServer.Objects.ControllerClasses;
using PisscordServer.Objects.HeaderParams;

namespace PisscordServer.Controllers {
    
    [ApiController]
    [Route("/api/auth")]
    public class AuthenticationController : ApiController {
        
        // This will give the user a token to use in the future
        [HttpGet]
        public IActionResult Login([FromHeader] AuthorizationHeaderParams authorization) {
            
            ExternalUser externalUser;
            try {
                if (authorization.GetAuthType() != "Basic") {
                    // bad
                    return BadRequest("Authorization header must be Basic");
                }

                externalUser = new ExternalUser {
                    Username = authorization.GetUsername(),
                    Password = authorization.GetPassword()
                };
            } catch (Exception e) {
                // Invalid something
                string text = Program.Debug ? "Invalid Authorization header" + e.Message : "Invalid Authorization header";
                return BadRequest(text);
            }
            
            // Get user
            User realUser;
            try {
                realUser = Program.Storage.GetFromNameUser(externalUser.Username);
                if (realUser == null) { throw new Exception("User not found"); }
            }
            catch (Exception) {
                // User not found
                return NotFound();
            }
            
            // Check password
            if (realUser.Password != externalUser.Password) {
                // Wrong password
                HttpContext.Response.Headers.Add("WWW-Authenticate", Program.WwwAuthHeader);
                return Unauthorized();
            }

            // Return token
            return Ok(TokenHandler.GenerateToken(realUser.Uuid));
        }
        
        // This will allow users to register
        [HttpPost]
        public IActionResult Register([FromBody] ExternalUser userInfo) {
            User user = new User(userInfo);
            Program.Storage.AddUser(user);
            return Ok();
        }
        
        [HttpOptions]
        public IActionResult Options() {
            HttpContext.Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
        
    }
    
}
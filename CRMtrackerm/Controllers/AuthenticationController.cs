using CrmTracker.Contracts;
using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;
using CrmTracker.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CrmTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext appDbContext;
        private IJwtTokenManager jwtTokenManager;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext, IJwtTokenManager jwtTokenManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appDbContext = appDbContext;
            this.jwtTokenManager = jwtTokenManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModels registerModel)
        {
            //check if user already exists
            var userExists = await userManager.FindByEmailAsync(registerModel.Email);
            if(userExists != null)
            {
                return BadRequest($"user with email {registerModel.Email} already exists");
            }

            IdentityUser identityUser = new IdentityUser()
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName,

            };

            
                

              var result = await userManager.CreateAsync(identityUser, registerModel.Password);
            if (result.Succeeded)
            {
               var userid= await userManager.FindByEmailAsync(registerModel.Email);
                var id = userid.Id;
                return Ok("user created!");

            }
            else
            {
                var message = string.Join("\n", result.Errors.Select(x => "Code : " + x.Code + " Description : " + x.Description));
                return BadRequest(message);
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            var userExists = await userManager.FindByEmailAsync(loginModel.Email);

            
            if (userExists != null && await userManager.CheckPasswordAsync(userExists, loginModel.Password))

            {

                var user = await userManager.FindByEmailAsync(loginModel.Email);

                var roleNames = await userManager.GetRolesAsync(user);
 
                var userrole = roleNames[0];
                //generate token
                var token = jwtTokenManager.GenerateToken(userExists.UserName, userrole);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }



        [HttpPost]
        [Route("Create/Role")]
        //--------------method to create a role
        public async Task<IActionResult> CreateRole([Required] string roleName)
        {

            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok(roleName + "Role Created Successfully");
            }

            else
            {
                Errors(result);
                return Ok(roleName + "Role Not Created ");
            }
        }

        [HttpDelete]
        [Route("Roles/DeleteRoleByName/{rolename}")]
        public async Task<IActionResult> DeleteRoleByName([Required] string rolename)
        {

            var role = await roleManager.FindByNameAsync(rolename);
            if (role != null)
            {
                var res = await roleManager.DeleteAsync(role);
                if (!res.Succeeded)
                    return Ok(role + " could not be deleted");

                return Ok(role + " deleted Successfully");
            }
            else
            {
                return Ok("Wrong role given");
            }

        }


        [HttpPost]
        [Route("AddUserToRole")]
        //-------------------------method to add user to role
        public async Task<IActionResult> AddUserToRole(UserRole ur)
        {

            //Get the appln user
            IdentityUser user = await userManager.FindByNameAsync(ur.UserName);
            if (user != null)
            {

                if (ur.Designation == "sales" )
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, "sales");


                    if (result.Succeeded)
                    {
                        return Ok("Role has been added to user Successfully");

                    }
                    else
                    {
                        return Ok("Role not added to user");
                    }
                    return Ok("Role not added to user");
                }
                if (ur.Designation == "Manager")
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, "Manager");


                    if (result.Succeeded)
                    {
                        return Ok("Role has been added to user Successfully");

                    }
                    else
                    {
                        return Ok("Role not added to user");
                    }
                    return Ok("Role not added to user");
                }
                if (ur.Designation == "customer")
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, "customer");


                    if (result.Succeeded)
                    {
                        return Ok("Role has been added to user Successfully");

                    }
                    else
                    {
                        return Ok("Role not added to user");
                    }
                    return Ok("Role not added to user");
                }
                if (ur.Designation == "Developer" || ur.Designation == "Software Engineer" || ur.Designation == "Testing")
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, "Technical");


                    if (result.Succeeded)
                    {
                        return Ok("Role has been added to user Successfully");

                    }
                    else
                    {
                        return Ok("Role not added to user");
                    }
                    return Ok("Role not added to user");
                }
                return Ok("Role not added to user");
            }
            else
            {
                return Ok("Wrong User Given");
            }
        }

        [HttpDelete]
        [Route("RemoveUser")]
        //method to remove a role from user
        public async Task<IActionResult> RemoveUserFromRole([Required] string userName, [Required] string roleName)
        {
            //Get the appln user
            IdentityUser user = await userManager.FindByNameAsync(userName);
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
                return Ok("Role has been removed from user Successfully");
            else
            {
                Errors(result);
                return Ok("User not removed from Role");
            }
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        [HttpGet]
        [Route("AllRoles")]
        //-------------------method to get all roles 
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(roleManager.Roles);
        }
    }
}

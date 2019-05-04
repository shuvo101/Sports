using Microsoft.AspNetCore.Identity;
using Sports.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Data.CodeSeed
{
    public class AppSeedData
    {
        //private static readonly List<ApplicationRole> roles = new List<ApplicationRole>()
        //{
        //    new ApplicationRole ("Admin"),
        //    new ApplicationRole ("Coach"),
        //    new ApplicationRole ("Athlete")
        //};

        private static readonly List<ApplicationUser> useres = new List<ApplicationUser>()
        {
            new ApplicationUser { FirstName = "Mitchel", LastName = "Fausto", UserName = "mitchel", PhoneNumber = "6902341234"},
               new ApplicationUser { FirstName = "Queen", LastName = "Jacobi", UserName = "queen",PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Magen", LastName = "Faye", UserName = "magen", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Delicia", LastName = "Ledonne", UserName = "delicia", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Camille", LastName = "Grantham", UserName = "camille", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Marc", LastName = "Voth", UserName = "marc", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Randy", LastName = "Rondon", UserName = "randy", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Delora", LastName = "Saville", UserName = "delora", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Rosario", LastName = "Reuben", UserName = "rosario", PhoneNumber = "6902341234"},
               new ApplicationUser{ FirstName = "Lula", LastName = "Uhlman", UserName = "lula", PhoneNumber = "6902341234"}
        };

        private static readonly List<TestType> testTypes = new List<TestType>()
        {
            new TestType {Name= "Coopertest" },
            new TestType {Name= "100 meter sprint" }
        };

        private static readonly List<FitnessRating> cooperTestfitnessRatings = new List<FitnessRating>()
        {
            new FitnessRating {Name= "Below Average", MinValue = 0,MaxValue=1000 },
            new FitnessRating {Name= "Average", MinValue = 1001,MaxValue=2000 },
            new FitnessRating {Name= "Good", MinValue = 2001,MaxValue=3500 },
            new FitnessRating {Name= "Very good", MinValue = 3501,MaxValue=10000 },
            
        };

        private static readonly List<FitnessRating> sprintTestfitnessRatings = new List<FitnessRating>()
        {
            new FitnessRating {Name= "Below Average", MinValue = 18,MaxValue=100 },
            new FitnessRating {Name= "Average", MinValue = 15,MaxValue=17 },
            new FitnessRating {Name= "Good", MinValue = 11,MaxValue=14 },
            new FitnessRating {Name= "Very good", MinValue = 0,MaxValue=10 },

        };


        public static async Task Initialize(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<ApplicationRole> roleManager)
        {
            //context.Database.EnsureCreated();

            
            string password = "Asd123@";

            //foreach (var role in roles) {
            //    if (await roleManager.FindByNameAsync(role.Name) == null)
            //    {
            //        await roleManager.CreateAsync(role);
            //    }
            //}

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }
            if (await roleManager.FindByNameAsync("Coach") == null)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Coach";
                await roleManager.CreateAsync(role);
            }
            if (await roleManager.FindByNameAsync("Athlete") == null)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Athlete";
                await roleManager.CreateAsync(role);
            }


            foreach (var user in useres)
            {
                if (await userManager.FindByNameAsync(user.UserName) == null)
                {
                    var result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await userManager.AddPasswordAsync(user, password);
                        if (user.UserName == "mitchel")
                        {

                            await userManager.AddToRoleAsync(user, "Coach");

                        }
                        else
                        {

                            await userManager.AddToRoleAsync(user, "Athlete");

                        }
                        
                    }
                }
            }

            #region TestType

            foreach (var type in testTypes)
            {
                if (context.TestType.FirstOrDefault(t => t.Name == type.Name) == null)
                {
                    context.TestType.Add(type);
                    context.SaveChanges();
                    if (type.ID > 0)
                    {
                        if (type.Name == "Coopertest")
                        {
                            foreach (var rating in cooperTestfitnessRatings)
                            {
                                rating.TestTypeID = type.ID;
                                context.FitnessRating.Add(rating);
                                context.SaveChanges();
                            }

                        }
                        else
                        {
                            foreach (var rating in sprintTestfitnessRatings)
                            {
                                rating.TestTypeID = type.ID;
                                context.FitnessRating.Add(rating);
                                context.SaveChanges();
                            }

                        }
                    }
                }
            }
            #endregion

            #region CoachAthlate
            var userList = await userManager.GetUsersInRoleAsync("Coach");
            var coachUser = userList.FirstOrDefault();
            var athleteList = await userManager.GetUsersInRoleAsync("Athlete");
            var coachAtheles = context.CoachAthlete.Where(c => c.CoachID == coachUser.Id && !c.IsRemoved).ToList();
            if (coachAtheles == null || coachAtheles.Count <= 0)
            {
                foreach (var athlete in athleteList)
                {
                    var coachAthlete = new CoachAthlete();
                    coachAthlete.CoachID = coachUser.Id;
                    coachAthlete.AthleteID = athlete.Id;
                    context.CoachAthlete.Add(coachAthlete);
                    context.SaveChanges();
                }
            }
            #endregion

        }

    }
}

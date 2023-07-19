using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess
{
	public class WalletDbContext : DbContext
	{
		public WalletDbContext(DbContextOptions<WalletDbContext> options)
			: base(options)
		{

		}


		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			//Вкарах ги в отделни методи за по-чисто, дано не направи проблем в бъдеще.
			ConfigureMigration(builder);

			base.OnModelCreating(builder);

			CreateSeed(builder);
		}
		protected void ConfigureMigration(ModelBuilder builder)
		{

		}
		protected void CreateSeed(ModelBuilder builder)
		{
			IList<Role> roles = new List<Role>
			{
				new Role
				{
					Id = 1,
					Name="Blocked"
				},
				new Role
				{
					Id = 2,
					Name = "User"
				},
				new Role
				{
					Id = 3,
					Name = "Admin"
				}
			};
			IList<User> users = new List<User>
			{
				new User()
			{
				Id = 1,
				FirstName = "Georgi",
				LastName = "Georgiev",
				Username = "goshoXx123",
				Email = "gosho@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 2,
				FirstName = "Nikolai",
				LastName = "Barekov",
				Username = "BarekaXx123",
				Email = "Barekov@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123

            },
			new User()
			{
				Id = 3,
				FirstName = "Shtilian",
				LastName = "Uzunov",
				Username = "Uzunkata",
				Email = "Uzunkata@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 4,
				FirstName = "Vladislav",
				LastName = "Cvetanov",
				Username = "Cvete123",
				Email = "Cvetan@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 5,
				FirstName = "Kosta",
				LastName = "Kostev",
				Username = "BrainDamage123",
				Email = "Kostev@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 6,
				FirstName = "Admin",
				LastName = "Adminov",
				Username = "Admin",
				Email = "Admin@gmail.com",
				Password = "MTIz",
				RoleId = 3,
				ProfilePicPath="/Images/UserProfilePics/35c6a7f8-decb-440c-853e-d32b5d0a3c64_3853-136116.jpg"


                //123
            },
			new User()
			{
				Id = 7,
				FirstName = "Andrea",
				LastName = "Paynera",
				Username = "TopAndreika",
				Email = "Andrea@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 8,
				FirstName = "Emanuela",
				LastName = "Paynera",
				Username = "TopEmanuelka",
				Email = "Emanuela@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			new User()
			{
				Id = 9,
				FirstName = "Katrin",
				LastName = "lilova",
				Username = "Katrin",
				Email = "Katrin@gmail.com",
				Password = "MTIz",
				RoleId = 2

                //123
            },
			//new User()
			//{
			//	Id = 10,
			//	FirstName = "Atanas",
			//	LastName = "Iliev",
			//	Username = "Nachosa",
			//	Email = "Nachosa@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2,
			//	ProfilePicPath="/Images/UserProfilePics/70f63493-d80b-44ed-a8cc-36e8b84b140c_photo.jpeg"

   //             //123
   //         },
			//new User()
			//{
			//	Id = 11,
			//	FirstName = "Nikolai",
			//	LastName = "Gigov",
			//	Username = "Nikolai",
			//	Email = "Gigov@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2

   //             //123
   //         },
			//new User()
			//{
			//	Id = 12,
			//	FirstName = "Vlado",
			//	LastName = "Vladov",
			//	Username = "BatVlad",
			//	Email = "Vlad@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2

   //             //123
   //         },
			//new User()
			//{
			//	Id = 13,
			//	FirstName = "Ivan",
			//	LastName = "Vanov",
			//	Username = "BatVanko",
			//	Email = "Vanko@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2,
			//	ProfilePicPath="/Images/UserProfilePics/6c456879-135e-482b-9ba9-bdbda1e6fe8e_309988-profileavatar.jpeg"

   //             //123
   //         },
			//new User()
			//{
			//	Id = 14,
			//	FirstName = "Petar",
			//	LastName = "Ivanov",
			//	Username = "Peshaka",
			//	Email = "Peshaka@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2

   //             //123
   //         },
			//new User()
			//{
			//	Id = 15,
			//	FirstName = "Georgi",
			//	LastName = "Goshev",
			//	Username = "BatGergi",
			//	Email = "Gergi@gmail.com",
			//	Password = "MTIz",
			//	RoleId = 2,
			//	//ProfilePicPath="/Images/UserProfilePics/c372cf2e-0cab-43a0-81e1-73ea610f9dfd_ddh0598-18d7e667-d117-4b11-8ef0-244eb60bfa45.jpg"

   //             //123
   //         }
			};


			builder.Entity<Role>().HasData(roles);
			builder.Entity<User>().HasData(users);


		}

	}
}

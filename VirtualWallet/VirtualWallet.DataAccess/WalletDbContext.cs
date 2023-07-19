using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
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
			builder.Entity<Wallet>()
			.Property(w => w.BGN)
			.HasPrecision(18, 6);

			builder.Entity<Wallet>()
			.Property(w => w.CHF)
			.HasPrecision(18, 6);

			builder.Entity<Wallet>()
			.Property(w => w.EUR)
			.HasPrecision(18, 6);

			builder.Entity<Wallet>()
			.Property(w => w.USD)
			.HasPrecision(18, 6);

			builder.Entity<Wallet>()
			.Property(w => w.GBP)
			.HasPrecision(18, 6);

			builder.Entity<User>()
				.HasOne(u => u.Wallet)
				.WithOne(vw => vw.User)
				//.HasForeignKey(vw=>vw.)
				.OnDelete(DeleteBehavior.NoAction);

			//modelBuilder.Entity<Blog>()
			//.HasOne(e => e.Header)
			//.WithOne(e => e.Blog)
			//.HasForeignKey<BlogHeader>(e => e.BlogId)
			//.IsRequired();

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
				RoleId = 2,
				WalletId = 1

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
				RoleId = 2,
				WalletId = 2

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
				RoleId = 2,
				WalletId = 3

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
				RoleId = 2,
				WalletId = 4

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
				RoleId = 2,
				WalletId = 5

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
				WalletId = 6


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
				RoleId = 2,
				WalletId = 7

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
				RoleId = 2,
				WalletId = 8

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
				RoleId = 2,
				WalletId = 9

                //123
            },
			};

			IList<Wallet> wallets = new List<Wallet>
			{
				new Wallet()
			{
				Id = 1,
				UserId = 1,
			},
					new Wallet()
			{
				Id = 2,
				UserId = 2,
			},
						new Wallet()
			{
				Id = 3,
				UserId = 3,
			},
					new Wallet()
			{
				Id = 4,
				UserId = 4,
			},
						new Wallet()
			{
				Id = 5,
				UserId = 5
			},
					new Wallet()
			{
				Id = 6,
				UserId = 6,
			},
						new Wallet()
			{
				Id = 7,
				UserId = 7,
			},
					new Wallet()
			{
				Id = 8,
				UserId = 8,
			},
								new Wallet()
			{
				Id = 9,
				UserId = 9,
			},


			};


			builder.Entity<Role>().HasData(roles);
			builder.Entity<User>().HasData(users);
			builder.Entity<Wallet>().HasData(wallets);


		}

	}
}

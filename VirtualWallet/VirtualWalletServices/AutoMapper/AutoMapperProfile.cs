using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.UserDto;

namespace VirtualWallet.Business.AutoMapper
{
	public class AutoMapperProfile:Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<CreateUserDto, User>();
			CreateMap<UpdateUserDto, User>();
		}
	}
}

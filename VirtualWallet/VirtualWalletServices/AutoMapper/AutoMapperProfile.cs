using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.UserDto;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;

namespace VirtualWallet.Business.AutoMapper
{
	public class AutoMapperProfile : Profile
	{

		private IUserService userService;

		public AutoMapperProfile(IUserService userService)
		{
			this.userService = userService;

			CreateMap<CreateUserDto, User>();
			CreateMap<UpdateUserDto, User>();

			CreateMap<CreateTransactionDto, WalletTransaction>()
				//TODO: Изкарване в методи и повече контрол над exception и формат.
				.ForMember(t => t.CurrencyId, opt => opt.MapFrom(tDto => (int)Enum.Parse<CurrencyCode>(tDto.CurrencyCode)))
				.ForMember(t => t.Recipient, opt => opt.MapFrom(tDto => this.userService.GetUserByUsername(tDto.RecipientUsername)))
				.ForMember(t => t.Amount, opt => opt.MapFrom(tDto => tDto.Amount));
		}
	}
}

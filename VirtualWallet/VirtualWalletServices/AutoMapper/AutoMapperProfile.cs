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
using VirtualWallet.Dto.CardDto;

namespace VirtualWallet.Business.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
		private IUserService userService;

		public AutoMapperProfile(IUserService userService)
		{
			this.userService = userService;

            CreateMap<CardInfoDto, Card>();
            CreateMap<CreateUserDto, User>();
			CreateMap<UpdateUserDto, User>();

			CreateMap<CreateWalletTransactionDto, WalletTransaction>()
				//TODO: Изкарване в методи и повече контрол над exception и формат.
				.ForMember(wt => wt.CurrencyId, opt => opt.MapFrom(wtDto => (int)Enum.Parse<CurrencyCode>(wtDto.CurrencyCode)))
				.ForMember(wt => wt.Recipient, opt => opt.MapFrom(wtDto => this.userService.GetUserByUsername(wtDto.RecipientUsername)))
				.ForMember(wt => wt.Amount, opt => opt.MapFrom(wtDto => wtDto.Amount));

			CreateMap<WalletTransaction, GetWalletTransactionDto>()
				.ForMember(wtDto => wtDto.SenderUsername, opt => opt.MapFrom(wt => wt.Sender.Username))
				.ForMember(wtDto => wtDto.RecipientUsername, opt => opt.MapFrom(wt => wt.Recipient.Username))
				.ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()))
				.ForMember(wtDto => wtDto.Amount, opt => opt.MapFrom(wt => wt.Amount));
		}
	}
}
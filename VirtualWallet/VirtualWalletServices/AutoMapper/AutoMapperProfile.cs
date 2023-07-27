﻿using AutoMapper;
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
using VirtualWallet.Dto.TransferDto;
using VirtualWallet.Dto.ExchangeDto;

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
			CreateMap<User, GetUserDto>()
				.ForMember(guDto=>guDto.CardsCount, opt=> opt.MapFrom(u=>u.Cards.Count))
				.ForMember(guDto => guDto.Role, opt=> opt.MapFrom(u=>u.Role.Name.ToString()));

			CreateMap<CreateWalletTransactionDto, WalletTransaction>()
				//TODO: Изкарване в методи и повече контрол над exception и формат.
				.ForMember(wt => wt.CurrencyId, opt => opt.MapFrom(wtDto => (int)Enum.Parse<CurrencyCode>(wtDto.CurrencyCode)))
				.ForMember(wt => wt.Recipient, opt => opt.MapFrom(wtDto => this.userService.GetUserByUsername(wtDto.RecipientUsername)));
			CreateMap<WalletTransaction, GetWalletTransactionDto>()
				.ForMember(wtDto => wtDto.SenderUsername, opt => opt.MapFrom(wt => wt.Sender.Username))
				.ForMember(wtDto => wtDto.RecipientUsername, opt => opt.MapFrom(wt => wt.Recipient.Username))
				.ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()))
				.ForMember(wtDto => wtDto.Amount, opt => opt.MapFrom(wt => wt.Amount));

			CreateMap<CreateTransferDto, Transfer>();
			CreateMap<Transfer, GetTransferDto>()
				.ForMember(wtDto => wtDto.CurrencyCode, opt => opt.MapFrom(wt => wt.Currency.Code.ToString()));

			CreateMap<Exchange, GetExchangeDto>()
				.ForMember(ExDto => ExDto.FromCurrency, opt => opt.MapFrom(e => e.FromCurrency.Code.ToString()))
				.ForMember(ExDto => ExDto.ToCurrency, opt => opt.MapFrom(e => e.ToCurrency.Code.ToString()));

		}
	}
}